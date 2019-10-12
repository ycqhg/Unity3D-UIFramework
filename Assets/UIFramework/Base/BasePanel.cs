using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    [HideInInspector]
    public CanvasGroup canvasGroup;

    public void InitCanvasGroup(bool isEffect = false)
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
        if (!isEffect)
        {

            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
        }
    }

    /// <summary>
    /// UI显示
    /// </summary>
    public virtual void OnEnter()
    {
        InitCanvasGroup();
    }

    /// <summary>
    /// UI暂停
    /// </summary>
    public virtual void OnPause()
    {
        //当弹出新的面板时，让当前层不再交互
        canvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// UI继续
    /// </summary>
    public virtual void OnResume()
    {
        canvasGroup.blocksRaycasts = true;
    }

    /// <summary>
    /// UI退出关闭
    /// </summary>
    public virtual void OnExit()
    {
        //关闭
        canvasGroup.alpha = 0;
        //不再交互
        canvasGroup.blocksRaycasts = false;

        //防止使用特效时，特效时间内也就是当前界面还没完全关闭，也可以点其他界面了，
        //当前界面完全退出时再激活上层
        UIManager.Instance.OnResumeImpl();
    }

    /// <summary>
    /// 关闭按钮
    /// </summary>
    public void OnClosePanel()
    {
        canvasGroup.blocksRaycasts = false;
        UIManager.Instance.PopPanel();
    }

    /// <summary>
    /// 打开按钮
    /// </summary>
    /// <param name="panelTypeString"></param>
    public void OnPushPanel(string panelTypeString)
    {
        UIPanelType panelType = (UIPanelType)System.Enum.Parse(typeof(UIPanelType), panelTypeString);
        UIManager.Instance.PushPanel(panelType);
    }

    #region DoTween封装
    protected void DOFadeEnter(float endValue, float duration, TweenCallback action = null)
    {
        InitCanvasGroup(true);
        var t = canvasGroup.DOFade(endValue, duration);
        TweenCallback(t, action);
        EnterCallback(t);
    }

    protected void DOFadeExit(float endValue, float duration, TweenCallback action = null)
    {
        var t = canvasGroup.DOFade(endValue, duration);
        TweenCallback(t, action);
        ExitCallback(t);
    }

    protected void DOLocalMoveXEnter(float transformX, float endValue, float duration, TweenCallback action = null)
    {
        InitCanvasGroup();
        //防止还没有完全弹出时点了界面上其他内容
        canvasGroup.blocksRaycasts = false;
        Vector3 tmp = transform.localPosition;
        tmp.x = transformX;
        transform.localPosition = tmp;
        var t = transform.DOLocalMoveX(endValue, duration);
        TweenCallback(t, action);
        EnterCallback(t);
    }

    protected void DOLocalMoveXExit(float endValue, float duration, TweenCallback action = null)
    {
        var t = transform.DOLocalMoveX(endValue, duration);
        TweenCallback(t, action);
        ExitCallback(t);
    }

    protected void DOScale(float endValue, float duration, bool isEnter = true, TweenCallback action = null)
    {
        if (isEnter)
        {
            InitCanvasGroup();
            transform.localScale = Vector3.zero;
            var t = transform.DOScale(endValue, duration);
            TweenCallback(t, action);
            EnterCallback(t);
        }
        else
        {
            //防止多次点击关闭按钮
            canvasGroup.blocksRaycasts = false;

            var t = transform.DOScale(endValue, duration);
            TweenCallback(t, action);
            ExitCallback(t);
        }
    }

    #region private Com
    private Tweener TweenCallback(Tweener t, TweenCallback action = null)
    {
        if (action != null)
        {
            return t.OnComplete(() => action());
        };
        return t;
    }
    private Tweener ExitCallback(Tweener t)
    {
        return t.OnComplete(() =>
         {
             //关闭
             canvasGroup.alpha = 0;
             //不再交互
             canvasGroup.blocksRaycasts = false;

             UIManager.Instance.OnResumeImpl();
         });
    }

    private Tweener EnterCallback(Tweener t)
    {
        return t.OnComplete(() =>
         {
             canvasGroup.alpha = 1;
             canvasGroup.blocksRaycasts = true;
         });
    }
    #endregion

    #endregion
}
