using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Text.RegularExpressions;
using System;

//https://www.youtube.com/watch?v=34736DHWzaI&ab_channel=TurboMakesGames

#if UNITY_EDITOR
public class CS_Renamer : EditorWindow
{
    private string firstName;
    private List<GameObject> objects;
    private string textRename = String.Empty;
    private GameObject textRenameEditorGUI;
    private bool createIndex = true;

    [MenuItem("Tools/Renamer/Show Window")]

    public static void ShowWindow()
    {
        GetWindow(typeof(CS_Renamer));
    }

    private void OnGUI()
    {
        //*****************************RENAMER**************************************************
        GUILayout.Label("Renamer", EditorStyles.boldLabel);
        GUILayout.Space(10);

        textRename = EditorGUILayout.TextField(new GUIContent("New Name", "Le future nom de base de chaque objets"), textRename);
        if (GUILayout.Button("Clear"))
            ClearTextRename();
        if (GUILayout.Button(new GUIContent("GetFirstName", "Prend pour valeur le nom du premier objet de la liste")))
            GetFirstName();

        createIndex = EditorGUILayout.Toggle(new GUIContent("Create index", "Créer un index sur les objets à renommer"), createIndex);

        if (GUILayout.Button("Rename"))
            RenameButton();

        //*****************************indexer**************************************************
        /*GUILayout.Space(10);
        GUILayout.Label("Indexer", EditorStyles.boldLabel);

        if (GUILayout.Button(new GUIContent("Indexer", "Ré indexe la selection en gardant les noms de de chacun")))
            ReIndexer();*/
        

    }

    private void ReIndexer()
    {
        GetSelection();
    }

    private void GetFirstName()
    {
        GetSelection();
        textRename = ClearIndex(objects[0].name);
    }

    private void ClearTextRename()
    {
        textRename = string.Empty;
    }

    private void Init()
    {
        objects = new List<GameObject>();
        firstName = string.Empty;
    }

    private void GetSelection()
    {
        GameObject[] objt = Selection.gameObjects; //On récupère la selection
        System.Array.Sort(objt, new UnityTransformSort());
        objects = objt.ToList();
    }

    private void RenameButton()
    {
        GetSelection();

        foreach (var obj in objects)
        {
            obj.name = textRename;
        }

        if (createIndex)
        {
            int index = 1;
            foreach (var obj in objects)
            {
                obj.name = obj.name + " (" + index + ")";
                index++;
            }
        }
    }

    private void ClearAllIndexUnity()
    {
        foreach (var obj in objects)
        {
            obj.name = ClearIndex(obj.name);
        }
    }

    private string ClearIndex(string input)
    {
        string patern = @"\(\d*\)";

        Match match;

        match = Regex.Match(input, patern);
        if (match.Success)
        {
            input = input.Trim(match.Value.ToCharArray());
        }
        return input;
    }

    /// <summary>
    /// https://answers.unity.com/questions/1372671/editor-selection-order.html
    /// permet de trier la selection dans l'ordre de la hierarchie
    /// </summary>
    public class UnityTransformSort : System.Collections.Generic.IComparer<GameObject>
    {
        public int Compare(GameObject lhs, GameObject rhs)
        {
            if (lhs == rhs) return 0;
            if (lhs == null) return -1;
            if (rhs == null) return 1;
            return (lhs.transform.GetSiblingIndex() > rhs.transform.GetSiblingIndex()) ? 1 : -1;
        }
    }
}
#endif