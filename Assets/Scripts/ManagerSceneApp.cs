using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ManagerSceneApp : MonoBehaviour
{
  public static ManagerSceneApp Instance = null;
  public new Camera camera = null;
  public Canvas canvas_001 = null;
  public Canvas canvas_002 = null;
  public Canvas canvas_003 = null;
  private List<RaycastResult> raycast_results = new();
  private bool camera_is_dragging = false;
  private Vector3 camera_drag_origin = default;
  private bool deselect_enabled_down, deselect_enabled_up = false;
  private void Awake()
  {
    Instance = this;
  }
  private void Update()
  {
    // element click deselect
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
          if (result.gameObject == canvas_002.transform.Find("background").gameObject)
            deselect_enabled_down = true;
        }
      }
      if (Input.GetMouseButtonUp(0))
      {
        PointerEventData pointerData = new(EventSystem.current) { position = Input.mousePosition };
        var results = raycast_results;
        results.Clear();
        EventSystem.current.RaycastAll(pointerData, results);
        if (results.Count > 0)
        {
          var result = results[0];
          if (result.gameObject == canvas_002.transform.Find("background").gameObject)
            deselect_enabled_up = true;
        }
        if (deselect_enabled_down && deselect_enabled_up)
          deselect_element();
        deselect_enabled_down = false;
        deselect_enabled_up = false;
        void deselect_element()
        {
          ManagerApp.Clear_Element_Selected();
          if (UIScene.Instance.page != null) { UIScene.Instance.page.Update_All(); }
          if (UIScene.Instance.tools_right != null) { UIScene.Instance.tools_right.Update_All(); }
        }
      }
    }
    // camera drag
    {
      if (Input.GetMouseButtonDown(2))
      {
        PointerEventData pointerData = new(EventSystem.current) { position = Input.mousePosition };
        var results = raycast_results;
        results.Clear();
        EventSystem.current.RaycastAll(pointerData, results);
        if (results.Count > 0)
        {
          var result = results[0];
          if (result.module.eventCamera == camera)
            drag();
        }
        void drag()
        {
          camera_is_dragging = true;
          camera_drag_origin = camera.ScreenToWorldPoint(Input.mousePosition);
        }
      }
      if (Input.GetMouseButtonUp(2))
      {
        camera_is_dragging = false;
      }
      if (camera_is_dragging)
      {
        Vector3 current_pos = camera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 difference = camera_drag_origin - current_pos;
        camera.transform.position += difference;
        // clamp        
        {
          var clamp = camera.transform.position;
          clamp.x = Mathf.Clamp(clamp.x, -8f, 70);
          clamp.y = Mathf.Clamp(clamp.y, -44, 10);
          camera.transform.position = clamp;
        }
      }
    }
    // camera zoom
    {
      var sens_zoom = 2f;
      float scroll = Input.GetAxis("Mouse ScrollWheel") * sens_zoom;
      if (scroll != 0f)
      {
        PointerEventData pointerData = new(EventSystem.current) { position = Input.mousePosition };
        var results = raycast_results;
        results.Clear();
        EventSystem.current.RaycastAll(pointerData, results);
        if (results.Count > 0)
        {
          var result = results[0];
          if (result.module.eventCamera == camera)
            zoom();
        }
        void zoom()
        {
          float size = camera.orthographicSize - scroll;
          camera.orthographicSize = Mathf.Clamp(size, 1, 7);
        }
      }
    }
  }
}