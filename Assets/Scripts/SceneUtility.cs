using UnityEditor;

public class SceneUtility
{
    public static string[] GetSceneAllScenesNamesInBuild()
    {
        string[] scenePaths = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes);
        string[] sceneNames = new string[scenePaths.Length];

        for(int i = 0; i < scenePaths.Length; i++)
        {
            sceneNames[i] = System.IO.Path.GetFileNameWithoutExtension(scenePaths[i]);
        }

        return sceneNames;
    }
}
