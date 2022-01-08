using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.WSA;

public class TurnUseCase//P1が主人公のプレゼンターと、P２の方も作りそれで発動することによってプレイヤー問題解決
{
    private Player _nowPlayer;
    private IHand MyHand { get; set; }
    private IHand EnemyHand { get; set; }
    private IDeck NowDeck { get; set; }
    private IUsedBox UsedBox { get; set; }
    
    private MoveMainPresenter _moveMainPresenter;
    private MoveEffectPresenter _moveEffectPresenter;//各ユースケースに教えて各ユースケースにやってもらう
    private EffectView _effectView;
    
    CompositeDisposable _disposables;

    private int _activateCard;

    private Subject<Unit> _finishTurnSubject = new Subject<Unit>();
    private Subject<Player> _fourGuardSubject = new Subject<Player>();
    private Subject<Unit> _setThreeDrawSubject = new Subject<Unit>();
    private Subject<IHand[]> _finishEmptySubject = new Subject<IHand[]>();
    private Subject<Unit> _successTwoSubject = new Subject<Unit>();//こんな感じの名前が良い
    private Subject<IHand[]> _finishGameBySixSubject = new Subject<IHand[]>();
    private Subject<Unit> _finishGameByNineSubject = new Subject<Unit>();

    public IObservable<Unit> OnEndTurnObservable() 
    { return _finishTurnSubject; }
    public IObservable<Player> FourGuardObservable()
    { return _fourGuardSubject; }
    public IObservable<Unit> SetThreeDrawSubject() 
    { return _setThreeDrawSubject; }
    public IObservable<IHand[]> FinishEmpty() 
    { return _finishEmptySubject; }
    public IObservable<Unit> FinishGameByTwo()
    { return _successTwoSubject;}
    public IObservable<IHand[]> FinishGameBySix()
    { return _finishGameBySixSubject; }
    public IObservable<Unit> FinishGameByNine()
    { return _finishGameByNineSubject; }

    private Subject<Unit> ChangeSevenFlagSubject = new Subject<Unit>();

    public IObservable<Unit> ChangeSevenFlagObservable()
    { return ChangeSevenFlagSubject; }

    private Subject<Unit> _enemyDrawSubject = new Subject<Unit>();//ユースケースでドローさせて、ここでサブスクライブ
    private Subject<int> _oneSuccessSubject = new Subject<int>();//ユースケイスでニューして、オブザーバブル公開、ターンユースケースでサブスク
    private Subject<bool> _twoSuccessSubject = new Subject<bool>();
    private Subject<Unit> _threeFinishSubject = new Subject<Unit>();
    private Subject<WhichCard> _fiveFinishSubject = new Subject<WhichCard>();
    private Subject<int> _sevenFinishSubject = new Subject<int>();
    private Subject<Unit> _eightEffectSubject = new Subject<Unit>();
    private Subject<Unit> _eightFinishSubject = new Subject<Unit>();

    public TurnUseCase(
        Player nowPlayer,
        IHand myHand, 
        IHand enemyHand,
        IDeck nowDeck,
        IUsedBox usedBox,
        MoveMainPresenter moveMainPresenter,
        MoveEffectPresenter moveEffectPresenter,
        EffectView effectView)
    {
        _nowPlayer = nowPlayer;
        MyHand = myHand;
        EnemyHand = enemyHand;
        NowDeck = nowDeck;
        UsedBox = usedBox;
        _moveMainPresenter = moveMainPresenter;
        _moveEffectPresenter = moveEffectPresenter;
        _effectView = effectView;
        
        _disposables = new CompositeDisposable();
    }

    public void Begin(bool guard,  bool threeDraw)//こるーちんにする　toYield Interaction
    {
        if (NowDeck.Count() != 0)
        {
            if (threeDraw)
            {
                SevenEffectUseCase sevE = new SevenEffectUseCase(MyHand, NowDeck, _effectView);
                _sevenFinishSubject
                    .Subscribe(No =>
                    {
                        _moveEffectPresenter.No7_selectCard(_nowPlayer, No);
                        ChangeSevenFlagSubject.OnNext(Unit.Default);
                        SelectActivateCoroutine(guard);
                    }).AddTo(_disposables);
                sevE.Activate(_sevenFinishSubject);
            }
            else
            {
                DrawAndAdd();
                SelectActivateCoroutine(guard);
            }
        }
        else
        {
            _finishEmptySubject.OnNext(new []{MyHand,EnemyHand});
        }

        _enemyDrawSubject.Subscribe(_ =>
        {
            EnemyDrawAndAdd();
        }).AddTo(_disposables);
    }
    

