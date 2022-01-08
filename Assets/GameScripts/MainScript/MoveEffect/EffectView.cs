using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class EffectView : MonoBehaviour//one effect view　にする
{
    [SerializeField] private GameObject oneSelectPanel;
    [SerializeField] private GameObject twoSelectionPanel;
    [SerializeField] private GameObject threeLookPanel;
    
    [SerializeField] private GameObject fiveSelectPanel;

    [SerializeField] private GameObject sevenSelectPanel;

    [SerializeField] private GameObject nineSelectPanel;

    private List<CardPrefabView> _oneSelection = new List<CardPrefabView>();//これ５個一個で良くね？//はいれつ
    private List<CardPrefabView> _twoSelection = new List<CardPrefabView>();
    private List<CardPrefabView> _fivePrefabs = new List<CardPrefabView>();
    private List<CardPrefabView> _sevenSelection = new List<CardPrefabView>();
    private List<CardPrefabView> _nineSelection = new List<CardPrefabView>();
    
    CompositeDisposable _disposables;

    private void Start()
    {
        _disposables = new CompositeDisposable();
        _oneSelection = oneSelectPanel.GetComponentsInChildren<CardPrefabView>().ToList();//配列で良くね？

        _twoSelection = twoSelectionPanel.GetComponentsInChildren<CardPrefabView>().ToList();
        for (var i = 0; i < _twoSelection.Count(); i++)
        {
            _twoSelection[i].SetNo(i + 1);
        }
    }

    public IEnumerator One_SelectRL(IObserver<int> observer, IHand enemyHand)
    //presenterがint(enemyHand)をSetNo, effectviewはどっちが選ばれたかIndexでかえす
    {
        Debug.Log("どちらのカードを捨てますか？");
        enemyHand.DebugLog();
        
        for(var i = 0; i  < _oneSelection.Count(); i++)
        {
            _oneSelection[i].SetNo(enemyHand.ElementAt(i));
        }
        oneSelectPanel.SetActive(true);
        var good = false;
        int selectInt = 0;
        OneSelectButtonObservable()
            .Subscribe(select =>
            {
                selectInt = select;
                good = true;
            }).AddTo(_disposables);
        
        yield return new WaitUntil(() => good);
        oneSelectPanel.SetActive(false);
        observer.OnNext(selectInt);

    }
    private IObservable<int> OneSelectButtonObservable()
    {
        return Observable.Merge(//全部あんぶううううううううううううううううううううううううううううううう
            _oneSelection
                .Select(item => item.CardButtonAndNoObservable()));
    }

    public IEnumerator Two_SelectNo(IObserver<int> observer)
    {
        Debug.Log("敵のカードを当てててください");
        twoSelectionPanel.SetActive(true);
        var good = false;
        int selectInt = 0;
        TwoSelectButtonObservable()
            .Subscribe(select =>
            {
                selectInt = select;
                good = true;
            }).AddTo(_disposables);
        
        yield return new WaitUntil(() => good);
        twoSelectionPanel.SetActive(false);
        observer.OnNext(selectInt);
    }
    private IObservable<int> TwoSelectButtonObservable()
    {
        return Observable.Merge(
                _twoSelection
                .Select(item => item.CardButtonAndNoObservable()));
    }

    public IEnumerator three_See(IObserver<Unit> observer, IHand enemyHand)
    {
        Debug.Log("敵のカードはこちらです");
        threeLookPanel.GetComponentInChildren<CardPrefabView>().SetNo(enemyHand.ElementAt(0));
        threeLookPanel.SetActive(true);
        yield return new WaitForSeconds(3);
        threeLookPanel.SetActive(false);
        observer.OnNext(Unit.Default);
    }

    public IEnumerator Five_Dump(IObserver<WhichCard> observer)
    {
        Debug.Log("左右どちらのカードを捨てさせますか？");
        var good = false;
        var Which = WhichCard.None;
        _fivePrefabs = fiveSelectPanel.GetComponentsInChildren<CardPrefabView>().ToList();
        fiveSelectPanel.SetActive(true);
        FiveSelectButtonObservable()
            .Subscribe(which =>
            {
                Which = WhichCard.Left;///////////////////////////////////////////////////////////////////////////
                good = true;
            }).AddTo(_disposables);
        
        yield return new WaitUntil(()=> good);
        fiveSelectPanel.SetActive(false);
        observer.OnNext(Which);
    }

    private IObservable<int> FiveSelectButtonObservable()
    {
        return Observable.Merge( //セレクトで[0][1]
            _fivePrefabs[0].CardButtonAndNoObservable(),
            _fivePrefabs[1].CardButtonAndNoObservable());
    }

    public IEnumerator Seven_Select(IObserver<int> observer, List<int> threeCards)
    {
        _sevenSelection = sevenSelectPanel.GetComponentsInChildren<CardPrefabView>().ToList();
        for (var i = 0; i < _sevenSelection.Count(); i++)
        {
            _sevenSelection[i].SetNo(threeCards[i]);
        }
        
        Debug.Log("手札に加えるカードを選んでください");
        var good = false;
        int selectCard = 0;
        sevenSelectPanel.SetActive(true);
        SevenSelectButtonObservable()
            .Subscribe(No =>
            {
                selectCard = No;
                sevenSelectPanel.SetActive(false);
                good = true;
            }).AddTo(_disposables);
        
        yield return new WaitUntil(() => good);
        observer.OnNext(selectCard);
    }

    private IObservable<int> SevenSelectButtonObservable()
    {
        return Observable.Merge(
            _sevenSelection
                .Select(prefab => prefab.CardButtonAndNoObservable()));
    }

    // public IEnumerator Nine_Select(IObserver<int> observer, List<int> enemyCard)
    // {
    //     _nineSelection = nineSelectPanel.GetComponentsInChildren<CardPrefabView>().ToList();
    //     for (var i = 0; i < 2; i++)
    //     {
    //         _nineSelection[i].SetNo(enemyCard[i]);
    //     }
    //     nineSelectPanel.SetActive(true);
    // }

    private IObservable<int> NineSelectButtonObservable()
    {
        return Observable.Merge(
            _nineSelection
                .Select(prefab => prefab.CardButtonAndNoObservable()));
    }

    public void Dispose()
    {
        _disposables.Dispose();
    }
}
