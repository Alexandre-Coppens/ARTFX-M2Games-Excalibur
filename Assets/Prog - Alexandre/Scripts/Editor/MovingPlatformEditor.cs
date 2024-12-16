using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MovingPlatforms))]
public class MovingPlatformEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MovingPlatforms movingPlatforms = (MovingPlatforms)target;
        movingPlatforms.openPosition = EditorGUILayout.Vector2Field(new GUIContent("Open Position", "The position the platform goes when the lock is activated"), movingPlatforms.openPosition);
        movingPlatforms.closedPosition = EditorGUILayout.Vector2Field(new GUIContent("Closed Position", "The position the platform goes when the lock is deactivated"), movingPlatforms.closedPosition);

        if(GUILayout.Button("Center Gizmos"))
        {
            movingPlatforms.openPosition = movingPlatforms.gameObject.transform.position;
            movingPlatforms.closedPosition = movingPlatforms.gameObject.transform.position;
        }
    }
}
