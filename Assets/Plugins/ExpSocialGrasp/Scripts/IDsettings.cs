using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IDsettings : MonoBehaviour
{
    [SerializeField] private GameObject inputFieldRun;
    [SerializeField] private GameObject inputFieldID;
    TMP_Dropdown m_Dropdown;

    void Start()
    {
        //Fetch the Dropdown GameObject the script is attached to
        m_Dropdown = GetComponent<TMP_Dropdown>();
        //Clear the old options of the Dropdown menu
        //Add listener for when the value of the Dropdown changes, to take action
        m_Dropdown.onValueChanged.AddListener(delegate {
                    DropdownValueChanged(m_Dropdown);
        });
    }
    public void NRun(int nRun)
    {
        //int nRun_s = int.Parse(nRun);
        PlayerPrefs.SetInt("NRun", nRun+1);
    }

    void SubID()
    {
        PlayerPrefs.SetString("ID", inputFieldID.GetComponent<TMP_InputField>().text);
    }

    void DropdownValueChanged(TMP_Dropdown change)
    {
        Debug.Log("Run prefs are " + change.value.ToString());
        if (change.value != 0)
        PlayerPrefs.SetInt("NRun", change.value);
    }
}