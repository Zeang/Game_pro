using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class CreateAsset : Editor {

    // 在菜单栏创建功能项
    [MenuItem("CharacAsset/Create Asset")]
    static void Create()
    {
        // 实例化类  Character
        ScriptableObject charac = ScriptableObject.CreateInstance<Character>();

        // 如果实例化 Character 类为空，返回
        if (!charac)
        {
            Debug.LogWarning("Character not found");
            return;
        }

        // 自定义资源保存路径
        string path = Application.dataPath + "/CharacterAsset";

        // 如果项目总不包含该路径，创建一个
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        //将类名 Character 转换为字符串
        //拼接保存自定义资源（.asset） 路径
        path = string.Format("Assets/CharacterAsset/{0}.asset", (typeof(Character).ToString()));

        // 生成自定义资源到指定路径
        AssetDatabase.CreateAsset(charac, path);
    }
}
