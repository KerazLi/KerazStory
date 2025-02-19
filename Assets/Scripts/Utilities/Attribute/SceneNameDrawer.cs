using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

 /*
* @Program:SceneNameDrawer.cs
* @Author: Keraz
* @Description:自定义属性编辑器，用于SceneNameAttribute属性
* @Date: 2025年02月19日 星期三 19:59:57
*/

[CustomPropertyDrawer(typeof(SceneNameAttribute))]
public class SceneNameDrawer : PropertyDrawer
{
    // 当前选择的场景索引
    int sceneIndex = -1;
    // 场景名称数组
    GUIContent[] sceneNames;

    // 用于分割场景路径的字符串数组
    readonly string[] scenePathSplit = { "/", ".unity" };

    // 重写OnGUI方法以自定义属性的编辑界面
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // 如果没有设置任何场景，则不执行任何操作
        if (EditorBuildSettings.scenes.Length == 0) return;

        // 初始化场景名称数组
        if (sceneIndex == -1)
            GetSceneNameArray(property);

        // 记录旧的场景索引
        int oldIndex = sceneIndex;

        // 使用Popup绘制场景选择界面
        sceneIndex = EditorGUI.Popup(position, label, sceneIndex, sceneNames);

        // 如果用户选择了不同的场景，则更新属性的字符串值
        if (oldIndex != sceneIndex)
            property.stringValue = sceneNames[sceneIndex].text;
    }

    // 获取场景名称数组
    private void GetSceneNameArray(SerializedProperty property)
    {
        // 获取所有场景
        var scenes = EditorBuildSettings.scenes;
        // 初始化数组
        sceneNames = new GUIContent[scenes.Length];

        // 遍历所有场景，提取场景名称
        for (int i = 0; i < sceneNames.Length; i++)
        {
            string path = scenes[i].path;
            string[] splitPath = path.Split(scenePathSplit, System.StringSplitOptions.RemoveEmptyEntries);

            string sceneName = "";

            // 如果分割后的路径不为空，则取最后一个元素作为场景名称
            if (splitPath.Length > 0)
            {
                sceneName = splitPath[splitPath.Length - 1];
            }
            else
            {
                // 如果路径为空，则标记为已删除的场景
                sceneName = "(Deleted Scene)";
            }

            sceneNames[i] = new GUIContent(sceneName);
        }

        // 如果场景数组为空，则显示提示信息
        if (sceneNames.Length == 0)
        {
            sceneNames = new[] { new GUIContent("Check Your Build Settings") };
        }

        // 如果属性的字符串值不为空，则尝试找到对应的场景索引
        if (!string.IsNullOrEmpty(property.stringValue))
        {
            bool nameFound = false;

            for (int i = 0; i < sceneNames.Length; i++)
            {
                if (sceneNames[i].text == property.stringValue)
                {
                    sceneIndex = i;
                    nameFound = true;
                    break;
                }
            }

            // 如果没有找到对应的场景，则将索引设置为第一个场景
            if (nameFound == false)
                sceneIndex = 0;
        }
        else
        {
            // 如果属性的字符串值为空，则将索引设置为第一个场景
            sceneIndex = 0;
        }

        // 更新属性的字符串值为当前选择的场景名称
        property.stringValue = sceneNames[sceneIndex].text;
    }
}
