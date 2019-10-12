using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ItemMsgBoxPanel : BasePanel
{
    public override void OnEnter()
    {
        DOScale(1, 0.5f);
    }

    public override void OnExit()
    {
        DOScale(0, 0.3f, false);
    }
}
