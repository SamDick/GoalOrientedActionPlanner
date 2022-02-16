using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(ActionAnimatorInterface))]

public class ActorAnimatorInterfaceEditor : Editor
{
    public override void OnInspectorGUI(){
        DrawDefaultInspector();
        ActionAnimatorInterface script = (ActionAnimatorInterface)target;
        if(GUILayout.Button("Initialize!")){
            script.Initialize();
        }
    }
}
