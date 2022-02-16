using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeTarget : MoveAction
{
    public AudioClip chargeTarget_footsteps;
    public override void PerformAction(){
        base.PerformAction();
        target = GetActor().target;
        StartCoroutine(GoTo());
        GetActor().audioSource.loop = true;
        GetActor().audioSource.clip = chargeTarget_footsteps;
        GetActor().audioSource.Play();
    }
    private IEnumerator GoTo(){
        var s = GetActor().navigationAgent.speed;
        GetActor().animator.Play("chargeTarget_Run");
        GetActor().navigationAgent.speed = speed;
        GetActor().navigationAgent.isStopped = false;
        GetActor().navigationAgent.SetDestination(target.position);
        while(GetActor().navigationAgent.remainingDistance > stopDistance && GetActor().distanceToTarget > stopDistance)
            yield return new WaitForEndOfFrame();
        target = null;
        GetActor().audioSource.loop = false;
        GetActor().navigationAgent.speed = s;
        FinishAction();
        yield break;
    }
    public override void FinishAction(){
        base.FinishAction();
        GetActor().animator.Play("chargeTarget_End");
        GetActor().worldstateHandler.SetWorldState(effects);
        GetActor().InitializeNextAction();
    }
}
