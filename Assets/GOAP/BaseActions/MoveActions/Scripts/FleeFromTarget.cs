using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeFromTarget : MoveAction
{
    //TODO
    //Find a spot within range, without vision of target and continue running for a random time or until 
    // enough distance has been covered
    private int iterations;
    public int maxIterations;
    public override void PerformAction()
    {
        var nodes = GetActor().GetNodeHandler().GetVisionNodes(minRange,maxRange,0);
        var d = 0f;
        var distance = maxRange;
        foreach(VisionNode node in nodes){
            d = (node.transform.position - target.transform.position).magnitude;
            if(d < distance){
                distance = d;
                target = node.transform;
            }
        }
        GetActor().target = target;
        base.PerformAction();
    }
    private IEnumerator Flee(){
        GetActor().navigationAgent.isStopped = false;
        GetActor().navigationAgent.SetDestination(target.position);
        yield return new WaitForFixedUpdate();
        while(GetActor().navigationAgent.remainingDistance > stopDistance * 1.1f)
            yield return new WaitForEndOfFrame();
        if (UnityEngine.Random.Range(0,maxIterations) > iterations){
            PerformAction();
            iterations++;
            yield break;
        }
        iterations = 0;
        FinishAction();
        yield break;
    }
    public override void FinishAction(){

    }
    
}
