using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Input", menuName = "Input/Movement", order = 1)]
public class MovementInput : CustomInput
{
    public enum MoveType
    {
        Forward = 1,
        Backward = 2,
        Left = 4,
        Right = 8,
        Jump = 16,
        Slam = 32
    }
    public MoveType m_MoveType;

    public override void PerformKeyDownAction()
    {
        base.PerformKeyDownAction();
        MovementSystem.Instance.ReciveInputDown(m_MoveType);
    }

    public override void PerformKeyUpAction()
    {
        base.PerformKeyUpAction();
        MovementSystem.Instance.RecievInputUp(m_MoveType);
    }
}
