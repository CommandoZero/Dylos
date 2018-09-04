using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Input", menuName = "Input/Menu", order = 1)]
public class MenuInput : CustomInput
{
    public enum MenuType
    {
        MainMenu = 1,
        Inventory = 2,
        Options = 4,
        Lobby = 8,
        Other = 16
        
    }
    [SerializeField]
    MenuType m_MenuType;

    public override void PerformKeyDownAction()
    {
        base.PerformKeyDownAction();
        PlayerMenuSystem.Instance.ShowMenu(m_MenuType);
    }

}
