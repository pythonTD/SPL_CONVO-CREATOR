using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Creator;
[Serializable]
public class UIToolsLeft
{
  public Transform transform = null;
  public TAB tab_current = TAB.PAGES;
  public UIPages pages = new();
  public UIElements elements = new();
  private ButtonC button_tab_elements = null;
  private ButtonC button_tab_pages = null;
  public void Awake()
  {
    pages.Awake();
    elements.Awake();
    button_tab_elements = transform.Find("tabs/elements").GetComponent<ButtonC>();
    button_tab_pages = transform.Find("tabs/pages").GetComponent<ButtonC>();
    button_tab_elements.onClick.AddListener(() => Tab_Select(TAB.ELEMENTS));
    button_tab_pages.onClick.AddListener(() => Tab_Select(TAB.PAGES));
    // tooltip
    {
      var tooltip_data = new UITooltips.GUI_001.DATA();
      {
        tooltip_data.game_object = button_tab_pages.gameObject;
        tooltip_data.transform_position = button_tab_pages.transform;
        tooltip_data.text = $"Pages";
        tooltip_data.anchor = TextAnchor.UpperCenter;
        tooltip_data.spacing = new(0, -20);
      }
      UITooltips.GUI_001.AddListener(tooltip_data);
    }
    // tooltip
    {
      var tooltip_data = new UITooltips.GUI_001.DATA();
      {
        tooltip_data.game_object = button_tab_elements.gameObject;
        tooltip_data.transform_position = button_tab_elements.transform;
        tooltip_data.text = $"Elements";
        tooltip_data.anchor = TextAnchor.UpperCenter;
        tooltip_data.spacing = new(0, -20);
      }
      UITooltips.GUI_001.AddListener(tooltip_data);
    }
  }
  public void Start()
  {
    Tab_Select(tab_current);
  }
  public void Update()
  {
    elements.Update();
  }
  public void Update_All()
  {
    var class_current = GameData.Class_Current;
    transform.gameObject.SetActive(class_current != null);
    if (class_current == null) return;
    pages.Update_All();
  }
  public void Tab_Select(TAB tab)
  {
    tab_current = tab;
    switch (tab_current)
    {
      case TAB.ELEMENTS:
        button_tab_pages.SetSkinByName("off");
        button_tab_elements.SetSkinByName("on");
        pages.Quit();
        elements.Start();
        break;
      case TAB.PAGES:
        button_tab_elements.SetSkinByName("off");
        button_tab_pages.SetSkinByName("on");
        elements.Quit();
        pages.Start();

        break;
    }
    Update_All();
  }
  public enum TAB { ELEMENTS, PAGES }
  [Serializable]
  public class UIPages
  {
    public Transform transform = null;
    public void SetActive(bool value) => transform.gameObject.SetActive(value);
    public bool ActiveSelf => transform.gameObject.activeSelf;
    private ScrollRect scroll_rect = null;
    private Class class_current => GameData.Class_Current;
    public struct ELEMENTS
    {
      public static GameObject LESSON => Resources.Load<GameObject>("Prefabs/Elements_ToolsLeft_Pages/LESSON");
      public static GameObject LESSON_PAGE => Resources.Load<GameObject>("Prefabs/Elements_ToolsLeft_Pages/LESSON_PAGE");
      public static GameObject LESSON_PAGE_NEW => Resources.Load<GameObject>("Prefabs/Elements_ToolsLeft_Pages/LESSON_PAGE_NEW");
      public static GameObject QUIZ => Resources.Load<GameObject>("Prefabs/Elements_ToolsLeft_Pages/QUIZ");
      public static GameObject QUIZ_PAGE => Resources.Load<GameObject>("Prefabs/Elements_ToolsLeft_Pages/QUIZ_PAGE");
      public static GameObject QUIZ_PAGE_NEW => Resources.Load<GameObject>("Prefabs/Elements_ToolsLeft_Pages/QUIZ_PAGE_NEW");
      public static GameObject QUIZ_PAGE_RESULT => Resources.Load<GameObject>("Prefabs/Elements_ToolsLeft_Pages/QUIZ_PAGE_RESULT");
      public static GameObject QUIZ_PAGE_ANSWERS => Resources.Load<GameObject>("Prefabs/Elements_ToolsLeft_Pages/QUIZ_PAGE_ANSWERS");
      public static GameObject QUIZ_PAGE_MENU => Resources.Load<GameObject>("Prefabs/Elements_ToolsLeft_Pages/QUIZ_PAGE_MENU");
    }
    ButtonC btn_setting_upload_background;

