using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using UnityEngine;
using Creator;
public class GameData
{
  public static Class Class_Current = null;
  public static string path_current = string.Empty;
  public static event EventHandler on_class_current_updated = null;
  public static void invoke_on_class_current_updated(EventArgs args = default)
  => on_class_current_updated?.Invoke(args.Equals(default(EventArgs)) ? EventArgs.Default : args);
  public static void clear_on_class_current_updated()
  => on_class_current_updated = null;
  public static void Load(string path)
  {
    var json_options = new JsonSerializerOptions { WriteIndented = false, IncludeFields = true };
    Class_Current = null;
    if (!File.Exists(path)) return;
    var json = File.ReadAllText(path);
    // var json = Resources.Load<TextAsset>($"class_001/class_001").text;
    try
    {
      Class_Current = JsonSerializer.Deserialize<Class>(json, json_options);
      // foreach (var lesson in Class_Current.Lessons)
      // {
      //   foreach (var page in lesson.pages)
      //   {
      //     foreach (var element in page.elements)
      //     {
      //       Debug.Log(element.GetType());
      //     }
      //   }
      // }
      // foreach (var lesson in Class_Current.Lessons)
      //   foreach (var content in lesson.pages)
      //     foreach (var element in content.elements)
      //       element.Deserialize();
      // foreach (var quiz in Class_Current.Quizzes)
      // {
      //   foreach (var exercise in quiz.pages)
      //     foreach (var element in exercise.elements)
      //       element.Deserialize();
      //   foreach (var element in quiz.page_result.elements)
      //     element.Deserialize();
      // }
    }
    catch (Exception) { Class_Current = new(); }
  }
  public static void Save_All()
  {
    var json_options = new JsonSerializerOptions { WriteIndented = false, IncludeFields = true, };
    if (!File.Exists(path_current)) return;
    File.WriteAllText(path_current, JsonSerializer.Serialize(Class_Current, json_options));
    // var path = Path.Combine(Path.GetDirectoryName(Application.dataPath) + "/Assets/Resources/class_001/class_001.json");
    // File.WriteAllText(path, JsonSerializer.Serialize(Class_Current, json_options));
  }
  public delegate void EventHandler(EventArgs args);
  public struct EventArgs
  {
    public static EventArgs Default
    {
      get
      {
        return new EventArgs()
        {
          on_save = true,
          on_update_tools_top = true,
          on_update_tools_left = true,
          on_update_tools_right = true,
          on_update_tools_page = true,
        };
      }
    }
    public bool on_save;
    public bool on_update_tools_top;
    public bool on_update_tools_left;
    public bool on_update_tools_right;
    public bool on_update_tools_page;
  }
}