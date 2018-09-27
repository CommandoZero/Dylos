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
    [HideInInspector]
    public Vector3 origin;

    bool switchable = false;
    bool interactable = true;

    private void Start()
    {
        origin = new Vector3(transform.position.x, transform.position.y, transform.position.z);
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

    CardLogic SwitchCard(Transform aTransform, CardScript aCard)
    {
        Vector3 temp = new Vector3(origin.x, origin.y, origin.z);
        origin = new Vector3(aTransform.position.x, aTransform.position.y, aTransform.position.z);
        aCard.origin = temp;
        aCard.transform.position = aCard.origin;

        return null;
    }
    [SerializeField]
    CardScript tempCard;

    private void OnMouseDrag()
    {
        if (!HandScript.Instance.IsTurn())
        {
            Vector3 temp = Input.mousePosition;
            temp.z = 1.3f;
            transform.position = Camera.main.ScreenToWorldPoint(temp);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, new Vector3(0, 0, 10), out hit, 1f))
            {
                if (hit.transform.gameObject.GetComponent<CardScript>())
                {
                    tempCard = hit.transform.gameObject.GetComponent<CardScript>();
                    switchable = true;
                }
            }
            else
            {
                tempCard = null;
                switchable = false;
            }
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
        if (!HandScript.Instance.IsTurn())
        {
            if (switchable)
            {
                SwitchCard(tempCard.gameObject.transform, tempCard);
            }
            transform.position = origin;
        }

    }



    public void ResetValues()
    {
        m_Suit = Suit.Null;
        m_Number = Number.Null;
    }
}
