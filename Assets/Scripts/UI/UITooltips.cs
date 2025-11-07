using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
[Serializable]
public class UITooltips
{
  public GUI_001 gui_001 = null;
  private List<RaycastResult> raycast_results = new();
  private GameObject obj_current = null;
  public void Awake()
  {
    gui_001.Awake();
    // // test anchor
    // {
    //   var obj = new GameObject("a");
    //   GUI_001.AddListener(
    //     new()
    //     {
    //       game_object = obj,
    //       title = "title",
    //       text = "text.. text.. text..",
    //       anchor = TextAnchor.LowerRight,
    //       follow_cursor = true
    //     });
    // }
  }
  public void LateUpdate()
  {
    // on_pointer_enter
    {
      PointerEventData pointerData = new(EventSystem.current) { position = Input.mousePosition };
      var results = raycast_results;
      results.Clear();
      EventSystem.current?.RaycastAll(pointerData, results);
      if (results.Count > 0) obj_current = results[0].gameObject;
      else obj_current = null;
      if (obj_current != null)
      {
        if (GUI_001.subscribed_objects.TryGetValue(obj_current, out var data))
          gui_001.Show(data);
        else gui_001.Hide();
      }
      else
      {
        gui_001.Hide();
      }
    }
    // // test anchor
    // {
    //   gui_001.Show(GUI_001.subscribed_objects_list[0]);
    // }
  }
  [Serializable]
  public class GUI_001
  {
    public RectTransform rect_transform = null;
    public void SetActive(bool value) => rect_transform.gameObject.SetActive(value);
    public bool ActiveSelf => rect_transform.gameObject.activeSelf;
    private LayoutGroup layout_group = null;
    private RectTransform layout = null;
    private TMP_Text text_field = null;
    private TMP_Text title_field = null;
    private Image frame_image = null;
    public static Dictionary<GameObject, DATA> subscribed_objects = new();
    public static List<DATA> subscribed_objects_list = new();
    private static DATA data_prev = default;
    public void Awake()
    {
      subscribed_objects = new();
      subscribed_objects_list = new();
      layout_group = rect_transform.GetComponent<LayoutGroup>();
      layout = rect_transform.GetChild(0).GetComponent<RectTransform>();
      text_field = layout.Find("text").GetComponent<TMP_Text>();
      title_field = layout.Find("title").GetComponent<TMP_Text>();
      frame_image = layout.Find("frame").GetComponent<Image>();
    }
    public static void AddListener(DATA data)
    {
      var game_object = data.game_object;
      if (subscribed_objects.ContainsKey(game_object))
      {
        subscribed_objects_list[subscribed_objects_list.IndexOf(subscribed_objects[game_object])] = data;
        subscribed_objects[game_object] = data;
        RemoveNulls();
        return;
      }
      subscribed_objects_list.Add(data);
      subscribed_objects.Add(game_object, data);
      RemoveNulls();
      Canvas.ForceUpdateCanvases();
    }
    public static void RemoveListener(GameObject game_object)
    {
      if (!subscribed_objects.ContainsKey(game_object)) return;
      subscribed_objects_list.RemoveAt(subscribed_objects_list.IndexOf(subscribed_objects[game_object]));
      subscribed_objects.Remove(game_object);
      RemoveNulls();
    }
    public static bool ContainsObject(GameObject game_object)
    {
      return subscribed_objects.ContainsKey(game_object);
    }
    private static void RemoveNulls()
    {
      subscribed_objects.Keys
      .Where(key => subscribed_objects[key] == null)
      .ToList()
      .ForEach(key => subscribed_objects.Remove(key));
    }
    public void Show(DATA data)
    {
      if (data_prev != data)
      {
        data_prev = data;
      }
      // setup
      {
        // var obj = element.Key;
        // var data = element.Value;

        // var position = Input.mousePosition;
        // var spacing = new Vector2(0, 0);
        // var anchor = TextAnchor.LowerCenter;
        // var position = data.position;
        var spacing = data.spacing;
        var anchor = data.anchor;
        var title = data.title;
        var text = data.text;
        var scale = data.scale;
        var frame_color = data.frame_color;

        var pivot = anchor switch
        {
          TextAnchor.UpperLeft => new(0, 1),
          TextAnchor.UpperCenter => new(0.5f, 1),
          TextAnchor.UpperRight => new(1, 1),
          TextAnchor.LowerLeft => new(0f, 0f),
          TextAnchor.LowerCenter => new(0.5f, 0f),
          TextAnchor.LowerRight => new(1f, 0f),
          TextAnchor.MiddleLeft => new(0f, 0.5f),
          TextAnchor.MiddleCenter => new(0.5f, 0.5f),
          TextAnchor.MiddleRight => new(1f, 0.5f),
          _ => Vector2.zero
        };

        rect_transform.position
        = data.follow_cursor
        ? Input.mousePosition
        : data.transform_position.position;
        rect_transform.pivot = pivot;
        rect_transform.localScale = Vector3.one * scale;
        layout_group.childAlignment = anchor;
        rect_transform.anchoredPosition += spacing;
        text_field.text = text;
        title_field.text = title;
        frame_image.color = frame_color;

        // fixer
        {
          var position_current = rect_transform.position;
          var position_fixed = position_current;
          var screen_size = new Vector2(Screen.width, Screen.height);
          var layout_size = layout.sizeDelta * rect_transform.lossyScale;
          switch (anchor)
          {
            case TextAnchor.UpperLeft:
              break;
            case TextAnchor.UpperCenter:
              // clamp x
              {
                if ((position_current.x + (layout_size.x / 2f)) > screen_size.x)
                  position_fixed.x -= (position_current.x + (layout_size.x / 2f)) - screen_size.x;
                else if ((position_current.x - (layout_size.x / 2f)) < 0)
                  position_fixed.x += Mathf.Abs(position_current.x - (layout_size.x / 2f));
              }
              // clamp y
              {
                if (position_current.y - layout_size.y < 0)
                  position_fixed.y += Mathf.Abs(position_current.y - layout_size.y);
              }
              break;
            case TextAnchor.UpperRight:
              // clamp x
              {
                if (position_current.x < 0)
                  position_fixed.x += Mathf.Abs(position_current.x);
              }
              break;
            case TextAnchor.MiddleLeft:
              break;
            case TextAnchor.MiddleCenter:
              // clamp x
              {
                if ((position_current.x + (layout_size.x / 2f)) > screen_size.x)
                  position_fixed.x -= (position_current.x + (layout_size.x / 2f)) - screen_size.x;
                else if ((position_current.x - (layout_size.x / 2f)) < 0)
                  position_fixed.x += Mathf.Abs(position_current.x - (layout_size.x / 2f));
              }
              // clamp y
              {
                if (position_current.y + (layout_size.y / 2f) > screen_size.y)
                  position_fixed.y -= (position_current.y + (layout_size.y / 2f)) - screen_size.y;
                if (position_current.y - (layout_size.y / 2f) < 0)
                  position_fixed.y += Mathf.Abs(position_current.y - (layout_size.y / 2f));
              }
              break;
            case TextAnchor.MiddleRight:
              break;
            case TextAnchor.LowerLeft:
              // clamp x
              {
                if (position_current.x < 0)
                  position_fixed.x += Mathf.Abs(position_current.x);
              }
              // clamp y
              {
                if (position_current.y + layout_size.y > screen_size.y)
                  position_fixed.y -= (position_current.y + layout_size.y) - screen_size.y;
              }
              break;
            case TextAnchor.LowerCenter:
              // clamp x
              {
                if ((position_current.x + (layout_size.x / 2f)) > screen_size.x)
                  position_fixed.x -= (position_current.x + (layout_size.x / 2f)) - screen_size.x;
                else if ((position_current.x - (layout_size.x / 2f)) < 0)
                  position_fixed.x += Mathf.Abs(position_current.x - (layout_size.x / 2f));
              }
              // clamp y
              {
                if (position_current.y + layout_size.y > screen_size.y)
                  position_fixed.y -= (position_current.y + layout_size.y) - screen_size.y;
              }
              break;
            case TextAnchor.LowerRight:
              // clamp x
              {
                if (position_current.x > screen_size.x)
                  position_fixed.x -= (position_current.x - screen_size.x);
              }
              // clamp y
              {
                if (position_current.y + layout_size.y > screen_size.y)
                  position_fixed.y -= (position_current.y + layout_size.y) - screen_size.y;
              }
              break;
          }
          rect_transform.position = position_fixed;
        }
      }
      data.invoke_on_show();
      SetActive(true);
    }
    public void Hide()
    {
      if (!ActiveSelf) return;
      data_prev = default;
      SetActive(false);
    }
    public class DATA
    {
      public GameObject game_object = null;
      public Transform transform_position = null;
      public float scale = 1;
      public string title = string.Empty;
      public string text = string.Empty;
      public Vector2 spacing = Vector2.zero;
      public TextAnchor anchor = TextAnchor.MiddleCenter;
      public bool follow_cursor = false;
      public Color32 frame_color = new(81, 81, 81, 255);
      public Action on_show = null;
      public void invoke_on_show() => on_show?.Invoke();
    }
  }
}