using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class CustomInput : ScriptableObject
{


    public string m_InputName;
    enum InputType
    {
        Movement = 1,
        Ability = 2,
        Menu = 4,

    }
    [SerializeField]
    InputType m_Type;
    public KeyCode m_Key;

    public void SetKey(KeyCode aKey)
    {
        m_Key = aKey;
    }

    public Text m_BindText;

    public virtual void PerformKeyDownAction() { }
    public virtual void PerformKeyUpAction() { }

}
