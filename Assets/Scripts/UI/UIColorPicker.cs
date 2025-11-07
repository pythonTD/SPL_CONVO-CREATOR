using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[Serializable]
public class UIColorPicker
{
  public Transform transform = null;
  public void SetActive(bool value) => transform.gameObject.SetActive(value);
  public bool ActiveSelf => transform.gameObject.activeSelf;
  private Transform window = null;
  private ColorPicker color_picker = null;
  private Image color_current_image = null;
  private Image color_prev_image = null;
  private TMP_InputField hex_field = null;
  private Action<string> on_confirm = null;
  private Action on_cancel = null;
  public void Awake()
  {
    window = transform.Find("window");
    hex_field = window.Find("hex_input").GetComponent<TMP_InputField>();
    color_current_image = window.Find("current").GetComponent<Image>();
    color_prev_image = window.Find("previous").GetComponent<Image>();
    color_picker = window.Find("color_picker").GetComponent<ColorPicker>();
    window.Find("confirm").GetComponent<Button>().onClick.AddListener(() => { on_confirm?.Invoke(ColorUtility.ToHtmlStringRGB(color_picker.color)); });
    window.Find("cancel").GetComponent<Button>().onClick.AddListener(() => { on_cancel?.Invoke(); });
    window.Find("hex_input").GetComponent<TMP_InputField>().onEndEdit.AddListener((value) =>
    {
      if (ColorUtility.TryParseHtmlString($"#{value}", out var color))
      {
        color_current_image.color = color;
        color_picker.color = color;
      }
    });
    color_picker.onColorChanged += (color) =>
    {
      hex_field.text = ColorUtility.ToHtmlStringRGB(color);
      color_current_image.color = color;
    };
  }
  public void Show(
    Color color_current,
    Action<string> on_confirm,
    Action on_cancel)
  {
    SetActive(true);
    this.on_confirm = on_confirm;
    this.on_cancel = on_cancel;
    // color_current_image.color = color_current;
    // hex_field.text = ColorUtility.ToHtmlStringRGB(color_current);
    color_prev_image.color = color_current;
    color_picker.color = color_current;
  }
  public void Hide()
  {
    on_confirm = null;
    on_cancel = null;
    SetActive(false);
  }
}