    private IEnumerator ActivateCard(int No, bool GuardFlug)
    {
        switch (No)
        {
            case 1:
                OneEffectUseCase oneE = new OneEffectUseCase(EnemyHand, NowDeck, UsedBox, _effectView, _moveEffectPresenter);
                oneE.Activate(GuardFlug,_enemyDrawSubject, _nowPlayer);
                
                var Hensuu = oneE.FinishObservable().First().ToYieldInstruction();
                yield return Hensuu;
                
                Debug.Log("1はOnNeXtされたよ");
                if (Hensuu.Result != 0)//movePresenter教えればFinishのみで分岐いらない
                {
                    _moveEffectPresenter.No1_DumpEnemyCard(_nowPlayer,Hensuu.Result);
                }
                FinishAndGoNextTurn();
                break;
            
            case 2:
                TwoEffectUseCase twoE = new TwoEffectUseCase(EnemyHand,_effectView);
                _twoSuccessSubject.Subscribe(success =>
                {
                    if (success)
                    {
                        _successTwoSubject.OnNext(Unit.Default);
                    }
                    else
                    {
                        FinishAndGoNextTurn();
                    }
                }).AddTo(_disposables);
                twoE.Activate(GuardFlug,_twoSuccessSubject);

                break;
            
            case 3:
                ThreeEffectUseCase thrE = new ThreeEffectUseCase(EnemyHand,_effectView);
                _threeFinishSubject.Subscribe(_ =>
                {
                    FinishAndGoNextTurn();
                }).AddTo(_disposables);
                thrE.Activate(GuardFlug, _threeFinishSubject);
                break;
            case 4:
                FourEffectUseCase fouE = new FourEffectUseCase();
                _fourGuardSubject.OnNext(_nowPlayer);
                FinishAndGoNextTurn();
                break;
            case 5:
                FiveEffectUseCase fivE = new FiveEffectUseCase(EnemyHand, UsedBox, NowDeck,_effectView);
                _fiveFinishSubject.Subscribe(which =>
                {
                    _moveEffectPresenter.No5_DumpEnemyCard(_nowPlayer,which);
                    FinishAndGoNextTurn();
                }).AddTo(_disposables);
                fivE.Activate(GuardFlug,_fiveFinishSubject,_enemyDrawSubject);
                break;
            case 6:
                SixEffectUseCase sixE = new SixEffectUseCase(MyHand, EnemyHand,UsedBox);
                var sixJudge = sixE.Activate(GuardFlug);
                if (sixJudge)
                {
                    _finishGameBySixSubject.OnNext(new []{MyHand,EnemyHand});
                }
                else
                {
                    FinishAndGoNextTurn();
                }
                break;
            case 7:
                _setThreeDrawSubject.OnNext(Unit.Default);
                FinishAndGoNextTurn();
                break;
            case 8:
                EightEffectUseCase eigE = new EightEffectUseCase(MyHand, EnemyHand);
                _eightEffectSubject.Subscribe(_ =>
                {
                    _moveMainPresenter.MoveActivateCard(_nowPlayer,_activateCard);
                    UsedBox.Add(_activateCard);
                    _moveEffectPresenter.No8_ChangeCard();
                    _finishTurnSubject.OnNext(Unit.Default);
                    
                }).AddTo(_disposables);
                _eightFinishSubject.Subscribe(_ =>
                {
                    FinishAndGoNextTurn(); 
                    
                }).AddTo(_disposables);
                
                eigE.Activate(GuardFlug, _eightEffectSubject, _eightFinishSubject);
                break;
            case 9:
                NineEffectUseCase ninE = new NineEffectUseCase(EnemyHand, NowDeck, UsedBox);
                var selectedCard = ninE.Activate(GuardFlug);
                if (selectedCard == 10)
                {
                   _finishGameByNineSubject.OnNext(Unit.Default);
                }
                else
                {
                    FinishAndGoNextTurn();
                }
                break;
        }
        UsedBox.Add(No);
    }

    private void FinishAndGoNextTurn()
    {
        _moveMainPresenter.MoveActivateCard(_nowPlayer,_activateCard);
        UsedBox.Add(_activateCard);
        _finishTurnSubject.OnNext(Unit.Default);
    }
    
    
    private IEnumerator SelectActivateCard()
    {
        yield return new WaitForSeconds(1);
        Debug.Log("使うカードを入力してください");
        var keyStream = Observable.EveryUpdate()
            .Select(_ => Input.inputString) 
            .Where(xs => Input.anyKeyDown)
            .Subscribe(xs=> _activateCard = int.Parse(xs));
        yield return new WaitUntil(() => Input.anyKeyDown);
        keyStream.Dispose();
    }

    private void SelectActivateCoroutine(bool guard)
    {
        Observable.FromCoroutine(SelectActivateCard)
            .Subscribe(_ =>
            {
                if (MyHand.CheckHaveCard(_activateCard))
                {
                    MyHand.RemoveNo(_activateCard);

                    Observable.FromCoroutine(__ => ActivateCard(_activateCard, guard))
                        .Subscribe(___ =>
                        {
                            
                        });

                }
                else
                {
                    Debug.Log("そのカードは所持していません。持っているカードから再入力してください");
                    SelectActivateCoroutine(guard);       
                }
            });
    }


    private void DrawAndAdd()
    {
        var card = NowDeck.Draw();
        MyHand.Add(card);
        _moveMainPresenter.MoveDrawCard(_nowPlayer,card);
        MyHand.DebugLog();
    }

    private void EnemyDrawAndAdd()
    {
        var card = NowDeck.Draw();
        EnemyHand.Add(card);
        Player nowEnemyPlayer;
        if (_nowPlayer == Player.P1)
        {
            nowEnemyPlayer = Player.P2;
        }
        else
        {
            nowEnemyPlayer = Player.P1;
        }
        _moveMainPresenter.MoveDrawCard(nowEnemyPlayer,card);
    }
}
