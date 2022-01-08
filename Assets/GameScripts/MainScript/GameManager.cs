using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;

public enum Player
{ P1, P2 }
public enum WhichCard
{Left, Right, None}

public class GameManager 
{
    private IDeck _deck;
    private IHand _player1Hand;
    private IHand _player2Hand;
    private IUsedBox _usedBox;
    private TurnUseCase _turnUseCase;
    private BeforeGame _beforeGame;
    private FinishUseCase _finishUseCase;
    private MoveMainPresenter _moveMainPresenter;
    private MoveEffectPresenter _moveEffectPresenter;
    private EffectView _effectView;

    bool Guard_P1 = false;
    bool Guard_P2 = false;
    bool Seven_P1 = false;
    bool Seven_P2 = false;
    
    public bool _gameFinish = false;

    private Player _nowPlayer = Player.P1;
    
    CompositeDisposable _disposables;
    public GameManager(
        IDeck deck, IHand player1Hand, IHand player2Hand, IUsedBox usedBox,
        BeforeGame beforeGame,FinishUseCase finishUseCase, MoveMainPresenter moveMainPresenter,
        MoveEffectPresenter moveEffectPresenter, EffectView effectView)
    {
        _deck = deck;
        _player1Hand = player1Hand;
        _player2Hand = player2Hand;
        _usedBox = usedBox;
        _beforeGame = beforeGame;
        _finishUseCase = finishUseCase;
        _moveMainPresenter = moveMainPresenter;
        _moveEffectPresenter = moveEffectPresenter;
        _effectView = effectView;
        
        _disposables = new CompositeDisposable();
    }
    public void StartByInitializer()
    {
        StartGame();
    }

    private void StartGame()
    {
        Debug.Log(DebugFirstDeck());
        _moveMainPresenter.InstantiateDeck(_deck.NowDeck());
        
        _beforeGame.Prepare(_player1Hand,_player2Hand);
        
        StartTurn(_nowPlayer);
    }

    private void StartTurn(Player nowPlayer)
    {
        if (_gameFinish)
        {
            return;
        }
        Debug.Log("Start Turn");
        if (_nowPlayer == Player.P1)
        {
            Guard_P1 = false;
        }
        else
        {
            Guard_P2 = false;
        }

        _turnUseCase = new TurnUseCase(_nowPlayer,NowPlayerHandAndEnemyHand()[0], NowPlayerHandAndEnemyHand()[1]
            , _deck, _usedBox, _moveMainPresenter,_moveEffectPresenter,_effectView);//毎回ニューしなくてよい
        _turnUseCase.OnEndTurnObservable()
            .Subscribe(_ =>
            {
                StartTurn(ChangeNowPlayer());
            }).AddTo(_disposables);
        _turnUseCase.FourGuardObservable()
            .Subscribe(player =>
            {
                Guard(player);
            }).AddTo(_disposables);
        _turnUseCase.SetThreeDrawSubject()
            .Subscribe(_ =>
            {
                SevenEffectThreeDraw(_nowPlayer);
            }).AddTo(_disposables);
        _turnUseCase.FinishEmpty()
            .Subscribe(hands =>
            {
                _gameFinish = true;
                _finishUseCase.FinishGameByEmptyDeck(hands[0], hands[1]);
            }).AddTo(_disposables);
        _turnUseCase.FinishGameByTwo()
            .Subscribe(_ =>
            {
                _gameFinish = true;
                _finishUseCase.FinishGameByTwo(_nowPlayer);
            }).AddTo(_disposables);
        _turnUseCase.FinishGameBySix()
            .Subscribe(Hands =>
            {
                _gameFinish = true;
                _finishUseCase.FinishGameBySix(Hands[0], Hands[1]);
            }).AddTo(_disposables);
        _turnUseCase.ChangeSevenFlagObservable()
            .Subscribe(_ =>
            {
                if (_nowPlayer == Player.P1)
                { Seven_P1 = false; }
                else
                { Seven_P2 = false; }
            }).AddTo(_disposables);
        _turnUseCase.FinishGameByNine()
            .Subscribe(_ =>
            {
                _gameFinish = true;
                _finishUseCase.FinishGameByNine(_nowPlayer);
            }).AddTo(_disposables);

        bool guard;
        bool threeDraw;
        if (_nowPlayer == Player.P1)
        {
            guard = Guard_P2;
            threeDraw = Seven_P1;
        }
        else
        {
            guard = Guard_P1;
            threeDraw = Seven_P2;
        }
        _turnUseCase.Begin(guard,threeDraw);
    }

    private List<IHand> NowPlayerHandAndEnemyHand()
    {
        if (_nowPlayer == Player.P1)
        {
            return new List<IHand>(){_player1Hand, _player2Hand};
        }
        else
        {
            return new List<IHand>(){_player2Hand, _player1Hand};
        }
    }

    public Player ChangeNowPlayer()
    {
        if (_nowPlayer == Player.P1)
        {
            _nowPlayer = Player.P2;
            return _nowPlayer;
        }
        else
        {
            _nowPlayer = Player.P1;
            return _nowPlayer;
        }
    }

    private string DebugFirstDeck()
    {
        var nowDeck = SendFirstDeck();
        var log = "";
        
        foreach (var content in nowDeck.Select((val, idx) => new { val, idx }))
        {
            if (content.idx == nowDeck.Count - 1)
                log += content.val.ToString();
            else
                log += content.val.ToString() + ", ";
        }
        return log;
    }
    private List<int> SendFirstDeck()
    {
        return _deck.Shuffle();
    }
    private int SendDeckCount()
    {
        return _deck.Count();
    }

    public void Guard(Player player)
    {
        if (player == Player.P1)
        {
            Guard_P1 = true;
        }
        else
        {
            Guard_P2 = true;
        }
    }

    private void SevenEffectThreeDraw(Player player)
    {
        if (player == Player.P1)
        {
            Seven_P1 = true;
        }
        else
        {
            Seven_P2 = true;
        }
    }

    public void SetGameFinish()
    {
        _gameFinish = true;
    }

    private IEnumerator DebugKeyDown()
    {
        var keyStream = Observable.EveryUpdate()
            .Select(_ => Input.inputString) 
            .Where(xs => Input.anyKeyDown)
            .Subscribe(xs=> Debug.Log(xs));
        Debug.Log("finish keyStream");
        return null;
    }
    private string DebugForTom(List<int> list)
    {
        var log = "";
        
        foreach (var content in list.Select((val, idx) => new { val, idx }))
        {
            if (content.idx == list.Count - 1)
                log += content.val.ToString();
            else
                log += content.val.ToString() + ", ";
        }
        return log;
    }
    public void Dispose()
    {
        _disposables.Dispose();
    }
}
