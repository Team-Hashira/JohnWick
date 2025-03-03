using UnityEditor;
using System.IO;

public static class ScriptGenerator
{
    private const string scriptPath = "Assets/01.Script/Utils/ScriptGenerator/";

    [MenuItem("Tools/Create Custom ScriptableObject")]
    public static void CreateScriptableObjectScript()
    {
        string className = "NewScriptableObject"; // 원하는 기본 클래스명
        string filePath = Path.Combine(scriptPath, className, ".cs");

        if (!Directory.Exists(scriptPath))
            Directory.CreateDirectory(scriptPath);

        if (File.Exists(filePath))
        {
            EditorUtility.DisplayDialog("Error", "파일이 이미 존재합니다!", "OK");
            return;
        }

        string scriptContent = $@"using UnityEngine;

[CreateAssetMenu(fileName = ""New {className}"", menuName = ""Custom/{className}"")]
public class {className} : ScriptableObject
{{
    public int exampleValue;
}}";

        File.WriteAllText(filePath, scriptContent);
        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog("Success", $"{className}.cs 생성 완료!", "OK");
    }
}