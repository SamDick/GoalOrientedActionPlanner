using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolMove : MoveAction
{
    
    [Header("\n")]  
    public float maxIterations;
    public AudioClip patrol_StopLook;
    public AudioClip patrol_End;
    private NodeHandler nodeHandler;
    private VisionNode[] patrolNodes;
    private VisionNode targetNode;
    private int iterations;
    
    public override void PerformAction(){
        base.PerformAction();
        StartCoroutine(Patrol());
        
    }
    private IEnumerator Patrol(){
        patrolNodes = GetActor().GetNodeHandler().GetVisionNodes(minRange,maxRange);
        targetNode = patrolNodes[UnityEngine.Random.Range(0,patrolNodes.Length)];
        GetActor().target = targetNode.transform;
        yield return new WaitForFixedUpdate();
        GetActor().navigationAgent.SetDestination(targetNode.GetPosition());
        GetActor().navigationAgent.isStopped = false;
        GetActor().animator.Play(animInterface.animationStates[0]);
        yield return new WaitForFixedUpdate();
        while(GetActor().navigationAgent.remainingDistance > stopDistance * 1.1f)
            yield return new WaitForEndOfFrame();
        if (UnityEngine.Random.Range(0,maxIterations) > iterations){
            GetActor().audioSource.PlayOneShot(patrol_StopLook);
            GetActor().animator.Play(animInterface.animationStates[1]);
            GetActor().SetAnimationLock(true);
            yield return new WaitForFixedUpdate();
            while(GetActor().GetAnimationLock())
                yield return new WaitForEndOfFrame();
            iterations++;
            targetNode = null;
            patrolNodes = null;
            this.PerformAction();
            yield break;

        }
        iterations = 0;
        targetNode = null;
        patrolNodes = null;
        
        GetActor().navigationAgent.isStopped = true;
        GetActor().animator.Play(animInterface.animationStates[2]);
        GetActor().SetAnimationLock(true);
        yield return new WaitForFixedUpdate();
        while(GetActor().GetAnimationLock())
            yield return new WaitForEndOfFrame();
        this.FinishAction();
        yield break;
    }
    public override void FinishAction(){
        if(GetActor().GActionPlan.Count == 0){
            GetActor().audioSource.PlayOneShot(patrol_End);
            
            base.FinishAction();
        }
        
        GetActor().InitializeNextAction();
    }
}
