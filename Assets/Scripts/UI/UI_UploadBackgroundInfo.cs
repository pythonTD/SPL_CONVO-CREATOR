using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Creator.Class.Class_Setting;

public class UI_UploadBackgroundInfo : UISettingElement
{
    public Action<List<BackgroundData>> Action_Save;
    public Action<bool> Action_ShowLoading;

    private Button btn_upload;
    private GameObject uploaded_unit;

    private List<BackgroundData> uploaded_list = new List<BackgroundData>();
    private List<BG_Details> uploaded_list_obj = new List<BG_Details>();
    

    public UI_UploadBackgroundInfo(Transform t)
    {
        Main_Transform = t;
        GetChildren();
        AddListener();
    }

    public async void SetDataFromJson(List<BackgroundData> data)
    {
        foreach (var i in data)
        {
            _=CreateUnit(i.Name, i.OpenAI_FildId);
        }
        await Task.Yield();
        UpdateLayout();
    }


    #region ::: Override :::
    public override void Show()
    {
        base.Show();
        Main_Transform.gameObject.SetActive(true);
    }

    public override void Hide()
    {
        base.Hide();
        Main_Transform.gameObject.SetActive(false);
    }
    #endregion


    #region ::: Button :::

    private BackgroundData current = new BackgroundData();
    private async void BtnUpload()
    {
        
        (string file_name, byte[] bytes) = await OpenFile.Open();  //上傳
        if (bytes == null) { return; }
        Action_ShowLoading?.Invoke(true);

        bool is_override = false;
        BackgroundData old_data = uploaded_list.FirstOrDefault(x => x.Name == file_name);  //確認有沒有舊的
        
        string url = string.Empty;
        string file_id = string.Empty;
        
        string rename = $"{GameData.Class_Current.name}_{file_name}"; //used in SupabaseStorage

        if (old_data != null)
        {
            url = await CheckFileExistingAndDoing(rename, bytes);
            if (url != null) //overwrite
            {
                is_override = true;
            }
            else //cancel any action
            {
                Action_ShowLoading?.Invoke(false);
                return;
            }
        }
        else
        {
            url = await SupabaseStorageUploader.UploadDocument(rename, bytes);
        }

        if (is_override)
        {
            await ManagerAI_File.DELETE_FILE_WITH_ID(old_data.OpenAI_FildId);
            uploaded_list.Remove(old_data);
        }
        file_id = await ManagerAI_File.SEND_PDF_FILE(bytes, file_name);
        current = new BackgroundData()
        {
            Name = file_name,
            FileSize = bytes.Length,
            Supebase_URL = url,
            OpenAI_FildId = file_id
        };
        
        if (!is_override)
        {
            CreateUnit(file_name, file_id);
        }
        
        uploaded_list.Add(current);
        Action_Save?.Invoke(uploaded_list);
        Action_ShowLoading?.Invoke(false);
    }

    #endregion

    #region ::: Private Methods :::

    private void GetChildren()
    {
        btn_upload = Main_Transform.Find("upload_background/btn_upload").GetComponent<Button>();
        uploaded_unit = Main_Transform.Find("upload_background/uploaded_list/QUIZ_SETTING_UPLOADED_UNIT").gameObject;
        uploaded_unit.SetActive(false);
    }
    private void AddListener()
    {
        btn_upload.onClick.AddListener(BtnUpload);
    }
    private async Task CreateUnit(string name, string file_id)
    {
        GameObject unit = GameObject.Instantiate(uploaded_unit, uploaded_unit.transform.parent);
        TMP_Text text = unit.transform.Find("infomation/file_name").GetComponent<TMP_Text>();
        text.text = name;
        text.ForceMeshUpdate();
        unit.SetActive(true);

        BG_Details details = new BG_Details()
        {
            Name = name,
            Obj = unit
        };

        uploaded_list_obj.Add(details);
        Button btn_remove = unit.transform.Find("btn_remove").GetComponent<Button>();
        btn_remove.onClick.AddListener(async () =>
        {
            RemoveByName(name, file_id);
           
        });
        
        await Task.Yield();
        await Task.Yield();
        UpdateLayout();
    }

