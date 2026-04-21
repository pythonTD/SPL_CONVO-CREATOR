using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[Serializable]
public class UIToolsRight
{
  public Transform transform = null;
  public void SetActive(bool value) => transform.gameObject.SetActive(value);
  public bool ActiveSelf => transform.gameObject.activeSelf;
  public UIInspector Inspector = new();
  public void Awake()
  {
    Inspector.Awake();
  }
  public void Start()
  {
    Update_All();
  }
  public void Update_All()
  {
    transform.gameObject.SetActive(ManagerApp.element_selected != null);
    Inspector.Update_All();
  }
  [Serializable]
  public class UIInspector
  {
    public Transform transform = null;
    public void SetActive(bool value) => transform.gameObject.SetActive(value);
    public bool ActiveSelf => transform.gameObject.activeSelf;
    private Transform bar_trfm = null;
    private ScrollRect scroll_rect = null;
    private Creator.Element element_current => ManagerApp.element_selected;
    public void Awake()
    {
      bar_trfm = transform.Find("bar");
      scroll_rect = transform.Find("layout").GetComponent<ScrollRect>();
    }
    public void Update_All()
    {
      foreach (Transform trfm in bar_trfm)
        trfm.gameObject.SetActive(false);
      foreach (Transform trfm in scroll_rect.content)
        trfm.gameObject.SetActive(false);
      if (element_current != null)
      {
        var element_base = element_current;
        var list_elements = ManagerApp.page_selected?.elements;
        switch (element_base.type)
        {
          case Creator.Element.TYPE.EMPTY:
            break;
          case Creator.Element.TYPE.GENERAL_TEXT:
            {
              var element = (Creator.Element.GENERAL_TEXT)element_base;
              var trfm_inst = scroll_rect.content.Find("GENERAL_TEXT");
              var trfm_content = trfm_inst.Find("content");
              trfm_inst.gameObject.SetActive(true);
              bar_trfm.Find("GENERAL_TEXT").gameObject.SetActive(true);
              update_bar(bar_trfm.Find("GENERAL_TEXT/bar"));
              // text content
              {
                var input = trfm_content.Find("text_input").GetComponent<TMP_InputField>();
                input.text = element.text_content;
                input.onEndEdit.RemoveAllListeners();
                input.onEndEdit.AddListener((value) =>
                {
                  element.text_content = value;
                  GameData.invoke_on_class_current_updated();
                });
              }
              // font color
              {
                ColorUtility.TryParseHtmlString(element.color_hex, out var color_current);
                trfm_content.Find("color/frame").GetComponent<Image>().color = color_current;
                var button = trfm_content.Find("color").GetComponent<Button>();
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() =>
                {
                  UIScene.Instance.color_picker.Show(
                    color_current,
                    (value) =>
                    {
                      element.color_hex = $"#{value}";
                      GameData.invoke_on_class_current_updated();
                      UIScene.Instance.color_picker.Hide();
                    },
                    () =>
                    {
                      UIScene.Instance.color_picker.Hide();
                    }
                  );
                  GameData.invoke_on_class_current_updated();
                });
                // tooltip
                {
                  var tooltip_data = new UITooltips.GUI_001.DATA();
                  {
                    tooltip_data.game_object = button.gameObject;
                    tooltip_data.transform_position = button.transform;
                    tooltip_data.text = $"Color";
                    tooltip_data.anchor = TextAnchor.LowerCenter;
                    tooltip_data.spacing = new(0, 20);
                  }
                  UITooltips.GUI_001.AddListener(tooltip_data);
                }
              }
              // font size
              {
                var input = trfm_content.Find("font_size/input").GetComponent<TMP_InputField>();
                input.text = element.font_size.ToString();
                input.onEndEdit.RemoveAllListeners();
                input.onEndEdit.AddListener((value) =>
                {
                  element.font_size = int.Parse(value);
                  GameData.invoke_on_class_current_updated();
                });
                var button_up = trfm_content.Find("font_size/up").GetComponent<Button>();
                button_up.onClick.RemoveAllListeners();
                button_up.onClick.AddListener(() =>
                {
                  element.font_size += 1;
                  GameData.invoke_on_class_current_updated();
                });
                var button_down = trfm_content.Find("font_size/down").GetComponent<Button>();
                button_down.onClick.RemoveAllListeners();
                button_down.onClick.AddListener(() =>
                {
                  element.font_size = Mathf.Max(0, element.font_size - 1);
                  GameData.invoke_on_class_current_updated();
                });
              }
              // font format
              {
                // bold
                {
                  var button = trfm_content.Find("font_format/bold").GetComponent<ButtonC>();
                  button.SetSkinByName(
                    element.bold_enabled
                    ? "toggle_on"
                    : "toggle_off"
                  );
                  button.onClick.RemoveAllListeners();
                  button.onClick.AddListener(() =>
                  {
                    element.bold_enabled = !element.bold_enabled;
                    GameData.invoke_on_class_current_updated();
                  });
                  // tooltip
                  {
                    var tooltip_data = new UITooltips.GUI_001.DATA();
                    {
                      tooltip_data.game_object = button.gameObject;
                      tooltip_data.transform_position = button.transform;
                      tooltip_data.text = $"Bold";
                      tooltip_data.anchor = TextAnchor.LowerCenter;
                      tooltip_data.spacing = new(0, 20);
                    }
                    UITooltips.GUI_001.AddListener(tooltip_data);
                  }
                }
                // italic
                {
                  var button = trfm_content.Find("font_format/italic").GetComponent<ButtonC>();
                  button.SetSkinByName(
                    element.italic_enabled
                    ? "toggle_on"
                    : "toggle_off"
                  );
                  button.onClick.RemoveAllListeners();
                  button.onClick.AddListener(() =>
                  {
                    element.italic_enabled = !element.italic_enabled;
                    GameData.invoke_on_class_current_updated();
                  });
                  // tooltip
                  {
                    var tooltip_data = new UITooltips.GUI_001.DATA();
                    {
                      tooltip_data.game_object = button.gameObject;
                      tooltip_data.transform_position = button.transform;
                      tooltip_data.text = $"Italic";
                      tooltip_data.anchor = TextAnchor.LowerCenter;
                      tooltip_data.spacing = new(0, 20);
                    }
                    UITooltips.GUI_001.AddListener(tooltip_data);
                  }
                }
                // underline
                {
                  var button = trfm_content.Find("font_format/underline").GetComponent<ButtonC>();
                  button.SetSkinByName(
                    element.underline_enabled
                    ? "toggle_on"
                    : "toggle_off"
                  );
                  button.onClick.RemoveAllListeners();
                  button.onClick.AddListener(() =>
                  {
                    element.underline_enabled = !element.underline_enabled;
                    GameData.invoke_on_class_current_updated();
                  });
                  // tooltip
                  {
                    var tooltip_data = new UITooltips.GUI_001.DATA();
                    {
                      tooltip_data.game_object = button.gameObject;
                      tooltip_data.transform_position = button.transform;
                      tooltip_data.text = $"Underline";
                      tooltip_data.anchor = TextAnchor.LowerCenter;
                      tooltip_data.spacing = new(0, 20);
                    }
                    UITooltips.GUI_001.AddListener(tooltip_data);
                  }
                }
              }
              // align horizontal
              {
                // left
                {
                  var aligment = Creator.Element.ALIGNMENT_HORIZONTAL.LEFT;
                  var button = trfm_content.Find("align_horizontal/left").GetComponent<ButtonC>();
                  button.SetSkinByName(
                    element.alignment_horizontal == aligment
                    ? "toggle_on"
                    : "toggle_off"
                  );
                  button.onClick.RemoveAllListeners();
                  button.onClick.AddListener(() =>
                  {
                    element.alignment_horizontal = aligment;
                    GameData.invoke_on_class_current_updated();
                  });
                  // tooltip
                  {
                    var tooltip_data = new UITooltips.GUI_001.DATA();
                    {
                      tooltip_data.game_object = button.gameObject;
                      tooltip_data.transform_position = button.transform;
                      tooltip_data.text = $"Left";
                      tooltip_data.anchor = TextAnchor.LowerCenter;
                      tooltip_data.spacing = new(0, 20);
                    }
                    UITooltips.GUI_001.AddListener(tooltip_data);
                  }
                }
                // center
                {
                  var aligment = Creator.Element.ALIGNMENT_HORIZONTAL.CENTER;
                  var button = trfm_content.Find("align_horizontal/center").GetComponent<ButtonC>();
                  button.SetSkinByName(
                    element.alignment_horizontal == aligment
                    ? "toggle_on"
                    : "toggle_off"
                  );
                  button.onClick.RemoveAllListeners();
                  button.onClick.AddListener(() =>
                  {
                    element.alignment_horizontal = aligment;
                    GameData.invoke_on_class_current_updated();
                  });
                  // tooltip
                  {
                    var tooltip_data = new UITooltips.GUI_001.DATA();
                    {
                      tooltip_data.game_object = button.gameObject;
                      tooltip_data.transform_position = button.transform;
                      tooltip_data.text = $"Center";
                      tooltip_data.anchor = TextAnchor.LowerCenter;
                      tooltip_data.spacing = new(0, 20);
                    }
                    UITooltips.GUI_001.AddListener(tooltip_data);
                  }
                }
                // right
                {
                  var aligment = Creator.Element.ALIGNMENT_HORIZONTAL.RIGHT;
                  var button = trfm_content.Find("align_horizontal/right").GetComponent<ButtonC>();
                  button.SetSkinByName(
                    element.alignment_horizontal == aligment
                    ? "toggle_on"
                    : "toggle_off"
                  );
                  button.onClick.RemoveAllListeners();
                  button.onClick.AddListener(() =>
                  {
                    element.alignment_horizontal = aligment;
                    GameData.invoke_on_class_current_updated();
                  });
                  // tooltip
                  {
                    var tooltip_data = new UITooltips.GUI_001.DATA();
                    {
                      tooltip_data.game_object = button.gameObject;
                      tooltip_data.transform_position = button.transform;
                      tooltip_data.text = $"Right";
                      tooltip_data.anchor = TextAnchor.LowerCenter;
                      tooltip_data.spacing = new(0, 20);
                    }
                    UITooltips.GUI_001.AddListener(tooltip_data);
                  }
                }
              }
              // spacing
              {
                update_spacing(trfm_content.Find("spacing"), element.spacing);
              }
              break;
            }
          case Creator.Element.TYPE.GENERAL_CODE_COMMAND:
            {
              var element = (Creator.Element.GENERAL_CODE_COMMAND)element_base;
              var trfm_inst = scroll_rect.content.Find("GENERAL_CODE_COMMAND");
              var trfm_content = trfm_inst.Find("content");
              trfm_inst.gameObject.SetActive(true);
              bar_trfm.Find("GENERAL_CODE_COMMAND").gameObject.SetActive(true);
              update_bar(bar_trfm.Find("GENERAL_CODE_COMMAND/bar"));
              // lines
              {
                var button = trfm_content.Find("lines").GetComponent<ButtonC>();
                button.SetSkinByName(
                  element.lines_enabled
                  ? "toggle_on"
                  : "toggle_off"
                );
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() =>
                {
                  element.lines_enabled = !element.lines_enabled;
                  GameData.invoke_on_class_current_updated();
                });
                // tooltip
                {
                  var tooltip_data = new UITooltips.GUI_001.DATA();
                  {
                    tooltip_data.game_object = button.gameObject;
                    tooltip_data.transform_position = button.transform;
                    tooltip_data.text = $"Lines Number";
                    tooltip_data.anchor = TextAnchor.LowerCenter;
                    tooltip_data.spacing = new(0, 20);
                  }
                  UITooltips.GUI_001.AddListener(tooltip_data);
                }
              }
              // text content
              {
                var input = trfm_content.Find("text_input").GetComponent<TMP_InputField>();
                input.text = element.text_content;
                input.onEndEdit.RemoveAllListeners();
                input.onEndEdit.AddListener((value) =>
                {
                  element.text_content = value;
                  GameData.invoke_on_class_current_updated();
                });
              }
              // spacing
              {
                update_spacing(trfm_content.Find("spacing"), element.spacing);
              }
              break;
            }
          case Creator.Element.TYPE.GENERAL_LINE:
            {
              var element = (Creator.Element.GENERAL_LINE)element_base;
              var trfm_inst = scroll_rect.content.Find("GENERAL_LINE");
              var trfm_content = trfm_inst.Find("content");
              trfm_inst.gameObject.SetActive(true);
              bar_trfm.Find("GENERAL_LINE").gameObject.SetActive(true);
              update_bar(bar_trfm.Find("GENERAL_LINE/bar"));
              // spacing
              {
                update_spacing(trfm_content.Find("spacing"), element.spacing);
              }
              // color
              {
                ColorUtility.TryParseHtmlString(element.color_hex, out var color_current);
                trfm_content.Find("color/frame").GetComponent<Image>().color = color_current;
                var button = trfm_content.Find("color").GetComponent<Button>();
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() =>
                {
                  UIScene.Instance.color_picker.Show(
                    color_current,
                    (value) =>
                    {
                      element.color_hex = $"#{value}";
                      GameData.invoke_on_class_current_updated();
                      UIScene.Instance.color_picker.Hide();
                    },
                    () =>
                    {
                      UIScene.Instance.color_picker.Hide();
                    }
                  );
                  GameData.invoke_on_class_current_updated();
                });
                // tooltip
                {
                  var tooltip_data = new UITooltips.GUI_001.DATA();
                  {
                    tooltip_data.game_object = button.gameObject;
                    tooltip_data.transform_position = button.transform;
                    tooltip_data.text = $"Color";
                    tooltip_data.anchor = TextAnchor.LowerCenter;
                    tooltip_data.spacing = new(0, 20);
                  }
                  UITooltips.GUI_001.AddListener(tooltip_data);
                }
              }
              // height
              {
                var input = trfm_content.Find("height/input").GetComponent<TMP_InputField>();
                input.text = element.height.ToString();
                input.onEndEdit.RemoveAllListeners();
                input.onEndEdit.AddListener((value) =>
                {
                  element.height = int.Parse(value);
                  GameData.invoke_on_class_current_updated();
                });
                var button_up = trfm_content.Find("height/up").GetComponent<Button>();
                button_up.onClick.RemoveAllListeners();
                button_up.onClick.AddListener(() =>
                {
                  element.height += 1;
                  GameData.invoke_on_class_current_updated();
                });
                var button_down = trfm_content.Find("height/down").GetComponent<Button>();
                button_down.onClick.RemoveAllListeners();
                button_down.onClick.AddListener(() =>
                {
                  element.height = Mathf.Max(0, element.height - 1);
                  GameData.invoke_on_class_current_updated();
                });
              }
              break;
            }
          case Creator.Element.TYPE.GENERAL_IMAGE:
            {
              var element = (Creator.Element.GENERAL_IMAGE)element_base;
              // Reuse GENERAL_CODE_SCRIPT template for now (has text input for URL)
              var trfm_inst = scroll_rect.content.Find("GENERAL_CODE_SCRIPT");
              var trfm_content = trfm_inst.Find("content");
              trfm_inst.gameObject.SetActive(true);
              bar_trfm.Find("GENERAL_CODE_SCRIPT").gameObject.SetActive(true);
              update_bar(bar_trfm.Find("GENERAL_CODE_SCRIPT/bar"));
              // Change title from "Code Script" to "Image"
              var title_text = bar_trfm.Find("GENERAL_CODE_SCRIPT/bar/title")?.GetComponent<TMP_Text>();
              if (title_text != null) title_text.text = "Image";
              // hide lines toggle (not needed for image)
              trfm_content.Find("lines")?.gameObject.SetActive(false);
              update_input_label(trfm_content, "Insert URL");
              // URL input (reuse title_input)
              {
                var input = trfm_content.Find("title_input").GetComponent<TMP_InputField>();
                update_input_placeholder(input, "Insert URL");
                input.text = element.image_url;
                input.onEndEdit.RemoveAllListeners();
                input.onEndEdit.AddListener((value) =>
                {
                  element.image_url = value;
                  GameData.invoke_on_class_current_updated();
                });
              }
              // size input (reuse text_input for max dimensions as "WIDTHxHEIGHT")
              {
                var input = trfm_content.Find("text_input").GetComponent<TMP_InputField>();
                input.text = $"{element.max_width}x{element.max_height}";
                input.onEndEdit.RemoveAllListeners();
                input.onEndEdit.AddListener((value) =>
                {
                  var parts = value.Split('x');
                  if (parts.Length == 2 && float.TryParse(parts[0], out var w) && float.TryParse(parts[1], out var h))
                  {
                    element.max_width = w;
                    element.max_height = h;
                  }
                  GameData.invoke_on_class_current_updated();
                });
              }
              // spacing
              {
                update_spacing(trfm_content.Find("spacing"), element.spacing);
              }
              break;
            }
          case Creator.Element.TYPE.GENERAL_TTS:
            break;
          case Creator.Element.TYPE.GENERAL_CODE_SCRIPT:
            {
              var element = (Creator.Element.GENERAL_CODE_SCRIPT)element_base;
              var trfm_inst = scroll_rect.content.Find("GENERAL_CODE_SCRIPT");
              var trfm_content = trfm_inst.Find("content");
              trfm_inst.gameObject.SetActive(true);
              bar_trfm.Find("GENERAL_CODE_SCRIPT").gameObject.SetActive(true);
              update_bar(bar_trfm.Find("GENERAL_CODE_SCRIPT/bar"));
              // Restore title to "Code Script" (may have been changed by Image element)
              var title_text = bar_trfm.Find("GENERAL_CODE_SCRIPT/bar/title")?.GetComponent<TMP_Text>();
              if (title_text != null) title_text.text = "Code Script";
              update_input_label(trfm_content, "Script Name");
              // lines
              {
                var button = trfm_content.Find("lines").GetComponent<ButtonC>();
                button.SetSkinByName(
                  element.lines_enabled
                  ? "toggle_on"
                  : "toggle_off"
                );
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() =>
                {
                  element.lines_enabled = !element.lines_enabled;
                  GameData.invoke_on_class_current_updated();
                });
                // tooltip
                {
                  var tooltip_data = new UITooltips.GUI_001.DATA();
                  {
                    tooltip_data.game_object = button.gameObject;
                    tooltip_data.transform_position = button.transform;
                    tooltip_data.text = $"Lines Number";
                    tooltip_data.anchor = TextAnchor.LowerCenter;
                    tooltip_data.spacing = new(0, 20);
                  }
                  UITooltips.GUI_001.AddListener(tooltip_data);
                }
              }
              // title content
              {
                var input = trfm_content.Find("title_input").GetComponent<TMP_InputField>();
                update_input_placeholder(input, "Script Name");
                input.text = element.text_title;
                input.onEndEdit.RemoveAllListeners();
                input.onEndEdit.AddListener((value) =>
                {
                  element.text_title = value;
                  GameData.invoke_on_class_current_updated();
                });
              }
              // text content
              {
                var input = trfm_content.Find("text_input").GetComponent<TMP_InputField>();
                input.text = element.text_content;
                input.onEndEdit.RemoveAllListeners();
                input.onEndEdit.AddListener((value) =>
                {
                  element.text_content = value;
                  GameData.invoke_on_class_current_updated();
                });
              }
              // spacing
              {
                update_spacing(trfm_content.Find("spacing"), element.spacing);
              }
              break;
            }
          case Creator.Element.TYPE.CHALLENGE_MULTIPLE_CHOICE:
            {
              var element = (Creator.Element.CHALLENGE_MULTIPLE_CHOICE)element_base;
              var trfm_inst = scroll_rect.content.Find("CHALLENGE_MULTIPLE_CHOICE");
              var trfm_content = trfm_inst.Find("content");
              trfm_inst.gameObject.SetActive(true);
              bar_trfm.Find("CHALLENGE_MULTIPLE_CHOICE").gameObject.SetActive(true);
              update_bar(bar_trfm.Find("CHALLENGE_MULTIPLE_CHOICE/bar"));
              var trfm_question = trfm_inst.Find("question");
              var trfm_options = trfm_inst.Find("options");
              var trfm_display_ECAs = trfm_inst.Find("toggle_ECAs");
              // points
              {
                var input = trfm_inst.Find("score/amount/input").GetComponent<TMP_InputField>();
                input.text = element.submit_points.ToString();
                input.onEndEdit.RemoveAllListeners();
                input.onEndEdit.AddListener((value) =>
                {
                  element.submit_points = int.Parse(value);
                  GameData.invoke_on_class_current_updated();
                });
                var button_up = trfm_inst.Find("score/amount/up").GetComponent<Button>();
                button_up.onClick.RemoveAllListeners();
                button_up.onClick.AddListener(() =>
                {
                  element.submit_points += 1;
                  GameData.invoke_on_class_current_updated();
                });
                var button_down = trfm_inst.Find("score/amount/down").GetComponent<Button>();
                button_down.onClick.RemoveAllListeners();
                button_down.onClick.AddListener(() =>
                {
                  element.submit_points = Mathf.Max(0, element.submit_points - 1);
                  GameData.invoke_on_class_current_updated();
                });
              }
              // question
              {
                // text content
                {
                  var input = trfm_question.Find("text_input").GetComponent<TMP_InputField>();
                  input.text = element.question.text_content;
                  input.onEndEdit.RemoveAllListeners();
                  input.onEndEdit.AddListener((value) =>
                  {
                    element.question.text_content = value;
                    GameData.invoke_on_class_current_updated();
                  });
                }
                // font color
                {
                  ColorUtility.TryParseHtmlString(element.question.color_hex, out var color_current);
                  trfm_question.Find("color/frame").GetComponent<Image>().color = color_current;
                  var button = trfm_question.Find("color").GetComponent<Button>();
                  button.onClick.RemoveAllListeners();
                  button.onClick.AddListener(() =>
                  {
                    UIScene.Instance.color_picker.Show(
                      color_current,
                      (value) =>
                      {
                        element.question.color_hex = $"#{value}";
                        GameData.invoke_on_class_current_updated();
                        UIScene.Instance.color_picker.Hide();
                      },
                      () =>
                      {
                        UIScene.Instance.color_picker.Hide();
                      }
                    );
                    GameData.invoke_on_class_current_updated();
                  });
                  // tooltip
                  {
                    var tooltip_data = new UITooltips.GUI_001.DATA();
                    {
                      tooltip_data.game_object = button.gameObject;
                      tooltip_data.transform_position = button.transform;
                      tooltip_data.text = $"Color";
                      tooltip_data.anchor = TextAnchor.LowerCenter;
                      tooltip_data.spacing = new(0, 20);
                    }
                    UITooltips.GUI_001.AddListener(tooltip_data);
                  }
                }
                // font size
                {
                  var input = trfm_question.Find("font_size/input").GetComponent<TMP_InputField>();
                  input.text = element.question.font_size.ToString();
                  input.onEndEdit.RemoveAllListeners();
                  input.onEndEdit.AddListener((value) =>
                  {
                    element.question.font_size = int.Parse(value);
                    GameData.invoke_on_class_current_updated();
                  });
                  var button_up = trfm_question.Find("font_size/up").GetComponent<Button>();
                  button_up.onClick.RemoveAllListeners();
                  button_up.onClick.AddListener(() =>
                  {
                    element.question.font_size += 1;
                    GameData.invoke_on_class_current_updated();
                  });
                  var button_down = trfm_question.Find("font_size/down").GetComponent<Button>();
                  button_down.onClick.RemoveAllListeners();
                  button_down.onClick.AddListener(() =>
                  {
                    element.question.font_size = Mathf.Max(0, element.question.font_size - 1);
                    GameData.invoke_on_class_current_updated();
                  });
                }
                // font format
                {
                  // bold
                  {
                    var button = trfm_question.Find("font_format/bold").GetComponent<ButtonC>();
                    button.SetSkinByName(
                      element.question.bold_enabled
                      ? "toggle_on"
                      : "toggle_off"
                    );
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(() =>
                    {
                      element.question.bold_enabled = !element.question.bold_enabled;
                      GameData.invoke_on_class_current_updated();
                    });
                    // tooltip
                    {
                      var tooltip_data = new UITooltips.GUI_001.DATA();
                      {
                        tooltip_data.game_object = button.gameObject;
                        tooltip_data.transform_position = button.transform;
                        tooltip_data.text = $"Bold";
                        tooltip_data.anchor = TextAnchor.LowerCenter;
                        tooltip_data.spacing = new(0, 20);
                      }
                      UITooltips.GUI_001.AddListener(tooltip_data);
                    }
                  }
                  // italic
                  {
                    var button = trfm_question.Find("font_format/italic").GetComponent<ButtonC>();
                    button.SetSkinByName(
                      element.question.italic_enabled
                      ? "toggle_on"
                      : "toggle_off"
                    );
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(() =>
                    {
                      element.question.italic_enabled = !element.question.italic_enabled;
                      GameData.invoke_on_class_current_updated();
                    });
                    // tooltip
                    {
                      var tooltip_data = new UITooltips.GUI_001.DATA();
                      {
                        tooltip_data.game_object = button.gameObject;
                        tooltip_data.transform_position = button.transform;
                        tooltip_data.text = $"Italic";
                        tooltip_data.anchor = TextAnchor.LowerCenter;
                        tooltip_data.spacing = new(0, 20);
                      }
                      UITooltips.GUI_001.AddListener(tooltip_data);
                    }
                  }
                  // underline
                  {
                    var button = trfm_question.Find("font_format/underline").GetComponent<ButtonC>();
                    button.SetSkinByName(
                      element.question.underline_enabled
                      ? "toggle_on"
                      : "toggle_off"
                    );
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(() =>
                    {
                      element.question.underline_enabled = !element.question.underline_enabled;
                      GameData.invoke_on_class_current_updated();
                    });
                    // tooltip
                    {
                      var tooltip_data = new UITooltips.GUI_001.DATA();
                      {
                        tooltip_data.game_object = button.gameObject;
                        tooltip_data.transform_position = button.transform;
                        tooltip_data.text = $"Underline";
                        tooltip_data.anchor = TextAnchor.LowerCenter;
                        tooltip_data.spacing = new(0, 20);
                      }
                      UITooltips.GUI_001.AddListener(tooltip_data);
                    }
                  }
                }
                // align horizontal
                {
                  // left
                  {
                    var aligment = Creator.Element.ALIGNMENT_HORIZONTAL.LEFT;
                    var button = trfm_question.Find("align_horizontal/left").GetComponent<ButtonC>();
                    button.SetSkinByName(
                      element.question.alignment_horizontal == aligment
                      ? "toggle_on"
                      : "toggle_off"
                    );
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(() =>
                    {
                      element.question.alignment_horizontal = aligment;
                      GameData.invoke_on_class_current_updated();
                    });
                    // tooltip
                    {
                      var tooltip_data = new UITooltips.GUI_001.DATA();
                      {
                        tooltip_data.game_object = button.gameObject;
                        tooltip_data.transform_position = button.transform;
                        tooltip_data.text = $"Left";
                        tooltip_data.anchor = TextAnchor.LowerCenter;
                        tooltip_data.spacing = new(0, 20);
                      }
                      UITooltips.GUI_001.AddListener(tooltip_data);
                    }
                  }
                  // center
                  {
                    var aligment = Creator.Element.ALIGNMENT_HORIZONTAL.CENTER;
                    var button = trfm_question.Find("align_horizontal/center").GetComponent<ButtonC>();
                    button.SetSkinByName(
                      element.question.alignment_horizontal == aligment
                      ? "toggle_on"
                      : "toggle_off"
                    );
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(() =>
                    {
                      element.question.alignment_horizontal = aligment;
                      GameData.invoke_on_class_current_updated();
                    });
                    // tooltip
                    {
                      var tooltip_data = new UITooltips.GUI_001.DATA();
                      {
                        tooltip_data.game_object = button.gameObject;
                        tooltip_data.transform_position = button.transform;
                        tooltip_data.text = $"Center";
                        tooltip_data.anchor = TextAnchor.LowerCenter;
                        tooltip_data.spacing = new(0, 20);
                      }
                      UITooltips.GUI_001.AddListener(tooltip_data);
                    }
                  }
                  // right
                  {
                    var aligment = Creator.Element.ALIGNMENT_HORIZONTAL.RIGHT;
                    var button = trfm_question.Find("align_horizontal/right").GetComponent<ButtonC>();
                    button.SetSkinByName(
                      element.question.alignment_horizontal == aligment
                      ? "toggle_on"
                      : "toggle_off"
                    );
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(() =>
                    {
                      element.question.alignment_horizontal = aligment;
                      GameData.invoke_on_class_current_updated();
                    });
                    // tooltip
                    {
                      var tooltip_data = new UITooltips.GUI_001.DATA();
                      {
                        tooltip_data.game_object = button.gameObject;
                        tooltip_data.transform_position = button.transform;
                        tooltip_data.text = $"Right";
                        tooltip_data.anchor = TextAnchor.LowerCenter;
                        tooltip_data.spacing = new(0, 20);
                      }
                      UITooltips.GUI_001.AddListener(tooltip_data);
                    }
                  }
                }
                // spacing
                {
                  update_spacing(trfm_question.Find("spacing"), element.question.spacing);
                }
              }
              // options
              {
                // shuffle
                {
                  var button = trfm_options.Find("tools/shuffle").GetComponent<ButtonC>();
                  button.SetSkinByName(
                    element.options.shuffle_enabled
                    ? "toggle_on"
                    : "toggle_off"
                  );
                  button.onClick.RemoveAllListeners();
                  button.onClick.AddListener(() =>
                  {
                    element.options.shuffle_enabled = !element.options.shuffle_enabled;
                    GameData.invoke_on_class_current_updated();
                  });
                  // tooltip
                  {
                    var tooltip_data = new UITooltips.GUI_001.DATA();
                    {
                      tooltip_data.game_object = button.gameObject;
                      tooltip_data.transform_position = button.transform;
                      tooltip_data.text = $"Shuffle In App";
                      tooltip_data.anchor = TextAnchor.LowerCenter;
                      tooltip_data.spacing = new(0, 20);
                    }
                    UITooltips.GUI_001.AddListener(tooltip_data);
                  }
                }
                // option correct
                {
                  var dropdown = trfm_options.Find("tools/option_correct").GetComponent<DropdownA>();
                  dropdown.ClearOptions();
                  dropdown.AddOption(new("None") { key = 0 });
                  foreach (var option in element.options.list)
                    dropdown.AddOption(new($"Option {element.options.list.IndexOf(option) + 1}") { key = option.key });
                  dropdown.SelectByKey(element.options.option_correct_key);
                  dropdown.on_option_clicked = (index, data) =>
                  {
                    element.options.option_correct_key = data.key;
                    GameData.invoke_on_class_current_updated();
                  };
                }
                // list
                {
                  // add
                  {
                    var button = trfm_options.Find("tools/add_option").GetComponent<ButtonA>();
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(() =>
                    {
                      element.options.list.Add(new());
                      GameData.invoke_on_class_current_updated();
                    });
                  }
                  var content_parent = trfm_options.Find("list");
                  content_parent.GetChild(0).gameObject.SetActive(false);
                  for (int i = 1; i < content_parent.childCount; i++) GameObject.Destroy(content_parent.GetChild(i).gameObject);
                  foreach (var option in element.options.list)
                  {
                    var trfm_option = GameObject.Instantiate(content_parent.GetChild(0), content_parent);
                    trfm_option.gameObject.SetActive(true);
                    trfm_option.Find("name").GetComponent<TMP_Text>().text =
                    $"● Option {element.options.list.IndexOf(option) + 1}";
                    // move up
                    {
                      var button = trfm_option.Find("tools/move_up").GetComponent<ButtonC>();
                      button.onClick.RemoveAllListeners();
                      button.onClick.AddListener(() =>
                      {
                        var index = element.options.list.IndexOf(option);
                        if (index <= 0) return;
                        var item_1 = element.options.list[index - 1];
                        var item_2 = element.options.list[index];
                        element.options.list[index - 1] = item_2;
                        element.options.list[index] = item_1;
                        GameData.invoke_on_class_current_updated();
                      });
                      // tooltip
                      {
                        var tooltip_data = new UITooltips.GUI_001.DATA();
                        {
                          tooltip_data.game_object = button.gameObject;
                          tooltip_data.transform_position = button.transform;
                          tooltip_data.text = $"Move Up";
                          tooltip_data.anchor = TextAnchor.LowerCenter;
                          tooltip_data.spacing = new(0, 20);
                        }
                        UITooltips.GUI_001.AddListener(tooltip_data);
                      }
                    }
                    // move down
                    {
                      var button = trfm_option.Find("tools/move_down").GetComponent<ButtonC>();
                      button.onClick.RemoveAllListeners();
                      button.onClick.AddListener(() =>
                      {
                        var index = element.options.list.IndexOf(option);
                        if (index + 1 >= element.options.list.Count) return;
                        var item_1 = element.options.list[index + 1];
                        var item_2 = element.options.list[index];
                        element.options.list[index + 1] = item_2;
                        element.options.list[index] = item_1;
                        GameData.invoke_on_class_current_updated();
                      });
                      // tooltip
                      {
                        var tooltip_data = new UITooltips.GUI_001.DATA();
                        {
                          tooltip_data.game_object = button.gameObject;
                          tooltip_data.transform_position = button.transform;
                          tooltip_data.text = $"Move Down";
                          tooltip_data.anchor = TextAnchor.LowerCenter;
                          tooltip_data.spacing = new(0, 20);
                        }
                        UITooltips.GUI_001.AddListener(tooltip_data);
                      }
                    }
                    // delete
                    {
                      var button = trfm_option.Find("tools/delete").GetComponent<Button>();
                      button.onClick.AddListener(() =>
                      {
                        element.options.list.Remove(option);
                        GameData.invoke_on_class_current_updated();
                      });
                      // tooltip
                      {
                        var tooltip_data = new UITooltips.GUI_001.DATA();
                        {
                          tooltip_data.game_object = button.gameObject;
                          tooltip_data.transform_position = button.transform;
                          tooltip_data.text = $"Delete";
                          tooltip_data.anchor = TextAnchor.LowerCenter;
                          tooltip_data.spacing = new(0, 20);
                        }
                        UITooltips.GUI_001.AddListener(tooltip_data);
                      }
                    }
                    // input
                    {
                      trfm_option.Find("text_input").GetComponent<TMP_InputField>().text = option.text;
                      trfm_option.Find("text_input").GetComponent<TMP_InputField>().onEndEdit.AddListener((value) =>
                      {
                        option.text = value;
                        GameData.invoke_on_class_current_updated();
                      });
                    }
                  }
                }
                // spacing
                {
                  update_spacing(trfm_options.Find("tools/spacing"), element.options.spacing);
                }
              }
              //display ECAs
              {
                var localElement = element; 

                Toggle toggle_on = trfm_display_ECAs.Find("toggles/toggle_on").GetComponent<Toggle>();
                Toggle toggle_off = trfm_display_ECAs.Find("toggles/toggle_off").GetComponent<Toggle>();
                toggle_on.SetIsOnWithoutNotify(element.displayECAs);
                toggle_off.SetIsOnWithoutNotify(!element.displayECAs);
                
                toggle_on.onValueChanged.RemoveAllListeners();
                toggle_on.onValueChanged.AddListener(isOn =>
                {
                  if (element.displayECAs == isOn) { return; } 
                  element.displayECAs = isOn;
                  GameData.invoke_on_class_current_updated();
                });
              }
              break;
            }
          case Creator.Element.TYPE.CHALLENGE_INPUT_NORMAL:
          {
            var element = (Creator.Element.CHALLENGE_INPUT_NORMAL)element_base;
            var trfm_inst = scroll_rect.content.Find("CHALLENGE_INPUT_NORMAL");
            trfm_inst.gameObject.SetActive(true);
            bar_trfm.Find("CHALLENGE_INPUT_NORMAL").gameObject.SetActive(true);
            update_bar(bar_trfm.Find("CHALLENGE_INPUT_NORMAL/bar"));
            var trfm_question = trfm_inst.Find("question");
            var trfm_content = trfm_inst.Find("content");
            var trfm_display_ECAs = trfm_inst.Find("toggle_ECAs");
            #region + question 
            {
              // text content
              {
                var input = trfm_question.Find("text_input").GetComponent<TMP_InputField>();
                input.text = element.question.text_content;
                input.onEndEdit.RemoveAllListeners();
                input.onEndEdit.AddListener((value) =>
                {
                  element.question.text_content = value;
                  GameData.invoke_on_class_current_updated();
                });
              }
              // font color
              {
                ColorUtility.TryParseHtmlString(element.question.color_hex, out var color_current);
                trfm_question.Find("color/frame").GetComponent<Image>().color = color_current;
                var button = trfm_question.Find("color").GetComponent<Button>();
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() =>
                {
                  UIScene.Instance.color_picker.Show(
                    color_current,
                    (value) =>
                    {
                      element.question.color_hex = $"#{value}";
                      GameData.invoke_on_class_current_updated();
                      UIScene.Instance.color_picker.Hide();
                    },
                    () =>
                    {
                      UIScene.Instance.color_picker.Hide();
                    }
                  );
                  GameData.invoke_on_class_current_updated();
                });
                // tooltip
                {
                  var tooltip_data = new UITooltips.GUI_001.DATA();
                  {
                    tooltip_data.game_object = button.gameObject;
                    tooltip_data.transform_position = button.transform;
                    tooltip_data.text = $"Color";
                    tooltip_data.anchor = TextAnchor.LowerCenter;
                    tooltip_data.spacing = new(0, 20);
                  }
                  UITooltips.GUI_001.AddListener(tooltip_data);
                }
              }
              // font size
              {
                var input = trfm_question.Find("font_size/input").GetComponent<TMP_InputField>();
                input.text = element.question.font_size.ToString();
                input.onEndEdit.RemoveAllListeners();
                input.onEndEdit.AddListener((value) =>
                {
                  element.question.font_size = int.Parse(value);
                  GameData.invoke_on_class_current_updated();
                });
                var button_up = trfm_question.Find("font_size/up").GetComponent<Button>();
                button_up.onClick.RemoveAllListeners();
                button_up.onClick.AddListener(() =>
                {
                  element.question.font_size += 1;
                  GameData.invoke_on_class_current_updated();
                });
                var button_down = trfm_question.Find("font_size/down").GetComponent<Button>();
                button_down.onClick.RemoveAllListeners();
                button_down.onClick.AddListener(() =>
                {
                  element.question.font_size = Mathf.Max(0, element.question.font_size - 1);
                  GameData.invoke_on_class_current_updated();
                });
              }
              // font format
              {
                // bold
                {
                  var button = trfm_question.Find("font_format/bold").GetComponent<ButtonC>();
                  button.SetSkinByName(
                    element.question.bold_enabled
                    ? "toggle_on"
                    : "toggle_off"
                  );
                  button.onClick.RemoveAllListeners();
                  button.onClick.AddListener(() =>
                  {
                    element.question.bold_enabled = !element.question.bold_enabled;
                    GameData.invoke_on_class_current_updated();
                  });
                  // tooltip
                  {
                    var tooltip_data = new UITooltips.GUI_001.DATA();
                    {
                      tooltip_data.game_object = button.gameObject;
                      tooltip_data.transform_position = button.transform;
                      tooltip_data.text = $"Bold";
                      tooltip_data.anchor = TextAnchor.LowerCenter;
                      tooltip_data.spacing = new(0, 20);
                    }
                    UITooltips.GUI_001.AddListener(tooltip_data);
                  }
                }
                // italic
                {
                  var button = trfm_question.Find("font_format/italic").GetComponent<ButtonC>();
                  button.SetSkinByName(
                    element.question.italic_enabled
                    ? "toggle_on"
                    : "toggle_off"
                  );
                  button.onClick.RemoveAllListeners();
                  button.onClick.AddListener(() =>
                  {
                    element.question.italic_enabled = !element.question.italic_enabled;
                    GameData.invoke_on_class_current_updated();
                  });
                  // tooltip
                  {
                    var tooltip_data = new UITooltips.GUI_001.DATA();
                    {
                      tooltip_data.game_object = button.gameObject;
                      tooltip_data.transform_position = button.transform;
                      tooltip_data.text = $"Italic";
                      tooltip_data.anchor = TextAnchor.LowerCenter;
                      tooltip_data.spacing = new(0, 20);
                    }
                    UITooltips.GUI_001.AddListener(tooltip_data);
                  }
                }
                // underline
                {
                  var button = trfm_question.Find("font_format/underline").GetComponent<ButtonC>();
                  button.SetSkinByName(
                    element.question.underline_enabled
                    ? "toggle_on"
                    : "toggle_off"
                  );
                  button.onClick.RemoveAllListeners();
                  button.onClick.AddListener(() =>
                  {
                    element.question.underline_enabled = !element.question.underline_enabled;
                    GameData.invoke_on_class_current_updated();
                  });
                  // tooltip
                  {
                    var tooltip_data = new UITooltips.GUI_001.DATA();
                    {
                      tooltip_data.game_object = button.gameObject;
                      tooltip_data.transform_position = button.transform;
                      tooltip_data.text = $"Underline";
                      tooltip_data.anchor = TextAnchor.LowerCenter;
                      tooltip_data.spacing = new(0, 20);
                    }
                    UITooltips.GUI_001.AddListener(tooltip_data);
                  }
                }
              }
              // align horizontal
              {
                // left
                {
                  var aligment = Creator.Element.ALIGNMENT_HORIZONTAL.LEFT;
                  var button = trfm_question.Find("align_horizontal/left").GetComponent<ButtonC>();
                  button.SetSkinByName(
                    element.question.alignment_horizontal == aligment
                    ? "toggle_on"
                    : "toggle_off"
                  );
                  button.onClick.RemoveAllListeners();
                  button.onClick.AddListener(() =>
                  {
                    element.question.alignment_horizontal = aligment;
                    GameData.invoke_on_class_current_updated();
                  });
                  // tooltip
                  {
                    var tooltip_data = new UITooltips.GUI_001.DATA();
                    {
                      tooltip_data.game_object = button.gameObject;
                      tooltip_data.transform_position = button.transform;
                      tooltip_data.text = $"Left";
                      tooltip_data.anchor = TextAnchor.LowerCenter;
                      tooltip_data.spacing = new(0, 20);
                    }
                    UITooltips.GUI_001.AddListener(tooltip_data);
                  }
                }
                // center
                {
                  var aligment = Creator.Element.ALIGNMENT_HORIZONTAL.CENTER;
                  var button = trfm_question.Find("align_horizontal/center").GetComponent<ButtonC>();
                  button.SetSkinByName(
                    element.question.alignment_horizontal == aligment
                    ? "toggle_on"
                    : "toggle_off"
                  );
                  button.onClick.RemoveAllListeners();
                  button.onClick.AddListener(() =>
                  {
                    element.question.alignment_horizontal = aligment;
                    GameData.invoke_on_class_current_updated();
                  });
                  // tooltip
                  {
                    var tooltip_data = new UITooltips.GUI_001.DATA();
                    {
                      tooltip_data.game_object = button.gameObject;
                      tooltip_data.transform_position = button.transform;
                      tooltip_data.text = $"Center";
                      tooltip_data.anchor = TextAnchor.LowerCenter;
                      tooltip_data.spacing = new(0, 20);
                    }
                    UITooltips.GUI_001.AddListener(tooltip_data);
                  }
                }
                // right
                {
                  var aligment = Creator.Element.ALIGNMENT_HORIZONTAL.RIGHT;
                  var button = trfm_question.Find("align_horizontal/right").GetComponent<ButtonC>();
                  button.SetSkinByName(
                    element.question.alignment_horizontal == aligment
                    ? "toggle_on"
                    : "toggle_off"
                  );
                  button.onClick.RemoveAllListeners();
                  button.onClick.AddListener(() =>
                  {
                    element.question.alignment_horizontal = aligment;
                    GameData.invoke_on_class_current_updated();
                  });
                  // tooltip
                  {
                    var tooltip_data = new UITooltips.GUI_001.DATA();
                    {
                      tooltip_data.game_object = button.gameObject;
                      tooltip_data.transform_position = button.transform;
                      tooltip_data.text = $"Right";
                      tooltip_data.anchor = TextAnchor.LowerCenter;
                      tooltip_data.spacing = new(0, 20);
                    }
                    UITooltips.GUI_001.AddListener(tooltip_data);
                  }
                }
              }
              // spacing
              {
                update_spacing(trfm_question.Find("spacing"), element.question.spacing);
              }
            }
            #endregion
            #region  + content(INPUT_FIELD)
            //Height
            {
              var input = trfm_content.Find("height/input").GetComponent<TMP_InputField>();
              input.text = element.input_field.height.ToString();
              input.onEndEdit.RemoveAllListeners();
              input.onEndEdit.AddListener((value) =>
              {
                element.input_field.height = int.Parse(value);
                GameData.invoke_on_class_current_updated();
              });
              var button_up = trfm_content.Find("height/up").GetComponent<ButtonC>();
              button_up.onClick.RemoveAllListeners();
              button_up.onClick.AddListener(() =>
              {
                element.input_field.height += 1;
                GameData.invoke_on_class_current_updated();
              });
              var button_down = trfm_content.Find("height/down").GetComponent<ButtonC>();
              button_down.onClick.RemoveAllListeners();
              button_down.onClick.AddListener(() =>
              {
                element.input_field.height = Mathf.Max(0, element.input_field.height - 1);
                GameData.invoke_on_class_current_updated();
              });
            }
            
