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
    [MenuItem("BuildAsset/Build Role Data")]
    public static void ExcuteBuildRole()
    {
        PackageRole holder = ScriptableObject.CreateInstance<PackageRole>();

        //查询excel表中数据，赋值给asset文件
        holder.items = ExcelAccess.SelectRoleItem();

        string path = "Assets/Resources/DataAssets/roledata.asset";

        AssetDatabase.CreateAsset(holder, path);
        AssetDatabase.Refresh();
        Debug.Log("BuildAsset Success!");
    }
    [MenuItem("BuildAsset/Build Type Data")]
    public static void ExcuteBuildType()
    {
        PackageType holder = ScriptableObject.CreateInstance<PackageType>();

        //查询excel表中数据，赋值给asset文件
        holder.items = ExcelAccess.SelectTypeItem();

        string path = "Assets/Resources/DataAssets/typedata.asset";

        AssetDatabase.CreateAsset(holder, path);
        AssetDatabase.Refresh();
        Debug.Log("BuildAsset Success!");
    }
    [MenuItem("BuildAsset/Build Talent Data")]
    public static void ExcuteBuildTalent()
    {
        PackageTalent holder = ScriptableObject.CreateInstance<PackageTalent>();

        //查询excel表中数据，赋值给asset文件
        holder.items = ExcelAccess.SelectTalentItem();

        string path = "Assets/Resources/DataAssets/talentdata.asset";

        AssetDatabase.CreateAsset(holder, path);
        AssetDatabase.Refresh();
        Debug.Log("BuildAsset Success!");
    }
}
