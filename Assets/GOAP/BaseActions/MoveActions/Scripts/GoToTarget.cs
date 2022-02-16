using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToTarget : MoveAction
{
    public AudioClip goToTarget_footsteps;
    public override void PerformAction(){
        base.PerformAction();
        target = GetActor().target;
        StartCoroutine(GoToTargetPosition());
    }
    private IEnumerator GoToTargetPosition(){
        animInterface.PlayAnimation(0);
        GetActor().navigationAgent.isStopped = false;
        GetActor().navigationAgent.SetDestination(target.position);
        yield return new WaitForFixedUpdate();
        while(GetActor().distanceToTarget > stopDistance)
            yield return new WaitForEndOfFrame();
        target = null;
        FinishAction();
        yield break;
    }
    public override void FinishAction(){
        base.FinishAction();
        //GetActor().audioSource.PlayOneShot(goToTarget_footsteps);
        //GetActor().worldstateHandler.move = WorldStateHandler.Move.Stand;
        GetActor().InitializeNextAction();
    }
}
