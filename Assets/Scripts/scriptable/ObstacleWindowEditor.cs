using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObstacleWindowEditor : EditorWindow
{
    public ObstacleData obstacleData;
    [MenuItem("Window/Obstacle Editor")]
    public static void ShowWindow()
    {
        GetWindow<ObstacleWindowEditor>("Obstacle Editor");
    }

    private void OnGUI()
    {
        //---------------Window Definition---------------

        GUILayout.Label("Obstacle Editor", EditorStyles.boldLabel);
        obstacleData = (ObstacleData)EditorGUILayout.ObjectField("Obstacle Data", obstacleData, typeof(ObstacleData), false);
        if (obstacleData == null)
        {
            EditorGUILayout.HelpBox("Please assign a obstacle Data Object", MessageType.Warning);
            return;
        }
        
        //---------------Drawing Boxes---------------

        for (int y = 0; y < 10; y++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < 10; x++)
            {
                int index = y * 10 + x;
                obstacleData.obstacleGrid[index] = GUILayout.Toggle(obstacleData.obstacleGrid[index], GUIContent.none, GUILayout.Width(20), GUILayout.Height(20));

            }
            EditorGUILayout.EndHorizontal();
        }

        //---------------Save Button---------------
        
        if (GUILayout.Button("Save"))
        {
            EditorUtility.SetDirty(obstacleData);
            AssetDatabase.SaveAssets();
        }
    }
}
