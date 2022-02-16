using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ActionNode : MonoBehaviour
{
    public float baseCost;
    public string actionName;
    public GOAP_Action.Goals goal;
    public WorldStateHandler conditions;
    public WorldStateHandler effects;
    public bool isAvailable;
    public Transform enterPosition;
    public Transform exitPosition;
    private void Start(){
    }
    public float GetBaseCost(){
        return baseCost;
    }
    public WorldStateHandler GetConditions(){
        return conditions;
    }
    public WorldStateHandler GetEffects(){
        return effects;
    }
    public float GetDistance(Vector3 pos){
        return (transform.position - pos).magnitude;
    }
    public GOAP_Action.Goals GetGoal(){
        return goal;
    }
    public virtual IEnumerator UseNodeAction(GOAP_Action action){
        yield break;
    }
}
