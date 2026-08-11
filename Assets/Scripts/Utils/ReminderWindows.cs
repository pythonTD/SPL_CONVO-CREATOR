using System;
using System.Collections.Generic;
using UnityEngine;

public class ReminderWindows : MonoBehaviour
{
    private static bool showWindow = false; // 預設先關閉，等呼叫 ShowWindow 才開啟
    private static string gui_title = string.Empty;
    private static List<ReminderBtnData> btns = new List<ReminderBtnData>();

    private Rect windowRect = new Rect(0, 0, 800, 500);

    void Start()
    {
        // 只在 Start 中計算一次居中位置
        float rectX = (Screen.width - windowRect.width) / 2f;
        float rectY = (Screen.height - windowRect.height) / 2f;
        windowRect = new Rect(rectX, rectY, windowRect.width, windowRect.height);
    }

    /// <summary>
    /// 外部呼叫彈窗的靜態方法
    /// </summary>
    /// <param name="content">提示標題內文</param>
    /// <param name="buttonDatas">按鈕資料列表</param>
    public static void ShowWindow(string content, List<ReminderBtnData> buttonDatas)
    {
        gui_title = content;

        // 先清空舊按鈕，再加入新傳進來的按鈕！防止按鈕數量暴增
        btns.Clear();
        if (buttonDatas != null)
        {
            btns.AddRange(buttonDatas);
        }

        showWindow = true;
    }

    public void HideWindow()
    {
        showWindow = false;
        btns.Clear();
    }

    void OnGUI()
    {
        if (showWindow)
        {
            // ID 0, 代入內容繪製方法
            windowRect = GUILayout.Window(0, windowRect, DrawWindowContents, "");
        }
    }

    // 視窗內部的內容
    private void DrawWindowContents(int windowID)
    {
        GUI.skin.label.fontSize = 36; // 調整合適字型大小，避免 45 太大超出框外
        GUI.skin.button.fontSize = 28;
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;

        GUILayout.Space(20);
        GUILayout.Label(gui_title);
        GUILayout.FlexibleSpace();

        GUILayout.BeginHorizontal();
        for (int i = 0; i < btns.Count; i++)
        {
            // 修正：整理為單一 GUILayout.Height(80)
            if (GUILayout.Button(btns[i].BtnContent, GUILayout.Height(80)))
            {
                // 點擊後執行對應的操作，並關閉視窗
                btns[i].BtnAction?.Invoke();
                Close();
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(20);
        GUI.DragWindow(new Rect(0, 0, 10000, 100));
    }

    private void Close()
    {
        showWindow = false;
        btns.Clear();
    }
}

public class ReminderBtnData
{
    public string BtnContent;
    public Action BtnAction;
}