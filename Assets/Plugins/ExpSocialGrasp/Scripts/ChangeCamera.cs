using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeCamera : MonoBehaviour
{
    public Camera leftCamera;
    public Camera rightCamera;
    private Transform Avatar1Pp;
    private float destValue;
    [SerializeField] private GameObject inputField;
    // Start is called before the first frame update

    public void Start()
    {
        Avatar1Pp = GameObject.Find("Animated Avatar").transform;
    }

    public void Increase()
    {
        float startValue = float.Parse(inputField.GetComponent<TMP_InputField>().text);

        if (inputField.name == "PupilDistance")
        {
            destValue = startValue + 1f;
            PlayerPrefs.SetFloat("PupilDistance", destValue);
            inputField.GetComponent<TMP_InputField>().text = destValue.ToString();
        }
        if (inputField.name == "Convergence")
        {
            destValue = startValue + 1f;
            PlayerPrefs.SetFloat("Convergence", destValue);
            inputField.GetComponent<TMP_InputField>().text = destValue.ToString();
        }
        if (inputField.name == "BarrelK")
        {
            destValue = startValue + 0.1f;
            PlayerPrefs.SetFloat("BarrelK", destValue);
            inputField.GetComponent<TMP_InputField>().text = destValue.ToString();
        }
        if (inputField.name == "BarrelKCube")
        {
            destValue = startValue + 0.1f;
            PlayerPrefs.SetFloat("BarrelKCube", destValue);
            inputField.GetComponent<TMP_InputField>().text = destValue.ToString();
        }
        if (inputField.name == "FOV")
        {
            destValue = startValue + 1f;
            PlayerPrefs.SetFloat("FOV", destValue);
            rightCamera.fieldOfView = destValue;
            leftCamera.fieldOfView = destValue;
            inputField.GetComponent<TMP_InputField>().text = destValue.ToString();
        }
        if (inputField.name == "Position")
        {
            if (transform.name == "Y+")
                Avatar1Pp.localPosition = new Vector3(Avatar1Pp.localPosition.x, Avatar1Pp.localPosition.y + 0.01f, Avatar1Pp.localPosition.z);
            if (transform.name == "X+")
                Avatar1Pp.localPosition = new Vector3(Avatar1Pp.localPosition.x + 0.01f, Avatar1Pp.localPosition.y, Avatar1Pp.localPosition.z);
            if (transform.name == "Z+")
                Avatar1Pp.localPosition = new Vector3(Avatar1Pp.localPosition.x, Avatar1Pp.localPosition.y, Avatar1Pp.localPosition.z - 0.01f);

            PlayerPrefs.SetFloat("CameraX", Avatar1Pp.localPosition.x);
            PlayerPrefs.SetFloat("CameraY", Avatar1Pp.localPosition.y);
            PlayerPrefs.SetFloat("CameraZ", Avatar1Pp.localPosition.z);
        }

        if (inputField.name == "PlatformPosition")
        {
            Transform platform = Avatar1Pp.Find("StartingPlatform").gameObject.transform;
            if (transform.name == "Y+")
                platform.localPosition = new Vector3(platform.localPosition.x, platform.localPosition.y + 0.01f, platform.localPosition.z);

            PlayerPrefs.SetFloat("PlatformY", platform.localPosition.y);
        }

    }

    public void Decrease()
    {
        float startValue = float.Parse(inputField.GetComponent<TMP_InputField>().text);
        if (inputField.name == "PupilDistance")
        {
            destValue = startValue - 1f;
            PlayerPrefs.SetFloat("PupilDistance", destValue);
            inputField.GetComponent<TMP_InputField>().text = destValue.ToString();
        }
        if (inputField.name == "Convergence")
        {
            destValue = startValue - 1f;
            PlayerPrefs.SetFloat("Convergence", destValue);
            inputField.GetComponent<TMP_InputField>().text = destValue.ToString();
        }
        if (inputField.name == "BarrelK")
        {
            destValue = startValue - 0.1f;
            PlayerPrefs.SetFloat("BarrelK", destValue);
            inputField.GetComponent<TMP_InputField>().text = destValue.ToString();
        }
        if (inputField.name == "BarrelKCube")
        {
            destValue = startValue - 0.1f;
            PlayerPrefs.SetFloat("BarrelKCube", destValue);
            inputField.GetComponent<TMP_InputField>().text = destValue.ToString();
        }
        if (inputField.name == "FOV")
        {
            destValue = startValue - 1f;
            PlayerPrefs.SetFloat("FOV", destValue);
            rightCamera.fieldOfView = destValue;
            leftCamera.fieldOfView = destValue;
            inputField.GetComponent<TMP_InputField>().text = destValue.ToString();
        }
        if (inputField.name == "Position")
        {

            if (transform.name == "Y-")
                Avatar1Pp.localPosition = new Vector3(Avatar1Pp.localPosition.x, Avatar1Pp.localPosition.y - 0.01f, Avatar1Pp.localPosition.z);
            if (transform.name == "X-")
                Avatar1Pp.localPosition = new Vector3(Avatar1Pp.localPosition.x - 0.01f, Avatar1Pp.localPosition.y, Avatar1Pp.localPosition.z);
            if (transform.name == "Z-")
                Avatar1Pp.localPosition = new Vector3(Avatar1Pp.localPosition.x, Avatar1Pp.localPosition.y, Avatar1Pp.localPosition.z + 0.01f);

            PlayerPrefs.SetFloat("CameraX", Avatar1Pp.localPosition.x);
            PlayerPrefs.SetFloat("CameraY", Avatar1Pp.localPosition.y);
            PlayerPrefs.SetFloat("CameraZ", Avatar1Pp.localPosition.z);
        }

        if (inputField.name == "PlatformPosition")
        {
            Transform platform = Avatar1Pp.Find("StartingPlatform").gameObject.transform;
            if (transform.name == "Y-")
                platform.localPosition = new Vector3(platform.localPosition.x, platform.localPosition.y - 0.01f, platform.localPosition.z);

            PlayerPrefs.SetFloat("PlatformY", platform.localPosition.y);
        }

    }
}