            //Spacing
            {
              update_spacing(trfm_content.Find("spacing"), element.input_field.spacing);
            }
            #endregion

            #region + displayECAs
            {
              var localElement = element; 

              Toggle toggle_on = trfm_display_ECAs.Find("toggles/toggle_on").GetComponent<Toggle>();
              Toggle toggle_off = trfm_display_ECAs.Find("toggles/toggle_off").GetComponent<Toggle>();
              toggle_on.SetIsOnWithoutNotify(element.displayECAs);
              toggle_off.SetIsOnWithoutNotify(!element.displayECAs);
                
              toggle_on.onValueChanged.RemoveAllListeners();
              toggle_on.onValueChanged.AddListener(isOn =>
              {
                if (element.displayECAs == isOn) { return; } 
                element.displayECAs = isOn;
                GameData.invoke_on_class_current_updated();
              });
            }
            #endregion
            break;
          }
          case Creator.Element.TYPE.GENERAL_WARNING:
            {
              var element = (Creator.Element.GENERAL_WARNING)element_base;
              var trfm_inst = scroll_rect.content.Find("GENERAL_WARNING");
              var trfm_content = trfm_inst.Find("content");
              trfm_inst.gameObject.SetActive(true);
              bar_trfm.Find("GENERAL_WARNING").gameObject.SetActive(true);
              update_bar(bar_trfm.Find("GENERAL_WARNING/bar"));
              // text content
              {
                var input = trfm_content.Find("text_input").GetComponent<TMP_InputField>();
                input.text = element.text_content;
                input.onEndEdit.RemoveAllListeners();
                input.onEndEdit.AddListener((value) =>
                {
                  element.text_content = value;
                  GameData.invoke_on_class_current_updated();
                });
              }
              // color frame
              {
                ColorUtility.TryParseHtmlString(element.color_hex_frame, out var color_current);
                trfm_content.Find("color_frame/frame").GetComponent<Image>().color = color_current;
                var button = trfm_content.Find("color_frame").GetComponent<Button>();
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() =>
                {
                  UIScene.Instance.color_picker.Show(
                    color_current,
                    (value) =>
                    {
                      element.color_hex_frame = $"#{value}";
                      GameData.invoke_on_class_current_updated();
                      UIScene.Instance.color_picker.Hide();
                    },
                    () =>
                    {
                      UIScene.Instance.color_picker.Hide();
                    }
                  );
                  GameData.invoke_on_class_current_updated();
                });
                // tooltip
                {
                  var tooltip_data = new UITooltips.GUI_001.DATA();
                  {
                    tooltip_data.game_object = button.gameObject;
                    tooltip_data.transform_position = button.transform;
                    tooltip_data.text = $"Color Frame";
                    tooltip_data.anchor = TextAnchor.LowerCenter;
                    tooltip_data.spacing = new(0, 20);
                  }
                  UITooltips.GUI_001.AddListener(tooltip_data);
                }
              }
              // color text
              {
                ColorUtility.TryParseHtmlString(element.color_hex_text, out var color_current);
                trfm_content.Find("color_text/frame").GetComponent<Image>().color = color_current;
                var button = trfm_content.Find("color_text").GetComponent<Button>();
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() =>
                {
                  UIScene.Instance.color_picker.Show(
                    color_current,
                    (value) =>
                    {
                      element.color_hex_text = $"#{value}";
                      GameData.invoke_on_class_current_updated();
                      UIScene.Instance.color_picker.Hide();
                    },
                    () =>
                    {
                      UIScene.Instance.color_picker.Hide();
                    }
                  );
                  GameData.invoke_on_class_current_updated();
                });
                // tooltip
                {
                  var tooltip_data = new UITooltips.GUI_001.DATA();
                  {
                    tooltip_data.game_object = button.gameObject;
                    tooltip_data.transform_position = button.transform;
                    tooltip_data.text = $"Color Text";
                    tooltip_data.anchor = TextAnchor.LowerCenter;
                    tooltip_data.spacing = new(0, 20);
                  }
                  UITooltips.GUI_001.AddListener(tooltip_data);
                }
              }
              // spacing
              {
                update_spacing(trfm_content.Find("spacing"), element.spacing);
              }
              break;
            }
          case Creator.Element.TYPE.AI_PROMPT:
            break;
          case Creator.Element.TYPE.SYSTEM_QUIZ_RESULT:
            break;
          case Creator.Element.TYPE.SYSTEM_QUIZ_ANSWERS:
            break;
          case Creator.Element.TYPE.SYSTEM_QUIZZES_MENU:
            {
              var element = (Creator.Element.SYSTEM_QUIZZES_MENU)element_base;
              var trfm_inst = scroll_rect.content.Find("SYSTEM_QUIZZES_MENU");
              var trfm_content = trfm_inst.Find("content");
              trfm_inst.gameObject.SetActive(true);
              bar_trfm.Find("SYSTEM_QUIZZES_MENU").gameObject.SetActive(true);
              update_bar(bar_trfm.Find("SYSTEM_QUIZZES_MENU/bar"));
              // title
              {
                var input = trfm_content.Find("title_input").GetComponent<TMP_InputField>();
                input.text = element.title;
                input.onEndEdit.RemoveAllListeners();
                input.onEndEdit.AddListener((value) =>
                {
                  element.title = value;
                  GameData.invoke_on_class_current_updated();
                });
              }
              // description
              {
                var input = trfm_content.Find("description_input").GetComponent<TMP_InputField>();
                input.text = element.description;
                input.onEndEdit.RemoveAllListeners();
                input.onEndEdit.AddListener((value) =>
                {
                  element.description = value;
                  GameData.invoke_on_class_current_updated();
                });
              }
              break;
            }
        }
        /*
        switch (element.type)
        {
          case Creator.Element.TYPE.CHALLENGE_MULTIPLE_CHOICE:
            {
              var content = (Creator.Element.CHALLENGE_MULTIPLE_CHOICE)element.content;
              var trfm_inst = scroll_rect.content.Find("CHALLENGE_MULTIPLE_CHOICE");
              var trfm_question = trfm_inst.Find("question");
              var trfm_options = trfm_inst.Find("options");
              trfm_inst.gameObject.SetActive(true);
              update_bar(trfm_inst.Find("bar"), element);
              // question
              {
                // text content
                {
                  var input = trfm_question.Find("text_input").GetComponent<TMP_InputField>();
                  input.text = content.question.text_content;
                  input.onEndEdit.RemoveAllListeners();
                  input.onEndEdit.AddListener((value) =>
                  {
                    content.question.text_content = value;
                    GameData.invoke_on_class_current_updated();
                  });
                }
                // font color
                {
                  ColorUtility.TryParseHtmlString(content.question.color_hex, out var color_current);
                  trfm_question.Find("color/frame").GetComponent<Image>().color = color_current;
                  var button = trfm_question.Find("color").GetComponent<Button>();
                  button.onClick.RemoveAllListeners();
                  button.onClick.AddListener(() =>
                  {
                    UIScene.Instance.color_picker.Show(
                      color_current,
                      (value) =>
                      {
                        content.question.color_hex = $"#{value}";
                        GameData.invoke_on_class_current_updated();
                        UIScene.Instance.color_picker.Hide();
                      },
                      () =>
                      {
                        UIScene.Instance.color_picker.Hide();
                      }
                    );
                    GameData.invoke_on_class_current_updated();
                  });
                  // tooltip
                  {
                    var tooltip_data = new UITooltips.GUI_001.DATA();
                    {
                      tooltip_data.game_object = button.gameObject;
                      tooltip_data.transform_position = button.transform;
                      tooltip_data.text = $"Color";
                      tooltip_data.anchor = TextAnchor.LowerCenter;
                      tooltip_data.spacing = new(0, 20);
                    }
                    UITooltips.GUI_001.AddListener(tooltip_data);
                  }
                }
                // font size
                {
                  var input = trfm_question.Find("font_size/input").GetComponent<TMP_InputField>();
                  input.text = content.question.font_size.ToString();
                  input.onEndEdit.RemoveAllListeners();
                  input.onEndEdit.AddListener((value) =>
                  {
                    content.question.font_size = int.Parse(value);
                    GameData.invoke_on_class_current_updated();
                  });
                  var button_up = trfm_question.Find("font_size/up").GetComponent<Button>();
                  button_up.onClick.RemoveAllListeners();
                  button_up.onClick.AddListener(() =>
                  {
                    content.question.font_size += 1;
                    GameData.invoke_on_class_current_updated();
                  });
                  var button_down = trfm_question.Find("font_size/down").GetComponent<Button>();
                  button_down.onClick.RemoveAllListeners();
                  button_down.onClick.AddListener(() =>
                  {
                    content.question.font_size = Mathf.Max(0, content.question.font_size - 1);
                    GameData.invoke_on_class_current_updated();
                  });
                }
                // font format
                {
                  // bold
                  {
                    var button = trfm_question.Find("font_format/bold").GetComponent<ButtonC>();
                    button.SetSkinByName(
                      content.question.bold_enabled
                      ? "toggle_on"
                      : "toggle_off"
                    );
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(() =>
                    {
                      content.question.bold_enabled = !content.question.bold_enabled;
                      GameData.invoke_on_class_current_updated();
                    });
                    // tooltip
                    {
                      var tooltip_data = new UITooltips.GUI_001.DATA();
                      {
                        tooltip_data.game_object = button.gameObject;
                        tooltip_data.transform_position = button.transform;
                        tooltip_data.text = $"Bold";
                        tooltip_data.anchor = TextAnchor.LowerCenter;
                        tooltip_data.spacing = new(0, 20);
                      }
                      UITooltips.GUI_001.AddListener(tooltip_data);
                    }
                  }
                  // italic
                  {
                    var button = trfm_question.Find("font_format/italic").GetComponent<ButtonC>();
                    button.SetSkinByName(
                      content.question.italic_enabled
                      ? "toggle_on"
                      : "toggle_off"
                    );
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(() =>
                    {
                      content.question.italic_enabled = !content.question.italic_enabled;
                      GameData.invoke_on_class_current_updated();
                    });
                    // tooltip
                    {
                      var tooltip_data = new UITooltips.GUI_001.DATA();
                      {
                        tooltip_data.game_object = button.gameObject;
                        tooltip_data.transform_position = button.transform;
                        tooltip_data.text = $"Italic";
                        tooltip_data.anchor = TextAnchor.LowerCenter;
                        tooltip_data.spacing = new(0, 20);
                      }
                      UITooltips.GUI_001.AddListener(tooltip_data);
                    }
                  }
                  // underline
                  {
                    var button = trfm_question.Find("font_format/underline").GetComponent<ButtonC>();
                    button.SetSkinByName(
                      content.question.underline_enabled
                      ? "toggle_on"
                      : "toggle_off"
                    );
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(() =>
                    {
                      content.question.underline_enabled = !content.question.underline_enabled;
                      GameData.invoke_on_class_current_updated();
                    });
                    // tooltip
                    {
                      var tooltip_data = new UITooltips.GUI_001.DATA();
                      {
                        tooltip_data.game_object = button.gameObject;
                        tooltip_data.transform_position = button.transform;
                        tooltip_data.text = $"Underline";
                        tooltip_data.anchor = TextAnchor.LowerCenter;
                        tooltip_data.spacing = new(0, 20);
                      }
                      UITooltips.GUI_001.AddListener(tooltip_data);
                    }
                  }
                }
                // align horizontal
                {
                  // left
                  {
                    var aligment = Creator.Element.ALIGNMENT_HORIZONTAL.LEFT;
                    var button = trfm_question.Find("align_horizontal/left").GetComponent<ButtonC>();
                    button.SetSkinByName(
                      content.question.alignment_horizontal == aligment
                      ? "toggle_on"
                      : "toggle_off"
                    );
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(() =>
                    {
                      content.question.alignment_horizontal = aligment;
                      GameData.invoke_on_class_current_updated();
                    });
                    // tooltip
                    {
                      var tooltip_data = new UITooltips.GUI_001.DATA();
                      {
                        tooltip_data.game_object = button.gameObject;
                        tooltip_data.transform_position = button.transform;
                        tooltip_data.text = $"Left";
                        tooltip_data.anchor = TextAnchor.LowerCenter;
                        tooltip_data.spacing = new(0, 20);
                      }
                      UITooltips.GUI_001.AddListener(tooltip_data);
                    }
                  }
                  // center
                  {
                    var aligment = Creator.Element.ALIGNMENT_HORIZONTAL.CENTER;
                    var button = trfm_question.Find("align_horizontal/center").GetComponent<ButtonC>();
                    button.SetSkinByName(
                      content.question.alignment_horizontal == aligment
                      ? "toggle_on"
                      : "toggle_off"
                    );
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(() =>
                    {
                      content.question.alignment_horizontal = aligment;
                      GameData.invoke_on_class_current_updated();
                    });
                    // tooltip
                    {
                      var tooltip_data = new UITooltips.GUI_001.DATA();
                      {
                        tooltip_data.game_object = button.gameObject;
                        tooltip_data.transform_position = button.transform;
                        tooltip_data.text = $"Center";
                        tooltip_data.anchor = TextAnchor.LowerCenter;
                        tooltip_data.spacing = new(0, 20);
                      }
                      UITooltips.GUI_001.AddListener(tooltip_data);
                    }
                  }
                  // right
                  {
                    var aligment = Creator.Element.ALIGNMENT_HORIZONTAL.RIGHT;
                    var button = trfm_question.Find("align_horizontal/right").GetComponent<ButtonC>();
                    button.SetSkinByName(
                      content.question.alignment_horizontal == aligment
                      ? "toggle_on"
                      : "toggle_off"
                    );
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(() =>
                    {
                      content.question.alignment_horizontal = aligment;
                      GameData.invoke_on_class_current_updated();
                    });
                    // tooltip
                    {
                      var tooltip_data = new UITooltips.GUI_001.DATA();
                      {
                        tooltip_data.game_object = button.gameObject;
                        tooltip_data.transform_position = button.transform;
                        tooltip_data.text = $"Right";
                        tooltip_data.anchor = TextAnchor.LowerCenter;
                        tooltip_data.spacing = new(0, 20);
                      }
                      UITooltips.GUI_001.AddListener(tooltip_data);
                    }
                  }
                }
                // spacing
                {
                  update_spacing(trfm_question.Find("spacing"), content.question.spacing);
                }
              }
              // options
              {
                // shuffle
                {
                  var button = trfm_options.Find("tools/shuffle").GetComponent<ButtonC>();
                  button.SetSkinByName(
                    content.options.shuffle_enabled
                    ? "toggle_on"
                    : "toggle_off"
                  );
                  button.onClick.RemoveAllListeners();
                  button.onClick.AddListener(() =>
                  {
                    content.options.shuffle_enabled = !content.options.shuffle_enabled;
                    GameData.invoke_on_class_current_updated();
                  });
                  // tooltip
                  {
                    var tooltip_data = new UITooltips.GUI_001.DATA();
                    {
                      tooltip_data.game_object = button.gameObject;
                      tooltip_data.transform_position = button.transform;
                      tooltip_data.text = $"Shuffle In App";
                      tooltip_data.anchor = TextAnchor.LowerCenter;
                      tooltip_data.spacing = new(0, 20);
                    }
                    UITooltips.GUI_001.AddListener(tooltip_data);
                  }
                }
                // option correct
                {
                  var dropdown = trfm_options.Find("tools/option_correct").GetComponent<DropdownA>();
                  dropdown.ClearOptions();
                  dropdown.AddOption(new("None") { key = 0 });
                  foreach (var option in content.options.list)
                    dropdown.AddOption(new($"Option {content.options.list.IndexOf(option) + 1}") { key = option.key });
                  dropdown.SelectByKey(content.options.option_correct);
                  dropdown.on_option_clicked = (index, data) =>
                  {
                    content.options.option_correct = data.key;
                    GameData.invoke_on_class_current_updated();
                  };
                }
                // list
                {
                  // add
                  {
                    var button = trfm_options.Find("tools/add_option").GetComponent<ButtonA>();
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(() =>
                    {
                      content.options.list.Add(new());
                      GameData.invoke_on_class_current_updated();
                    });
                  }
                  var content_parent = trfm_options.Find("list");
                  content_parent.GetChild(0).gameObject.SetActive(false);
                  for (int i = 1; i < content_parent.childCount; i++) GameObject.Destroy(content_parent.GetChild(i).gameObject);
                  foreach (var option in content.options.list)
                  {
                    var trfm_option = GameObject.Instantiate(content_parent.GetChild(0), content_parent);
                    trfm_option.gameObject.SetActive(true);
                    trfm_option.Find("name").GetComponent<TMP_Text>().text =
                    $"● Option {content.options.list.IndexOf(option) + 1}";
                    // move up
                    {
                      var button = trfm_option.Find("tools/move_up").GetComponent<ButtonC>();
                      button.onClick.RemoveAllListeners();
                      button.onClick.AddListener(() =>
                      {
                        var index = content.options.list.IndexOf(option);
                        if (index <= 0) return;
                        var item_1 = content.options.list[index - 1];
                        var item_2 = content.options.list[index];
                        content.options.list[index - 1] = item_2;
                        content.options.list[index] = item_1;
                        GameData.invoke_on_class_current_updated();
                      });
                    }
                    // move down
                    {
                      var button = trfm_option.Find("tools/move_down").GetComponent<ButtonC>();
                      button.onClick.RemoveAllListeners();
                      button.onClick.AddListener(() =>
                      {
                        var index = content.options.list.IndexOf(option);
                        if (index + 1 >= content.options.list.Count) return;
                        var item_1 = content.options.list[index + 1];
                        var item_2 = content.options.list[index];
                        content.options.list[index + 1] = item_2;
                        content.options.list[index] = item_1;
                        GameData.invoke_on_class_current_updated();
                      });
                    }
                    // delete
                    {
                      var button = trfm_option.Find("tools/delete").GetComponent<Button>();
                      button.onClick.AddListener(() =>
                      {
                        content.options.list.Remove(option);
                        GameData.invoke_on_class_current_updated();
                      });
                    }
                    // input
                    {
                      trfm_option.Find("text_input").GetComponent<TMP_InputField>().text = option.text;
                      trfm_option.Find("text_input").GetComponent<TMP_InputField>().onEndEdit.AddListener((value) =>
                      {
                        option.text = value;
                        GameData.invoke_on_class_current_updated();
                      });
                    }
                  }
                }
                // spacing
                {
                  update_spacing(trfm_options.Find("tools/spacing"), content.question.spacing);
                }
              }
              break;
            }
          case Creator.Element.TYPE.AI_PROMPT:
            {
              var content = (Creator.Element.AI_PROMPT)element.content;
              var trfm_inst = scroll_rect.content.Find("AI_PROMPT");
              var trfm_content = trfm_inst.Find("content");
              trfm_inst.gameObject.SetActive(true);
              update_bar(trfm_inst.Find("bar"), element);
              // ai_model
              {
                var dropdown = trfm_content.Find("ai_model").GetComponent<DropdownA>();
                dropdown.ClearOptions();
                foreach (Creator.Element.AI_MODEL item in Enum.GetValues(typeof(Creator.Element.AI_MODEL)))
                {
                  if (item == Creator.Element.AI_MODEL.EMPTY) continue;
                  dropdown.AddOption(new($"{item.ToString()}") { key = (int)item });
                }
                dropdown.SelectByKey((int)content.ai_model);
                dropdown.on_option_clicked = (index, data) =>
                {
                  content.ai_model = (Creator.Element.AI_MODEL)data.key;
                  GameData.invoke_on_class_current_updated();
                };
              }
              // tts
              {
                var button = trfm_content.Find("tools/tts").GetComponent<ButtonC>();
                button.SetSkinByName(
                  content.tts
                  ? "toggle_on"
                  : "toggle_off"
                );
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() =>
                {
                  content.tts = !content.tts;
                  GameData.invoke_on_class_current_updated();
                });
                // tooltip
                {
                  var tooltip_data = new UITooltips.GUI_001.DATA();
                  {
                    tooltip_data.game_object = button.gameObject;
                    tooltip_data.transform_position = button.transform;
                    tooltip_data.text = $"TTS";
                    tooltip_data.anchor = TextAnchor.LowerCenter;
                    tooltip_data.spacing = new(0, 20);
                  }
                  UITooltips.GUI_001.AddListener(tooltip_data);
                }
              }
              // writter_effect
              {
                var button = trfm_content.Find("tools/writter_effect").GetComponent<ButtonC>();
                button.SetSkinByName(
                  content.writter_effect
                  ? "toggle_on"
                  : "toggle_off"
                );
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() =>
                {
                  content.writter_effect = !content.writter_effect;
                  GameData.invoke_on_class_current_updated();
                });
                // tooltip
                {
                  var tooltip_data = new UITooltips.GUI_001.DATA();
                  {
                    tooltip_data.game_object = button.gameObject;
                    tooltip_data.transform_position = button.transform;
                    tooltip_data.text = $"Writter Effect";
                    tooltip_data.anchor = TextAnchor.LowerCenter;
                    tooltip_data.spacing = new(0, 20);
                  }
                  UITooltips.GUI_001.AddListener(tooltip_data);
                }
              }
              // page_context
              {
                var button = trfm_content.Find("tools/page_context").GetComponent<ButtonC>();
                button.SetSkinByName(
                  content.page_context
                  ? "toggle_on"
                  : "toggle_off"
                );
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() =>
                {
                  content.page_context = !content.page_context;
                  GameData.invoke_on_class_current_updated();
                });
                // tooltip
                {
                  var tooltip_data = new UITooltips.GUI_001.DATA();
                  {
                    tooltip_data.game_object = button.gameObject;
                    tooltip_data.transform_position = button.transform;
                    tooltip_data.text = $"Add Page Context internal in prompt";
                    tooltip_data.anchor = TextAnchor.LowerCenter;
                    tooltip_data.spacing = new(0, 20);
                  }
                  UITooltips.GUI_001.AddListener(tooltip_data);
                }
              }
              // submit
              {
                var button = trfm_content.Find("tools/submit").GetComponent<ButtonC>();
                button.SetSkinByName(
                  content.submit
                  ? "toggle_on"
                  : "toggle_off"
                );
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() =>
                {
                  content.submit = !content.submit;
                  GameData.invoke_on_class_current_updated();
                });
                // tooltip
                {
                  var tooltip_data = new UITooltips.GUI_001.DATA();
                  {
                    tooltip_data.game_object = button.gameObject;
                    tooltip_data.transform_position = button.transform;
                    tooltip_data.text = $"Create with Submit Action";
                    tooltip_data.anchor = TextAnchor.LowerCenter;
                    tooltip_data.spacing = new(0, 20);
                  }
                  UITooltips.GUI_001.AddListener(tooltip_data);
                }
              }
              // message system
              {
                var input = trfm_content.Find("system_input").GetComponent<TMP_InputField>();
                input.text = content.message_system;
                input.onEndEdit.RemoveAllListeners();
                input.onEndEdit.AddListener((value) =>
                {
                  content.message_system = value;
                  GameData.invoke_on_class_current_updated();
                });
              }
              // message user
              {
                var input = trfm_content.Find("user_input").GetComponent<TMP_InputField>();
                input.text = content.message_user;
                input.onEndEdit.RemoveAllListeners();
                input.onEndEdit.AddListener((value) =>
                {
                  content.message_user = value;
                  GameData.invoke_on_class_current_updated();
                });
              }
              // user_input_helper
              {
                var button = trfm_content.Find("user_help").GetComponent<Button>();
                // tooltip
                {
                  var tooltip_data = new UITooltips.GUI_001.DATA();
                  {
                    tooltip_data.game_object = button.gameObject;
                    tooltip_data.transform_position = button.transform;
                    tooltip_data.text = $"This represents what the user is saying to the assistant.\n\nIt's the actual message or question that the AI will respond to.";
                    tooltip_data.anchor = TextAnchor.LowerCenter;
                    tooltip_data.spacing = new(0, 20);
                  }
                  UITooltips.GUI_001.AddListener(tooltip_data);
                }
              }
              // system_input_helper
              {
                var button = trfm_content.Find("system_help").GetComponent<Button>();
                // tooltip
                {
                  var tooltip_data = new UITooltips.GUI_001.DATA();
                  {
                    tooltip_data.game_object = button.gameObject;
                    tooltip_data.transform_position = button.transform;
                    tooltip_data.text = $"This defines the assistant's behavior and tone before the conversation starts.\n\nUse it to instruct the AI on how it should respond (e.g., as a friendly teacher, technical support agent, etc.).\n\nMay be empty for standard AI model behavior";
                    tooltip_data.anchor = TextAnchor.LowerCenter;
                    tooltip_data.spacing = new(0, 20);
                  }
                  UITooltips.GUI_001.AddListener(tooltip_data);
                }
              }
              break;
            }
          case Creator.Element.TYPE.CHALLENGE_INPUT_NORMAL:
            {
              var content = (Creator.Element.CHALLENGE_INPUT_NORMAL)element.content;
              var trfm_inst = scroll_rect.content.Find("CHALLENGE_INPUT_NORMAL");
              var trfm_question = trfm_inst.Find("question");
              var trfm_content = trfm_inst.Find("content");
              trfm_inst.gameObject.SetActive(true);
              update_bar(trfm_inst.Find("bar"), element);
              // question
              {
                // text content
                {
                  var input = trfm_question.Find("text_input").GetComponent<TMP_InputField>();
                  input.text = content.question.text_content;
                  input.onEndEdit.RemoveAllListeners();
                  input.onEndEdit.AddListener((value) =>
                  {
                    content.question.text_content = value;
                    GameData.invoke_on_class_current_updated();
                  });
                }
                // font color
                {
                  ColorUtility.TryParseHtmlString(content.question.color_hex, out var color_current);
                  trfm_question.Find("color/frame").GetComponent<Image>().color = color_current;
                  var button = trfm_question.Find("color").GetComponent<Button>();
                  button.onClick.RemoveAllListeners();
                  button.onClick.AddListener(() =>
                  {
                    UIScene.Instance.color_picker.Show(
                      color_current,
                      (value) =>
                      {
                        content.question.color_hex = $"#{value}";
                        GameData.invoke_on_class_current_updated();
                        UIScene.Instance.color_picker.Hide();
                      },
                      () =>
                      {
                        UIScene.Instance.color_picker.Hide();
                      }
                    );
                    GameData.invoke_on_class_current_updated();
                  });
                  // tooltip
                  {
                    var tooltip_data = new UITooltips.GUI_001.DATA();
                    {
                      tooltip_data.game_object = button.gameObject;
                      tooltip_data.transform_position = button.transform;
                      tooltip_data.text = $"Color";
                      tooltip_data.anchor = TextAnchor.LowerCenter;
                      tooltip_data.spacing = new(0, 20);
                    }
                    UITooltips.GUI_001.AddListener(tooltip_data);
                  }
                }
                // font size
                {
                  var input = trfm_question.Find("font_size/input").GetComponent<TMP_InputField>();
                  input.text = content.question.font_size.ToString();
                  input.onEndEdit.RemoveAllListeners();
                  input.onEndEdit.AddListener((value) =>
                  {
                    content.question.font_size = int.Parse(value);
                    GameData.invoke_on_class_current_updated();
                  });
                  var button_up = trfm_question.Find("font_size/up").GetComponent<Button>();
                  button_up.onClick.RemoveAllListeners();
                  button_up.onClick.AddListener(() =>
                  {
                    content.question.font_size += 1;
                    GameData.invoke_on_class_current_updated();
                  });
                  var button_down = trfm_question.Find("font_size/down").GetComponent<Button>();
                  button_down.onClick.RemoveAllListeners();
                  button_down.onClick.AddListener(() =>
                  {
                    content.question.font_size = Mathf.Max(0, content.question.font_size - 1);
                    GameData.invoke_on_class_current_updated();
                  });
                }
                // font format
                {
                  // bold
                  {
                    var button = trfm_question.Find("font_format/bold").GetComponent<ButtonC>();
                    button.SetSkinByName(
                      content.question.bold_enabled
                      ? "toggle_on"
                      : "toggle_off"
                    );
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(() =>
                    {
                      content.question.bold_enabled = !content.question.bold_enabled;
                      GameData.invoke_on_class_current_updated();
                    });
                    // tooltip
                    {
                      var tooltip_data = new UITooltips.GUI_001.DATA();
                      {
                        tooltip_data.game_object = button.gameObject;
                        tooltip_data.transform_position = button.transform;
                        tooltip_data.text = $"Bold";
                        tooltip_data.anchor = TextAnchor.LowerCenter;
                        tooltip_data.spacing = new(0, 20);
                      }
                      UITooltips.GUI_001.AddListener(tooltip_data);
                    }
                  }
                  // italic
                  {
                    var button = trfm_question.Find("font_format/italic").GetComponent<ButtonC>();
                    button.SetSkinByName(
                      content.question.italic_enabled
                      ? "toggle_on"
                      : "toggle_off"
                    );
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(() =>
                    {
                      content.question.italic_enabled = !content.question.italic_enabled;
                      GameData.invoke_on_class_current_updated();
                    });
                    // tooltip
                    {
                      var tooltip_data = new UITooltips.GUI_001.DATA();
                      {
                        tooltip_data.game_object = button.gameObject;
                        tooltip_data.transform_position = button.transform;
                        tooltip_data.text = $"Italic";
                        tooltip_data.anchor = TextAnchor.LowerCenter;
                        tooltip_data.spacing = new(0, 20);
                      }
                      UITooltips.GUI_001.AddListener(tooltip_data);
                    }
                  }
                  // underline
                  {
                    var button = trfm_question.Find("font_format/underline").GetComponent<ButtonC>();
                    button.SetSkinByName(
                      content.question.underline_enabled
                      ? "toggle_on"
                      : "toggle_off"
                    );
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(() =>
                    {
                      content.question.underline_enabled = !content.question.underline_enabled;
                      GameData.invoke_on_class_current_updated();
                    });
                    // tooltip
                    {
                      var tooltip_data = new UITooltips.GUI_001.DATA();
                      {
                        tooltip_data.game_object = button.gameObject;
                        tooltip_data.transform_position = button.transform;
                        tooltip_data.text = $"Underline";
                        tooltip_data.anchor = TextAnchor.LowerCenter;
                        tooltip_data.spacing = new(0, 20);
                      }
                      UITooltips.GUI_001.AddListener(tooltip_data);
                    }
                  }
                }
                // align horizontal
                {
                  // left
                  {
                    var aligment = Creator.Element.ALIGNMENT_HORIZONTAL.LEFT;
                    var button = trfm_question.Find("align_horizontal/left").GetComponent<ButtonC>();
                    button.SetSkinByName(
                      content.question.alignment_horizontal == aligment
                      ? "toggle_on"
                      : "toggle_off"
                    );
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(() =>
                    {
                      content.question.alignment_horizontal = aligment;
                      GameData.invoke_on_class_current_updated();
                    });
                    // tooltip
                    {
                      var tooltip_data = new UITooltips.GUI_001.DATA();
                      {
                        tooltip_data.game_object = button.gameObject;
                        tooltip_data.transform_position = button.transform;
                        tooltip_data.text = $"Left";
                        tooltip_data.anchor = TextAnchor.LowerCenter;
                        tooltip_data.spacing = new(0, 20);
                      }
                      UITooltips.GUI_001.AddListener(tooltip_data);
                    }
                  }
                  // center
                  {
                    var aligment = Creator.Element.ALIGNMENT_HORIZONTAL.CENTER;
                    var button = trfm_question.Find("align_horizontal/center").GetComponent<ButtonC>();
                    button.SetSkinByName(
                      content.question.alignment_horizontal == aligment
                      ? "toggle_on"
                      : "toggle_off"
                    );
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(() =>
                    {
                      content.question.alignment_horizontal = aligment;
                      GameData.invoke_on_class_current_updated();
                    });
                    // tooltip
                    {
                      var tooltip_data = new UITooltips.GUI_001.DATA();
                      {
                        tooltip_data.game_object = button.gameObject;
                        tooltip_data.transform_position = button.transform;
                        tooltip_data.text = $"Center";
                        tooltip_data.anchor = TextAnchor.LowerCenter;
                        tooltip_data.spacing = new(0, 20);
                      }
                      UITooltips.GUI_001.AddListener(tooltip_data);
                    }
                  }
                  // right
                  {
                    var aligment = Creator.Element.ALIGNMENT_HORIZONTAL.RIGHT;
                    var button = trfm_question.Find("align_horizontal/right").GetComponent<ButtonC>();
                    button.SetSkinByName(
                      content.question.alignment_horizontal == aligment
                      ? "toggle_on"
                      : "toggle_off"
                    );
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(() =>
                    {
                      content.question.alignment_horizontal = aligment;
                      GameData.invoke_on_class_current_updated();
                    });
                    // tooltip
                    {
                      var tooltip_data = new UITooltips.GUI_001.DATA();
                      {
                        tooltip_data.game_object = button.gameObject;
                        tooltip_data.transform_position = button.transform;
                        tooltip_data.text = $"Right";
                        tooltip_data.anchor = TextAnchor.LowerCenter;
                        tooltip_data.spacing = new(0, 20);
                      }
                      UITooltips.GUI_001.AddListener(tooltip_data);
                    }
                  }
                }
                // spacing
                {
                  update_spacing(trfm_question.Find("spacing"), content.question.spacing);
                }
              }
              // input field
              {
                // height
                {
                  var input = trfm_content.Find("height/input").GetComponent<TMP_InputField>();
                  input.text = content.input_field.height.ToString();
                  input.onEndEdit.RemoveAllListeners();
                  input.onEndEdit.AddListener((value) =>
                  {
                    content.input_field.height = int.Parse(value);
                    GameData.invoke_on_class_current_updated();
                  });
                  var button_up = trfm_inst.Find("height/up").GetComponent<Button>();
                  button_up.onClick.RemoveAllListeners();
                  button_up.onClick.AddListener(() =>
                  {
                    content.input_field.height += 1;
                    GameData.invoke_on_class_current_updated();
                  });
                  var button_down = trfm_inst.Find("height/down").GetComponent<Button>();
                  button_down.onClick.RemoveAllListeners();
                  button_down.onClick.AddListener(() =>
                  {
                    content.input_field.height = Mathf.Max(0, content.input_field.height - 1);
                    GameData.invoke_on_class_current_updated();
                  });
                }
                // spacing
                {
                  update_spacing(trfm_content.Find("spacing"), content.question.spacing);
                }
              }
              break;
            }
          case Creator.Element.TYPE.GENERAL_WARNING:
            {
              var content = (Creator.Element.GENERAL_WARNING)element.content;
              var trfm_inst = scroll_rect.content.Find("GENERAL_WARNING");
              var trfm_content = trfm_inst.Find("content");
              trfm_inst.gameObject.SetActive(true);
              update_bar(trfm_inst.Find("bar"), element);
              // text content
              {
                var input = trfm_content.Find("text_input").GetComponent<TMP_InputField>();
                input.text = content.text_content;
                input.onEndEdit.RemoveAllListeners();
                input.onEndEdit.AddListener((value) =>
                {
                  content.text_content = value;
                  GameData.invoke_on_class_current_updated();
                });
              }
              // color
              {
                ColorUtility.TryParseHtmlString(content.color_hex, out var color_current);
                trfm_content.Find("color/frame").GetComponent<Image>().color = color_current;
                var button = trfm_content.Find("color").GetComponent<Button>();
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() =>
                {
                  UIScene.Instance.color_picker.Show(
                    color_current,
                    (value) =>
                    {
                      content.color_hex = $"#{value}";
                      GameData.invoke_on_class_current_updated();
                      UIScene.Instance.color_picker.Hide();
                    },
                    () =>
                    {
                      UIScene.Instance.color_picker.Hide();
                    }
                  );
                  GameData.invoke_on_class_current_updated();
                });
              }
              // font format
              {
                // bold
                {
                  var button = trfm_content.Find("font_format/bold").GetComponent<ButtonC>();
                  button.SetSkinByName(
                    content.bold_enabled
                    ? "toggle_on"
                    : "toggle_off"
                  );
                  button.onClick.RemoveAllListeners();
                  button.onClick.AddListener(() =>
                  {
                    content.bold_enabled = !content.bold_enabled;
                    GameData.invoke_on_class_current_updated();
                  });
                }
                // italic
                {
                  var button = trfm_content.Find("font_format/italic").GetComponent<ButtonC>();
                  button.SetSkinByName(
                    content.italic_enabled
                    ? "toggle_on"
                    : "toggle_off"
                  );
                  button.onClick.RemoveAllListeners();
                  button.onClick.AddListener(() =>
                  {
                    content.italic_enabled = !content.italic_enabled;
                    GameData.invoke_on_class_current_updated();
                  });
                }
                // underline
                {
                  var button = trfm_content.Find("font_format/underline").GetComponent<ButtonC>();
                  button.SetSkinByName(
                    content.underline_enabled
                    ? "toggle_on"
                    : "toggle_off"
                  );
                  button.onClick.RemoveAllListeners();
                  button.onClick.AddListener(() =>
                  {
                    content.underline_enabled = !content.underline_enabled;
                    GameData.invoke_on_class_current_updated();
                  });
                }
              }
              // spacing
              {
                update_spacing(trfm_content.Find("spacing"), content.spacing);
              }
              break;
            }
          case Creator.Element.TYPE.SYSTEM_QUIZ_RESULT:
            break;
          case Creator.Element.TYPE.GENERAL_TTS:
            {
              var content = (Creator.Element.GENERAL_TTS)element.content;
              var trfm_inst = scroll_rect.content.Find("GENERAL_TTS");
              var trfm_content = trfm_inst.Find("content");
              trfm_inst.gameObject.SetActive(true);
              update_bar(trfm_inst.Find("bar"), element);
              // text content
              {
                var input = trfm_content.Find("text_input").GetComponent<TMP_InputField>();
                input.text = content.text;
                input.onEndEdit.RemoveAllListeners();
                input.onEndEdit.AddListener((value) =>
                {
                  content.text = value;
                  GameData.invoke_on_class_current_updated();
                });
              }
              // writter_effect
              {
                var button = trfm_content.Find("tools/writter_effect").GetComponent<ButtonC>();
                button.SetSkinByName(
                  content.writter_effect
                  ? "toggle_on"
                  : "toggle_off"
                );
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() =>
                {
                  content.writter_effect = !content.writter_effect;
                  GameData.invoke_on_class_current_updated();
                });
                // tooltip
                {
                  var tooltip_data = new UITooltips.GUI_001.DATA();
                  {
                    tooltip_data.game_object = button.gameObject;
                    tooltip_data.transform_position = button.transform;
                    tooltip_data.text = $"Writter Effect";
                    tooltip_data.anchor = TextAnchor.LowerCenter;
                    tooltip_data.spacing = new(0, 20);
                  }
                  UITooltips.GUI_001.AddListener(tooltip_data);
                }
              }
              // submit
              {
                var button = trfm_content.Find("tools/submit").GetComponent<ButtonC>();
                button.SetSkinByName(
                  content.submit
                  ? "toggle_on"
                  : "toggle_off"
                );
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() =>
                {
                  content.submit = !content.submit;
                  GameData.invoke_on_class_current_updated();
                });
                // tooltip
                {
                  var tooltip_data = new UITooltips.GUI_001.DATA();
                  {
                    tooltip_data.game_object = button.gameObject;
                    tooltip_data.transform_position = button.transform;
                    tooltip_data.text = $"Create with Submit Action";
                    tooltip_data.anchor = TextAnchor.LowerCenter;
                    tooltip_data.spacing = new(0, 20);
                  }
                  UITooltips.GUI_001.AddListener(tooltip_data);
                }
              }
              break;
            }
        }
        */
        void update_bar(Transform trfm)
        {
          // move up
          {
            var button = trfm.Find("tools/move_up").GetComponent<ButtonC>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
              var index = list_elements.IndexOf(element_base);
              if (index <= 0) return;
              var item_1 = list_elements[index - 1];
              var item_2 = list_elements[index];
              list_elements[index - 1] = item_2;
              list_elements[index] = item_1;
              GameData.invoke_on_class_current_updated();
            });
            // tooltip
            {
              var tooltip_data = new UITooltips.GUI_001.DATA();
              {
                tooltip_data.game_object = button.gameObject;
                tooltip_data.transform_position = button.transform;
                tooltip_data.text = $"Move Up";
                tooltip_data.anchor = TextAnchor.LowerCenter;
                tooltip_data.spacing = new(0, 20);
              }
              UITooltips.GUI_001.AddListener(tooltip_data);
            }
          }
          // move down
          {
            var button = trfm.Find("tools/move_down").GetComponent<ButtonC>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
              var index = list_elements.IndexOf(element_base);
              if (index + 1 >= list_elements.Count) return;
              var item_1 = list_elements[index + 1];
              var item_2 = list_elements[index];
              list_elements[index + 1] = item_2;
              list_elements[index] = item_1;
              GameData.invoke_on_class_current_updated();
            });
            // tooltip
            {
              var tooltip_data = new UITooltips.GUI_001.DATA();
              {
                tooltip_data.game_object = button.gameObject;
                tooltip_data.transform_position = button.transform;
                tooltip_data.text = $"Move Down";
                tooltip_data.anchor = TextAnchor.LowerCenter;
                tooltip_data.spacing = new(0, 20);
              }
              UITooltips.GUI_001.AddListener(tooltip_data);
            }
          }
          // copy
          {
            var button = trfm.Find("tools/copy").GetComponent<ButtonC>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
              ManagerApp.element_copied = element_base.Clone();
            });
            // tooltip
            {
              var tooltip_data = new UITooltips.GUI_001.DATA();
              {
                tooltip_data.game_object = button.gameObject;
                tooltip_data.transform_position = button.transform;
                tooltip_data.text = $"Copy";
                tooltip_data.anchor = TextAnchor.LowerCenter;
                tooltip_data.spacing = new(0, 20);
              }
              UITooltips.GUI_001.AddListener(tooltip_data);
            }
          }
          // paste
          {
            var button = trfm.Find("tools/paste").GetComponent<ButtonC>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
              Debug.Log(ManagerApp.element_copied);
              if (ManagerApp.element_copied == null) return;
              if (ManagerApp.element_copied.type == element_base.type)
              {
                var list_elements = ManagerApp.page_selected?.elements;
                var index = list_elements.IndexOf(element_base);
                var element_copied = ManagerApp.element_copied;
                ManagerApp.element_copied = null;
                element_copied.Keys_CopyBy(element_base);
                list_elements[index] = element_copied;
                ManagerApp.Select_Element(element_copied);
                GameData.invoke_on_class_current_updated();
              }
            });
            // tooltip
            {
              var tooltip_data = new UITooltips.GUI_001.DATA();
              {
                tooltip_data.game_object = button.gameObject;
                tooltip_data.transform_position = button.transform;
                tooltip_data.text = $"Paste";
                tooltip_data.anchor = TextAnchor.LowerCenter;
                tooltip_data.spacing = new(0, 20);
              }
              UITooltips.GUI_001.AddListener(tooltip_data);
            }
          }
          // duplicate
          {
            var button = trfm.Find("tools/duplicate").GetComponent<ButtonC>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
              var list_elements = ManagerApp.page_selected?.elements;
              var element_cloned = element_base.Clone();
              element_cloned.Keys_CopyBy(null);
              list_elements.Insert(list_elements.IndexOf(element_base) + 1, element_cloned);
              GameData.invoke_on_class_current_updated();
            });
            // tooltip
            {
              var tooltip_data = new UITooltips.GUI_001.DATA();
              {
                tooltip_data.game_object = button.gameObject;
                tooltip_data.transform_position = button.transform;
                tooltip_data.text = $"Duplicate";
                tooltip_data.anchor = TextAnchor.LowerCenter;
                tooltip_data.spacing = new(0, 20);
              }
              UITooltips.GUI_001.AddListener(tooltip_data);
            }
          }
          // delete
          {
            var button = trfm.Find("tools/delete").GetComponent<ButtonC>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
              list_elements.Remove(element_base);
              ManagerApp.Clear_Element_Selected();
              GameData.invoke_on_class_current_updated();
            });
            // tooltip
            {
              var tooltip_data = new UITooltips.GUI_001.DATA();
              {
                tooltip_data.game_object = button.gameObject;
                tooltip_data.transform_position = button.transform;
                tooltip_data.text = $"Delete";
                tooltip_data.anchor = TextAnchor.LowerCenter;
                tooltip_data.spacing = new(0, 20);
              }
              UITooltips.GUI_001.AddListener(tooltip_data);
            }
          }
        }
        void update_spacing(Transform trfm, Creator.Element.SPACING spacing)
        {
          // top
          {
            var input = trfm.Find("top/input").GetComponent<TMP_InputField>();
            input.text = spacing.top.ToString();
            input.onEndEdit.RemoveAllListeners();
            input.onEndEdit.AddListener((value) =>
            {
              spacing.top = int.Parse(value);
              GameData.invoke_on_class_current_updated();
            });
            var button_up = trfm.Find("top/up").GetComponent<Button>();
            button_up.onClick.RemoveAllListeners();
            button_up.onClick.AddListener(() =>
            {
              spacing.top += 1;
              GameData.invoke_on_class_current_updated();
            });
            var button_down = trfm.Find("top/down").GetComponent<Button>();
            button_down.onClick.RemoveAllListeners();
            button_down.onClick.AddListener(() =>
            {
              spacing.top = Mathf.Max(0, spacing.top - 1);
              GameData.invoke_on_class_current_updated();
            });
          }
          // bottom
          {
            var input = trfm.Find("bottom/input").GetComponent<TMP_InputField>();
            input.text = spacing.bottom.ToString();
            input.onEndEdit.RemoveAllListeners();
            input.onEndEdit.AddListener((value) =>
            {
              spacing.bottom = int.Parse(value);
              GameData.invoke_on_class_current_updated();
            });
            var button_up = trfm.Find("bottom/up").GetComponent<Button>();
            button_up.onClick.RemoveAllListeners();
            button_up.onClick.AddListener(() =>
            {
              spacing.bottom += 1;
              GameData.invoke_on_class_current_updated();
            });
            var button_down = trfm.Find("bottom/down").GetComponent<Button>();
            button_down.onClick.RemoveAllListeners();
            button_down.onClick.AddListener(() =>
            {
              spacing.bottom = Mathf.Max(0, spacing.bottom - 1);
              GameData.invoke_on_class_current_updated();
            });
          }
          // left
          {
            var input = trfm.Find("left/input").GetComponent<TMP_InputField>();
            input.text = spacing.left.ToString();
            input.onEndEdit.RemoveAllListeners();
            input.onEndEdit.AddListener((value) =>
            {
              spacing.left = int.Parse(value);
              GameData.invoke_on_class_current_updated();
            });
            var button_up = trfm.Find("left/up").GetComponent<Button>();
            button_up.onClick.RemoveAllListeners();
            button_up.onClick.AddListener(() =>
            {
              spacing.left += 1;
              GameData.invoke_on_class_current_updated();
            });
            var button_down = trfm.Find("left/down").GetComponent<Button>();
            button_down.onClick.RemoveAllListeners();
            button_down.onClick.AddListener(() =>
            {
              spacing.left = Mathf.Max(0, spacing.left - 1);
              GameData.invoke_on_class_current_updated();
            });
          }
          // right
          {
            var input = trfm.Find("right/input").GetComponent<TMP_InputField>();
            input.text = spacing.right.ToString();
            input.onEndEdit.RemoveAllListeners();
            input.onEndEdit.AddListener((value) =>
            {
              spacing.right = int.Parse(value);
              GameData.invoke_on_class_current_updated();
            });
            var button_up = trfm.Find("right/up").GetComponent<Button>();
            button_up.onClick.RemoveAllListeners();
            button_up.onClick.AddListener(() =>
            {
              spacing.right += 1;
              GameData.invoke_on_class_current_updated();
            });
            var button_down = trfm.Find("right/down").GetComponent<Button>();
            button_down.onClick.RemoveAllListeners();
            button_down.onClick.AddListener(() =>
            {
              spacing.right = Mathf.Max(0, spacing.right - 1);
              GameData.invoke_on_class_current_updated();
            });
          }
        }
      }
    }
    private static void update_input_placeholder(TMP_InputField input, string value)
    {
      if (input?.placeholder is TMP_Text placeholder)
        placeholder.text = value;
    }

    private static void update_input_label(Transform content, string value)
    {
      var label = content?.Find("title_")?.GetComponent<TMP_Text>();
      if (label != null)
        label.text = value;
    }
  }
}
