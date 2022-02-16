using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(AIActor))]
public class ActorEditor : Editor
{
    public override void OnInspectorGUI(){
        DrawDefaultInspector();
        AIActor script = (AIActor)target;
        if(GUILayout.Button("Initialize!")){
            script.Initialize();
        }
    }
}
