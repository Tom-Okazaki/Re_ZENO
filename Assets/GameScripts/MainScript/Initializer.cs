using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    [SerializeField] private DeckView _deckView;
    [SerializeField] private P1_FieldView _p1FieldView;
    [SerializeField] private P2_FieldView _p2FieldView;
    [SerializeField] private P1_HandView _p1HandView;
    [SerializeField] private P2_HandView _p2HandView;
    [SerializeField] private CardPrefabView _cardPrefabView;
    [SerializeField] private EffectView _effectView;
    
    private IHand _player1Hand;
    private IHand _player2Hand;
    private IUsedBox _usedBox;
    private IDeck _deck;

    private GameManager _gameManager;
    private BeforeGame _beforeGame;
    private TurnUseCase _turnUseCase;
    private FinishUseCase _finishUseCase;
    private MoveMainPresenter _moveMainPresenter;
    private MoveEffectPresenter _moveEffectPresenter;

    private Subject<Unit> _startGameSubject;

    void Start()
    {
        _player1Hand = new Hand();
        _player2Hand = new Hand();
        _usedBox = new UsedBox();
        _deck = new Deck();

        _moveMainPresenter = new MoveMainPresenter(_cardPrefabView,_deckView,_p1FieldView,_p2FieldView,_p1HandView,_p2HandView);
        _moveEffectPresenter = new MoveEffectPresenter(_cardPrefabView, _deckView, _p1FieldView, _p2FieldView,
            _p1HandView, _p2HandView);
        _beforeGame = new BeforeGame(_moveMainPresenter,_deck);
        _finishUseCase = new FinishUseCase(_moveMainPresenter);
        _gameManager = new GameManager(_deck, _player1Hand, _player2Hand, _usedBox
            , _beforeGame, _finishUseCase ,_moveMainPresenter,_moveEffectPresenter,_effectView);

        _gameManager.StartByInitializer();
    }
    
    public IObservable<Unit> StartGameObservable()
    {
        return _startGameSubject;
    }

    private void Update()
    {
        
    }
    private void OnDestroy()
    {
        _gameManager.Dispose();
        _effectView.Dispose();
    }
}
