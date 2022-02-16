using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleNode : ActionNode
{
    public float idleTime;
    private AIActor actor;
    private ActionAnimatorInterface animInterface;
    public override IEnumerator UseNodeAction(GOAP_Action action){
        animInterface = action.animInterface;
        actor = action.GetActor();
        animInterface.PlayAnimation(0);
        actor.SetAnimationLock(true);
        yield return new WaitForFixedUpdate();
        var t = idleTime;
        while(t > 0){
            actor.transform.position = Vector3.MoveTowards(actor.transform.position,enterPosition.position,.1f);
            actor.transform.forward = Vector3.MoveTowards(actor.transform.forward,enterPosition.forward,.1f);
            t-=Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        animInterface.PlayAnimation(2);
        actor.SetAnimationLock(true);
        yield return new WaitForFixedUpdate();
        while(actor.GetAnimationLock()){
            
            actor.transform.position = Vector3.MoveTowards(actor.transform.position,exitPosition.position,.1f);
            actor.transform.forward = Vector3.MoveTowards(actor.transform.forward,exitPosition.forward,.1f);
            yield return new WaitForFixedUpdate();
        }
        actor = null;
        animInterface = null;
        action.FinishAction();
        
        yield break;
    }
}
