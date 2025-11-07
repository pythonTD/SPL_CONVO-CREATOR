using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ButtonA : Button, IPointerDownHandler // Normal
{
  public UnityEvent onEnter { get; set; } = new();
  public UnityEvent onExit { get; set; } = new();
  public UnityEvent onDown { get; set; } = new();
  public UnityEvent onClick_Right { get; set; } = new();

  public override void OnPointerEnter(PointerEventData eventData)
  {
    base.OnPointerEnter(eventData);
    onEnter.Invoke();
    if (cursor_hand_hover)
      Cursor_Hand_Hover.AddListener(gameObject); 
  }
  public override void OnPointerExit(PointerEventData eventData)
  {
    base.OnPointerExit(eventData);
    onExit.Invoke();
  }
  public override void OnPointerClick(PointerEventData eventData)
  {
    base.OnPointerClick(eventData);
    if (eventData.button == PointerEventData.InputButton.Right)
      onClick_Right.Invoke();
  }
  public override void OnPointerDown(PointerEventData eventData)
  {
    base.OnPointerDown(eventData);
    onDown.Invoke();
  }

  public bool cursor_hand_hover = false;
  public List<GraphicsColor> graphics_color = new();
  public List<GraphicsSprite> graphics_sprite = new();
  public List<GraphicY> graphics_y = new();

  protected override void DoStateTransition(SelectionState state, bool instant)
  {
    base.DoStateTransition(state, instant);

    // custom color
    {
      foreach (var element in graphics_color)
      {
        var color = state switch
        {
          SelectionState.Normal => element.colors.normalColor,
          SelectionState.Highlighted => element.colors.highlightedColor,
          SelectionState.Pressed => element.colors.pressedColor,
          SelectionState.Selected => element.colors.selectedColor,
          SelectionState.Disabled => element.colors.disabledColor,
          _ => Color.black,
        };
        foreach (var graphic in element.graphics)
        {
          if (graphic == null) continue;
          graphic.CrossFadeColor(color, instant ? 0f : element.colors.fadeDuration, true, true);
        }
      }
    }

    // custom color
    {
      foreach (var element in graphics_sprite)
      {
        var sprite = state switch
        {
          SelectionState.Normal => element.sprites.highlightedSprite,
          SelectionState.Highlighted => element.sprites.highlightedSprite,
          SelectionState.Pressed => element.sprites.pressedSprite,
          SelectionState.Selected => element.sprites.selectedSprite,
          SelectionState.Disabled => element.sprites.disabledSprite,
          _ => element.sprites.highlightedSprite,
        };
        foreach (var graphic in element.graphics)
        {
          if (graphic == null) continue;
          ((Image)graphic).sprite = sprite;
        }
      }
    }

    // custom y
    {
      foreach (var element in graphics_y)
      {
        if (element.graphic == null) continue;

        Vector2 pos = new(element.graphic.rectTransform.anchoredPosition.x, element.px_default);

        if (state == SelectionState.Pressed)
          pos.y += element.px_add;

        element.graphic.rectTransform.anchoredPosition = pos;
      }
    }
  }

  [Serializable]
  public class GraphicsColor
  {
    public ColorBlock colors = new();
    public List<Graphic> graphics = new();
  }
  [Serializable]
  public class GraphicsSprite
  {
    public SpriteState sprites = new();
    public List<Graphic> graphics = new();
  }
  [Serializable]
  public class GraphicY
  {
    public Graphic graphic = null;
    public float px_default = 0;
    public float px_add = 0;
  }
}