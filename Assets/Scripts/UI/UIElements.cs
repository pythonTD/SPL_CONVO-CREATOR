using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[Serializable]
public class UIElements
{
  public Transform transform = null;
  public void SetActive(bool value) => transform.gameObject.SetActive(value);
  public bool ActiveSelf => transform.gameObject.activeSelf;
  public bool on_drag_element = false;
  public Creator.Element.TYPE drag_element_type = Creator.Element.TYPE.EMPTY;
  public GameObject drag_object = null;
  public event EventHandler<string> on_begin_drag = null;
  private void invoke_on_begin_drag(object sender, string args = "")
  => on_begin_drag?.Invoke(sender, args);
  public event EventHandler<Creator.Element.TYPE> on_end_drag = null;
  private void invoke_on_end_drag(object sender, Creator.Element.TYPE element_type)
  => on_end_drag?.Invoke(sender, element_type);
  public void Awake()
  {
    // reset
    {
      drag_object.SetActive(false);
    }
    var content_parent = transform.Find("scroll").GetComponent<ScrollRect>().content;
    content_parent.GetChild(0).gameObject.SetActive(false);
    for (int i = 1; i < content_parent.childCount; i++) GameObject.Destroy(content_parent.GetChild(i).gameObject);
    foreach (Creator.Element.TYPE type in Enum.GetValues(typeof(Creator.Element.TYPE)))
    {
      if (type == Creator.Element.TYPE.EMPTY) continue;
      var trfm_inst = GameObject.Instantiate(content_parent.GetChild(0), content_parent);
      trfm_inst.gameObject.SetActive(true);
      trfm_inst.Find("text").GetComponent<TMP_Text>().text = type.ToString();
      // events
      {
        var button = trfm_inst.GetComponent<ButtonB>();
        button.onBeginDrag.AddListener(() =>
        {
          // begin drag
          {
            on_drag_element = true;
            drag_element_type = type;
            drag_object.SetActive(true);
            drag_object.transform.Find("text").GetComponent<TMP_Text>().text = type.ToString();
            button.GetComponent<CanvasGroup>().alpha = 0;
            invoke_on_begin_drag(this);
          }
        });
        button.onEndDrag.AddListener(() =>
        {
          // end drag
          {
            on_drag_element = false;
            drag_element_type = type;
            drag_object.SetActive(false);
            button.GetComponent<CanvasGroup>().alpha = 1;
            invoke_on_end_drag(this, type);
          }
        });
      }
    }
  }
  public void Update()
  {
    if (drag_object.activeSelf)
    {
      var new_pos = (Vector2)Input.mousePosition;
      drag_object.transform.position = new_pos;
    }
  }
}