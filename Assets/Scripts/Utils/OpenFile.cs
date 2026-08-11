using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Security.Cryptography;


#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

public static class OpenFile
{
    private static int limit_size = 3;

#if UNITY_WEBGL && !UNITY_EDITOR

    [DllImport("__Internal")]
    private static extern void TriggerPdfPicker(string objectName, string methodName);

    private class WebGLPickerBridge : MonoBehaviour
    {
        public TaskCompletionSource<(string fileName, byte[] data)> Tcs;

        public void OnPdfSelected(string rawResult)
        {
            if (string.IsNullOrEmpty(rawResult) || rawResult == "ERROR_SIZE")
            {
                Tcs?.TrySetResult((null, null));
                Destroy(gameObject);
                return;
            }

            try
            {
                string fileName = "";
                string base64Data = rawResult;

                if (rawResult.Contains("|"))
                {
                    string[] parts = rawResult.Split(new char[] { '|' }, 2);
                    fileName = parts[0];
                    base64Data = parts[1];
                }

                if (!string.IsNullOrEmpty(fileName) && !ConfirmPdfFileName(fileName))
                {
                    string reminder = "File name can only contain letters, numbers, and underscores.";
                    ShowReminderWindow(reminder);
                    Tcs?.TrySetResult((null, null));
                    Destroy(gameObject);
                    return;
                }

                if (base64Data.StartsWith("data:"))
                {
                    int commaIndex = base64Data.IndexOf(',');
                    if (commaIndex >= 0)
                    {
                        base64Data = base64Data.Substring(commaIndex + 1);
                    }
                }

                base64Data = base64Data.Trim().Replace("\r", "").Replace("\n", "");
                byte[] bytes = Convert.FromBase64String(base64Data);

                if (bytes.Length > limit_size * 1024 * 1024)
                {
                    string reminder = $"The file size exceeds the {limit_size}MB limit!";
                    ShowReminderWindow(reminder);
                    Tcs?.TrySetResult((null, null));
                    Destroy(gameObject);
                    return;
                }
                Tcs?.TrySetResult((fileName, bytes));
            }
            catch (Exception e)
            {
                Debug.LogError($"Base64 Decode Error: {e.Message}");
                Tcs?.TrySetResult((null, null));
            }
            finally
            {
                Destroy(gameObject);
            }
        }
    }
#endif

    public static async Task<(string fileName, byte[] data)> Open()
    {
#if UNITY_EDITOR
        string path = EditorUtility.OpenFilePanel($"Please select a PDF file (Max {limit_size}MB).", "", "pdf");
        if (string.IsNullOrEmpty(path))
        {
            return (null, null);
        }

        string fileName = Path.GetFileName(path);

        if (!ConfirmPdfFileName(fileName))
        {
            string reminder = "File name can only contain letters, numbers, and underscores.";
            ShowReminderWindow(reminder);
            return (null, null);
        }

        byte[] bytes = File.ReadAllBytes(path);
        long maxSizeBytes = limit_size * 1024 * 1024;

        if (bytes.Length > maxSizeBytes)
        {
            string reminder = $"The file size exceeds the {limit_size}MB limit!";
            ShowReminderWindow(reminder);
            return (null, null);
        }
        return (fileName, bytes);

#elif UNITY_WEBGL

        var tcs = new TaskCompletionSource<(string fileName, byte[] data)>();

        GameObject bridgeObject = new GameObject("[WebGLFilePickerBridge]");
        UnityEngine.Object.DontDestroyOnLoad(bridgeObject);
        var bridge = bridgeObject.AddComponent<WebGLPickerBridge>();
        bridge.Tcs = tcs;

        TriggerPdfPicker(bridgeObject.name, "OnPdfSelected");

        return await tcs.Task;

#else
        await Task.Yield();
        return (null, null);
#endif
    }

    private static bool ConfirmPdfFileName(string filePath)
    {
        if (string.IsNullOrEmpty(filePath)) return false;

        string pattern = @"^[a-zA-Z0-9_]+\.pdf$";
        string fileName = Path.GetFileName(filePath);

        return Regex.IsMatch(fileName, pattern, RegexOptions.IgnoreCase);
    }

    private static void ShowReminderWindow(string reminder)
    {
        ReminderBtnData btn = new ReminderBtnData()
        {
            BtnContent = "Okay, I got it.",
            BtnAction = null
        };

        List<ReminderBtnData> btns = new List<ReminderBtnData>();
        btns.Add(btn);
        ReminderWindows.ShowWindow(reminder, btns);
    
    }
}