using UnityEngine;
using UnityEditor;

/// <summary>
/// 利用ScriptableObject创建资源文件
/// </summary>
public class BuildAsset : Editor {

    [MenuItem("BuildAsset/Build Scriptable Data")]
    public static void ExcuteBuildLevel()
    {
        PackageItem holder = ScriptableObject.CreateInstance<PackageItem>();

        //查询excel表中数据，赋值给asset文件
        holder.items = ExcelAccess.SelectMenuLevel();

        string path= "Assets/Resources/DataAssets/levelRegular.asset";

        AssetDatabase.CreateAsset(holder, path);
        AssetDatabase.Refresh();
        Debug.Log("BuildAsset Success!");
    }
}
