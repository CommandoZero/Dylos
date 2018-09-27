using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DealerScript : MonoBehaviour {


    [SerializeField]
    List<GameObject> allCards;

    [SerializeField]
    int numberOfShuffles;
    private void Start()
    {
        DistributeCards(1);
    }
    

   
    void DistributeCards(int aPlayerNum)
    {
        for (int i = 0; i < numberOfShuffles; i++)
        {
            allCards.Shuffle();
        }
        
        List<GameObject> hand = new List<GameObject>();
        for (int i = 0; i < 13; i++)
        {
            hand.Add(allCards[i]);
        }
        PileScript.Instance.SetCardTransform(hand.ToArray(), hand.Count, HandScript.Instance.gameObject.transform);
    }
}
