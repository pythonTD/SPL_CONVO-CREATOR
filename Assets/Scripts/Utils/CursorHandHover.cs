using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Cursor_Hand_Hover : MonoBehaviour
{
  private static Cursor_Hand_Hover instance = null;
  public static Cursor_Hand_Hover Instance
  {
    get
    {
      if (instance == null)
      {
        instance = new GameObject("[Cursor Hand Hover]").AddComponent<Cursor_Hand_Hover>();
        GameObject.DontDestroyOnLoad(instance.gameObject);
      }
      return instance;
    }
  }
  private HashSet<GameObject> subscribed_objects = new();
  private List<RaycastResult> raycast_results = new();
  private bool hand_hover_current_enabled = false;
  public static void AddListener(GameObject obj)
  {
    if (Instance.subscribed_objects.Contains(obj)) return;
    Instance.subscribed_objects.Add(obj);
  }
  public static void RemoveListener(GameObject obj)
  {
    Instance.subscribed_objects.Remove(obj);
  }
  public void LateUpdate()
  {
    var hand_hover_enabled = false;

    // on_pointer_enter
    {
      PointerEventData pointerData = new(EventSystem.current) { position = Input.mousePosition };
      var results = Instance.raycast_results;
      results.Clear();
      EventSystem.current.RaycastAll(pointerData, results);
      if (results.Count > 0)
      {
        hand_hover_enabled = find_obj();
        bool find_obj()
        {
          if (Instance.subscribed_objects.Contains(results[0].gameObject))
            return true;
          return false;
        }
      }
    }

    // check prev
    {
      if (hand_hover_enabled != Instance.hand_hover_current_enabled)
      {
        Instance.hand_hover_current_enabled = hand_hover_enabled;
        if (hand_hover_enabled)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
          Cursor.SetCursor(Resources.Load<Texture2D>("Sprites/hand_hover_web"), new(7, 43), CursorMode.Auto);
#else
          Cursor.SetCursor(Resources.Load<Texture2D>("Sprites/hand_hover_normal"), new(10, 23), CursorMode.Auto);
#endif
        }
        else Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
      }
    }
  }
}