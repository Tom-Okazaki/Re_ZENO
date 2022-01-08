using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hand : IHand
{
    List<int> _Hand = new List<int>();

    public void Add(int card)
    {
        _Hand.Add(card);
    }

    public int RemoveIndex(int index)
    {
        var removeCard = _Hand[index];
        _Hand.RemoveAt(index);
        return removeCard;
    }

    public void RemoveNo(int No)
    {
        _Hand.Remove(No);
    }
    public int ElementAt(int index)
    {
        return _Hand[index];
    }

    public string DebugLog()
    {
        var nowDeck = _Hand;
        var log = "";

        foreach (var content in nowDeck.Select((val, idx) => new { val, idx }))
        {
            if (content.idx == nowDeck.Count - 1)
                log += content.val.ToString();
            else
                log += content.val.ToString() + ", ";
        }

        Debug.Log(log);
        return log;
    }

    public bool CheckHaveCard(int No)
    {
        if (_Hand.IndexOf(No) == -1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
