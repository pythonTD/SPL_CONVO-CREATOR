using System.Collections.Generic;
using Creator;
public class ManagerApp
{
  public static Class.Lesson lesson_selected { get; private set; } = null;
  public static Class.Quiz quiz_selected { get; private set; } = null;
  public static Page page_selected { get; private set; } = null;
  public static Element element_selected { get; private set; } = null;
  public static Element element_copied { get; set; } = null;
  public static UISettingElement setting_selected { get; private set; } = null;

  public static void Select_Lesson(Class.Lesson lesson)
  {
    lesson_selected = lesson;
  }
  public static void Select_Quiz(Class.Quiz quiz)
  {
    quiz_selected = quiz;
  }
  public static void Select_Page(Page page)
  {
    page_selected = page;
  }
  public static void Select_Element(Element element)
  {
    element_selected = element;
  }
  public static void Select_Setting(UISettingElement setting)
  {
    setting_selected = setting;
  }
  public static void Clear_Element_Selected()
  {
    element_selected = null;
  }

  public static void Clear_All_Selected()
  {
    lesson_selected = null;
    quiz_selected = null;
    page_selected = null;
    element_selected = null;
    setting_selected = null;
  }

  // public static Class.Lesson.Content lesson_content_selected { get; private set; } = null;
  // public static Class.Quiz.Exercise quiz_exercise_selected { get; private set; } = null;
  // public static List<Creator.Element> elements_selected { get; private set; } = null;
  // public static Creator.Element element_selected { get; private set; } = null;
  // public static Creator.Element element_copied { get; set; } = null;
  // public static void Select_Lesson_Content(Class.Lesson.Content value)
  // {
  //   quiz_exercise_selected = null;
  //   lesson_content_selected = value;
  //   elements_selected = lesson_content_selected.elements;
  // }
  // public static void Select_Quiz_Content(Class.Quiz.Exercise value)
  // {
  //   quiz_exercise_selected = value;
  //   lesson_content_selected = null;
  //   elements_selected = quiz_exercise_selected.elements;
  // }
  // public static void Select_Element(Creator.Element value)
  // {
  //   element_selected = value;
  // }
  // public static void Clear_All_Selected()
  // {
  //   quiz_exercise_selected = null;
  //   lesson_content_selected = null;
  //   element_selected = null;
  //   elements_selected = null;
  // }
  // public static void Clear_Element_Selected()
  // {
  //   element_selected = null;
  // }
}