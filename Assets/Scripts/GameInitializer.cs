using System;
using UnityEngine;
public class GameInitializer
{
  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
  private static void Initializer()
  {
    Application.targetFrameRate = 240;
    // reset
    {
      // GameData.path_current = "C:/Documentos/Unity/AI Class Assist - Creator/AI Class Assist - Creator/Assets/Resources/class_001/class_005.json";
      GameData.path_current = string.Empty;
      ManagerApp.Clear_All_Selected();
      ManagerApp.element_copied = null; 
    }
    GameData.Load(GameData.path_current);
    // events
    {
      GameData.clear_on_class_current_updated();
      GameData.on_class_current_updated += (args) =>
      {
        if (args.on_save)
          GameData.Save_All();
        if (args.on_update_tools_top)
          UIScene.Instance.tools_top.Update_All();
        if (args.on_update_tools_left)
          UIScene.Instance.tools_left.Update_All();
        if (args.on_update_tools_page)
          UIScene.Instance.page.Update_All();
        if (args.on_update_tools_right)
          UIScene.Instance.tools_right.Update_All();
      };
    }
    // keys creator
    // {
    // var t = string.Empty;
    // for (int i = 0; i < 15; i++)
    //   t += $"{Guid.NewGuid().GetHashCode()}\n";
    // Debug.Log(t);
    // }
  }
}