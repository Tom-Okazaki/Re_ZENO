using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : IDeck
{
    private List<int> Cards;
    List<int> SelectionList = new List<int>();

    public bool HasCard
    {
        get
        {
            if (Cards.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    public List<int> NowDeck()
    {
        return Cards;
    }

    public Deck()
    {
        Cards = new List<int> {1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 10};
    }

    public int Count()
    {
        return Cards.Count;
    }

    public List<int> Shuffle()
    {
        // for (var i = Cards.Count - 1; i > 1; i--)
        // {
        //     var rndNum = Random.Range(0, i);
        //     (Cards[rndNum], Cards[i]) = (Cards[i], Cards[rndNum]);
        // }
        return Cards;
    }
    public int Draw()
    {
        if (Cards.Count == 0)
        {
            return 0;
        }
        var nextCard = Cards[0];
        Cards.Remove(Cards[0]);
        //Debug.Log("NextCard: " + nextCard);
        return nextCard;
    }
    public List<int> SeeThreeFromHead()
    {
        SelectionList.Add(Cards[0]);
        SelectionList.Add(Cards[1]);
        SelectionList.Add(Cards[2]);
        return SelectionList;
    }
    public void SelectOf_3(int No)
    {
        Cards.Remove(No);
        SelectionList.Clear();
    }
}
