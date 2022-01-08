using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUsedBox
{
    void Add(int Card);
    bool ContainCheck(int cardNO);
}
