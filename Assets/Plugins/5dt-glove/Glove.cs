using UnityEngine;
using FDTGloveUltraCSharpWrapper;

namespace Glove5dt
{
    public class Glove
    {
        readonly private CfdGlove glove = new CfdGlove();
        readonly public GloveCalibration calibration;
        private string port;


        public Glove(string port = "USB0", Hand hand = Hand.RIGHT)
        {
            calibration = new GloveCalibration(glove);
            Port = port;
            Hand = hand;
        }

        public string Port
        {
            get
            {
                return port;
            }
            set
            {
                Disconnect();
                port = value;
                glove.Open(port);
            }
        }
        public Hand Hand;

        public bool Connected
        {
            get
            {
                return glove.IsOpen();
            }
        }

        public void Disconnect()
        {
            if (glove.IsOpen())
                glove.Close();
        }

        public float[] Read()
        {
            float[] values = new float[20];
            if (Connected)
            {
                glove.GetSensorScaledAll(ref values);
                //Modified on 4/11/22 by FB
                values[Sensors.THUMB_NEAR] = values[Sensors.THUMB_NEAR] * 90.0f + 10.0f;//original: +10, then 0; original: 90 degrees, then 60 (60 + 0 FB; 60 + 10 MT)
                values[Sensors.THUMB_FAR] = values[Sensors.THUMB_FAR] * 90.0f + 0.0f;//original: 10
                values[Sensors.THUMB_INDEX] = (int)Hand * (values[Sensors.THUMB_INDEX] * 30.0f - 10.0f); //original: - 0
                values[Sensors.INDEX_NEAR] = values[Sensors.INDEX_NEAR] * 90.0f - 10.0f;
                values[Sensors.INDEX_FAR] = values[Sensors.INDEX_FAR] * 90.0f - 0.0f; //original -10
                values[Sensors.INDEX_MIDDLE] = (int)Hand * (values[Sensors.INDEX_MIDDLE] * 24.0f - 12.0f);//original: 12
                values[Sensors.MIDDLE_NEAR] = values[Sensors.MIDDLE_NEAR] * 90.0f - 10.0f;
                values[Sensors.MIDDLE_FAR] = values[Sensors.MIDDLE_FAR] * 90.0f - 0.0f;//original: -10
                values[Sensors.MIDDLE_RING] = (int)Hand * (values[Sensors.MIDDLE_RING] * 15.0f - 10.0f);//original: 10
                values[Sensors.RING_NEAR] = values[Sensors.RING_NEAR] * 100.0f - 0.0f;//original: -10, 90 degrees
                values[Sensors.RING_FAR] = values[Sensors.RING_FAR] * 90.0f - 0.0f;//original: -10
                values[Sensors.RING_LITTLE] = (int)Hand * (values[Sensors.RING_LITTLE] * 20.0f - 10.0f);//original: 25; degree original 30
                values[Sensors.LITTLE_NEAR] = values[Sensors.LITTLE_NEAR] * 100.0f - 0.0f;//original: 10, 90 degrees
                values[Sensors.LITTLE_FAR] = values[Sensors.LITTLE_FAR] * 90.0f - 0.0f;//original: 10
            }
            else
            {
                glove.Open(Port);
            }
            return values;
        }
    }

    [System.Serializable]
    public class GloveCalibration
    {
        readonly private CfdGlove glove;
        private ushort[] pCurrentData = new ushort[20];
        public ushort[] pLower = new ushort[20];
        public ushort[] pUpper = new ushort[20];

        public GloveCalibration(CfdGlove aGlove)
        {
            glove = aGlove;
        }

        public void Reset()
        {
            glove.ResetCalibration();
        }

        public void Load()
        {
            if (PlayerPrefs.HasKey("GloveCalibration"))
            {
                JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("GloveCalibration"), this);
                glove.SetCalibrationAll(pUpper, pLower);
            }
        }

        public void Save()
        {
            glove.GetCalibrationAll(ref pUpper, ref pLower);
            PlayerPrefs.SetString("GloveCalibration", JsonUtility.ToJson(this));
        }

        public void Calibrate(Calibration type)
        {
            glove.GetSensorRawAll(ref pCurrentData);
            glove.GetCalibrationAll(ref pUpper, ref pLower);

            switch (type)
            {
                case Calibration.FIST:
                    CalibrateUpper(Sensors.INDEX_NEAR);
                    CalibrateUpper(Sensors.INDEX_FAR);
                    CalibrateUpper(Sensors.MIDDLE_NEAR);
                    CalibrateUpper(Sensors.MIDDLE_FAR);
                    CalibrateUpper(Sensors.RING_NEAR);
                    CalibrateUpper(Sensors.RING_FAR);
                    CalibrateUpper(Sensors.LITTLE_NEAR);
                    CalibrateUpper(Sensors.LITTLE_FAR);
                    break;

                case Calibration.THUMB:
                    CalibrateUpper(Sensors.THUMB_NEAR);
                    CalibrateUpper(Sensors.THUMB_FAR);
                    break;

                case Calibration.OPEN:
                    CalibrateLower(Sensors.THUMB_NEAR);
                    CalibrateLower(Sensors.THUMB_FAR);
                    CalibrateLower(Sensors.INDEX_NEAR);
                    CalibrateLower(Sensors.INDEX_FAR);
                    CalibrateLower(Sensors.MIDDLE_NEAR);
                    CalibrateLower(Sensors.MIDDLE_FAR);
                    CalibrateLower(Sensors.RING_NEAR);
                    CalibrateLower(Sensors.RING_FAR);
                    CalibrateLower(Sensors.LITTLE_NEAR);
                    CalibrateLower(Sensors.LITTLE_FAR);
                    CalibrateLower(Sensors.THUMB_INDEX);
                    CalibrateLower(Sensors.INDEX_MIDDLE);
                    CalibrateLower(Sensors.MIDDLE_RING);
                    CalibrateLower(Sensors.RING_LITTLE);
                    break;

                case Calibration.FLAT:
                    CalibrateUpper(Sensors.THUMB_INDEX);
                    CalibrateUpper(Sensors.INDEX_MIDDLE);
                    CalibrateUpper(Sensors.MIDDLE_RING);
                    CalibrateUpper(Sensors.RING_LITTLE);
                    break;
            }
        }

        private void CalibrateLower(int sensor)
        {
            glove.SetCalibration(sensor, pUpper[sensor], pCurrentData[sensor]);
        }

        private void CalibrateUpper(int sensor)
        {
            glove.SetCalibration(sensor, pCurrentData[sensor], pLower[sensor]);
        }
    }
}
