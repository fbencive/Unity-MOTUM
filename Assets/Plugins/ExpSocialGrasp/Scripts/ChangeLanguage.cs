using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeLanguage : MonoBehaviour
{
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
    void It2Eng()
    {
        PlayerPrefs.SetString("Language", "English");
    }

    public void DropdownValueChanged(TMP_Dropdown change)
    {
        Debug.Log("Language prefs are " + change.value.ToString());
        if (change.value == 1)
            PlayerPrefs.SetString("Language", "Italian");    
        else if (change.value == 2)
        PlayerPrefs.SetString("Language", "English");

        PlayerPrefs.SetInt("LanguageCode", change.value);   
    }
}
