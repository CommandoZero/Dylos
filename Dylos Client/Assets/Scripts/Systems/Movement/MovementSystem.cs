using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSystem : MonoBehaviour
{

    public static MovementSystem Instance;
    Rigidbody m_PlayerRidg;
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        m_PlayerRidg = GetComponent<Rigidbody>();
        StartCoroutine(HorizontalMovement());
    }

    [SerializeField]
    float m_HorizontalSpeed;
    [SerializeField]
    float m_VerticalSpeed;

    [SerializeField]
    float
        forward,
        backward,
        left,
        right;

    public void ReciveInputDown(MovementInput.MoveType aMoveType)
    {
        if (aMoveType == MovementInput.MoveType.Forward)
        {
            Debug.Log("Forward");
            forward = m_VerticalSpeed;
        }
        else if (aMoveType == MovementInput.MoveType.Backward)
        {
            Debug.Log("Backward");
            backward = -m_VerticalSpeed;
        }
        else if (aMoveType == MovementInput.MoveType.Left)
        {
            Debug.Log("Left");
            left = -m_HorizontalSpeed;
        }
        else if (aMoveType == MovementInput.MoveType.Right)
        {
            Debug.Log("Right");
            right = m_HorizontalSpeed;
        }
    }
    public void RecievInputUp(MovementInput.MoveType aMoveType)
    {
        if (aMoveType == MovementInput.MoveType.Forward)
        {
            Debug.Log("Forward");
            forward = 0;
        }
        else if (aMoveType == MovementInput.MoveType.Backward)
        {
            Debug.Log("Backward");
            backward = 0;
        }
        else if (aMoveType == MovementInput.MoveType.Left)
        {
            Debug.Log("Left");
            left = 0;
        }
        else if (aMoveType == MovementInput.MoveType.Right)
        {
            Debug.Log("Right");
            right = 0;
        }
    }

    IEnumerator HorizontalMovement()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            Vector3 ver = transform.forward * (forward + backward);
            Vector3 hor = transform.right * (left + right);
           
            m_PlayerRidg.velocity = (ver + hor + Physics.gravity).normalized * 400 * Time.fixedDeltaTime;
        }
    }
}
