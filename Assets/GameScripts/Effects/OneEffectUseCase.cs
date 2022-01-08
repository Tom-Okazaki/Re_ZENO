using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class OneEffectUseCase
{
    IHand _EnemyHand;
    IDeck _Deck;
    IUsedBox _usedBox;
    
    WhichCard _nowSelect;
    private EffectView _effectView;
    private MoveEffectPresenter _moveEffectPresenter;
    
    private ReplaySubject<int> _finishSubject = new ReplaySubject<int>();

    public OneEffectUseCase(IHand EnemyHand,IDeck Deck,IUsedBox usedBox, 
        EffectView effectView, MoveEffectPresenter moveEffectPresenter)
    {
        _EnemyHand = EnemyHand;
        _Deck = Deck;
        _usedBox = usedBox;
        _effectView = effectView;
        _moveEffectPresenter = moveEffectPresenter;
    }

    public void Activate(bool GuardFlug,IObserver<Unit> enemyDrawObserver, Player player)
    {
        if (GuardFlug == true)
        {
            Debug.Log("One effect guarded");
            _finishSubject.OnNext(0);//intでなくなる Default
            return;
        }

        if (!_usedBox.ContainCheck(1))
        {
            Debug.Log("1回目の１が使用されました　効果なし");
            _finishSubject.OnNext(0);
            return;
        }

        Debug.Log("One Effect Act");
        
        enemyDrawObserver.OnNext(Unit.Default);

        Observable.FromCoroutine<int>(observer2 => _effectView.One_SelectRL(observer2,_EnemyHand))
            .Subscribe(select =>
            {
                _EnemyHand.RemoveNo(select);
                _usedBox.Add(select);
                _finishSubject.OnNext(select);
                _moveEffectPresenter.No1_DumpEnemyCard(player, select);
            });
    }

    public IObservable<int> FinishObservable()
    {
        return _finishSubject;
    }
}
