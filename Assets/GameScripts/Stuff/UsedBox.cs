using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsedBox : IUsedBox
{
    List<int> Cards = new List<int>();

    public void Add(int card)
    {
        Cards.Add(card);
    }

    public bool ContainCheck(int cardNO)
    {
        return Cards.Contains(cardNO);
    }
}
