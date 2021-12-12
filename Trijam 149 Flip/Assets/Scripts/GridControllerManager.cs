#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class GridControllerManager : MonoBehaviour
{
    
    public GridController controller;

}

[CustomEditor(typeof(GridControllerManager))]
public class GridEditor : Editor
{

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        GridControllerManager manager = (GridControllerManager)target;

        manager.controller = (GridController)EditorGUILayout.ObjectField(manager.controller, typeof(GridController), true);

        if (GUILayout.Button("Generate Grid"))
        {
            manager.controller.GenerateGrid();
        }

        if (EditorGUI.EndChangeCheck())
        {
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }

}

#endif