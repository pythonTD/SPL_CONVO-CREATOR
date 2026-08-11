using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public static class SupabaseStorageUploader
{
    private const string supabaseUrl = "https://reaymuosokhwlkwtqixx.supabase.co";  //Project URL
    private const string supabaseAnonKey = "sb_publishable_knH64-hJaq8cIAZJLI_iNw_ArKH238x"; //publishable Key
    private const string buckets_name = "Developer_Uploaded_Files";
    public static async Task<string> UploadDocument(string file_name, byte[] bytes)
    {
        string url = $"{supabaseUrl}/storage/v1/object/{buckets_name}/{file_name}";
        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bytes);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("x-upsert", "true");
            request.SetRequestHeader("apikey", supabaseAnonKey);
            request.SetRequestHeader("Authorization", $"Bearer {supabaseAnonKey}");
            request.SetRequestHeader("Content-Type", "application/pdf");

            var operation = request.SendWebRequest();

            while (!operation.isDone)
            {
                await Task.Yield();
            }

            if (request.result == UnityWebRequest.Result.Success)
            {
                string permanentUrl = $"{supabaseUrl}/storage/v1/object/public/{buckets_name}/{file_name}";
                Debug.Log("Upload To Supabase Successful：" + permanentUrl);
                return permanentUrl;
            }
            else
            {
                Debug.LogError($"Upload To Supabase Error：{request.error}");
                return null;
            }
        }
    
    }
    public static async Task DeleteDocument(string fileName)
    {
        string url = $"{supabaseUrl}/storage/v1/object/{buckets_name}/{fileName}";

        using (UnityWebRequest request = new UnityWebRequest(url, "DELETE"))
        {
            request.downloadHandler = new DownloadHandlerBuffer();

            request.SetRequestHeader("apiKey", supabaseAnonKey);
            request.SetRequestHeader("Authorization", $"Bearer {supabaseAnonKey}");

            var operation = request.SendWebRequest();

            while (!operation.isDone)
            {
                await Task.Yield();
            }

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Remove Supabase File Successful:" + request.downloadHandler.text);
            }
            else
            {
                string responseText = request.downloadHandler != null ? request.downloadHandler.text : "No Response";
                Debug.LogError($"Remove Supabase File Error: {request.error} | Response: {responseText}");
            }
        }
    }
    public static async Task<int> GetFileBytesLen(string fileName)
    {
        string url = $"{supabaseUrl}/storage/v1/object/{buckets_name}/{fileName}";

        using UnityWebRequest request = new UnityWebRequest(url, "GET");
        request.SetRequestHeader("apiKey", supabaseAnonKey);
        request.downloadHandler = new DownloadHandlerBuffer();

        var operation = request.SendWebRequest();
        while (!operation.isDone)
        {
            await Task.Yield();
        }

        if (request.result == UnityWebRequest.Result.Success)
        {
            byte[] fileBytes = request.downloadHandler.data;
            return fileBytes.Length;
        }
        else
        {
            Debug.Log($"Download {request.error} | {request.downloadHandler.text}");
            return 0;
        }
    }
    
    // public static async Task<bool> CheckFileExists(string fileName)
    // {
    //     string url = $"{supabaseUrl}/storage/v1/object/list/{buckets_name}";
    //
    //     string jsonPayload = $"{{\"prefix\":\"\",\"search\":\"{fileName}\",\"limit\":1}}";
    //
    //     byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);
    //
    //     // 3. 發送 POST 請求給 Supabase List API
    //     using UnityWebRequest request = new UnityWebRequest(url, "POST");
    //     request.uploadHandler = new UploadHandlerRaw(bodyRaw);
    //     request.downloadHandler = new DownloadHandlerBuffer();
    //
    //     request.SetRequestHeader("apiKey", supabaseAnonKey);
    //     request.SetRequestHeader("Authorization", $"Bearer {supabaseAnonKey}");
    //     request.SetRequestHeader("Content-Type", "application/json");
    //
    //     var operation = request.SendWebRequest();
    //
    //     while (!operation.isDone)
    //         await Task.Yield();
    //
    //     if (request.result == UnityWebRequest.Result.Success)
    //     {
    //         string jsonResponse = request.downloadHandler.text;
    //         bool exists = jsonResponse.Contains($"\"name\":\"{fileName}\"");
    //         Debug.Log("exists=" + exists);
    //         return exists;
    //     }
    //     else
    //     {
    //         Debug.LogError($"[Supabase List Error] {request.error} | {request.downloadHandler.text}");
    //         return false;
    //     }
    // }



   
}
