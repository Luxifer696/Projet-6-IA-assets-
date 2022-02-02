using UnityEditor;
using UnityEngine;

namespace SeparatorTool
{
    [InitializeOnLoad]
    public class Separator
    {
        static Separator()
        {
            EditorApplication.hierarchyWindowItemOnGUI += OnGUI;
        }

        private static void OnGUI(int instanceID, Rect selectionRect)
        {
            Object obj = EditorUtility.InstanceIDToObject(instanceID);
            if(obj != null && obj.name != null && obj.name.Contains("=="))
            {
                selectionRect.width += selectionRect.x + 15;
                selectionRect.x = 0;

                if (obj.name.Contains("==R")) { EditorGUI.DrawRect(selectionRect, Color.red); }
                else if (obj.name.Contains("==B")) { EditorGUI.DrawRect(selectionRect, Color.blue); }
                else if (obj.name.Contains("==G")) { EditorGUI.DrawRect(selectionRect, Color.green); }
                else { EditorGUI.DrawRect(selectionRect, Color.white); }

                string tempDisplayName = obj.name.Remove(0, 4).ToUpper();
                GUIStyle tempStyle = new GUIStyle();
                tempStyle.alignment = TextAnchor.MiddleCenter;
                tempStyle.fontStyle = FontStyle.Bold;
                GUI.Label(selectionRect, tempDisplayName, tempStyle);
            }
        }
    }
}