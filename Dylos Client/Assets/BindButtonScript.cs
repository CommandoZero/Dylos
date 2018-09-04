using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BindButtonScript : MonoBehaviour
{

    [SerializeField]
    CustomInput m_Input;

    [SerializeField]
    Text m_TextToChange;

    [SerializeField]
    Text m_BindDescription;
    private void Start()
    {
        m_Input.m_BindText = m_TextToChange;
        m_Input.m_BindText.text = m_Input.m_Key.ToString();
        m_BindDescription.text = m_Input.m_InputName;      
    }
}
