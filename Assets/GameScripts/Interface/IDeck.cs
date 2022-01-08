using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDeck
{
    List<int> NowDeck();
    int Count();
    List<int> Shuffle();
    int Draw();
    List<int> SeeThreeFromHead();
    void SelectOf_3(int No);
}
