using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Glove5dt
{

    public class GloveComponent : MonoBehaviour
    {
        public string Port = "USB0";
        public bool Connected = false;
        public Hand Hand = Hand.RIGHT;
        public uint MovingAverageWidth = 3;
        public bool Simulation = false;
        public float[] values = new float[14];

        private Glove glove;
        private MovingAverage average;
        private uint keyPressed = 0;
        public Transform Images;

        public Toggle hasCalibrated;
        public Toggle LoadCalibration;
        public Toggle SaveCalibration;

        public TMP_Dropdown calibrationType;

        public bool hasBeenCalibrated = false;
        public bool hasBeenSaved = false;
        public bool hasBeenLoaded = false;

        public Transform imagesLeft;
        public Transform imagesRight;

        void Awake()
        {
            glove = new Glove(Port, Hand);
            average = new MovingAverage(MovingAverageWidth);
        }

        void Start()
        {
            calibrationType = calibrationType.GetComponent<TMP_Dropdown>();
            calibrationType.onValueChanged.AddListener(delegate
            {
                DropdownValueChanged(calibrationType);
            });
        }

        void OnApplicationQuit()
        {
            glove.Disconnect();
        }

        void Update()
        {
            if (Port != glove.Port)
                glove.Port = Port;
            if (Hand != glove.Hand)
                glove.Hand = Hand;
            if (MovingAverageWidth != average.Width)
                average.Width = MovingAverageWidth;

            if (hasCalibrated.isOn & (hasBeenCalibrated == false))
            {
                if (calibrationType.value == 1)
                {
                    Debug.Log("calibro fist");
                    glove.calibration.Calibrate(Calibration.FIST);
                }
                if (calibrationType.value == 2)
                {
                    Debug.Log("calibro pollice");
                    glove.calibration.Calibrate(Calibration.THUMB);
                }
                if (calibrationType.value == 3)
                {
                    Debug.Log("calibro open");
                    glove.calibration.Calibrate(Calibration.OPEN);
                }
                if (calibrationType.value == 4)
                {
                    Debug.Log("calibro flat");
                    glove.calibration.Calibrate(Calibration.FLAT);
                }
                hasBeenCalibrated = true;
            }

            if (!hasCalibrated.isOn)
                hasBeenCalibrated = false;

            if (SaveCalibration.isOn & !hasBeenSaved)
            {
                Debug.Log("saved calibration");
                glove.calibration.Save();
                hasBeenSaved = true;
            }
            else if (!SaveCalibration.isOn)
                hasBeenSaved = false;

            if (LoadCalibration.isOn & !hasBeenLoaded)
            {
                Debug.Log("loaded calibration");
                glove.calibration.Load();
                hasBeenLoaded = true;
            }
            else if (!LoadCalibration.isOn)
                hasBeenLoaded = false;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (!Simulation)
                values = average.addSample(glove.Read());
        }

        void DropdownValueChanged(TMP_Dropdown change)
        {
            hasBeenCalibrated = false;
        }

        void ShowCalibrationImages()
        {
            string imageName = calibrationType.options[calibrationType.value].text;

            foreach (Transform t in imagesLeft)
            {
                if (t.name == imageName)
                    t.gameObject.SetActive(true);
                else
                    t.gameObject.SetActive(false);
            }

            foreach (Transform tR in imagesRight)
            {
                //if (tR.name == imageName)
                    //tR.gameObject.SetActive(true);
                //else
                    //tR.gameObject.SetActive(false);
            }
        }

        void HideAllCalibrationImages()
        {
            foreach (Transform t in imagesLeft)
                t.gameObject.SetActive(false);

            foreach (Transform tR in imagesRight)
                tR.gameObject.SetActive(false);
        }

    }
}