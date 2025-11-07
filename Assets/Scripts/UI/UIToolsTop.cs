using System;
using System.IO;
using SFB;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[Serializable]
public class UIToolsTop
{
  public Transform transform = null;
  public void Awake()
  {
    // class
    {
      var dropdown = transform.Find("options/class").GetComponent<DropdownB>();
      dropdown.ClearOptions();
      dropdown.AddOption(new("New Class...")
      {
        action = () =>
        {
          var path = StandaloneFileBrowser.SaveFilePanel("Save Class", "", "new_class.json", "json");
          if (path.Trim() == string.Empty) return;
          File.WriteAllText(path, "");
          GameData.path_current = path;
          ManagerApp.Clear_All_Selected();
          GameData.Load(GameData.path_current);
          GameData.Class_Current.name = Path.GetFileNameWithoutExtension(path);
          GameData.invoke_on_class_current_updated();
        }
      });
      dropdown.AddOption(new("Import Class...")
      {
        action = () =>
        {
          var paths = StandaloneFileBrowser.OpenFilePanel("Select Class", "", "json", false);
          if (paths.Length == 0) return;
          GameData.path_current = paths[0];
          ManagerApp.Clear_All_Selected();
          GameData.Load(GameData.path_current);
          GameData.invoke_on_class_current_updated();
        }
      });
    }
    // version
    {
      transform.Find("version").GetComponent<TMP_Text>().text = $"v{Application.version}";
    }
  }
  public void Start()
  {
    Update_All();
  }
  public void Update_All()
  {
    var class_current = GameData.Class_Current;
    transform.Find("class_dir").gameObject.SetActive(class_current != null);
    transform.Find("class_name").gameObject.SetActive(class_current != null);
    if (class_current != null)
    {
      transform.Find("class_dir").GetComponent<TMP_Text>().text = GameData.path_current;
      transform.Find("class_name").GetComponent<TMP_Text>().text = class_current.name;
    }
  }
}