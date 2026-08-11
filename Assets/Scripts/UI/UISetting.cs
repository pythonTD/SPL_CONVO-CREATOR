using UnityEngine;
using System;
using System.Collections.Generic;
using static Creator.Class;

[Serializable]
public class UISetting
{
    public struct ELEMENTS
    {
        public static GameObject UPLOAD_BACKGROUND => Resources.Load<GameObject>("Prefabs/Canvas_003_Setting/QUIZ_SETTING_UPLOAD");
    }
    public Transform transform = null;
    private UI_UploadBackgroundInfo ui_UploadBackgroundInfo = null;
    private GameObject loading = null;
    private Transform list_parent;
    private UISettingElement current = null;
    private Class_Setting classSetting = new Class_Setting();
    public void Awake()
    {
        classSetting = new Class_Setting();
        loading = transform.parent.Find("loading").gameObject;
        list_parent = transform.Find("list");
        transform.gameObject.SetActive(false);

    }
    public void Show(SETTING_MODE mode)
    {
        transform.gameObject.SetActive(true);
        loading.SetActive(false);
        HideCurrent();
        switch (mode)
        {
            case SETTING_MODE.UPLOAD_BACKGROUND: Show_Upload_Background(); break;
        }
    }

    public void Quit()
    {
        HideCurrent();
        transform.gameObject.SetActive(false);
        loading.SetActive(false);
    }


    #region ::: Show Info ::: 
    private void HideCurrent()
    {
        if (current != null)
        {
            current.Hide();
            current = null;
        }
    }
    private void Show_Upload_Background()
    {
        if (ui_UploadBackgroundInfo == null)
        {
            var obj = GameObject.Instantiate(ELEMENTS.UPLOAD_BACKGROUND, list_parent);
            ui_UploadBackgroundInfo = new UI_UploadBackgroundInfo(obj.transform);
            ui_UploadBackgroundInfo.SetDataFromJson(GameData.Class_Current.ClassSetting.Quiz_Background_Info);
            current = ui_UploadBackgroundInfo;
            
            ui_UploadBackgroundInfo.Action_Save += SaveJsonData_Background;
            ui_UploadBackgroundInfo.Action_ShowLoading += ShowLoading;
        }
        else
        {
            current = ui_UploadBackgroundInfo;
        }

        current.Show();
        ManagerApp.Select_Setting(current);
    }
    #endregion

    #region ::: Save To Json

    public void SaveJsonData_Background(List<Class_Setting.BackgroundData> data)
    {
        Debug.Log("SaveJsonData_Background="+data.Count);
        GameData.Class_Current.ClassSetting.Quiz_Background_Info = data;
        GameData.Save_All();
    }

    #endregion

    private void ShowLoading(bool is_show)
    {
        loading.SetActive(is_show);
    }
}
public enum SETTING_MODE
{
    NONE,
    UPLOAD_BACKGROUND
}

public class UISettingElement
{
    public Transform Main_Transform;
    public virtual void Show() { }
    public virtual void Hide() { }
}