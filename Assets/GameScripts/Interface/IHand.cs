using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHand
{
    void Add(int card);
    int RemoveIndex(int index);
    void RemoveNo(int No);
    int ElementAt(int index);
    string DebugLog();
    bool CheckHaveCard(int No);
}
