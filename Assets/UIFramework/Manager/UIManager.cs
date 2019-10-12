using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UIManager
{
    private static UIManager _instance;

    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new UIManager();
            }
            return _instance;
        }
    }

    private Transform canvasTransform;
    public Transform CanvasTransform
    {
        get
        {
            if (canvasTransform == null)
            {
                canvasTransform = GameObject.Find("Canvas").transform;
            }
            return canvasTransform;
        }
    }

    private UIManager()
    {
        ParseUIPanelTypeJson();
        panelStack = new Stack<BasePanel>();
    }

    #region 填充
    private Dictionary<UIPanelType, UIPanelTypeJsonInfo> panelPathDic;
    private Dictionary<UIPanelType, BasePanel> panelDic;
    private Stack<BasePanel> panelStack;

    public void PushPanel(UIPanelType panelType)
    {
        //如果栈里已经有界面
        if (panelStack.Count > 0)
        {
            BasePanel topPanel = panelStack.Peek();
            //则暂停
            topPanel.OnPause();
        }

        BasePanel panel = GetPanel(panelType);
        panel.OnEnter();
        panelStack.Push(panel);
    }

    public void PopPanel()
    {
        //如果栈里已经有界面
        if (panelStack.Count <= 0)
        {
            return;
        }
        //关闭栈顶界面的显示（当前界面）
        BasePanel topPanel = panelStack.Pop();
        topPanel.OnExit();
    }

    public void OnResumeImpl()
    {
        //如果上一层还有界面，则继续显示上一层的界面
        if (panelStack.Count <= 0)
        {
            return;
        }
        BasePanel top2Panel = panelStack.Peek();
        top2Panel.OnResume();

    }

    private BasePanel GetPanel(UIPanelType panelType)
    {
        if (panelDic == null)
        {
            panelDic = new Dictionary<UIPanelType, BasePanel>();
        }
        BasePanel panel = panelDic.GetValue(panelType);
        if (panel == null)
        {
            var item = panelPathDic.GetValue(panelType);
            GameObject newPanel = GameObject.Instantiate(Resources.Load(item.path), CanvasTransform, false) as GameObject;
            panel = newPanel.GetComponent<BasePanel>();
            panelDic.Add(panelType, panel);
        }
        return panel;
    }

    private void ParseUIPanelTypeJson()
    {
        panelPathDic = new Dictionary<UIPanelType, UIPanelTypeJsonInfo>();
        var ta = Resources.Load<TextAsset>("UIPanelType");
        var lst = JsonUtil.FromJson<List<UIPanelTypeJsonInfo>>(ta.text);
        foreach (var item in lst)
        {
            panelPathDic.Add((UIPanelType)Enum.Parse(typeof(UIPanelType), item.panelType), item);
        }
    }

    [Serializable]
    private class UIPanelTypeJsonInfo
    {
        public string panelType;

        public string path;
    }
    #endregion
}
