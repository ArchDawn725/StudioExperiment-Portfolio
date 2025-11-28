using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PathController))]
public class EditorButton 
    : Editor
{
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();  // Draws the default inspector

        PathController script = (PathController)target;

        // Add a button to the inspector
        if (GUILayout.Button("Generate Pathfinding"))
        {
            script.StartUp();
        }
    }
    
}
