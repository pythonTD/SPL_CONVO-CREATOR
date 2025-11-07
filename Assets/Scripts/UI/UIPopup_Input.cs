using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[Serializable]
public class UIPopup_Input
{
  public Transform transform = null;
  public void SetActive(bool value) => transform.gameObject.SetActive(value);
  public bool ActiveSelf => transform.gameObject.activeSelf;
  private TMP_Text title_field = null;
  private TMP_InputField input_field = null;
  private Action<string> on_confirm = null;
  private Action on_cancel = null;
  public void Awake()
  {
    var window = transform.Find("window");
    title_field = window.Find("title").GetComponent<TMP_Text>();
    input_field = window.Find("input").GetComponent<TMP_InputField>();
    window.Find("confirm").GetComponent<Button>().onClick.AddListener(() => { on_confirm?.Invoke(input_field.text.Trim()); });
    window.Find("cancel").GetComponent<Button>().onClick.AddListener(() => { on_cancel?.Invoke(); });
  }
  public void Show(string title_text, string input_text, Action<string> on_confirm, Action on_cancel)
  {
    this.on_confirm = on_confirm;
    this.on_cancel = on_cancel;
    title_field.text = title_text;
    input_field.text = input_text;
    SetActive(true);
  }
  public void Hide()
  {
    on_confirm = null;
    on_cancel = null;
    SetActive(false);
  }
}