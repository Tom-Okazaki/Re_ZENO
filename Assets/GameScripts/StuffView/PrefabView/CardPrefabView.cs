using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class CardPrefabView : MonoBehaviour
{
    private int No { get; set; }

    public IObservable<Unit> CardButtonObservable()
    {
        return this.GetComponent<Button>().OnClickAsObservable();
    }
    public IObservable<int> CardButtonAndNoObservable()
    {
        return GetComponent<Button>().OnClickAsObservable().Select(_ => No);
    }
    public void SetNo(int no)
    {
        No = no;
        var prefab = this.GetComponentInChildren<Image>().GetComponentInChildren<Text>();
        prefab.text = No.ToString();
    }
    public int GetNo()
    {
        return No;
    }
}
