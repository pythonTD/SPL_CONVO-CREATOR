using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[Serializable]
public class UIElementClicked
{
  public Transform transform = null;
  public void SetActive(bool value) => transform.gameObject.SetActive(value);
  public bool ActiveSelf => transform.gameObject.activeSelf;
  public int element_current_key { get; private set; } = 0;
  public Creator.Element element_current { get; private set; } = null;
  private ScrollRect scroll_rect = null;
  public void Awake()
  {
    scroll_rect = transform.Find("scroll").GetComponent<ScrollRect>();
    Set_Element_Current(0);
  }
  public void Set_Element_Current(int key)
  {
    // element_current_key = key;
    // if (element_current_key != 0) element_current
    // = GameData.elements.Find(m => m.key == key);
    // else element_current
    // = null;
    Update_All();
  }
  public void Update_All()
  {
    // reset
    {
      foreach (Transform item in scroll_rect.content)
        item.gameObject.SetActive(false);
    }
    var element = element_current;
    if (element != null)
    {
      // switch (element.type)
      // {
      //   case Creator.Element.TYPE.TEXT:
      //     {
      //       var trfm_inst = scroll_rect.content.Find("TEXT");
      //       trfm_inst.gameObject.SetActive(true);
      //       var content = (Creator.Element.TEXT)element.content;
      //       // alignment horizontal
      //       {
      //         var dropdown = trfm_inst.Find("alignment_horizontal/dropdown").GetComponent<DropdownA>();
      //         dropdown.ClearOptions();
      //         foreach (Creator.TEXT_ALIGNMENT_HORIZONTAL item in Enum.GetValues(typeof(Creator.TEXT_ALIGNMENT_HORIZONTAL)))
      //           dropdown.AddOption(new(item.ToString()) { key = (int)item });
      //         dropdown.SelectByKey((int)content.alignment_horizontal);
      //         dropdown.on_option_clicked = (index, data) =>
      //         {
      //           content.alignment_horizontal = (Creator.TEXT_ALIGNMENT_HORIZONTAL)data.key;
      //           ManagerSceneApp.Instance.page_001.Update_All();
      //         };
      //       }
      //       // margin
      //       {
      //         var left = trfm_inst.Find("margin_horizontal/left").GetComponent<TMP_InputField>();
      //         left.text = content.margin.left.ToString();
      //         left.onEndEdit.RemoveAllListeners();
      //         left.onEndEdit.AddListener((value) =>
      //         {
      //           content.margin.left = int.Parse(value);
      //           ManagerSceneApp.Instance.page_001.Update_All();
      //         });
      //         var right = trfm_inst.Find("margin_horizontal/right").GetComponent<TMP_InputField>();
      //         right.text = content.margin.right.ToString();
      //         right.onEndEdit.RemoveAllListeners();
      //         right.onEndEdit.AddListener((value) =>
      //         {
      //           content.margin.right = int.Parse(value);
      //           ManagerSceneApp.Instance.page_001.Update_All();
      //         });
      //         var top = trfm_inst.Find("margin_vertical/top").GetComponent<TMP_InputField>();
      //         top.text = content.margin.top.ToString();
      //         top.onEndEdit.RemoveAllListeners();
      //         top.onEndEdit.AddListener((value) =>
      //         {
      //           content.margin.top = int.Parse(value);
      //           ManagerSceneApp.Instance.page_001.Update_All();
      //         });
      //         var bottom = trfm_inst.Find("margin_vertical/bottom").GetComponent<TMP_InputField>();
      //         bottom.text = content.margin.bottom.ToString();
      //         bottom.onEndEdit.RemoveAllListeners();
      //         bottom.onEndEdit.AddListener((value) =>
      //         {
      //           content.margin.bottom = int.Parse(value);
      //           ManagerSceneApp.Instance.page_001.Update_All();
      //         });
      //       }
      //       // color
      //       {
      //         ColorUtility.TryParseHtmlString(content.color_hex, out var color);
      //         trfm_inst.Find("color/color/frame").GetComponent<Image>().color = color;
      //         trfm_inst.Find("color/color/alpha").GetComponent<Image>().fillAmount = color.a;
      //         var input_field = trfm_inst.Find("color/input_field").GetComponent<TMP_InputField>();
      //         input_field.text = content.color_hex.ToString();
      //         input_field.onEndEdit.RemoveAllListeners();
      //         input_field.onEndEdit.AddListener((value) =>
      //         {
      //           content.color_hex = value;
      //           Update_All();
      //           ManagerSceneApp.Instance.page_001.Update_All();
      //         });
      //       }
      //       // font size
      //       {
      //         var input_field = trfm_inst.Find("font_size/input_field").GetComponent<TMP_InputField>();
      //         input_field.text = content.font_size.ToString();
      //         input_field.onEndEdit.RemoveAllListeners();
      //         input_field.onEndEdit.AddListener((value) =>
      //         {
      //           content.font_size = int.Parse(value);
      //           ManagerSceneApp.Instance.page_001.Update_All();
      //         });
      //       }
      //       // text content
      //       {
      //         var text_field = trfm_inst.Find("text/input_field").GetComponent<TMP_InputField>();
      //         text_field.text = content.text_content.ToString();
      //         text_field.onEndEdit.RemoveAllListeners();
      //         text_field.onEndEdit.AddListener((value) =>
      //         {
      //           content.text_content = value;
      //           ManagerSceneApp.Instance.page_001.Update_All();
      //         });
      //       }
      //       // buttons
      //       {
      //         // deselect
      //         {
      //           var button = trfm_inst.Find("buttons/deselect").GetComponent<ButtonA>();
      //           button.onClick.RemoveAllListeners();
      //           button.onClick.AddListener(() =>
      //           {
      //             Set_Element_Current(0);
      //             ManagerSceneApp.Instance.page_001.Update_All();
      //           });
      //         }
      //         // // move up
      //         // {
      //         //   var button = trfm_inst.Find("buttons/move_up").GetComponent<ButtonA>();
      //         //   button.onClick.RemoveAllListeners();
      //         //   button.onClick.AddListener(() =>
      //         //   {
      //         //     var index = GameData.elements.IndexOf(element);
      //         //     if (index <= 0) return;
      //         //     var item_1 = GameData.elements[index - 1];
      //         //     var item_2 = GameData.elements[index];
      //         //     GameData.elements[index - 1] = item_2;
      //         //     GameData.elements[index] = item_1;
      //         //     GameData.Save();
      //         //     ManagerSceneApp.Instance.page_001.Update_All();
      //         //   });
      //         // }
      //         // // move down
      //         // {
      //         //   var button = trfm_inst.Find("buttons/move_down").GetComponent<ButtonA>();
      //         //   button.onClick.RemoveAllListeners();
      //         //   button.onClick.AddListener(() =>
      //         //   {
      //         //     var index = GameData.elements.IndexOf(element);
      //         //     if (index + 1 >= GameData.elements.Count) return;
      //         //     var item_1 = GameData.elements[index + 1];
      //         //     var item_2 = GameData.elements[index];
      //         //     GameData.elements[index + 1] = item_2;
      //         //     GameData.elements[index] = item_1;
      //         //     GameData.Save();
      //         //     ManagerSceneApp.Instance.page_001.Update_All();
      //         //   });
      //         // }
      //         // // delete
      //         // {
      //         //   var button = trfm_inst.Find("buttons/delete").GetComponent<ButtonA>();
      //         //   button.onClick.RemoveAllListeners();
      //         //   button.onClick.AddListener(() =>
      //         //   {
      //         //     Set_Element_Current(0);
      //         //     GameData.elements.Remove(element);
      //         //     GameData.Save();
      //         //     ManagerSceneApp.Instance.page_001.Update_All();
      //         //   });
      //         // }
      //       }
      //       break;
      //     }
      //   case Creator.Element.TYPE.CODE_COMMAND:
      //     break;
      //   case Creator.Element.TYPE.LINE:
      //     break;
      //   case Creator.Element.TYPE.IMAGE:
      //     break;
      //   case Creator.Element.TYPE.CODE_SCRIPT:
      //     break;
      //   case Creator.Element.TYPE.EMPTY:
      //     break;
      //   case Creator.Element.TYPE.MULTIPLE_CHOICE:
      //     break;
      //   case Creator.Element.TYPE.AI_SUMMARY_CHALLENGE:
      //     {
      //       var trfm_inst = scroll_rect.content.Find("AI_SUMMARY_CHALLENGE");
      //       trfm_inst.gameObject.SetActive(true);
      //       var content = (Creator.Element.AI_SUMMARY_CHALLENGE)element.content;
      //       // rol
      //       {
      //         var input_field = trfm_inst.Find("rol/input_field").GetComponent<TMP_InputField>();
      //         input_field.text = content.rol_prompt.ToString();
      //         input_field.onEndEdit.RemoveAllListeners();
      //         input_field.onEndEdit.AddListener((value) =>
      //         {
      //           content.rol_prompt = value;
      //           GameData.Save_All();
      //           ManagerSceneApp.Instance.page_001.Update_All();
      //         });
      //       }
      //       // user
      //       {
      //         var input_field = trfm_inst.Find("user/input_field").GetComponent<TMP_InputField>();
      //         input_field.text = content.user_prompt.ToString();
      //         input_field.onEndEdit.RemoveAllListeners();
      //         input_field.onEndEdit.AddListener((value) =>
      //         {
      //           content.user_prompt = value;
      //           GameData.Save_All();
      //           ManagerSceneApp.Instance.page_001.Update_All();
      //         });
      //       }
      //       // // buttons
      //       // {
      //       //   // deselect
      //       //   {
      //       //     var button = trfm_inst.Find("buttons/deselect").GetComponent<ButtonA>();
      //       //     button.onClick.RemoveAllListeners();
      //       //     button.onClick.AddListener(() =>
      //       //     {
      //       //       Set_Element_Current(0);
      //       //       ManagerSceneApp.Instance.page_001.Update_All();
      //       //     });
      //       //   }
      //       //   // move up
      //       //   {
      //       //     var button = trfm_inst.Find("buttons/move_up").GetComponent<ButtonA>();
      //       //     button.onClick.RemoveAllListeners();
      //       //     button.onClick.AddListener(() =>
      //       //     {
      //       //       var index = GameData.elements.IndexOf(element);
      //       //       if (index <= 0) return;
      //       //       var item_1 = GameData.elements[index - 1];
      //       //       var item_2 = GameData.elements[index];
      //       //       GameData.elements[index - 1] = item_2;
      //       //       GameData.elements[index] = item_1;
      //       //       GameData.Save();
      //       //       ManagerSceneApp.Instance.page_001.Update_All();
      //       //     });
      //       //   }
      //       //   // move down
      //       //   {
      //       //     var button = trfm_inst.Find("buttons/move_down").GetComponent<ButtonA>();
      //       //     button.onClick.RemoveAllListeners();
      //       //     button.onClick.AddListener(() =>
      //       //     {
      //       //       var index = GameData.elements.IndexOf(element);
      //       //       if (index + 1 >= GameData.elements.Count) return;
      //       //       var item_1 = GameData.elements[index + 1];
      //       //       var item_2 = GameData.elements[index];
      //       //       GameData.elements[index + 1] = item_2;
      //       //       GameData.elements[index] = item_1;
      //       //       GameData.Save();
      //       //       ManagerSceneApp.Instance.page_001.Update_All();
      //       //     });
      //       //   }
      //       //   // delete
      //       //   {
      //       //     var button = trfm_inst.Find("buttons/delete").GetComponent<ButtonA>();
      //       //     button.onClick.RemoveAllListeners();
      //       //     button.onClick.AddListener(() =>
      //       //     {
      //       //       Set_Element_Current(0);
      //       //       GameData.elements.Remove(element);
      //       //       GameData.Save();
      //       //       ManagerSceneApp.Instance.page_001.Update_All();
      //       //     });
      //       //   }
      //       // }
      //       break;
          // }
      // }
    }
  }
}