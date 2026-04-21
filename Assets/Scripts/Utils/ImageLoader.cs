using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public static class ImageLoader
{
  private static readonly Color PlaceholderColor = new(0.86f, 0.86f, 0.86f, 1f);
  private static readonly Color FailureColor = new(0.96f, 0.76f, 0.76f, 1f);

  public static void Render(Creator.Element.GENERAL_IMAGE element, RawImage rawImage)
  {
    if (element == null || rawImage == null) return;

    ApplyPlaceholder(rawImage, element.max_width, element.max_height);
    if (string.IsNullOrWhiteSpace(element.image_url)) return;

    ManagerSceneApp.Instance.StartCoroutine(
      LoadImageCoroutine(element.image_url, rawImage, element.max_width, element.max_height)
    );
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
          ApplyTexture(rawImage, texture, maxWidth, maxHeight);
        }
      }
      else
      {
        if (rawImage != null)
          ApplyFailure(rawImage, maxWidth, maxHeight);

        Debug.LogWarning($"Failed to load image from URL: {url} - {request.error}");
      }
    }
  }

  private static void ApplyPlaceholder(RawImage rawImage, float maxWidth, float maxHeight)
  {
    ApplyVisualState(rawImage, Texture2D.whiteTexture, PlaceholderColor, maxWidth, maxHeight);
  }

  private static void ApplyFailure(RawImage rawImage, float maxWidth, float maxHeight)
  {
    ApplyVisualState(rawImage, Texture2D.whiteTexture, FailureColor, maxWidth, maxHeight);
  }

  private static void ApplyTexture(RawImage rawImage, Texture texture, float maxWidth, float maxHeight)
  {
    if (texture == null)
    {
      ApplyPlaceholder(rawImage, maxWidth, maxHeight);
      return;
    }

    var size = CalculateAspectFitSize(texture.width, texture.height, maxWidth, maxHeight);
    ApplyVisualState(rawImage, texture, Color.white, size.x, size.y);
  }

  private static void ApplyVisualState(
    RawImage rawImage,
    Texture texture,
    Color color,
    float width,
    float height)
  {
    rawImage.texture = texture;
    rawImage.color = color;
    UpdateSize(rawImage, width, height);
  }

  private static void UpdateSize(RawImage rawImage, float width, float height)
  {
    var layoutElement = rawImage.GetComponent<LayoutElement>();
    if (layoutElement != null)
    {
      layoutElement.preferredWidth = width;
      layoutElement.preferredHeight = height;
    }

    rawImage.rectTransform.sizeDelta = new Vector2(width, height);
  }

  private static Vector2 CalculateAspectFitSize(int sourceWidth, int sourceHeight, float maxWidth, float maxHeight)
  {
    if (sourceWidth <= 0 || sourceHeight <= 0)
      return new Vector2(maxWidth, maxHeight);

    float aspectRatio = (float)sourceWidth / sourceHeight;
    float width = Mathf.Min(sourceWidth, maxWidth);
    float height = width / aspectRatio;

    if (height > maxHeight)
    {
      height = maxHeight;
      width = height * aspectRatio;
    }

    return new Vector2(width, height);
  }
}
