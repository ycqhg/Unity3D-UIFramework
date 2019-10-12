using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskPanel : BasePanel
{
    /// <summary>
    /// 有特效要求，则重写
    /// </summary>
    public override void OnEnter()
    {
        DOFadeEnter(1, 0.5f);
    }

    /// <summary>
    /// 有特效要求，则重写
    /// </summary>
    public override void OnExit()
    {
        DOFadeExit(0, 0.5f);
    }
}
