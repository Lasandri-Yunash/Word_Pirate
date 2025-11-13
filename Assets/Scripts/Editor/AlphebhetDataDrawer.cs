using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using static AphabetData;


[CustomEditor(typeof(AphabetData))]
[CanEditMultipleObjects]
[System.Serializable]

public class AlphebhetDataDrawer : Editor
{
    
    private ReorderableList AlphabetPalinList;
    private ReorderableList AlphabetNormalList;
    private ReorderableList AlphabetHightlightedList;
    private ReorderableList AlphabetWrongList;


    private void OnEnable()
    {
        InitializeReorderableList(ref AlphabetPalinList, "AlphaPlain", "Alpha Plain");
        InitializeReorderableList(ref AlphabetNormalList, "AlphaNormal", "Alpha Normal");
        InitializeReorderableList(ref AlphabetHightlightedList, "AlphaHightlighted", "Alpha Hightlighted");
        InitializeReorderableList(ref AlphabetWrongList, "AlphaWrong", "Alpha Wrong");

    }
   

    public override void OnInspectorGUI()
    {

        serializedObject.Update();
        
        AlphabetPalinList.DoLayoutList();
        AlphabetNormalList.DoLayoutList();
        AlphabetHightlightedList.DoLayoutList();
        AlphabetWrongList.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
    }

    private void InitializeReorderableList (ref ReorderableList list, string propertyName, string listLable)
    {
        list = new ReorderableList(serializedObject, serializedObject.FindProperty(propertyName),
            true,true,true,true);

        list.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, listLable);
        };

        var l = list;

        list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
            var element = l.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;

            EditorGUI.PropertyField(new Rect(rect.x, rect.y, 60, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("letter"), GUIContent.none);

            EditorGUI.PropertyField(new Rect(
                rect.x + 70, rect.y, rect.width - 60 - 30, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("image"), GUIContent.none);
        };
    }
}