    private async Task<string> CheckFileExistingAndDoing(string file_name, byte[] new_file)
    {
        //check file byte size 
        int file_size = await SupabaseStorageUploader.GetFileBytesLen(file_name);
        if (file_size == 0)
        {
            Debug.Log("1");
            return null;
        }

        if (file_size == new_file.Length)
        {
            Debug.Log("2");
            return await SameNameAndSize();
        }
        else
        {
            Debug.Log("3");
            return await SameNameDiffSize(file_name, new_file);
        }
    }

    private async void RemoveByName(string name,string file_id)
    {
        Action_ShowLoading?.Invoke(true);
        string rename = $"{GameData.Class_Current.name}_{name}";
        await SupabaseStorageUploader.DeleteDocument(rename); ;
        await ManagerAI_File.DELETE_FILE_WITH_ID(file_id);
        
        BackgroundData data = uploaded_list.FirstOrDefault(x => x.Name == name);
        if (data != null) { uploaded_list.Remove(data); }
        Action_Save?.Invoke(uploaded_list);

        BG_Details details = uploaded_list_obj.FirstOrDefault(x => x.Name == name);
        if (details != null) { uploaded_list_obj.Remove(details); }
        GameObject.Destroy(details.Obj);

        await Task.Yield();
        await Task.Yield();
        UpdateLayout();
        
        Action_ShowLoading?.Invoke(false);
    }

    private void UpdateLayout()
    {
        Transform parentTransform = uploaded_unit.transform.parent;
        if (parentTransform == null) {return;}

        RectTransform parentRect = parentTransform as RectTransform;
        if (parentRect == null) {return;}

        LayoutGroup layoutGroup = parentRect.GetComponent<LayoutGroup>();
        if (layoutGroup != null)
        {
            layoutGroup.enabled = true;

            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.MarkLayoutForRebuild(parentRect);
            LayoutRebuilder.ForceRebuildLayoutImmediate(parentRect);
            Canvas.ForceUpdateCanvases();
        }
    }
    
    #endregion
    

    #region ::: pop-windows :::
    /// <summary>
    /// Same File Name But Different Byte Size
    /// </summary>
    private static async Task<string> SameNameDiffSize(string file_name, byte[] bytes)
    {
        TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
        
        string reminder = "Duplicate file name detected. The existing file in Supabase has been replaced.";
        ReminderBtnData btn01 = new ReminderBtnData()
        {
            BtnContent = "Overwrite File",
            BtnAction = async () =>
            {
                string result = await SupabaseStorageUploader.UploadDocument(file_name, bytes);
                tcs.SetResult(result);
            }
        };
        ReminderBtnData btn02 = new ReminderBtnData()
        {
            BtnContent = "Cancel",
            BtnAction = () =>
            {
                Debug.Log("Upload Canceled.");
                tcs.SetResult(null);
            }
        };

        List<ReminderBtnData> btns = new List<ReminderBtnData>();
        btns.Add(btn01);
        btns.Add(btn02);
        ReminderWindows.ShowWindow(reminder, btns);
        return await tcs.Task;
    }

    /// <summary>
    /// Same File Name And File Byte Size
    /// </summary>
    private static async Task<string> SameNameAndSize()
    {
        TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
        string reminder =
            "Detected a file in the cloud with the same filename and file size, so no action will be taken";
        ReminderBtnData btn01 = new ReminderBtnData()
        {
            BtnContent = "Okay",
            BtnAction = () =>
            {
                tcs.SetResult(null);
            }
        };
        List<ReminderBtnData> btns = new List<ReminderBtnData>();
        btns.Add(btn01);
        ReminderWindows.ShowWindow(reminder, btns);
        return await tcs.Task;
    }
    #endregion
    public class BG_Details
    {
        public string Name;
        public GameObject Obj;
    }
}
