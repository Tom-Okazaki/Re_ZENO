using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishUseCase
{
    private MoveMainPresenter _moveMainPresenter;
    public FinishUseCase(
        MoveMainPresenter moveMainPresenter)
    {
        _moveMainPresenter = moveMainPresenter;
    }
    
    public void FinishGameByEmptyDeck(IHand myHand, IHand enemyHand)
    {
        Debug.Log("デッキがなくなりました！勝敗判定します");
        if (myHand.ElementAt(0) < enemyHand.ElementAt(0))
        {
            Debug.Log("player2のかち！" + myHand.ElementAt(0) + " : " + enemyHand.ElementAt(0));
        }
        else if (myHand.ElementAt(0) > enemyHand.ElementAt(0))
        {
            Debug.Log("player1のかち！" + myHand.ElementAt(0) + " : " + enemyHand.ElementAt(0));
        }
        else
        {
            Debug.Log("ひきわけ！" + myHand.ElementAt(0) + " : " + enemyHand.ElementAt(0));
        }
    }

    public void FinishGameByTwo(Player player)
    {
        Debug.Log("Win : " + player);
    }

    public void FinishGameBySix(IHand myHand, IHand enemyHand)
    {
        if (myHand.ElementAt(0) < enemyHand.ElementAt(0))
        {
            Debug.Log("player2のかち！" + myHand.ElementAt(0) + " : " + enemyHand.ElementAt(0));
        }
        else if (myHand.ElementAt(0) > enemyHand.ElementAt(0))
        {
            Debug.Log("player1のかち！" + myHand.ElementAt(0) + " : " + enemyHand.ElementAt(0));
        }
        else
        {
            Debug.Log("ひきわけ！" + myHand.ElementAt(0) + " : " + enemyHand.ElementAt(0));
        }
    }

    public void FinishGameByNine(Player player)
    {
        Debug.Log("Win : " + player);
    }
}
