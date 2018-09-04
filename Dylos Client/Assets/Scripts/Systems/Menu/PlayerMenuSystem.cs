using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMenuSystem : MonoBehaviour
{
    public static PlayerMenuSystem Instance;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        isShown = bindMenu.activeSelf;
    }
    [SerializeField]
    GameObject bindMenu;
    bool isShown;
    public void ShowMenu(MenuInput.MenuType aType)
    {

        if (aType == MenuInput.MenuType.Options)
        {
            isShown = !isShown;
            bindMenu.SetActive(isShown);

            if (isShown)
            {
                StopCoroutine(InputSystem.Instance.mouseRot);
            }
            else
            {
                InputSystem.Instance.mouseRot = StartCoroutine(InputSystem.Instance.MouseRotation());
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

}
