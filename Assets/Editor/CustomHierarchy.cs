using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class CustomHierarchy
{
    private static HierarchyHeaderData _hierarchyHeaderData;

    static CustomHierarchy()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
        _hierarchyHeaderData = AssetDatabase.LoadAssetAtPath<HierarchyHeaderData>("Assets/Data/HierarchyHeaderData.asset");
    }

    private static void HandleHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        var gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

        if (gameObject == null)
        {
            return;
        }

        var headerRect = new Rect(selectionRect.x, selectionRect.y, selectionRect.width, EditorGUIUtility.singleLineHeight);

        foreach (var headerInfo in _hierarchyHeaderData.HeaderInfos)
        {
            if (!gameObject.name.Equals(headerInfo.type.ToString()))
            {
                continue;
            }

            EditorGUI.DrawRect(headerRect, headerInfo.color);
            EditorGUI.LabelField(headerRect, headerInfo.text, EditorStyles.whiteBoldLabel);
            break;
        }
    }
}