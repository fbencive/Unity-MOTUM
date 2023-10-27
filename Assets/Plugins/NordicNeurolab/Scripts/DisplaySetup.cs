using System;
using UnityEngine;

namespace BIL.Display
{
    //This script is an adaptation from the NordicNeuroLab package to project the visual input to 3D glasses.
    //Gaspare Galati October 2022
    public class DisplaySetup : MonoBehaviour
    {
        public MainDisplay mainDisplay = MainDisplay.Display1;
        public OptionalDisplay rightEyeDisplay = OptionalDisplay.None;
        public float pupilDistance = 64;
        public float convergence = -5.0f;
        public float barrelK;
        public float barrelKcube;

        [Header("Internal references")]

        [SerializeField]
        Camera mainCamera;

        [SerializeField]
        Camera rightCamera;

        [SerializeField]
        BarrelDistortion[] barrelDistortions;

        private float lastPupilDistance, lastBarrelK, lastBarrelKcube, lastConvergence;

        void Awake()
        {
            AssignDisplay((int)mainDisplay, mainCamera);
            AssignDisplay((int)rightEyeDisplay, rightCamera);

            //You can modify Pupil Distance and BarrelDistortion online - while in playing mode - by changing their public values. 
            ConfigurePupilDistance();
            ConfigureBarrelDistortion();
            PlayerPrefs.SetFloat("PupilDistance", pupilDistance);
            PlayerPrefs.SetFloat("Convergence", convergence);
        }

        void Update()
        {
            pupilDistance = PlayerPrefs.GetFloat("PupilDistance");
            convergence = PlayerPrefs.GetFloat("Convergence");

            if (lastPupilDistance != pupilDistance || lastConvergence != convergence)
                ConfigurePupilDistance();

            barrelK = PlayerPrefs.GetFloat("BarrelK");
            barrelKcube = PlayerPrefs.GetFloat("BarrelKCube");

            if (lastBarrelK != barrelK || lastBarrelKcube != barrelKcube)
                ConfigureBarrelDistortion();
        }

        void ConfigurePupilDistance()
        {
            if (rightEyeDisplay == OptionalDisplay.None)
            {
                mainCamera.transform.localPosition = new Vector3(0, 0, 0); //if there is only one camera, set it at the centre
            }
            else
            {
                //If there are two cameras (one for the left, the other for the right eye), switch them on the left/right according to pupil distance
                mainCamera.transform.localPosition = new Vector3(-(float)pupilDistance / 2000, 0, 0);
                rightCamera.transform.localPosition = new Vector3((float)pupilDistance / 2000, 0, 0);

                //check if convergence is equal to 0; if so, the two eyes are "parallel" (be careful because 0 is not accepted)
                var angle = convergence == 0? 0: 90.0f - Mathf.Atan2(convergence, pupilDistance / 2000) * Mathf.Rad2Deg;
                mainCamera.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.up);
                rightCamera.transform.localRotation = Quaternion.AngleAxis(-angle, Vector3.up);

            }
            lastPupilDistance = pupilDistance;
            lastConvergence = convergence;
        }

        void ConfigureBarrelDistortion()
        {
            Debug.Log("Barrel distortion: k = " + barrelK + ", kCube = " + barrelKcube);
            if (rightEyeDisplay == OptionalDisplay.None || (barrelK == 0 && barrelKcube == 0))
            {
                for (int i = 0; i < barrelDistortions.Length; i++)
                    barrelDistortions[i].enabled = false;
            }
            else
            {
                for (int i = 0; i < barrelDistortions.Length; i++)
                {
                    barrelDistortions[i].enabled = true;
                    barrelDistortions[i].RefreshMaterial(barrelK, barrelKcube);
                }
            }
            lastBarrelK = barrelK;
            lastBarrelKcube = barrelKcube;
        }


        void AssignDisplay(int display, Camera camera)
        {
            if (display < 0)
            {
                camera.enabled = false;
            }
            else
            {
                camera.enabled = true;
                camera.targetDisplay = display;
                UnityEngine.Display.displays[display].Activate();
            }
        }
    }

    public enum MainDisplay
    {
        Display1 = 0,
        Display2,
        Display3,
        Display4,
        Display5,
        Display6
    }

    public enum OptionalDisplay
    {
        None = -1,
        Display1,
        Display2,
        Display3,
        Display4,
        Display5,
        Display6
    }
}