    public void Awake()
    {
      scroll_rect = transform.GetComponent<ScrollRect>();
      // add lesson
      {
        var button = scroll_rect.content.Find("lessons_title/add").GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
          UIScene.Instance.popup_input.Show(
            $"Create New Lesson In Class \"{class_current.name}\"",
            "",
            (value) =>
            {
              value = value.Trim();
              if (value == string.Empty) return;
              // create lesson
              {
                class_current.Lessons.Add(new()
                {
                  name = value
                });
                GameData.Save_All();
                Update_All();
              }
              UIScene.Instance.popup_input.Hide();
            },
            () =>
            {
              UIScene.Instance.popup_input.Hide();
            }
          );
        });
        // tooltip
        {
          var tooltip_data = new UITooltips.GUI_001.DATA();
          {
            tooltip_data.game_object = button.gameObject;
            tooltip_data.transform_position = button.transform;
            tooltip_data.text = $"Add Lesson";
            tooltip_data.anchor = TextAnchor.LowerCenter;
            tooltip_data.spacing = new(0, 15);
          }
          UITooltips.GUI_001.AddListener(tooltip_data);
        }
      }
      // add quizz
      {
        var button = scroll_rect.content.Find("quizzes_title/add").GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
          UIScene.Instance.popup_input.Show(
            $"Create New Quiz In Class \"{class_current.name}\"",
            "",
            (value) =>
            {
              value = value.Trim();
              if (value == string.Empty) return;
              // create quiz
              {
                class_current.Quizzes.Add(new()
                {
                  name = value
                });
                GameData.Save_All();
                Update_All();
              }
              UIScene.Instance.popup_input.Hide();
            },
            () =>
            {
              UIScene.Instance.popup_input.Hide();
            }
          );
        });
        // tooltip
        {
          var tooltip_data = new UITooltips.GUI_001.DATA();
          {
            tooltip_data.game_object = button.gameObject;
            tooltip_data.transform_position = button.transform;
            tooltip_data.text = $"Add Quiz";
            tooltip_data.anchor = TextAnchor.LowerCenter;
            tooltip_data.spacing = new(0, 15);
          }
          UITooltips.GUI_001.AddListener(tooltip_data);
        }
      }
      //setting
      {
        btn_setting_upload_background = scroll_rect.content.Find("setting_upload_background").GetComponent<ButtonC>();
        btn_setting_upload_background.onClick.AddListener(() =>
        {
          ManagerApp.Clear_All_Selected();
          btn_setting_upload_background.SetSkinByName("selected");
          UIScene.Instance.quiz_setting.Show(SETTING_MODE.UPLOAD_BACKGROUND);
          ManagerApp.Select_Page(null);
          ManagerApp.Select_Lesson(null);
          Update_All();
        });
      }
    }
    public void Start()
    {
      SetActive(true);
    }
    public void Quit()
    {
      SetActive(false);
    }
    public void Update_All()
    {
      UIScene.Instance.page.transform.gameObject.SetActive(ManagerApp.page_selected != null || ManagerApp.lesson_selected != null);
      UIScene.Instance.quiz_setting.transform.gameObject.SetActive(ManagerApp.setting_selected != null);
      // lessons
      {
        var parent_lessons = scroll_rect.content.Find("lessons");
        for (int i = 0; i < parent_lessons.childCount; i++) GameObject.Destroy(parent_lessons.GetChild(i).gameObject);
        foreach (var lesson in class_current.Lessons)
        {
          var trfm_inst_lesson = GameObject.Instantiate(ELEMENTS.LESSON, parent_lessons).transform;
          trfm_inst_lesson.Find("name").GetComponent<TMP_Text>().text = $"\t{lesson.name}";
          // edit
          {
            var dropdown = trfm_inst_lesson.Find("name/edit").GetComponent<DropdownB>();
            dropdown.ClearOptions();
            dropdown.AddOption(new("Edit Name...")
            {
              action = () =>
              {
                UIScene.Instance.popup_input.Show(
                  $"Edit Name Lesson \"{lesson.name}\"",
                  $"{lesson.name}",
                  (value) =>
                  {
                    value = value.Trim();
                    if (value == string.Empty) return;
                    lesson.name = value;
                    GameData.invoke_on_class_current_updated();
                    UIScene.Instance.popup_input.Hide();
                  },
                  () =>
                  {
                    UIScene.Instance.popup_input.Hide();
                  }
                );
              }
            });
            dropdown.AddOption(new("Move Up...")
            {
              action = () =>
              {
                var index = GameData.Class_Current.Lessons.IndexOf(lesson);
                if (index <= 0) return;
                var item_1 = GameData.Class_Current.Lessons[index - 1];
                var item_2 = GameData.Class_Current.Lessons[index];
                GameData.Class_Current.Lessons[index - 1] = item_2;
                GameData.Class_Current.Lessons[index] = item_1;
                GameData.invoke_on_class_current_updated();
              }
            });
            dropdown.AddOption(new("Move Down...")
            {
              action = () =>
              {
                var index = GameData.Class_Current.Lessons.IndexOf(lesson);
                if (index + 1 >= GameData.Class_Current.Lessons.Count) return;
                var item_1 = GameData.Class_Current.Lessons[index + 1];
                var item_2 = GameData.Class_Current.Lessons[index];
                GameData.Class_Current.Lessons[index + 1] = item_2;
                GameData.Class_Current.Lessons[index] = item_1;
                GameData.invoke_on_class_current_updated();
              }
            });
            dropdown.AddOption(new("Duplicate Lesson...")
            {
              action = () =>
              {
                var item_duplicated = lesson;
                var item_new = new Class.Lesson
                {
                  name = item_duplicated.name
                };
                // pages
                {
                  foreach (var page_duplicated in item_duplicated.pages)
                  {
                    var page_new = new Page();
                    page_new.name = page_duplicated.name;
                    foreach (var element_duplicated in page_duplicated.elements)
                    {
                      var element_cloned = element_duplicated.Clone();
                      element_cloned.Keys_CopyBy(null);
                      page_new.elements.Add(element_cloned);
                    }
                    item_new.pages.Add(page_new);
                  }
                }
                var list = GameData.Class_Current.Lessons;
                list.Insert(list.IndexOf(item_duplicated) + 1, item_new);
                GameData.invoke_on_class_current_updated();
              }
            });
            dropdown.AddOption(new("Delete Lesson...")
            {
              action = () =>
              {
                class_current.Lessons.Remove(lesson);
                if (ManagerApp.lesson_selected == lesson)
                  ManagerApp.Clear_All_Selected();
                GameData.invoke_on_class_current_updated();
              }
            });
            // tooltip
            {
              var tooltip_data = new UITooltips.GUI_001.DATA();
              {
                tooltip_data.game_object = dropdown.transform.Find("header").gameObject;
                tooltip_data.transform_position = dropdown.transform.Find("header").transform;
                tooltip_data.text = $"Edit Lesson";
                tooltip_data.anchor = TextAnchor.LowerCenter;
                tooltip_data.spacing = new(0, 15);
              }
              UITooltips.GUI_001.AddListener(tooltip_data);
            }
          }
          // pages
          {
            var parent_pages = trfm_inst_lesson.Find("pages");
            for (int i = 0; i < parent_pages.childCount; i++) GameObject.Destroy(parent_pages.GetChild(i).gameObject);
            foreach (var page in lesson.pages)
            {
              var trfm_inst_content = GameObject.Instantiate(ELEMENTS.LESSON_PAGE, parent_pages).transform;
              trfm_inst_content.Find("name").GetComponent<TMP_Text>().text = $"\t\t{page.name}";
              // events
              {
                var button = trfm_inst_content.GetComponent<ButtonC>();
                button.SetSkinByName(
                  ManagerApp.page_selected == page
                  ? "selected"
                  : "default"
                );
                button.onClick.AddListener(() =>
                {
                  if (ManagerApp.page_selected == page) return;
                  ManagerApp.Clear_All_Selected();
                  ManagerApp.Select_Lesson(lesson);
                  ManagerApp.Select_Page(page);
                  Update_All();
                  UIScene.Instance.page.Update_All();
                  UIScene.Instance.tools_right.Update_All();
                });
              }
              // edit
              {
                var dropdown = trfm_inst_content.Find("name/edit").GetComponent<DropdownB>();
                dropdown.ClearOptions();
                dropdown.AddOption(new("Edit Name...")
                {
                  action = () =>
                  {
                    UIScene.Instance.popup_input.Show(
                      $"Edit Name Content \"{page.name}\"",
                      $"{page.name}",
                      (value) =>
                      {
                        value = value.Trim();
                        if (value == string.Empty) return;
                        page.name = value;
                        GameData.invoke_on_class_current_updated();
                        UIScene.Instance.popup_input.Hide();
                      },
                      () =>
                      {
                        UIScene.Instance.popup_input.Hide();
                      }
                    );
                  }
                });
                dropdown.AddOption(new("Move Up...")
                {
                  action = () =>
                  {
                    var index = lesson.pages.IndexOf(page);
                    if (index <= 0) return;
                    var item_1 = lesson.pages[index - 1];
                    var item_2 = lesson.pages[index];
                    lesson.pages[index - 1] = item_2;
                    lesson.pages[index] = item_1;
                    GameData.invoke_on_class_current_updated();
                  }
                });
                dropdown.AddOption(new("Move Down...")
                {
                  action = () =>
                  {
                    var index = lesson.pages.IndexOf(page);
                    if (index + 1 >= lesson.pages.Count) return;
                    var item_1 = lesson.pages[index + 1];
                    var item_2 = lesson.pages[index];
                    lesson.pages[index + 1] = item_2;
                    lesson.pages[index] = item_1;
                    GameData.invoke_on_class_current_updated();
                  }
                });
                dropdown.AddOption(new("Duplicate Content...")
                {
                  action = () =>
                  {
                    var page_duplicated = page;
                    var page_new = new Page()
                    {
                      name = page_duplicated.name
                    };
                    foreach (var element_duplicated in page_duplicated.elements)
                    {
                      var element_cloned = element_duplicated.Clone();
                      element_cloned.Keys_CopyBy(null);
                      page_new.elements.Add(element_cloned);
                    }
                    var list = lesson.pages;
                    list.Insert(list.IndexOf(page_duplicated) + 1, page_new);
                    GameData.invoke_on_class_current_updated();
                  }
                });
                dropdown.AddOption(new("Delete Content...")
                {
                  action = () =>
                  {
                    lesson.pages.Remove(page);
                    if (ManagerApp.page_selected == page)
                      ManagerApp.Clear_All_Selected();
                    GameData.invoke_on_class_current_updated();
                  }
                });
                // tooltip
                {
                  var tooltip_data = new UITooltips.GUI_001.DATA();
                  {
                    tooltip_data.game_object = dropdown.transform.Find("header").gameObject;
                    tooltip_data.transform_position = dropdown.transform.Find("header").transform;
                    tooltip_data.text = $"Edit Content";
                    tooltip_data.anchor = TextAnchor.LowerCenter;
                    tooltip_data.spacing = new(0, 15);
                  }
                  UITooltips.GUI_001.AddListener(tooltip_data);
                }
              }
            }
            // add
            {
              var trfm_inst = GameObject.Instantiate(ELEMENTS.LESSON_PAGE_NEW, parent_pages).transform;
              trfm_inst.GetComponent<Button>().onClick.AddListener(() =>
              {
                UIScene.Instance.popup_input.Show(
                  $"Create New Content In Lesson \"{lesson.name}\"",
                  "",
                  (value) =>
                  {
                    value = value.Trim();
                    if (value == string.Empty) return;
                    // create page
                    {
                      lesson.pages.Add(new()
                      {
                        name = value
                      });
                      GameData.invoke_on_class_current_updated();
                    }
                    UIScene.Instance.popup_input.Hide();
                  },
                  () =>
                  {
                    UIScene.Instance.popup_input.Hide();
                  }
                );
              });
            }
          }
        }
      }
      // quizzes
      {
        // quiz menu
        {
          var trfm_inst = scroll_rect.content.Find("quizzes_menu");
          // events
          {
            var button = trfm_inst.GetComponent<ButtonC>();

            button.SetSkinByName(
              ManagerApp.page_selected == class_current.Quizzes_Menu
              ? "selected"
              : "default"
            );
            button.onClick.AddListener(() =>
            {
              if (ManagerApp.page_selected == class_current.Quizzes_Menu) return;
              ManagerApp.Clear_All_Selected();
              ManagerApp.Select_Page(class_current.Quizzes_Menu);
              Update_All();
              UIScene.Instance.page.Update_All();
              UIScene.Instance.tools_right.Update_All();
            });
          }
        }
        var parent_quizzes = scroll_rect.content.Find("quizzes");
        for (int i = 0; i < parent_quizzes.childCount; i++) GameObject.Destroy(parent_quizzes.GetChild(i).gameObject);
        foreach (var quiz in class_current.Quizzes)
        {
          var trfm_inst_quiz = GameObject.Instantiate(ELEMENTS.QUIZ, parent_quizzes).transform;
          trfm_inst_quiz.Find("name").GetComponent<TMP_Text>().text = $"\t{quiz.name}";
          // edit
          {
            var dropdown = trfm_inst_quiz.Find("name/edit").GetComponent<DropdownB>();
            dropdown.ClearOptions();
            dropdown.AddOption(new("Edit Name...")
            {
              action = () =>
              {
                UIScene.Instance.popup_input.Show(
                  $"Edit Name Quiz \"{quiz.name}\"",
                  $"{quiz.name}",
                  (value) =>
                  {
                    value = value.Trim();
                    if (value == string.Empty) return;
                    quiz.name = value;
                    GameData.invoke_on_class_current_updated();
                    UIScene.Instance.popup_input.Hide();
                  },
                  () =>
                  {
                    UIScene.Instance.popup_input.Hide();
                  }
                );
              }
            });
            dropdown.AddOption(new("Move Up...")
            {
              action = () =>
              {
                var index = GameData.Class_Current.Quizzes.IndexOf(quiz);
                if (index <= 0) return;
                var item_1 = GameData.Class_Current.Quizzes[index - 1];
                var item_2 = GameData.Class_Current.Quizzes[index];
                GameData.Class_Current.Quizzes[index - 1] = item_2;
                GameData.Class_Current.Quizzes[index] = item_1;
                GameData.invoke_on_class_current_updated();
              }
            });
            dropdown.AddOption(new("Move Down...")
            {
              action = () =>
              {
                var index = GameData.Class_Current.Quizzes.IndexOf(quiz);
                if (index + 1 >= GameData.Class_Current.Quizzes.Count) return;
                var item_1 = GameData.Class_Current.Quizzes[index + 1];
                var item_2 = GameData.Class_Current.Quizzes[index];
                GameData.Class_Current.Quizzes[index + 1] = item_2;
                GameData.Class_Current.Quizzes[index] = item_1;
                GameData.invoke_on_class_current_updated();
              }
            });
            dropdown.AddOption(new("Duplicate Quiz...")
            {
              action = () =>
              {
                var quiz_duplicated = quiz;
                var quiz_new = new Class.Quiz
                {
                  name = quiz_duplicated.name
                };
                // pages
                {
                  foreach (var page_duplicated in quiz_duplicated.pages)
                  {
                    var page_new = new Page();
                    page_new.name = page_duplicated.name;
                    foreach (var element_duplicated in page_duplicated.elements)
                      page_new.elements.Add(element_duplicated.Clone());
                    quiz_new.pages.Add(page_new);
                  }
                }
                // page result
                {
                  var page_new = new Page();
                  var page_duplicated = quiz_duplicated.page_result;
                  foreach (var element_duplicated in page_duplicated.elements)
                    page_new.elements.Add(element_duplicated.Clone());
                  quiz_new.page_result = page_new;
                }
                // page answers
                {
                  var page_new = new Page();
                  var page_duplicated = quiz_duplicated.page_answers;
                  foreach (var element_duplicated in page_duplicated.elements)
                    page_new.elements.Add(element_duplicated.Clone());
                  quiz_new.page_answers = page_new;
                }
                var list = GameData.Class_Current.Quizzes;
                list.Insert(list.IndexOf(quiz_duplicated) + 1, quiz_new);
                GameData.invoke_on_class_current_updated();
              }
            });
            dropdown.AddOption(new("Delete Quiz...")
            {
              action = () =>
              {
                class_current.Quizzes.Remove(quiz);
                if (ManagerApp.quiz_selected == quiz)
                  ManagerApp.Clear_All_Selected();
                GameData.invoke_on_class_current_updated();
              }
            });
          }
          // pages
          {
            var parent_pages = trfm_inst_quiz.Find("pages");
            for (int i = 0; i < parent_pages.childCount; i++) GameObject.Destroy(parent_pages.GetChild(i).gameObject);
            foreach (var page in quiz.pages)
            {
              var trfm_inst_exercise = GameObject.Instantiate(ELEMENTS.QUIZ_PAGE, parent_pages).transform;
              trfm_inst_exercise.Find("name").GetComponent<TMP_Text>().text
              = $"\t\tQuiz {quiz.pages.IndexOf(page) + 1}";
              // events
              {
                var button = trfm_inst_exercise.GetComponent<ButtonC>();
                button.SetSkinByName(
                  ManagerApp.page_selected == page
                  ? "selected"
                  : "default"
                );
                button.onClick.AddListener(() =>
                {
                  if (ManagerApp.page_selected == page) return;
                  ManagerApp.Clear_All_Selected();
                  ManagerApp.Select_Quiz(quiz);
                  ManagerApp.Select_Page(page);
                  Update_All();
                  UIScene.Instance.page.Update_All();
                  UIScene.Instance.tools_right.Update_All();
                });
              }
              // edit
              {
                var dropdown = trfm_inst_exercise.Find("name/edit").GetComponent<DropdownB>();
                dropdown.ClearOptions();
                dropdown.AddOption(new("Move Up...")
                {
                  action = () =>
                  {
                    var index = quiz.pages.IndexOf(page);
                    if (index <= 0) return;
                    var item_1 = quiz.pages[index - 1];
                    var item_2 = quiz.pages[index];
                    quiz.pages[index - 1] = item_2;
                    quiz.pages[index] = item_1;
                    GameData.invoke_on_class_current_updated();
                  }
                });
                dropdown.AddOption(new("Move Down...")
                {
                  action = () =>
                  {
                    var index = quiz.pages.IndexOf(page);
                    if (index + 1 >= quiz.pages.Count) return;
                    var item_1 = quiz.pages[index + 1];
                    var item_2 = quiz.pages[index];
                    quiz.pages[index + 1] = item_2;
                    quiz.pages[index] = item_1;
                    GameData.invoke_on_class_current_updated();
                  }
                });
                dropdown.AddOption(new("Duplicate Exercise...")
                {
                  action = () =>
                  {
                    var page_duplicated = page;
                    var page_new = new Page()
                    {
                      name = page_duplicated.name
                    };
                    foreach (var element_duplicated in page_duplicated.elements)
                      page_new.elements.Add(element_duplicated.Clone());
                    var list = quiz.pages;
                    list.Insert(list.IndexOf(page_duplicated) + 1, page_new);
                    GameData.invoke_on_class_current_updated();
                  }
                });
                dropdown.AddOption(new("Delete Exercise...")
                {
                  action = () =>
                  {
                    quiz.pages.Remove(page);
                    if (ManagerApp.page_selected == page)
                      ManagerApp.Clear_All_Selected();
                    GameData.invoke_on_class_current_updated();
                  }
                });
              }
            }
            // quiz result
            {
              var trfm_inst = GameObject.Instantiate(ELEMENTS.QUIZ_PAGE_RESULT, parent_pages).transform;
              // events
              {
                var button = trfm_inst.GetComponent<ButtonC>();
                button.SetSkinByName(
                  ManagerApp.page_selected == quiz.page_result
                  ? "selected"
                  : "default"
                );
                button.onClick.AddListener(() =>
                {
                  if (ManagerApp.page_selected == quiz.page_result) return;
                  ManagerApp.Clear_All_Selected();
                  ManagerApp.Select_Quiz(quiz);
                  ManagerApp.Select_Page(quiz.page_result);
                  Update_All();
                  UIScene.Instance.page.Update_All();
                  UIScene.Instance.tools_right.Update_All();
                });
              }
            }
            // quiz answers
            {
              var trfm_inst = GameObject.Instantiate(ELEMENTS.QUIZ_PAGE_ANSWERS, parent_pages).transform;
              // events
              {
                var button = trfm_inst.GetComponent<ButtonC>();
                button.SetSkinByName(
                  ManagerApp.page_selected == quiz.page_answers
                  ? "selected"
                  : "default"
                );
                button.onClick.AddListener(() =>
                {
                  if (ManagerApp.page_selected == quiz.page_answers) return;
                  ManagerApp.Clear_All_Selected();
                  ManagerApp.Select_Quiz(quiz);
                  ManagerApp.Select_Page(quiz.page_answers);
                  Update_All();
                  UIScene.Instance.page.Update_All();
                  UIScene.Instance.tools_right.Update_All();
                });
              }
            }
            // add
            {
              var trfm_inst = GameObject.Instantiate(ELEMENTS.QUIZ_PAGE_NEW, parent_pages).transform;
              trfm_inst.GetComponent<Button>().onClick.AddListener(() =>
              {
                // create exercise
                {
                  quiz.pages.Add(new());
                  GameData.invoke_on_class_current_updated();
                }
              });
            }
          }
        }
      }
      //setting
      {
        bool is_upload = ManagerApp.setting_selected is UI_UploadBackgroundInfo;
        btn_setting_upload_background.SetSkinByName(is_upload
            ? "selected"
            : "default"
        );
        if (!is_upload)
        {
          UIScene.Instance.quiz_setting.Quit();
        }
        else
        {

        }
      }
    }
  }

  [Serializable]
  public class UIElements
  {
    public Transform transform = null;
    public void SetActive(bool value) => transform.gameObject.SetActive(value);
    public bool ActiveSelf => transform.gameObject.activeSelf;
    private ScrollRect scroll_rect = null;
    public GameObject drag_object = null;
    public event EventHandler<GameObject> on_begin_drag = null;
    private void invoke_on_begin_drag(object sender)
    => on_begin_drag?.Invoke(sender, drag_object);
    public event EventHandler<GameObject> on_drag = null;
    private void invoke_on_drag(object sender)
    => on_drag?.Invoke(sender, drag_object);
    public event EventHandler<Creator.Element.TYPE> on_end_drag = null;
    private void invoke_on_end_drag(object sender, Creator.Element.TYPE element_type)
    => on_end_drag?.Invoke(sender, element_type);
    public void Awake()
    {
      scroll_rect = transform.GetComponent<ScrollRect>();
      // reset
      {
        drag_object.SetActive(false);
      }
      // general
      {
        var content_parent = scroll_rect.content.Find("general");
        content_parent.GetChild(0).gameObject.SetActive(false);
        for (int i = 1; i < content_parent.childCount; i++) GameObject.Destroy(content_parent.GetChild(i).gameObject);
        foreach (Element.TYPE_GENERAL type in EnumUtility.GetOrderedValues(typeof(Creator.Element.TYPE_GENERAL)))
        {
          if (type == Creator.Element.TYPE_GENERAL.EMPTY) continue;
          var trfm_inst = GameObject.Instantiate(content_parent.GetChild(0), content_parent);
          trfm_inst.gameObject.SetActive(true);
          trfm_inst.Find("name").GetComponent<TMP_Text>().text = Element.Name((Element.TYPE)type);
          // events
          {
            var button = trfm_inst.GetComponent<ButtonB>();
            button.onBeginDrag.AddListener(() =>
            {
              // begin drag
              {
                drag_object.SetActive(true);
                drag_object.transform.Find("name").GetComponent<TMP_Text>().text = type.ToString();
                button.GetComponent<CanvasGroup>().alpha = 0;
                button.GetComponent<CanvasGroup>().interactable = false;
                button.GetComponent<CanvasGroup>().blocksRaycasts = false;
                invoke_on_begin_drag(this);
              }
            });
            button.onDrag.AddListener(() =>
            {
              invoke_on_drag(this);
            });
            button.onEndDrag.AddListener(() =>
            {
              // end drag
              {
                drag_object.SetActive(false);
                button.GetComponent<CanvasGroup>().alpha = 1;
                button.GetComponent<CanvasGroup>().interactable = true;
                button.GetComponent<CanvasGroup>().blocksRaycasts = true;
                invoke_on_end_drag(this, (Element.TYPE)type);
              }
            });
            // tooltip
            {
              var tooltip_data = new UITooltips.GUI_001.DATA();
              {
                tooltip_data.game_object = button.gameObject;
                tooltip_data.transform_position = button.transform;
                tooltip_data.text = Element.ToolTip((Element.TYPE)type);
                tooltip_data.anchor = TextAnchor.UpperLeft;
                tooltip_data.spacing = new(125, 17);
              }
              UITooltips.GUI_001.AddListener(tooltip_data);
            }
          }
        }
      }
      // challenges
      {
        var content_parent = scroll_rect.content.Find("challenges");
        content_parent.GetChild(0).gameObject.SetActive(false);
        for (int i = 1; i < content_parent.childCount; i++) GameObject.Destroy(content_parent.GetChild(i).gameObject);
        foreach (Creator.Element.TYPE_CHALLENGE type in EnumUtility.GetOrderedValues(typeof(Creator.Element.TYPE_CHALLENGE)))
        {
          if (type == Creator.Element.TYPE_CHALLENGE.EMPTY) continue;
          var trfm_inst = GameObject.Instantiate(content_parent.GetChild(0), content_parent);
          trfm_inst.gameObject.SetActive(true);
          trfm_inst.Find("name").GetComponent<TMP_Text>().text = Element.Name((Element.TYPE)type);
          // events
          {
            var button = trfm_inst.GetComponent<ButtonB>();
            button.onBeginDrag.AddListener(() =>
            {
              // begin drag
              {
                drag_object.SetActive(true);
                drag_object.transform.Find("name").GetComponent<TMP_Text>().text = type.ToString();
                button.GetComponent<CanvasGroup>().alpha = 0;
                invoke_on_begin_drag(this);
              }
            });
            button.onDrag.AddListener(() =>
            {
              invoke_on_drag(this);
            });
            button.onEndDrag.AddListener(() =>
            {
              // end drag
              {
                drag_object.SetActive(false);
                button.GetComponent<CanvasGroup>().alpha = 1;
                invoke_on_end_drag(this, (Creator.Element.TYPE)type);
              }
            });
            // tooltip
            {
              var tooltip_data = new UITooltips.GUI_001.DATA();
              {
                tooltip_data.game_object = button.gameObject;
                tooltip_data.transform_position = button.transform;
                tooltip_data.text = Element.ToolTip((Element.TYPE)type);
                tooltip_data.anchor = TextAnchor.UpperLeft;
                tooltip_data.spacing = new(125, 17);
              }
              UITooltips.GUI_001.AddListener(tooltip_data);
            }
          }
        }
      }
      // artificial intelligence
      {
        var content_parent = scroll_rect.content.Find("ai");
        content_parent.GetChild(0).gameObject.SetActive(false);
        for (int i = 1; i < content_parent.childCount; i++) GameObject.Destroy(content_parent.GetChild(i).gameObject);
        foreach (Element.TYPE_AI type in EnumUtility.GetOrderedValues(typeof(Creator.Element.TYPE_AI)))
        {
          if (type == Element.TYPE_AI.EMPTY) continue;
          var trfm_inst = GameObject.Instantiate(content_parent.GetChild(0), content_parent);
          trfm_inst.gameObject.SetActive(true);
          trfm_inst.Find("name").GetComponent<TMP_Text>().text = Element.Name((Element.TYPE)type);
          // events
          {
            var button = trfm_inst.GetComponent<ButtonB>();
            button.onBeginDrag.AddListener(() =>
            {
              // begin drag
              {
                drag_object.SetActive(true);
                drag_object.transform.Find("name").GetComponent<TMP_Text>().text = type.ToString();
                button.GetComponent<CanvasGroup>().alpha = 0;
                invoke_on_begin_drag(this);
              }
            });
            button.onDrag.AddListener(() =>
            {
              invoke_on_drag(this);
            });
            button.onEndDrag.AddListener(() =>
            {
              // end drag
              {
                drag_object.SetActive(false);
                button.GetComponent<CanvasGroup>().alpha = 1;
                invoke_on_end_drag(this, (Creator.Element.TYPE)type);
              }
            });
            // tooltip
            {
              var tooltip_data = new UITooltips.GUI_001.DATA();
              {
                tooltip_data.game_object = button.gameObject;
                tooltip_data.transform_position = button.transform;
                tooltip_data.text = Element.ToolTip((Element.TYPE)type);
                tooltip_data.anchor = TextAnchor.UpperLeft;
                tooltip_data.spacing = new(125, 17);
              }
              UITooltips.GUI_001.AddListener(tooltip_data);
            }
          }
        }
      }
    }
    public void Start()
    {
      SetActive(true);
    }
    public void Quit()
    {
      SetActive(false);
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
}
