using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class MainMenuScript : MonoBehaviour
{

    //Main Menu Script that controls all of the UI shown to the player

    //Each menu should go down a path
    //Play > Find Game or Create Lobby
    //Find Game > Queue UI
    //Create Lobby > Lobby Customization UI
    //When I hit back or esc it should go to the previous UI window

    //UI path
    [System.Serializable, ]
    class UIState 
    {
        [SerializeField]       
        List<GameObject> UIPath = new List<GameObject>();
        [SerializeField]
        int currentPathIndex;
    }
    //The list of each UI path
    [SerializeField]
    List<UIState> m_states = new List<UIState>();
    //Whats currently on our screen
    [SerializeField]
    UIState currentState;


    void NextUIState()
    {

    }
    void PreviousUIState()
    {

    }

    public void ShowPlayUI()
    {
        //Shows the Find Game & Create Lobby UI
    }
    public void ShowFindGameUI(bool aBool)
    {

    }

    public void CreateLobby()
    {

    }



}
