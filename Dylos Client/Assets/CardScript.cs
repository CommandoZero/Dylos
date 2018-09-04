using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript : MonoBehaviour
{

    public enum Suit
    {
        Null = 0,
        Spades = 1,
        Clubs = 2,
        Diamonds = 3,
        Hearts = 4
    }

    public Suit m_Suit;

    public enum Number
    {
        Null = 0,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Jack = 11,
        Queen = 12,
        King = 13,
        Ace = 14,
        Two = 15
    }

    public Number m_Number;

    [SerializeField]
    bool isSelected;

    Vector3 origin;

    bool switchable = false;
    bool interactable = true;

    private void Start()
    {
        origin = transform.position;
    }


    public delegate void CardLogic();

    CardLogic UpdateCard(bool aStatus)
    {
        HandScript.Instance.AddToSelected(this, aStatus);
        return null;
    }

   public CardLogic Play()
    {
        interactable = false;
        return null;
    }
    private void OnMouseDrag()
    {
        if (!HandScript.Instance.IsTurn())
        {
            Vector3 temp = Input.mousePosition;
            temp.z = transform.position.z - Camera.main.transform.position.z;
            if (temp.y - origin.y > 2)
                transform.position = Camera.main.ScreenToWorldPoint(temp);
        }

    }
    private void OnMouseDown()
    {
        if (HandScript.Instance.IsTurn())
        {
            if (interactable)
            {
                isSelected = !isSelected;
                UpdateCard(isSelected);
            }

        }
    }

    private void OnMouseUp()
    {
        if (switchable)
        {

        }
    }

    private void OnCollisionStay(Collision hit)
    {
        if (hit.gameObject.GetComponent<CardScript>())
        {

        }
    }

    public void ResetValues()
    {
        m_Suit = Suit.Null;
        m_Number = Number.Null;
    }
}
