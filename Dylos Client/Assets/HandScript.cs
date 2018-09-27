using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class HandScript : MonoBehaviour
{

    public static HandScript Instance;


    [SerializeField]
    float cardDistance;


    public List<CardScript> selectedCards;

    CardScript tempCard;

    public bool passed = false;
    public bool turn = true;


    public void SetTurn(bool aStatus)
    {
        turn = aStatus;
    }
    public bool IsTurn()
    {
        return turn;
    }
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        tempCard = GetComponent<CardScript>();
        ChangeTurn();
    }




    public void AddToSelected(CardScript aCard, bool aStatus)
    {
        if (aStatus && !selectedCards.Contains(aCard))
        {
            selectedCards.Add(aCard);
            //highlight card
            iTween.MoveTo(aCard.gameObject, iTween.Hash("position", aCard.transform.position + new Vector3(0f, 0.1f, 0f), "speed", 1f));
        }
        else if (!aStatus && selectedCards.Contains(aCard))
        {
            //dehighlight card
            iTween.MoveTo(aCard.gameObject, iTween.Hash("position", aCard.transform.position - new Vector3(0f, 0.1f, 0f), "speed", 1f));
            selectedCards.Remove(aCard);
        }
        UpdatePlay();
        PileScript.Instance.UpdateButton(PileScript.Instance.playButton, PileScript.Instance.IsPlayable(currentPlay));
    }


    void UpdatePlay()
    {
        //Get our temp suit holder reset
        tempCard.ResetValues();
        //Order our cards in our list from least to greatest in number
        selectedCards.Sort((a, b) => a.m_Number.CompareTo(b.m_Number));
        //If we select one card
        if (selectedCards.Count == 1)
        {
            currentPlay.UpdatePlay(Play.PlayType.Single, selectedCards[0].m_Suit, selectedCards[0].m_Number, selectedCards.Count);
        }
        //If we select two cards
        else if (selectedCards.Count == 2)
        {
            //If they are the same number
            if (selectedCards[0].m_Number == selectedCards[1].m_Number)
            {
                foreach (CardScript card in selectedCards)
                {
                    if (card.m_Suit > tempCard.m_Suit)
                    {
                        tempCard.m_Suit = card.m_Suit;
                    }
                }
                currentPlay.UpdatePlay(Play.PlayType.Double, tempCard.m_Suit, selectedCards[selectedCards.Count - 1].m_Number, selectedCards.Count);
            }
            //Else its invalid
            else
            {
                currentPlay.UpdatePlay(Play.PlayType.Invalid, CardScript.Suit.Null, CardScript.Number.Null, selectedCards.Count);
            }
        }
        //If we select three cards
        else if (selectedCards.Count == 3)
        {
            //If they are all the same number
            if (selectedCards.All(a => a.m_Number == selectedCards.First().m_Number))
            {
                foreach (CardScript card in selectedCards)
                {
                    if (card.m_Suit > tempCard.m_Suit)
                    {
                        tempCard.m_Suit = card.m_Suit;
                    }
                }
                currentPlay.UpdatePlay(Play.PlayType.Triple, tempCard.m_Suit, selectedCards[selectedCards.Count - 1].m_Number, selectedCards.Count);
            }
            //If its a straight
            else if (!selectedCards.Select((a, b) => a.m_Number - b).Distinct().Skip(1).Any())
            {
                currentPlay.UpdatePlay(Play.PlayType.Straight, selectedCards[selectedCards.Count - 1].m_Suit, selectedCards[selectedCards.Count - 1].m_Number, selectedCards.Count);
            }
            //Else its invalid
            else
            {
                currentPlay.UpdatePlay(Play.PlayType.Invalid, CardScript.Suit.Null, CardScript.Number.Null, selectedCards.Count);
            }
        }
        //If we select four cards
        else if (selectedCards.Count == 4)
        {
            //If they are all the same
            if (selectedCards.All(a => a.m_Number == selectedCards.First().m_Number))
            {
                currentPlay.UpdatePlay(Play.PlayType.Bomb, CardScript.Suit.Hearts, selectedCards[selectedCards.Count - 1].m_Number, selectedCards.Count);
                Debug.Log("first");
            }
            //If its a straight
            else if (!selectedCards.Select((a, b) => a.m_Number - b).Distinct().Skip(1).Any())
            {
                currentPlay.UpdatePlay(Play.PlayType.Straight, selectedCards[selectedCards.Count - 1].m_Suit, selectedCards[selectedCards.Count - 1].m_Number, selectedCards.Count);
            }
            //Else its invalid
            else
            {
                currentPlay.UpdatePlay(Play.PlayType.Invalid, CardScript.Suit.Null, CardScript.Number.Null, selectedCards.Count);
            }
        }
        //Else if we've selected more than 4 cards and its even
        else if (selectedCards.Count > 4 && selectedCards.Count % 2 == 0)
        {
            //If its a straight
            if (!selectedCards.Select((a, b) => a.m_Number - b).Distinct().Skip(1).Any())
            {
                //fix this
                if (selectedCards.Count == 13)
                    currentPlay.UpdatePlay(Play.PlayType.LionsHead, selectedCards[selectedCards.Count - 1].m_Suit, selectedCards[selectedCards.Count - 1].m_Number, selectedCards.Count);
                else
                    currentPlay.UpdatePlay(Play.PlayType.Straight, selectedCards[selectedCards.Count - 1].m_Suit, selectedCards[selectedCards.Count - 1].m_Number, selectedCards.Count);
            }
            else
            {
                //We make two lists
                //One odds and the other evens
                List<int> odds = new List<int>();
                List<int> evens = new List<int>();
                for (int i = 0; i < selectedCards.Count; i++)
                {
                    if (i % 2 == 0)
                    {
                        evens.Add((int)selectedCards[i].m_Number);
                    }
                    else if (i % 2 == 1)
                    {
                        odds.Add((int)selectedCards[i].m_Number);
                    }
                }
                //So if we had a card number sequence like 1, 1, 2, 2, 3, 3
                //odds: 1, 2, 3
                //evens: 1, 2, 3
                //If both of these lists are the same numberwise, then you have a straight of pairs

                //If both our odds and even are straights
                if (!evens.Select((a, b) => a - b).Distinct().Skip(1).Any() && !odds.Select((a, b) => a - b).Distinct().Skip(1).Any())
                {

                    //If our odds equal our evens
                    if (evens.SequenceEqual(odds))
                    {
                        //Get the highest suit of the two
                        if (selectedCards[selectedCards.Count - 1].m_Suit > selectedCards[selectedCards.Count - 2].m_Suit)
                        {
                            currentPlay.UpdatePlay(Play.PlayType.Bomb, selectedCards[selectedCards.Count - 1].m_Suit, selectedCards[selectedCards.Count - 1].m_Number, selectedCards.Count);
                        }
                        else
                        {
                            currentPlay.UpdatePlay(Play.PlayType.Bomb, selectedCards[selectedCards.Count - 2].m_Suit, selectedCards[selectedCards.Count - 2].m_Number, selectedCards.Count);
                        }
                    }
                    //Else its invalid
                    else
                    {
                        currentPlay.UpdatePlay(Play.PlayType.Invalid, CardScript.Suit.Null, CardScript.Number.Null, selectedCards.Count);
                    }
                }
                //Else its invalid
                else
                {
                    currentPlay.UpdatePlay(Play.PlayType.Invalid, CardScript.Suit.Null, CardScript.Number.Null, selectedCards.Count);
                }
            }
        }
        //Else if we've selected more than 4 cards and its odd
        else if (selectedCards.Count > 4 && selectedCards.Count % 2 == 1)
        {
            //If its a straight
            if (!selectedCards.Select((a, b) => a.m_Number - b).Distinct().Skip(1).Any())
            {
                currentPlay.UpdatePlay(Play.PlayType.Straight, selectedCards[selectedCards.Count - 1].m_Suit, selectedCards[selectedCards.Count - 1].m_Number, selectedCards.Count);
            }
            //Else its invalid
            else
            {
                currentPlay.UpdatePlay(Play.PlayType.Invalid, CardScript.Suit.Null, CardScript.Number.Null, selectedCards.Count);
            }
        }
        else
        {
            currentPlay.UpdatePlay(Play.PlayType.Invalid, CardScript.Suit.Null, CardScript.Number.Null, selectedCards.Count);
        }
    }

    void SetCardTransform(GameObject[] aCard, int numberOfCards)
    {
        float xPos = (-0.1f * numberOfCards) + -0.1f;
        for (int i = 0; i < aCard.Length; i++)
        {
            aCard[i].transform.SetParent(transform);

            xPos += (float)decimal.Ceiling((decimal)((float)2.4 / 13) * 10) / 10;
            aCard[i].transform.position = new Vector3(xPos, transform.position.y, transform.position.z);

        }

    }
    public Play currentPlay = new Play();



    [System.Serializable]
    public class Play
    {

        public enum PlayType
        {
            Invalid = -1,
            Null = 0,
            Single = 1,
            Double = 2,
            Triple = 3,
            Bomb = 4,
            Straight = 5,
            QuadTwo = 6,
            LionsHead = 7
        }
        public PlayType type;
        public CardScript.Suit highestSuit;
        public CardScript.Number highestNumber;
        public int numberOfCards;

        public void UpdatePlay(PlayType aType, CardScript.Suit aSuit, CardScript.Number aNumber, int aAmount)
        {
            type = aType;
            highestSuit = aSuit;
            highestNumber = aNumber;
            numberOfCards = aAmount;
        }
    }



    public void ClearHand()
    {
        currentPlay.UpdatePlay(Play.PlayType.Null, CardScript.Suit.Null, CardScript.Number.Null, 0);
        selectedCards.Clear();
    }
    [SerializeField]
    float timeTilSkip;
    public Coroutine timer;


    public void ChangeTurn()
    {
        //This is a function that will start we'll have networked, so that when you pass/play, the next person's timer starts
        timer = StartCoroutine(TurnTimer(timeTilSkip));
    }

    IEnumerator TurnTimer(float aTime)
    {
        float timer = aTime;
        while (timer >= 0)
        {
            PileScript.Instance.UpdateButton(PileScript.Instance.passText, string.Format("Pass {0}", Mathf.Ceil(timer)));
            //PileScript.Instance.timerText.text = Mathf.Ceil(timer).ToString();
            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        // PileScript.Instance.timerText.text = "";
        PileScript.Instance.Pass(true);
    }
}
