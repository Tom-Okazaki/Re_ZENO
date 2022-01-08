using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SixEffectUseCase
{
    IHand _myHand;
    IHand _enemyHand;
    IUsedBox _usedBox;

    public SixEffectUseCase(IHand MyHand,IHand EnemyHand, IUsedBox usedBox)
    {
        _myHand = MyHand;
        _enemyHand = EnemyHand;
        _usedBox = usedBox;
    }

    public bool Activate(bool GuardFlug)
    {
        if ((_usedBox.ContainCheck(6) && (!GuardFlug)))
        {
            Debug.Log("six effect Act");
            return true;
        }
        if(GuardFlug)
        {
            Debug.Log("six effect Guarded");
            return false;
        }
        else
        {
            Debug.Log("First six No Effect");
            return false;
        }
    }
}
