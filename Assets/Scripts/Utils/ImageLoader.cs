using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public static class ImageLoader
{
  public static void LoadImageFromUrl(string url, RawImage rawImage, float maxWidth = 400, float maxHeight = 300)
  {
    if (string.IsNullOrEmpty(url) || rawImage == null) return;
    ManagerSceneApp.Instance.StartCoroutine(LoadImageCoroutine(url, rawImage, maxWidth, maxHeight));
  }

  private static IEnumerator LoadImageCoroutine(string url, RawImage rawImage, float maxWidth, float maxHeight)
  {
    using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
    {
      yield return request.SendWebRequest();
      if (request.result == UnityWebRequest.Result.Success)
      {
        var texture = DownloadHandlerTexture.GetContent(request);
        if (rawImage != null)
        {
          rawImage.texture = texture;
          rawImage.color = Color.white;
          // Calculate size maintaining aspect ratio
          float aspectRatio = (float)texture.width / texture.height;
          float width = Mathf.Min(texture.width, maxWidth);
          float height = width / aspectRatio;
          if (height > maxHeight)
          {
            height = maxHeight;
            width = height * aspectRatio;
          }
          var layoutElement = rawImage.GetComponent<LayoutElement>();
          if (layoutElement != null)
          {
            layoutElement.preferredWidth = width;
            layoutElement.preferredHeight = height;
          }
          rawImage.rectTransform.sizeDelta = new Vector2(width, height);
        }
      }
      else
      {
        Debug.LogWarning($"Failed to load image from URL: {url} - {request.error}");
      }
    }
  }
}
