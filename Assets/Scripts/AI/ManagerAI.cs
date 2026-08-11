using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class ManagerAI
{
    public static class OPEN_AI
    {
        public static bool is_requesting = false;
        private static event EventHandler<string> on_cancellation_token = null;
        private static string openAI_path = "https://open-ai-key-gray.vercel.app/api/files";
        
        public static void invoke_on_cancellation_token()
            => on_cancellation_token?.Invoke(null, string.Empty);

        public static void Abort_Request()
        {
            invoke_on_cancellation_token();
        }
        
        public static async Task<Request_File> UPLOAD_PDF(byte[] pdfBytes, string fileName)
        {
            var cancellation_token = new CancellationTokenSource();
            EventHandler<string> handler_cancellation_token = (sender, args) => { cancellation_token.Cancel(); };
            on_cancellation_token += handler_cancellation_token;
            is_requesting = true;

            var formSections = new List<IMultipartFormSection>
            {
                new MultipartFormDataSection("purpose", "user_data"),
                new MultipartFormFileSection("file", pdfBytes, fileName, "application/pdf")
            };
            using var request = UnityWebRequest.Post(openAI_path, formSections);
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var operation = request.SendWebRequest();
            try
            {
                while (!operation.isDone)
                {
                    cancellation_token.Token.ThrowIfCancellationRequested();
                    await Task.Yield();
                }

                // Stop timing
                stopwatch.Stop();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log(
                        $"[AI Response Time] {stopwatch.ElapsedMilliseconds} ms ({stopwatch.Elapsed.TotalSeconds:F2} seconds)");
                    Debug.Log(request.downloadHandler.text);
                    var result = JsonConvert.DeserializeObject<Request_File>(request.downloadHandler.text);
                    is_requesting = false;
                    return result;
                }
                else
                {
                    Debug.Log($"[AI Response Time] {stopwatch.ElapsedMilliseconds} ms (FAILED)");
                    return null;
                }
            }
            catch (OperationCanceledException)
            {
                stopwatch.Stop();
                Debug.Log($"[AI Response Time] {stopwatch.ElapsedMilliseconds} ms (CANCELLED)");
                request.Abort();
                return null;
            }
            finally
            {
                on_cancellation_token -= handler_cancellation_token;
                is_requesting = false;
            }
        }
        
        public static async Task DELETE_PDF(string fileId)
        {
            var cancellation_token = new CancellationTokenSource();
            EventHandler<string> handler_cancellation_token = (sender, args) => { cancellation_token.Cancel(); };
            on_cancellation_token += handler_cancellation_token;
            is_requesting = true;

            using var request = UnityWebRequest.Delete(openAI_path);
            
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var operation = request.SendWebRequest();

            try
            {
                while (!operation.isDone)
                {
                    cancellation_token.Token.ThrowIfCancellationRequested();
                    await Task.Yield();
                }

                stopwatch.Stop();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log($"OpenAI Delete File Successful！File-ID: {fileId}");
                }
                else
                {
                    Debug.Log($"OpenAI Delete File Failed！Result is: {request.error}");
                }
            }
            catch (OperationCanceledException)
            {
                stopwatch.Stop();
                Debug.Log($"OpenAI Delete File Error : OperationCanceledException");
                request.Abort();
            }
            finally
            {
                on_cancellation_token -= handler_cancellation_token;
                is_requesting = false;
            }
        }
        
        
        [Serializable]
        public class Request_File
        {
            public string @object;
            public string id;
            public string purpose;
            public string filename;
            public int bytes;
            public long created_at;
            public string status;
        }

        [Serializable]
        public class Result
        {
            public Choice[] choices;
        }

        [Serializable]
        public class Choice
        {
            public TextMessage message;
        }

        [Serializable]
        public abstract class Message
        {
            public string role;
        }

        [Serializable]
        public sealed class TextMessage : Message
        {
            public string content { get; set; }
        }

        [Serializable]
        public sealed class ListMessage : Message
        {
            public List<MessageTypePart> content { get; set; }
        }
        
        [Serializable]
        public class SaveByMessageType
        {
            private List<MessageTypePart> total_data = new List<MessageTypePart>();

            public void SaveText(string text)
            {
                MessageTypePart_Text data = new MessageTypePart_Text();
                data.type = "text";
                data.text = text;
                total_data.Add(data);
            }

            public void SaveImage(string url)
            {
                MessageTypePart_Image.Image img = new MessageTypePart_Image.Image();
                img.url = url;
                
                MessageTypePart_Image data = new MessageTypePart_Image();
                data.type = "image";
                data.image_url = img;
                total_data.Add(data);
            }

            public void SaveFile(string fileId)
            {
                MessageTypePart_File.File file = new MessageTypePart_File.File();
                file.file_id = fileId;
                
                MessageTypePart_File data = new MessageTypePart_File();
                data.type = "file";
                data.file = file;
                total_data.Add(data);
            }

            public List<MessageTypePart> GetDataList()
            {
                return total_data;
            }
        }
        
        [Serializable]
        public abstract class MessageTypePart
        {
            public string type; //file,image_url,text 
        }

        [Serializable]
        public sealed class MessageTypePart_Text : MessageTypePart
        {
            public string text;
        }

        [Serializable]
        public sealed class MessageTypePart_Image : MessageTypePart
        {
            public Image image_url;
            
            [Serializable]
            public class Image
            {
                public string url;
            }
        }

        [Serializable]
        public sealed class MessageTypePart_File : MessageTypePart
        {
            public File file;

            public class File
            {
                public string file_id;
            }
        }
    }
}