using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class SevenEffectUseCase
{
    IHand _myHand;
    IDeck _deck;
    EffectView _effectView;
    public SevenEffectUseCase(IHand MyHand, IDeck deck, EffectView effectView)
    {
        _myHand = MyHand;
        _deck = deck;
        _effectView = effectView;
    }
    
    public void Activate(IObserver<int> observer)
    {
        List<int> SelectionInt = _deck.SeeThreeFromHead();
        
        Observable.FromCoroutine<int>(observer2 => _effectView.Seven_Select(observer2, SelectionInt))
            .Subscribe(No =>
            {
                _deck.SelectOf_3(No);
                _myHand.Add(No);
                _deck.Shuffle();
                observer.OnNext(No);
            });
    }
}
