using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionNodeHandler : MonoBehaviour
{
    
    public ActionNode[] actionNodes; 
    private void Start(){
        actionNodes = GetComponentsInChildren<ActionNode>();
    }
}
