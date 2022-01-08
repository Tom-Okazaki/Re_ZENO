using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NineEffectUseCase
{
    IHand _enemyHand;
    IDeck _deck;
    IUsedBox _used;
    public NineEffectUseCase(IHand Enemyhand, IDeck deck, IUsedBox used)
    {
        _enemyHand = Enemyhand;
        _deck = deck;
        _used = used;
    }

    public int Activate(bool GuardFlug)
    {
        if (GuardFlug == false)
        {
            Debug.Log("nine effect Act");
            var nextCard = _deck.Draw();
            if (nextCard == 0)
            {
                Debug.Log("カードを引けないので９の効果が消滅しました");
                return 999;
            }
            _enemyHand.Add(nextCard);
            var Selectcard = _enemyHand.RemoveIndex(0);
            _used.Add(Selectcard);
            return Selectcard;
        }
        else
        {
            Debug.Log("nine effect Guarded");
            return 999;
        }
        
    }
}
