using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeforeGame
{
    private MoveMainPresenter _moveMainPresenter;
    private IDeck _deck;
    public BeforeGame(
        MoveMainPresenter moveMainPresenter,
        IDeck deck
        )
    {
        _moveMainPresenter = moveMainPresenter;
        _deck = deck;
    }
    
    public void Prepare(IHand player1Hand, IHand player2Hand)
    {
        DrawBothPlayer(player1Hand,player2Hand);
    }
    public void DrawBothPlayer(IHand player1Hand, IHand player2Hand)
    {
        Debug.Log("Before DrawBoth");
        DrawAndAdd(Player.P1,player1Hand);
        DrawAndAdd(Player.P2,player2Hand);
    }
    private void DrawAndAdd(Player player,IHand hand)
    {
        var card = _deck.Draw();
        hand.Add(card);
        _moveMainPresenter.MoveDrawCard(player,card);
        hand.DebugLog();
    }
}
