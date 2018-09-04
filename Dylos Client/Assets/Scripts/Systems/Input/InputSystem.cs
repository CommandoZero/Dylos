using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputSystem : MonoBehaviour
{
    //Instance of this singleton
    public static InputSystem Instance;
    
    private void Start()
    {
        
        //Setting the singleton
        if (Instance == null)
        {
            Instance = this;
        }
        for (int i = 0; i < m_CustomInputs.Count; i++)
        {
            m_Inputs.Add(m_CustomInputs[i].m_Key, m_CustomInputs[i]);
        }

        //We start asking for input
        StartCoroutine(InputReciever());

        mouseRot = StartCoroutine(MouseRotation());

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    #region MouseRotation

    public Coroutine mouseRot;
    public IEnumerator MouseRotation()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();

            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivity;
            rotationY += Input.GetAxis("Mouse Y") * sensitivity;
            rotationY = Mathf.Clamp(rotationY, -maximumY, maximumY);
            transform.localEulerAngles = new Vector3(0, rotationX, 0);
            Camera.main.transform.localEulerAngles = new Vector3(-rotationY, 0, 0);
            
        }
    }

    [SerializeField]
    float sensitivity;
    float rotationY = 0F;
    [SerializeField]
    float maximumY = 60F;


    #endregion
    //This'll be our list of inputs
    [SerializeField]
    List<CustomInput> m_CustomInputs;
    //This will be how we will link our inputs to keys
    Dictionary<KeyCode, CustomInput> m_Inputs = new Dictionary<KeyCode, CustomInput>();

    /*
     * So I need an input system that when I press a key, sends it through a process to see if that key is bound to an action.
     * If the key pressed isn't bound to an action, then we forget about it. Now we can use Keycodes, since that encompases all keys/mousebuttons.
     * We should have a custom input class, that has a Keycode, and a fucntion. A list of these can define all of the inputs we need (movement/abilities/etc)
     * I feel like we could use a dictionary at some point, so instead of cycling through input classes, we can just grab the input using the keycode key
     */
    #region Input Recording
    //We start recording an input

    public void RecordInput(CustomInput aInput)
    {
        StartCoroutine(InputRecorder(aInput));
    }
    //Coroutine for recording input
    public IEnumerator InputRecorder(CustomInput aInput)
    {
        StartRecording(aInput);
        bool recording = true;
        while (recording)
        {
            //Wait til we press a button
            yield return new WaitUntil(() => Input.anyKeyDown);
            foreach (KeyCode kcode in System.Enum.GetValues(typeof(KeyCode)))
            {
                //Get the key that we pressed
                if (Input.GetKeyDown(kcode))
                {
                    //If we record something that isnt picked up we try again
                    if (kcode == KeyCode.None)
                    {
                        break;
                    }
                    else if (kcode == KeyCode.Escape)
                    {
                        Record(aInput, KeyCode.None);
                    }
                    //If something is already bound to this key, we must change the Custom input and set the key on the original CustomInput to none
                    if (m_Inputs.ContainsKey(kcode))
                    {
                        Record(m_Inputs[kcode], KeyCode.None);
                        Record(aInput, kcode);
                    }
                    //Else if no input 
                    else
                    {
                        Record(aInput, kcode);
                    }
                    recording = false;
                }
            }
        }

    }

    delegate void RecordingInputLogic();
    //We need a function, that when we press a key in our recording coroutine:
    //Sets the key in the CustomInput class
    //Updates our m_Inputs dictionary
    //Updates the text which is in our bind menu
    RecordingInputLogic StartRecording(CustomInput aInput)
    {
        m_Inputs.Remove(aInput.m_Key);
        aInput.SetKey(KeyCode.None);
        aInput.m_BindText.text = "Recording...";
        return null;
    }
    RecordingInputLogic Record(CustomInput aInput, KeyCode aKey)
    {
        m_Inputs.Add(aKey, aInput);
        aInput.SetKey(aKey);
        aInput.m_BindText.text = aKey.ToString();
        return null;
    }

    #endregion

    //Coroutine that'll normally be taking in out input
    public IEnumerator InputReciever()
    {
        while (true)
        {
            //Wait until we press akey
            yield return new WaitUntil(() => Input.anyKeyDown);
            StopCoroutine(MouseRotation());
            foreach (KeyCode kcode in m_Inputs.Keys)
            {
                if (Input.GetKeyDown(kcode))
                {
                    m_Inputs[kcode].PerformKeyDownAction();
                    StartCoroutine(GetKeyUpInput(kcode));
                    continue;
                }
            }
            #region Getting Multiple Keys Down
            //if (Input.GetKey(KeyCode.LeftShift))
            //{
            //    while (true)
            //    {
            //        if (Input.GetKeyUp(KeyCode.LeftShift))
            //        {
            //            break;
            //        }
            //        yield return new WaitUntil(() => Input.anyKeyDown);
            //        if (Input.inputString.Length > 0)
            //            Debug.Log(Input.inputString);
            //    }
            //}
            #endregion
        }
    }

    IEnumerator GetKeyUpInput(KeyCode aKey)
    {
        yield return new WaitUntil(() => Input.GetKeyUp(aKey));
        m_Inputs[aKey].PerformKeyUpAction();
    }



    //Function used to print the inputs string
    void PrintInput(string aInput)
    {
        Debug.Log(aInput);
    }
}
