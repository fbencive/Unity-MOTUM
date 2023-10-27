using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PPsetter : MonoBehaviour
{
    //The list of messages for the Dropdown

    //This is the Dropdown
    TMP_Dropdown m_Dropdown;
    //public TMP_Dropdown langDD;
    private GameObject MenuPanel;
    private Transform Avatar1Pp;
    private Transform environment;
    private Vector3 envStartPosition;
    private Vector3 avatarStartPosition;


    void Start()
    {
        //Fetch the Dropdown GameObject the script is attached to
        m_Dropdown = GetComponent<TMP_Dropdown>();
        //Clear the old options of the Dropdown menu
        //Add listener for when the value of the Dropdown changes, to take action
        m_Dropdown.onValueChanged.AddListener(delegate
        {
            DropdownValueChanged(m_Dropdown);
        });

        MenuPanel = GameObject.Find("Panels/SettingsPanel/Menu");
        environment = GameObject.Find("Env_3Avatar").transform;
        envStartPosition = environment.localPosition;
        Avatar1Pp = GameObject.Find("Animated Avatar").transform;
        avatarStartPosition = Avatar1Pp.localPosition;

    }

    //Ouput the new value of the Dropdown into Text
    void DropdownValueChanged(TMP_Dropdown change)
    {
        Debug.Log("PP prefs are " + change.value.ToString());
        if (change.value == 2)
        {
            PlayerPrefs.DeleteAll();

            float s = 1;
            environment.localScale = new Vector3(s, s, s);
            environment.localPosition = envStartPosition;
            Avatar1Pp.localPosition = avatarStartPosition;
            TMP_InputField szIF = GameObject.Find("Panels/SettingsPanel/Menu/EnvironmentSettings/Size").GetComponent<TMP_InputField>();
            szIF.text = s.ToString();
            PlayerPrefs.SetFloat("EnvironmentSize", s);
            float pd = 60f;
            PlayerPrefs.SetFloat("PupilDistance", pd);
            float cv = 5f;
            PlayerPrefs.SetFloat("Convergence", cv);
        }

        else if (change.value == 1)
        {
            MenuPanel.SetActive(true);

            //Set default values for dropdown menu
            TMP_Dropdown langDD = GameObject.Find("Panels/SettingsPanel/Menu/SessionSettings/Language").GetComponent<TMP_Dropdown>();
            langDD.value = PlayerPrefs.GetInt("LanguageCode");
            //langDD.RefreshShownValue(); //apparently it is not necessary to update the dropdown menu

            TMP_Dropdown runDD = GameObject.Find("Panels/SettingsPanel/Menu/SessionSettings/Run").GetComponent<TMP_Dropdown>();
            runDD.value = PlayerPrefs.GetInt("NRun");

            TMP_InputField idIF = GameObject.Find("Panels/SettingsPanel/Menu/SessionSettings/ID").GetComponent<TMP_InputField>();
            idIF.text = PlayerPrefs.GetString("ID");

            Transform environment = GameObject.Find("Env_3Avatar").transform;
            float envX = PlayerPrefs.GetFloat("EnvX");
            float envY = PlayerPrefs.GetFloat("EnvY");
            float envZ = PlayerPrefs.GetFloat("EnvZ");
            environment.localPosition = new Vector3(envX, envY, envZ);

            float envS = PlayerPrefs.GetFloat("EnvironmentSize");
            environment.localScale = new Vector3(envS, envS, envS);
            TMP_InputField szIF = GameObject.Find("Panels/SettingsPanel/Menu/EnvironmentSettings/Size").GetComponent<TMP_InputField>();
            szIF.text = envS.ToString();

            Debug.Log("Default Environment position is " + envX.ToString() + " " + envY.ToString() + " " + envZ.ToString());

            float CameraX = PlayerPrefs.GetFloat("CameraX");
            float CameraY = PlayerPrefs.GetFloat("CameraY");
            float CameraZ = PlayerPrefs.GetFloat("CameraZ");
            Avatar1Pp.localPosition = new Vector3(CameraX, CameraY, CameraZ);

            Debug.Log("Default Avatar1pp position is " + CameraX.ToString() + " " + CameraY.ToString() + " " + CameraZ.ToString());

            float PlatformY = PlayerPrefs.GetFloat("PlatformY");
            Transform platform = Avatar1Pp.Find("StartingPlatform").gameObject.transform;
            platform.localPosition = new Vector3(platform.localPosition.x, PlatformY, platform.localPosition.z);
        }
        MenuPanel.SetActive(true);
    }
}
