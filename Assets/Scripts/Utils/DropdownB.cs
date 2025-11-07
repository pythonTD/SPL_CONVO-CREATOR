using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class DropdownB : MonoBehaviour
{
  private bool awake_called = false;
  private bool is_dropping = false;
  private RectTransform list_trfm = null;
  private ScrollRect list_scroll_rect = null;
  public List<OPTION_DATA> options = new();
  public Action<int, OPTION_DATA> on_option_clicked = null;
  private List<RaycastResult> raycast_results = new();
  private List<GameObject> modules_obj = new();
  private void Awake()
  {
    if (awake_called) return;
    awake_called = true;
    list_trfm = transform.Find("list").GetComponent<RectTransform>();
    list_scroll_rect = transform.Find("list").GetComponent<ScrollRect>();
    modules_obj.Add(gameObject);
    modules_obj.Add(list_trfm.gameObject);
    transform.Find("header").GetComponent<Button>().onClick.AddListener(() =>
    {
      if (!is_dropping) Show();
      else Hide();
    });
  }
  private void Show()
  {
    if (is_dropping) return;
    is_dropping = true;
    list_trfm.gameObject.SetActive(true);
    // create options
    {
      var counter = 0;
      var parent_content = list_scroll_rect.content;
      for (int i = 1; i < parent_content.childCount; i++) GameObject.Destroy(parent_content.GetChild(i).gameObject);
      parent_content.GetChild(0).gameObject.SetActive(false);
      foreach (var option in options)
      {
        var trfm_inst = GameObject.Instantiate(parent_content.GetChild(0).gameObject, parent_content).transform;
        trfm_inst.gameObject.SetActive(true);
        trfm_inst.Find("text").GetComponent<TMP_Text>().text = option.text;
        var index = counter;
        // click option
        trfm_inst.GetComponent<Button>().onClick.AddListener(() =>
        {
          Hide();
          on_option_clicked?.Invoke(index, option);
          option.action?.Invoke();
        });
        counter++;
      }
    }
  }
  private void Hide()
  {
    if (!is_dropping) return;
    is_dropping = false;
    list_trfm.gameObject.SetActive(false);
  }
  public void AddOption(OPTION_DATA option_data)
  {
    options.Add(option_data);
  }
  public void ClearOptions()
  {
    options.Clear();
  }
  private void LateUpdate()
  {
    if (!is_dropping) return;
    // on_pointer_enter
    {
      if (Input.GetMouseButtonDown(0))
      {
        PointerEventData pointerData = new(EventSystem.current) { position = Input.mousePosition };
        var results = raycast_results;
        results.Clear();
        EventSystem.current.RaycastAll(pointerData, results);
        if (results.Count > 0)
        {
          var result = results[0];
          var on_hide = true;
          foreach (var obj in modules_obj)
            if (result.module.gameObject == obj)
              on_hide = false;
          if (on_hide)
            Hide();
        }
        else Hide();
      }
    }
  }
  [Serializable]
  public class OPTION_DATA
  {
    public int key = 0;
    public string text = string.Empty;
    public object obj = null;
    public Action action = null;
    public OPTION_DATA(string text) => this.text = text;
  }
}