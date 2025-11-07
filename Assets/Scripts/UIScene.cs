using System;
using UnityEngine;
public class UIScene : MonoBehaviour
{
  public static UIScene Instance = null;
  public UIToolsLeft tools_left = new();
  public UIToolsRight tools_right = new();
  public UIToolsTop tools_top = new();
  public UIPage page = new();
  public UIPopup_Input popup_input = new();
  public UIColorPicker color_picker = new();
  public UITooltips tooltips = new();
  private void Awake()
  {
    Instance = this;
    tooltips.Awake();
    tools_left.Awake();
    tools_right.Awake();
    tools_top.Awake();
    popup_input.Awake();
    color_picker.Awake();
    page.Awake();
  }
  private void Start()
  {
    tools_left.Start();
    tools_right.Start();
    tools_top.Start();
  }
  private void Update()
  {
    tools_left.Update();
  }
  private void LateUpdate()
  {
    tooltips.LateUpdate();
  }
}