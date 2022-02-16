using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    //MoveActions GO TO a thing
    //They are often not the primary action in plans
public class MoveAction : GOAP_Action
{   
    public static float BASE_STOP_DISTANCE = 1;
    internal float stopDistance = BASE_STOP_DISTANCE;
    public float speed;

    public override void PerformAction()
    {
        base.PerformAction();
        if(nextAction != null)
            stopDistance = nextAction.minRange; //TODO increase depth of calculations for stopDistance
    }
    public override void FinishAction()
    {
        base.FinishAction();
        stopDistance = BASE_STOP_DISTANCE;
    }
}
