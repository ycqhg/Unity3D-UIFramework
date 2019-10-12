using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BagPanel : BasePanel
{
    void Awake()
    {
        //InitCanvasGroup(true);
    }

    public override void OnEnter()
    {
        //Vector3 tmp = transform.localPosition;
        //tmp.x = 600;
        //transform.localPosition = tmp;
        //canvasGroup.alpha = 1;
        //canvasGroup.blocksRaycasts = true;
        //transform.DOLocalMoveX(0, 0.5f).OnComplete(() => base.OnEnter());

        DOLocalMoveXEnter(600, 0, 3.5f);
    }

    /// <summary>
    /// 有特效要求，则重写
    /// </summary>
    public override void OnExit()
    {
        DOLocalMoveXExit(600, 3.5f);
    }
}
