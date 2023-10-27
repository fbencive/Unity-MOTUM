using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeEnvironment : MonoBehaviour
{
    private Transform destObj;
    private float destValue;
    [SerializeField] private GameObject inputField;
    // Start is called before the first frame update
    void Start()
    {
        destObj = GameObject.Find("Env_3Avatar").transform;
    }
    public void Increase()
    {
        float startValue = float.Parse(inputField.GetComponent<TMP_InputField>().text);

        if (inputField.name == "Size")
        {
            destValue = startValue + 0.1f;
            PlayerPrefs.SetFloat("EnvironmentSize", destValue);
            destObj.localScale = new Vector3(destValue, destValue, destValue);
            inputField.GetComponent<TMP_InputField>().text = destValue.ToString();
        }
        if (inputField.name == "Position")
        {
            if (transform.name == "Y+")
                destObj.localPosition = new Vector3(destObj.localPosition.x, destObj.localPosition.y + 0.01f, destObj.localPosition.z);
            if (transform.name == "X+")
                destObj.localPosition = new Vector3(destObj.localPosition.x + 0.01f, destObj.localPosition.y, destObj.localPosition.z);
            if (transform.name == "Z+")
                destObj.localPosition = new Vector3(destObj.localPosition.x, destObj.localPosition.y, destObj.localPosition.z - 0.01f);

            PlayerPrefs.SetFloat("EnvX", destObj.localPosition.x);
            PlayerPrefs.SetFloat("EnvY", destObj.localPosition.y);
            PlayerPrefs.SetFloat("EnvZ", destObj.localPosition.z);
        }
    }

    public void Decrease()
    {
        float startValue = float.Parse(inputField.GetComponent<TMP_InputField>().text);
        if (inputField.name == "Size")
        {
            destValue = startValue - 0.1f;
            PlayerPrefs.SetFloat("EnvironmentSize", destValue);
            destObj.localScale = new Vector3(destValue, destValue, destValue);
            inputField.GetComponent<TMP_InputField>().text = destValue.ToString();
        }
        if (inputField.name == "Position")
        {

            if (transform.name == "Y-")
                destObj.localPosition = new Vector3(destObj.localPosition.x, destObj.localPosition.y - 0.01f, destObj.localPosition.z);
            if (transform.name == "X-")
                destObj.localPosition = new Vector3(destObj.localPosition.x - 0.01f, destObj.localPosition.y, destObj.localPosition.z);
            if (transform.name == "Z-")
                destObj.localPosition = new Vector3(destObj.localPosition.x, destObj.localPosition.y, destObj.localPosition.z + 0.01f);

            PlayerPrefs.SetFloat("EnvX", destObj.localPosition.x);
            PlayerPrefs.SetFloat("EnvY", destObj.localPosition.y);
            PlayerPrefs.SetFloat("EnvZ", destObj.localPosition.z);
        }
    }
}
