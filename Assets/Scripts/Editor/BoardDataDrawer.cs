using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Unity.VisualScripting.ReorderableList;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;


[CustomEditor(typeof(BoardData), false)]
[CanEditMultipleObjects]
[System.Serializable]

public class BoardDataDrawer : Editor
{
    private BoardData GameDataInstance => target as BoardData;
    private ReorderableList _dataList;


    private void OnEnable()
    {
        IntializeReordableList(ref _dataList, "SearchWords", "Searchining Word");

    }

    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector();

        serializedObject.Update();
        DrawColomnsRawsInputFields();
        EditorGUILayout.Space();
        CoverTopUpperButton();



        if (GameDataInstance.Board != null && GameDataInstance.Columns > 0 && GameDataInstance.Rows > 0)
            DrawBoardTable();
        

        GUILayout.BeginHorizontal(); 

        ClearBoardButton();
        FillUpWithRandomLettersButton();

        GUILayout.EndHorizontal();

        EditorGUILayout.Space();
        _dataList.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
        if(GUI.changed)
        {
            EditorUtility.SetDirty(GameDataInstance);

        }
        
    } 
    private void DrawColomnsRawsInputFields()
        {

            var colomnTemp = GameDataInstance.Columns;
            var rowTemp = GameDataInstance.Rows;

            GameDataInstance.Columns = EditorGUILayout.IntField("Colomns", GameDataInstance.Columns);
            GameDataInstance.Rows = EditorGUILayout.IntField("Rows", GameDataInstance.Rows);

            if((GameDataInstance.Columns != colomnTemp || GameDataInstance.Rows != rowTemp)
            && GameDataInstance.Columns > 0 && GameDataInstance.Rows >0)
            {
            GameDataInstance.CreateNewBoard();

            }

        }

    private void DrawBoardTable()
    {
        var tableStyle = new GUIStyle("box");
        tableStyle.padding = new RectOffset(10, 10, 10, 10);
        tableStyle.margin.left = 32;

        var headerColomnStyle = new GUIStyle();
        headerColomnStyle.fixedWidth = 35;
        
        var colomnStyle = new GUIStyle();
        colomnStyle.fixedWidth = 50;
        
        var rowStyle = new GUIStyle();
        rowStyle.fixedWidth = 25;
        rowStyle.fixedHeight = 40;
        rowStyle.alignment = TextAnchor.MiddleCenter;


        var textFieldStyle = new GUIStyle();
        textFieldStyle.normal.background = Texture2D.grayTexture;
        textFieldStyle.normal.textColor = Color.white;
        textFieldStyle.fontStyle = FontStyle.Bold;
        textFieldStyle.alignment = TextAnchor.MiddleCenter;

        EditorGUILayout.BeginHorizontal(tableStyle);
        for (var x = 0; x < GameDataInstance.Columns; x++)
        {
            EditorGUILayout.BeginVertical(x == -1 ? headerColomnStyle : colomnStyle);
            for(var y = 0; y < GameDataInstance.Rows; y++)
            {
                if(x >= 0 && y >= 0)
                {
                    EditorGUILayout.BeginHorizontal(rowStyle);
                    var character = (string)EditorGUILayout.TextArea(GameDataInstance.Board[x].Row[y], textFieldStyle);

                    if (GameDataInstance.Board[x].Row[y].Length > 1)
                    {

                        character = GameDataInstance.Board[x].Row[y].Substring(0, 1);

                    }

                    GameDataInstance.Board[x].Row[y] = character;
                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.EndVertical();

        }

        EditorGUILayout.EndHorizontal() ;

    }

    private void IntializeReordableList(ref ReorderableList list, string propertyName, string listLable)
    {
        list = new ReorderableList(serializedObject, serializedObject.FindProperty(propertyName), true,true,true,true);

        list.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, listLable);

        };

        var l = list;
        list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => { 
            var element = l.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;

            EditorGUI.PropertyField(new Rect(rect.x, rect.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("Word"), GUIContent.none);
        };

        

    }

    private void CoverTopUpperButton()
    {
        if(GUILayout.Button(" To Upper"))
        {
            for(var i = 0; i< GameDataInstance.Columns; i++)
            {
                for (var j = 0; j < GameDataInstance.Rows; j++)
                {
                    var errorCounter = Regex.Matches(GameDataInstance.Board[i].Row[j], @"[a-z]").Count;

                    if(errorCounter > 0)
                    {
                        GameDataInstance.Board[i].Row[j] = GameDataInstance.Board[i].Row[j].ToUpper();

                    }
                }

                
                
            }

            foreach (var searchWord in GameDataInstance.SearchWords)
            {
                var errorCounter = Regex.Matches(searchWord.Word, @"[a-z]").Count;
                if (errorCounter > 0)
                {
                    searchWord.Word = searchWord.Word.ToUpper();
                }


            }

        }
    }

    private void ClearBoardButton()
    {
        if(GUILayout.Button("Clear Button"))
        {
            for(int i = 0;i< GameDataInstance.Columns; i++)
            {
                for (int j = 0; j < GameDataInstance.Rows; j++)
                {
                    GameDataInstance.Board[i].Row[j] = " ";
                }
            }
        }
    }

    private void FillUpWithRandomLettersButton()
    {
        if (GUILayout.Button("Fill Up with Button"))
        {
            for (int i = 0; i < GameDataInstance.Columns; i++)
            {
                for (int j = 0; j < GameDataInstance.Rows; j++)
                {
                    int errorCounter = Regex.Matches(GameDataInstance.Board[i].Row[j], @"[a-zA-Z]").Count;
                    string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

                    int index = UnityEngine.Random.Range(0, letters.Length);

                    if(errorCounter == 0)
                    {
                        GameDataInstance.Board[i].Row[j] = letters[index].ToString();
                    }
                }
            }
        }
    }

}
