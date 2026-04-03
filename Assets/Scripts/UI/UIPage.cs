using System;
using System.Collections.Generic;
using System.Text.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[Serializable]
public class UIPage
{
  public Transform transform = null;
  public void SetActive(bool value) => transform.gameObject.SetActive(value);
  public bool ActiveSelf => transform.gameObject.activeSelf;
  private Transform elements_parent = null;
  private Transform predict_trfm = null;
  private Transform predict_closest_trfm = null;
  public struct ELEMENTS
  {
    public static GameObject EMPTY => Resources.Load<GameObject>("Prefabs/Elements_Page/EMPTY");
    public static GameObject GENERAL_TEXT => Resources.Load<GameObject>("Prefabs/Elements_Page/GENERAL_TEXT");
    public static GameObject GENERAL_WARNING => Resources.Load<GameObject>("Prefabs/Elements_Page/GENERAL_WARNING");
    public static GameObject GENERAL_IMAGE => Resources.Load<GameObject>("Prefabs/Elements_Page/GENERAL_IMAGE");
    public static GameObject GENERAL_LINE => Resources.Load<GameObject>("Prefabs/Elements_Page/GENERAL_LINE");
    public static GameObject GENERAL_CODE_COMMAND => Resources.Load<GameObject>("Prefabs/Elements_Page/GENERAL_CODE_COMMAND");
    public static GameObject GENERAL_CODE_SCRIPT => Resources.Load<GameObject>("Prefabs/Elements_Page/GENERAL_CODE_SCRIPT");
    public static GameObject GENERAL_TTS => Resources.Load<GameObject>("Prefabs/Elements_Page/GENERAL_TTS");
    public static GameObject CHALLENGE_MULTIPLE_CHOICE => Resources.Load<GameObject>("Prefabs/Elements_Page/CHALLENGE_MULTIPLE_CHOICE");
    public static GameObject CHALLENGE_INPUT_NORMAL => Resources.Load<GameObject>("Prefabs/Elements_Page/CHALLENGE_INPUT_NORMAL");
    public static GameObject AI_PROMPT => Resources.Load<GameObject>("Prefabs/Elements_Page/AI_PROMPT");
    public static GameObject SYSTEM_QUIZ_RESULT => Resources.Load<GameObject>("Prefabs/Elements_Page/SYSTEM_QUIZ_RESULT");
    public static GameObject SYSTEM_QUIZ_ANSWERS => Resources.Load<GameObject>("Prefabs/Elements_Page/SYSTEM_QUIZ_ANSWERS");
    public static GameObject SYSTEM_QUIZZES_MENU => Resources.Load<GameObject>("Prefabs/Elements_Page/SYSTEM_QUIZZES_MENU");
  }
  public void Awake()
  {
    elements_parent = transform.Find("elements");
    predict_trfm = transform.Find("predict");
    // events
    {
      UIScene.Instance.tools_left.elements.on_begin_drag += (sender, drag_object) =>
      {
        predict_closest_trfm = null;
        predict_trfm.gameObject.SetActive(false);
      };
      UIScene.Instance.tools_left.elements.on_drag += (sender, drag_object) =>
      {
        if (!transform.gameObject.activeSelf) return;
        var drag_element_position = ManagerSceneApp.Instance.canvas_002.worldCamera.ScreenToWorldPoint(drag_object.transform.position);
        if (find_closest_element(out var result))
        {
          predict_closest_trfm = result;
          predict_trfm.position = predict_closest_trfm.position;
          predict_trfm.gameObject.SetActive(true);
        }
        else
        {
          predict_trfm.gameObject.SetActive(false);
          predict_closest_trfm = null;
        }
        bool find_closest_element(out Transform result)
        {
          Transform closest = null;
          var closest_distance_sqr = Mathf.Infinity;
          foreach (Transform item in elements_parent)
          {
            float distance_sqr = (drag_element_position - item.transform.position).sqrMagnitude;
            if (distance_sqr < closest_distance_sqr)
            {
              closest_distance_sqr = distance_sqr;
              closest = item;
            }
          }
          if (closest_distance_sqr > 22512) result = null;
          else result = closest;
          return result != null;
        }
      };
      UIScene.Instance.tools_left.elements.on_end_drag += (sender, element_type) =>
      {
        predict_trfm.gameObject.SetActive(false);
        var list_elements = ManagerApp.page_selected?.elements;
        if (list_elements == null) return;
        if (predict_closest_trfm != null)
        {
          // insert element in page
          {
            switch (element_type)
            {
              case Creator.Element.TYPE.EMPTY:
                break;
              case Creator.Element.TYPE.GENERAL_TEXT:
                {
                  var sibling_index = predict_closest_trfm.GetSiblingIndex();
                  var element = new Creator.Element.GENERAL_TEXT();
                  list_elements.Insert(sibling_index, element);
                  GameData.Save_All();
                  Update_All();
                  break;
                }
              case Creator.Element.TYPE.GENERAL_CODE_COMMAND:
                {
                  var sibling_index = predict_closest_trfm.GetSiblingIndex();
                  var element = new Creator.Element.GENERAL_CODE_COMMAND();
                  list_elements.Insert(sibling_index, element);
                  GameData.Save_All();
                  Update_All();
                  break;
                }
              case Creator.Element.TYPE.GENERAL_LINE:
                {
                  var sibling_index = predict_closest_trfm.GetSiblingIndex();
                  var element = new Creator.Element.GENERAL_LINE();
                  list_elements.Insert(sibling_index, element);
                  GameData.Save_All();
                  Update_All();
                  break;
                }
              case Creator.Element.TYPE.GENERAL_IMAGE:
                {
                  var sibling_index = predict_closest_trfm.GetSiblingIndex();
                  var element = new Creator.Element.GENERAL_IMAGE();
                  list_elements.Insert(sibling_index, element);
                  GameData.Save_All();
                  Update_All();
                  break;
                }
              case Creator.Element.TYPE.GENERAL_TTS:
                break;
              case Creator.Element.TYPE.GENERAL_CODE_SCRIPT:
                {
                  var sibling_index = predict_closest_trfm.GetSiblingIndex();
                  var element = new Creator.Element.GENERAL_CODE_SCRIPT();
                  list_elements.Insert(sibling_index, element);
                  GameData.Save_All();
                  Update_All();
                  break;
                }
              case Creator.Element.TYPE.CHALLENGE_MULTIPLE_CHOICE:
                {
                  if (ManagerApp.quiz_selected == null) break;
                  var sibling_index = predict_closest_trfm.GetSiblingIndex();
                  var element = new Creator.Element.CHALLENGE_MULTIPLE_CHOICE();
                  list_elements.Insert(sibling_index, element);
                  GameData.Save_All();
                  Update_All();
                  break;
                }
              case Creator.Element.TYPE.CHALLENGE_INPUT_NORMAL:
                {
                  if (ManagerApp.quiz_selected == null) break;
                  var sibling_index = predict_closest_trfm.GetSiblingIndex();
                  var element = new Creator.Element.CHALLENGE_INPUT_NORMAL();
                  list_elements.Insert(sibling_index, element);
                  GameData.Save_All();
                  Update_All();
                  break;
                }
             
              case Creator.Element.TYPE.GENERAL_WARNING:
                {
                  var sibling_index = predict_closest_trfm.GetSiblingIndex();
                  var element = new Creator.Element.GENERAL_WARNING();
                  list_elements.Insert(sibling_index, element);
                  GameData.Save_All();
                  Update_All();
                  break;
                }
              case Creator.Element.TYPE.AI_PROMPT:
                break;
              case Creator.Element.TYPE.SYSTEM_QUIZ_RESULT:
                break;
              case Creator.Element.TYPE.SYSTEM_QUIZ_ANSWERS:
                break;
            }
            // var json_options = new JsonSerializerOptions() { IncludeFields = true };
            // switch (element_type)
            // {
            //   case Creator.Element.TYPE.EMPTY:
            //     break;
            //   case Creator.Element.TYPE.GENERAL_TEXT:
            //     {
            //       var sibling_index = predict_closest_trfm.GetSiblingIndex();
            //       var element = new Creator.Element();
            //       {
            //         element.type = element_type;
            //         element.content = new Creator.Element.TEXT();
            //         list_elements.Insert(sibling_index, element);
            //         GameData.Save_All();
            //       }
            //       Update_All();
            //       break;
            //     }
            //   case Creator.Element.TYPE.GENERAL_CODE_COMMAND:
            //     {
            //       var sibling_index = predict_closest_trfm.GetSiblingIndex();
            //       var element = new Creator.Element();
            //       {
            //         element.type = element_type;
            //         element.content = new Creator.Element.GENERAL_CODE_COMMAND();
            //         list_elements.Insert(sibling_index, element);
            //         GameData.Save_All();
            //       }
            //       Update_All();
            //       break;
            //     }
            //   case Creator.Element.TYPE.GENERAL_LINE:
            //     break;
            //   case Creator.Element.TYPE.GENERAL_IMAGE:
            //     break;
            //   case Creator.Element.TYPE.GENERAL_CODE_SCRIPT:
            //     {
            //       var sibling_index = predict_closest_trfm.GetSiblingIndex();
            //       var element = new Creator.Element();
            //       {
            //         element.type = element_type;
            //         element.content = new Creator.Element.GENERAL_CODE_SCRIPT();
            //         list_elements.Insert(sibling_index, element);
            //         GameData.Save_All();
            //       }
            //       Update_All();
            //       break;
            //     }
            //   case Creator.Element.TYPE.CHALLENGE_MULTIPLE_CHOICE:
            //     {
            //       var sibling_index = predict_closest_trfm.GetSiblingIndex();
            //       var element = new Creator.Element();
            //       {
            //         element.type = element_type;
            //         var content = new Creator.Element.CHALLENGE_MULTIPLE_CHOICE();
            //         content.options.list = new() { new() { text = "Option 1" } };
            //         element.content = content;
            //         list_elements.Insert(sibling_index, element);
            //         GameData.Save_All();
            //       }
            //       Update_All();
            //       break;
            //     }
            //   case Creator.Element.TYPE.AI_PROMPT:
            //     {
            //       var sibling_index = predict_closest_trfm.GetSiblingIndex();
            //       var element = new Creator.Element();
            //       {
            //         element.type = element_type;
            //         element.content = new Creator.Element.AI_PROMPT();
            //         list_elements.Insert(sibling_index, element);
            //         GameData.Save_All();
            //       }
            //       Update_All();
            //       break;
            //     }
            //   case Creator.Element.TYPE.CHALLENGE_INPUT_NORMAL:
            //     {
            //       var sibling_index = predict_closest_trfm.GetSiblingIndex();
            //       var element = new Creator.Element();
            //       {
            //         element.type = element_type;
            //         element.content = new Creator.Element.CHALLENGE_INPUT_NORMAL();
            //         list_elements.Insert(sibling_index, element);
            //         GameData.Save_All();
            //       }
            //       Update_All();
            //       break;
            //     }
            //   case Creator.Element.TYPE.GENERAL_WARNING:
            //     {
            //       var sibling_index = predict_closest_trfm.GetSiblingIndex();
            //       var element = new Creator.Element();
            //       {
            //         element.type = element_type;
            //         element.content = new Creator.Element.GENERAL_WARNING();
            //         list_elements.Insert(sibling_index, element);
            //         GameData.Save_All();
            //       }
            //       Update_All();
            //       break;
            //     }

            //   case Creator.Element.TYPE.SYSTEM_QUIZ_RESULT:
            //     break;
            //   case Creator.Element.TYPE.GENERAL_TTS:
            //     {
            //       var sibling_index = predict_closest_trfm.GetSiblingIndex();
            //       var element = new Creator.Element();
            //       {
            //         element.type = element_type;
            //         element.content = new Creator.Element.GENERAL_TTS();
            //         list_elements.Insert(sibling_index, element);
            //         GameData.Save_All();
            //       }
            //       Update_All();
            //       break;
            //     }
            // }
          }
        }
      };
    }
    Update_All();
  }
  // public void Update()
  // {
  //   if (ManagerApp.elements_selected != null)
  //     if (Input.GetKeyDown(KeyCode.X))
  //     {
  //       var list_elements = ManagerApp.elements_selected;
  //       var element_selected = ManagerApp.element_selected;
  //       list_elements.Remove(element_selected);
  //       ManagerApp.Clear_Element_Selected();
  //       GameData.invoke_on_class_current_updated(this);
  //     }
  // }
  public void Update_All()
  {
    var list_elements = ManagerApp.page_selected?.elements;
    if (ManagerApp.lesson_selected != null)
    {
      transform.Find("name").GetComponent<TMP_Text>().text =
      ManagerApp.lesson_selected.name + " > " + ManagerApp.page_selected.name;
      // tag
      {
        ColorUtility.TryParseHtmlString("#FF8400", out var color_frame);
        transform.Find("tag/frame").GetComponent<Image>().color = color_frame;
        transform.Find("tag/name").GetComponent<TMP_Text>().text = "Lesson";
      }
    }
    else if (ManagerApp.quiz_selected != null)
    {
      var quiz = ManagerApp.quiz_selected;
      var page = ManagerApp.page_selected;
      transform.Find("name").GetComponent<TMP_Text>().text =
      page == quiz.page_result
      ? quiz.name + " > " + $"Quiz Result"
      : page == quiz.page_answers
      ? quiz.name + " > " + $"Quiz Answers"
      : quiz.name + " > " + $"Quiz {quiz.pages.IndexOf(page) + 1}";
      // tag
      {
        ColorUtility.TryParseHtmlString("#6EFE00", out var color_frame);
        transform.Find("tag/frame").GetComponent<Image>().color = color_frame;
        transform.Find("tag/name").GetComponent<TMP_Text>().text = "Quiz";
      }
    }
    else if (
      ManagerApp.page_selected != null &&
      ManagerApp.page_selected == GameData.Class_Current.Quizzes_Menu)
    {
      transform.Find("name").GetComponent<TMP_Text>().text =
      "Quizzes Menu";
      // tag
      {
        ColorUtility.TryParseHtmlString("#6EFE00", out var color_frame);
        transform.Find("tag/frame").GetComponent<Image>().color = color_frame;
        transform.Find("tag/name").GetComponent<TMP_Text>().text = "Quiz";
      }
    }
    transform.gameObject.SetActive(list_elements != null);
    if (list_elements != null)
    {
      var content_parent = elements_parent;
      content_parent.GetChild(0).gameObject.SetActive(false);
      for (int i = 0; i < content_parent.childCount; i++) GameObject.Destroy(content_parent.GetChild(i).gameObject);
      foreach (var element_base in list_elements)
      {
        switch (element_base.type)
        {
          case Creator.Element.TYPE.GENERAL_TEXT:
            {
              var element = (Creator.Element.GENERAL_TEXT)element_base;
              var trfm_inst = GameObject.Instantiate(ELEMENTS.GENERAL_TEXT, content_parent).transform;
              trfm_inst.gameObject.SetActive(true);
              trfm_inst.name = element.key.ToString();
              element.Insert_In_Page(trfm_inst);
              // events
              {
                var button = trfm_inst.GetComponent<ButtonC>();
                button.onClick.AddListener(() =>
                {
                  ManagerApp.Select_Element(element);
                  UIScene.Instance.tools_right.Update_All();
                  update_frame_selector();
                });
              }
              break;
            }
          case Creator.Element.TYPE.GENERAL_CODE_COMMAND:
            {
              var element = (Creator.Element.GENERAL_CODE_COMMAND)element_base;
              var trfm_inst = GameObject.Instantiate(ELEMENTS.GENERAL_CODE_COMMAND, content_parent).transform;
              trfm_inst.gameObject.SetActive(true);
              trfm_inst.name = element.key.ToString();
              element.Insert_In_Page(trfm_inst);
              // events
              {
                var button = trfm_inst.GetComponent<ButtonC>();
                button.onClick.AddListener(() =>
                {
                  ManagerApp.Select_Element(element);
                  UIScene.Instance.tools_right.Update_All();
                  update_frame_selector();
                });
              }
              // var trfm_inst = GameObject.Instantiate(ELEMENTS.GENERAL_CODE_COMMAND, content_parent).transform;
              // trfm_inst.gameObject.SetActive(true);
              // trfm_inst.name = element_base.key.ToString();
              // var content = (Creator.Element.GENERAL_CODE_COMMAND)element_base.content;
              // var text_field = trfm_inst.Find("layout/content/text").GetComponent<TMP_Text>();
              // // margin
              // {
              //   var layout = trfm_inst.GetComponent<HorizontalOrVerticalLayoutGroup>();
              //   layout.padding.left = content.spacing.left;
              //   layout.padding.right = content.spacing.right;
              //   layout.padding.top = content.spacing.top;
              //   layout.padding.bottom = content.spacing.bottom;
              // }
              // // text content
              // {
              //   if (content.lines_enabled)
              //     text_field.text = add_line_numbers(content.text_content);
              //   else
              //     text_field.text = content.text_content;
              // }
              // // events
              // {
              //   var button = trfm_inst.GetComponent<ButtonC>();
              //   button.onClick.AddListener(() =>
              //   {
              //     ManagerApp.Select_Element(element_base);
              //     UIScene.Instance.tools_right.Update_All();
              //     select_frame();
              //   });
              // }
              // string add_line_numbers(string texto)
              // {
              //   var lines = texto.Replace("\r\n", "\n").Split('\n');
              //   var result = new System.Text.StringBuilder();
              //   for (int i = 0; i < lines.Length; i++)
              //   {
              //     string num = $"<color=#A6A6A6>{i + 1,3}</color>: ";
              //     result.AppendLine(num + lines[i]);
              //   }
              //   return result.ToString();
              // }
              break;
            }
          case Creator.Element.TYPE.GENERAL_LINE:
            {
              var element = (Creator.Element.GENERAL_LINE)element_base;
              var trfm_inst = GameObject.Instantiate(ELEMENTS.GENERAL_LINE, content_parent).transform;
              trfm_inst.gameObject.SetActive(true);
              trfm_inst.name = element.key.ToString();
              element.Insert_In_Page(trfm_inst);
              // events
              {
                var button = trfm_inst.GetComponent<ButtonC>();
                button.onClick.AddListener(() =>
                {
                  ManagerApp.Select_Element(element);
                  UIScene.Instance.tools_right.Update_All();
                  update_frame_selector();
                });
              }
              break;
            }
          case Creator.Element.TYPE.GENERAL_IMAGE:
            {
              var element = (Creator.Element.GENERAL_IMAGE)element_base;
              var trfm_inst = GameObject.Instantiate(ELEMENTS.GENERAL_IMAGE, content_parent).transform;
              trfm_inst.gameObject.SetActive(true);
              trfm_inst.name = element.key.ToString();
              element.Insert_In_Page(trfm_inst);
              // load image from URL
              if (!string.IsNullOrEmpty(element.image_url))
              {
                var rawImage = trfm_inst.Find("image").GetComponent<RawImage>();
                ImageLoader.LoadImageFromUrl(element.image_url, rawImage, element.max_width, element.max_height);
              }
              // events
              {
                var button = trfm_inst.GetComponent<ButtonC>();
                button.onClick.AddListener(() =>
                {
                  ManagerApp.Select_Element(element);
                  UIScene.Instance.tools_right.Update_All();
                  update_frame_selector();
                });
              }
              break;
            }
          case Creator.Element.TYPE.GENERAL_CODE_SCRIPT:
            {
              var element = (Creator.Element.GENERAL_CODE_SCRIPT)element_base;
              var trfm_inst = GameObject.Instantiate(ELEMENTS.GENERAL_CODE_SCRIPT, content_parent).transform;
              trfm_inst.gameObject.SetActive(true);
              trfm_inst.name = element.key.ToString();
              element.Insert_In_Page(trfm_inst);
              // events
              {
                var button = trfm_inst.GetComponent<ButtonC>();
                button.onClick.AddListener(() =>
                {
                  ManagerApp.Select_Element(element);
                  UIScene.Instance.tools_right.Update_All();
                  update_frame_selector();
                });
              }
              // var trfm_inst = GameObject.Instantiate(ELEMENTS.GENERAL_CODE_SCRIPT, content_parent).transform;
              // trfm_inst.gameObject.SetActive(true);
              // trfm_inst.name = element_base.key.ToString();
              // var content = (Creator.Element.GENERAL_CODE_SCRIPT)element_base.content;
              // var text_field = trfm_inst.Find("layout/content/text").GetComponent<TMP_Text>();
              // // margin
              // {
              //   var layout = trfm_inst.GetComponent<HorizontalOrVerticalLayoutGroup>();
              //   layout.padding.left = content.spacing.left;
              //   layout.padding.right = content.spacing.right;
              //   layout.padding.top = content.spacing.top;
              //   layout.padding.bottom = content.spacing.bottom;
              // }
              // // title
              // {
              //   trfm_inst.Find("layout/title/text").GetComponent<TMP_Text>().text = content.text_title;
              // }
              // // text content
              // {
              //   if (content.lines_enabled)
              //     text_field.text = add_line_numbers(content.text_content);
              //   else
              //     text_field.text = content.text_content;
              // }
              // // events
              // {
              //   var button = trfm_inst.GetComponent<ButtonC>();
              //   button.onClick.AddListener(() =>
              //   {
              //     ManagerApp.Select_Element(element_base);
              //     UIScene.Instance.tools_right.Update_All();
              //     select_frame();
              //   });
              // }
              // string add_line_numbers(string texto)
              // {
              //   var lines = texto.Replace("\r\n", "\n").Split('\n');
              //   var result = new System.Text.StringBuilder();
              //   for (int i = 0; i < lines.Length; i++)
              //   {
              //     string num = $"<color=#A6A6A6>{i + 1,3}</color>: ";
              //     result.AppendLine(num + lines[i]);
              //   }
              //   return result.ToString();
              // }
              break;
            }
          case Creator.Element.TYPE.CHALLENGE_MULTIPLE_CHOICE:
            {
              var element = (Creator.Element.CHALLENGE_MULTIPLE_CHOICE)element_base;
              var trfm_inst = GameObject.Instantiate(ELEMENTS.CHALLENGE_MULTIPLE_CHOICE, content_parent).transform;
              trfm_inst.gameObject.SetActive(true);
              trfm_inst.name = element.key.ToString();
              element.Insert_In_Page(trfm_inst);
              // events
              {
                var button = trfm_inst.GetComponent<ButtonC>();
                button.onClick.AddListener(() =>
                {
                  ManagerApp.Select_Element(element);
                  UIScene.Instance.tools_right.Update_All();
                  update_frame_selector();
                });
              }
              break;
              // var trfm_inst = GameObject.Instantiate(ELEMENTS.CHALLENGE_MULTIPLE_CHOICE, content_parent).transform;
              // trfm_inst.gameObject.SetActive(true);
              // trfm_inst.name = element_base.key.ToString();
              // var content = (Creator.Element.CHALLENGE_MULTIPLE_CHOICE)element_base.content;
              // // question
              // {
              //   var text_field = trfm_inst.Find("question/text").GetComponent<TMP_Text>();
              //   // alignment horizontal
              //   {
              //     switch (content.question.alignment_horizontal)
              //     {
              //       case Creator.Element.ALIGNMENT_HORIZONTAL.LEFT:
              //         text_field.horizontalAlignment = HorizontalAlignmentOptions.Left;
              //         break;
              //       case Creator.Element.ALIGNMENT_HORIZONTAL.CENTER:
              //         text_field.horizontalAlignment = HorizontalAlignmentOptions.Center;
              //         break;
              //       case Creator.Element.ALIGNMENT_HORIZONTAL.RIGHT:
              //         text_field.horizontalAlignment = HorizontalAlignmentOptions.Right;
              //         break;
              //     }
              //   }
              //   // margin
              //   {
              //     var layout = trfm_inst.GetComponent<HorizontalOrVerticalLayoutGroup>();
              //     layout.padding.left = content.question.spacing.left;
              //     layout.padding.right = content.question.spacing.right;
              //     layout.padding.top = content.question.spacing.top;
              //     layout.padding.bottom = content.question.spacing.bottom;
              //   }
              //   // color
              //   {
              //     ColorUtility.TryParseHtmlString(content.question.color_hex, out var color);
              //     text_field.color = color;
              //   }
              //   // font size
              //   {
              //     text_field.fontSize = content.question.font_size;
              //   }
              //   // text content
              //   {
              //     text_field.text = content.question.text_content;
              //   }
              //   // style
              //   {
              //     var style = FontStyles.Normal;
              //     if (content.question.bold_enabled)
              //       style |= FontStyles.Bold;
              //     if (content.question.italic_enabled)
              //       style |= FontStyles.Italic;
              //     if (content.question.underline_enabled)
              //       style |= FontStyles.Underline;
              //     text_field.fontStyle = style;
              //   }
              // }
              // // options
              // {
              //   // margin
              //   {
              //     var layout = trfm_inst.Find("options").GetComponent<HorizontalOrVerticalLayoutGroup>();
              //     layout.padding.left = content.options.spacing.left;
              //     layout.padding.right = content.options.spacing.right;
              //     layout.padding.top = content.options.spacing.top;
              //     layout.padding.bottom = content.options.spacing.bottom;
              //   }
              //   // list
              //   {
              //     var options_parent = trfm_inst.Find("options");
              //     options_parent.GetChild(0).gameObject.SetActive(false);
              //     for (int i = 1; i < options_parent.childCount; i++) GameObject.Destroy(options_parent.GetChild(i).gameObject);
              //     foreach (var option in content.options.list)
              //     {
              //       var trfm_option = GameObject.Instantiate(options_parent.GetChild(0), options_parent);
              //       trfm_option.gameObject.SetActive(true);
              //       trfm_option.Find("text").GetComponent<TMP_Text>().text = option.text;
              //     }
              //   }
              // }
              // // events
              // {
              //   var button = trfm_inst.GetComponent<ButtonC>();
              //   button.onClick.AddListener(() =>
              //   {
              //     ManagerApp.Select_Element(element_base);
              //     UIScene.Instance.tools_right.Update_All();
              //     select_frame();
              //   });
              // }
              // break;
            }
          case Creator.Element.TYPE.AI_PROMPT:
            {
              // var trfm_inst = GameObject.Instantiate(ELEMENTS.AI_PROMPT, content_parent).transform;
              // trfm_inst.gameObject.SetActive(true);
              // trfm_inst.name = element_base.key.ToString();
              // // events
              // {
              //   var button = trfm_inst.GetComponent<ButtonC>();
              //   button.onClick.AddListener(() =>
              //   {
              //     ManagerApp.Select_Element(element_base);
              //     UIScene.Instance.tools_right.Update_All();
              //     select_frame();
              //   });
              // }
              break;
            }
          case Creator.Element.TYPE.CHALLENGE_INPUT_NORMAL:
            {
              var element = (Creator.Element.CHALLENGE_INPUT_NORMAL)element_base;
              var trfm_inst = GameObject.Instantiate(ELEMENTS.CHALLENGE_INPUT_NORMAL, content_parent).transform;
              trfm_inst.gameObject.SetActive(true);
              trfm_inst.name = element_base.key.ToString();
              element.Insert_In_Page(trfm_inst);
              
              var button = trfm_inst.GetComponent<ButtonC>();
              button.onClick.AddListener(() =>
              {
                ManagerApp.Select_Element(element);
                UIScene.Instance.tools_right.Update_All();
                update_frame_selector();
              });
              break;
            }
          case Creator.Element.TYPE.GENERAL_WARNING:
            {
              var element = (Creator.Element.GENERAL_WARNING)element_base;
              var trfm_inst = GameObject.Instantiate(ELEMENTS.GENERAL_WARNING, content_parent).transform;
              trfm_inst.gameObject.SetActive(true);
              trfm_inst.name = element.key.ToString();
              element.Insert_In_Page(trfm_inst);
              // events
              {
                var button = trfm_inst.GetComponent<ButtonC>();
                button.onClick.AddListener(() =>
                {
                  ManagerApp.Select_Element(element);
                  UIScene.Instance.tools_right.Update_All();
                  update_frame_selector();
                });
              }
              break;
              // var trfm_inst = GameObject.Instantiate(ELEMENTS.GENERAL_WARNING, content_parent).transform;
              // trfm_inst.gameObject.SetActive(true);
              // trfm_inst.name = element_base.key.ToString();
              // var content = (Creator.Element.GENERAL_WARNING)element_base.content;
              // var text_field = trfm_inst.Find("text").GetComponent<TMP_Text>();
              // // margin
              // {
              //   var layout = trfm_inst.GetComponent<HorizontalOrVerticalLayoutGroup>();
              //   layout.padding.left = content.spacing.left;
              //   layout.padding.right = content.spacing.right;
              //   layout.padding.top = content.spacing.top;
              //   layout.padding.bottom = content.spacing.bottom;
              // }
              // // color
              // {
              //   ColorUtility.TryParseHtmlString(content.color_hex, out var color);
              //   text_field.color = color;
              //   text_field.transform.Find("icon").GetComponent<Image>().color = color;
              // }
              // // text content
              // {
              //   text_field.text = $"\t{content.text_content}";
              // }
              // // style
              // {
              //   var style = FontStyles.Normal;
              //   if (content.bold_enabled)
              //     style |= FontStyles.Bold;
              //   if (content.italic_enabled)
              //     style |= FontStyles.Italic;
              //   if (content.underline_enabled)
              //     style |= FontStyles.Underline;
              //   text_field.fontStyle = style;
              // }
              // // events
              // {
              //   var button = trfm_inst.GetComponent<ButtonC>();
              //   button.onClick.AddListener(() =>
              //   {
              //     ManagerApp.Select_Element(element_base);
              //     UIScene.Instance.tools_right.Update_All();
              //     select_frame();
              //   });
              // }
            }
          case Creator.Element.TYPE.SYSTEM_QUIZ_RESULT:
            {
              var trfm_inst = GameObject.Instantiate(ELEMENTS.SYSTEM_QUIZ_RESULT, content_parent).transform;
              trfm_inst.gameObject.SetActive(true);
              trfm_inst.name = element_base.key.ToString();
              break;
            }
          case Creator.Element.TYPE.GENERAL_TTS:
            {
              // var trfm_inst = GameObject.Instantiate(ELEMENTS.GENERAL_TTS, content_parent).transform;
              // trfm_inst.gameObject.SetActive(true);
              // trfm_inst.name = element_base.key.ToString();
              // var content = (Creator.Element.GENERAL_TTS)element_base.content;
              // var text_field = trfm_inst.Find("text").GetComponent<TMP_Text>();
              // // text content
              // {
              //   text_field.text = content.text;
              // }
              // // events
              // {
              //   var button = trfm_inst.GetComponent<ButtonC>();
              //   button.onClick.AddListener(() =>
              //   {
              //     ManagerApp.Select_Element(element_base);
              //     UIScene.Instance.tools_right.Update_All();
              //     select_frame();
              //   });
              // }
              break;
            }
          case Creator.Element.TYPE.EMPTY:
            break;
          case Creator.Element.TYPE.SYSTEM_QUIZ_ANSWERS:
            {
              var trfm_inst = GameObject.Instantiate(ELEMENTS.SYSTEM_QUIZ_ANSWERS, content_parent).transform;
              trfm_inst.gameObject.SetActive(true);
              trfm_inst.name = element_base.key.ToString();
              break;
            }
          case Creator.Element.TYPE.SYSTEM_QUIZZES_MENU:
            {
              var element = (Creator.Element.SYSTEM_QUIZZES_MENU)element_base;
              var trfm_inst = GameObject.Instantiate(ELEMENTS.SYSTEM_QUIZZES_MENU, content_parent).transform;
              trfm_inst.gameObject.SetActive(true);
              trfm_inst.name = element_base.key.ToString();
              element.Insert_In_Page(trfm_inst);
              // events
              {
                var button = trfm_inst.GetComponent<ButtonC>();
                button.onClick.AddListener(() =>
                {
                  ManagerApp.Select_Element(element);
                  UIScene.Instance.tools_right.Update_All();
                  update_frame_selector();
                });
              }
              break;
            }
          default:
            break;
        }
      }
      // put empty in sibling last
      {
        GameObject.Instantiate(ELEMENTS.EMPTY, content_parent);
      }
      update_frame_selector();
      void update_frame_selector()
      {
        foreach (Transform child in content_parent)
        {
          if (child.TryGetComponent<ButtonC>(out var button))
          {
            button.SetSkinByName(
              ManagerApp.element_selected?.key.ToString() != button.transform.name
              ? "default"
              : "selected"
            );
          }
        }
      }
    }
  }
}