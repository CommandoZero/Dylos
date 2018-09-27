using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PileScript : MonoBehaviour
{

    public static PileScript Instance;

    [SerializeField]
    float maxCardSize;

    [SerializeField]
    HandScript.Play m_currentPlay;


    public Button playButton;
    [SerializeField]
    Text playText;


    public Button passButton;
    public Text passText;


    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        playButton.onClick.AddListener(() => PlayHand(HandScript.Instance.currentPlay));

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {

        }
    }

    public delegate void PileLogic();

    PileLogic PlayHand(HandScript.Play aPlay)
    {
        UpdateButton(playButton, playText, false, "Played");
        UpdateButton(passButton, passText, false, "Pass");
        //Stop our skip timer
        StopCoroutine(HandScript.Instance.timer);


        //Reform our hand and pile

        //Get the gameobjects of the cards we selected in a list
        List<GameObject> aHand = new List<GameObject>();
        for (int i = 0; i < HandScript.Instance.selectedCards.Count; i++)
        {
            aHand.Add(HandScript.Instance.selectedCards[i].gameObject);
            //Make sure we cant touch these cards
            HandScript.Instance.selectedCards[i].Play();
        }
        //Pass that list into our transform setter 
        SetCardTransform(aHand.ToArray(), aHand.ToArray().Length, transform);

        //Reset the card objects
        aHand = new List<GameObject>();

        //Get the objects in our hand
        foreach (Transform child in HandScript.Instance.gameObject.transform)
        {
            aHand.Add(child.gameObject);
        }
        //Recenter the cards in our hand
        SetCardTransform(aHand.ToArray(), aHand.ToArray().Length, HandScript.Instance.gameObject.transform);
        //Update the current play on the pile
        m_currentPlay.UpdatePlay(aPlay.type, aPlay.highestSuit, aPlay.highestNumber, aPlay.numberOfCards);
        //Update our hand
        HandScript.Instance.ClearHand();
        HandScript.Instance.SetTurn(false);
        return null;
    }

    public void Pass(bool aStatus)
    {
        StopCoroutine(HandScript.Instance.timer);
        passButton.interactable = !aStatus;
        playButton.interactable = !aStatus;

        UpdateButton(playButton, playText, !aStatus, "Passed");
        UpdateButton(passText, "Pass");
        HandScript.Instance.SetTurn(!aStatus);

    }
    //Sets the place of the gameobjects
    public void SetCardTransform(GameObject[] aCard, int numberOfCards, Transform aTransform)
    {
        //Float we'll use to set the x position of our card objects
        float xPos = 0;
        //Formula for spacing the pile cards
        xPos = (-0.1f * numberOfCards) + -0.1f;


        //Loop that places each card in their new positions
        for (int i = 0; i < aCard.Length; i++)
        {
            aCard[i].transform.SetParent(aTransform);
            xPos += 0.2f;
            aCard[i].transform.position = new Vector3(xPos, aTransform.position.y, aTransform.position.z);
            if (aTransform != transform)
                aCard[i].GetComponentInParent<CardScript>().origin = new Vector3(aCard[i].transform.position.x, aCard[i].transform.position.y, aCard[i].transform.position.z);


        }
        //Set the pile cards size
        if (aTransform == transform)
            SetCardSize(numberOfCards);
    }

    //Sets the size of the gameobjects
    void SetCardSize(int numberOfCards)
    {
        //Resets the scale
        transform.localScale = new Vector3(1, 1, 1);
        //Set our maz size 
        float temp = maxCardSize;
        //The less cards we have, the larger the scale will be
        for (int i = 0; i < numberOfCards; i++)
        {
            temp -= (float)1 / 13;
        }
        //Sets the scale
        transform.localScale *= temp;
    }






    public void UpdateButton(Button aButton, bool aStatus)
    {
        aButton.interactable = aStatus;
    }
    public void UpdateButton(Button aButton, Text aText, bool aStatus, string aMessage)
    {
        aButton.interactable = aStatus;
        aText.text = aMessage;
    }
    public void UpdateButton(Text aText, string aMessage)
    {
        aText.text = aMessage;
    }



    public bool IsPlayable(HandScript.Play aPlay)
    {
        if (aPlay.type == HandScript.Play.PlayType.Invalid)
            return false;
        else if (m_currentPlay.type == HandScript.Play.PlayType.Null)
            return true;
        else if (aPlay.type == HandScript.Play.PlayType.QuadTwo)
            return true;
        else if (aPlay.type == HandScript.Play.PlayType.LionsHead)
            return true;
        else
        {
            #region Two's
            //If the highest number is two
            if (m_currentPlay.highestNumber == CardScript.Number.Two)
            {
                //If its a single
                if (m_currentPlay.type == HandScript.Play.PlayType.Single)
                {
                    //Do we have a bomb 
                    if (aPlay.type == HandScript.Play.PlayType.Bomb && (aPlay.numberOfCards == 6 || aPlay.numberOfCards == 4))
                    {
                        return true;
                    }
                    //Do we have a higher two
                    else if (aPlay.type == HandScript.Play.PlayType.Single && aPlay.highestNumber == CardScript.Number.Two
                        && aPlay.highestSuit > m_currentPlay.highestSuit)
                    {
                        return true;
                    }
                    //Else its not playable
                    else
                    {
                        return false;
                    }
                }
                //If its a double
                else if (m_currentPlay.type == HandScript.Play.PlayType.Double)
                {
                    //Do we have a bomb
                    if (aPlay.type == HandScript.Play.PlayType.Bomb && aPlay.numberOfCards == 8)
                    {
                        return true;
                    }
                    //Do we have a higher pair
                    else if (aPlay.type == HandScript.Play.PlayType.Double && aPlay.highestNumber == CardScript.Number.Two
                        && aPlay.highestSuit > m_currentPlay.highestSuit)
                    {
                        return true;
                    }
                    //Else its not playable
                    else
                    {
                        return false;
                    }
                }
                //If its a triple
                else if (m_currentPlay.type == HandScript.Play.PlayType.Triple)
                {
                    //Do we have a bomb
                    if (aPlay.type == HandScript.Play.PlayType.Bomb && aPlay.numberOfCards == 10)
                        return true;
                    //Else its not playable
                    else
                        return false;
                }
                //If its in a straight, we dont allow that, maybe a rule later on
                else
                {
                    return false;
                }
            }
            #endregion
            else
            {
                //If the play types match
                if (m_currentPlay.type == aPlay.type)
                {
                    #region Straights
                    //If we are playing straights
                    if (m_currentPlay.type == HandScript.Play.PlayType.Straight)
                    {
                        //Do we have a straight that has the same amount of cards
                        if (m_currentPlay.numberOfCards == aPlay.numberOfCards)
                        {
                            //If we have a higher number
                            if (m_currentPlay.highestNumber < aPlay.highestNumber)
                            {
                                return true;
                            }
                            //If we have a lower number 
                            else if (m_currentPlay.highestNumber > aPlay.highestNumber)
                            {
                                return false;
                            }
                            //If the highest number is the same
                            else
                            {
                                //Do we have a higher suit
                                if (m_currentPlay.highestSuit > aPlay.highestSuit)
                                    return false;
                                else
                                    return true;
                            }

                        }
                        //Else its not playable
                        else
                        {
                            return false;
                        }
                    }
                    #endregion
                    #region Singles
                    //If we are playing singles
                    else if (m_currentPlay.type == HandScript.Play.PlayType.Single)
                    {
                        //If we have a higher number
                        if (m_currentPlay.highestNumber < aPlay.highestNumber)
                        {
                            return true;
                        }
                        // If we have a lower number
                        else if (m_currentPlay.highestNumber > aPlay.highestNumber)
                        {
                            return false;
                        }
                        //If the highest number is the same
                        else
                        {
                            //Do we have a higher suit
                            if (m_currentPlay.highestSuit > aPlay.highestSuit)
                                return false;
                            else
                                return true;
                        }

                    }
                    #endregion
                    #region Doubles
                    //If we are playing doubles
                    else if (m_currentPlay.type == HandScript.Play.PlayType.Double)
                    {
                        //If we have a higher number
                        if (m_currentPlay.highestNumber < aPlay.highestNumber)
                        {
                            return true;
                        }
                        // If we have a lower number
                        else if (m_currentPlay.highestNumber > aPlay.highestNumber)
                        {
                            return false;
                        }
                        //If the highest number is the same
                        else
                        {
                            //Do we have a higher suit
                            if (m_currentPlay.highestSuit > aPlay.highestSuit)
                                return false;
                            else
                                return true;
                        }

                    }
                    #endregion
                    #region Triples
                    //If we are playing triples
                    else if (m_currentPlay.type == HandScript.Play.PlayType.Triple)
                    {
                        //If we have a higher number
                        if (m_currentPlay.highestNumber < aPlay.highestNumber)
                        {
                            return true;
                        }
                        // If we have a lower number
                        else
                        {
                            return false;
                        }
                    }
                    #endregion
                    //If we have some error/not playable
                    else
                    {
                        return false;
                    }
                }
                //If the play types are different
                else
                {
                    return false;
                }
            }
        }
    }

}