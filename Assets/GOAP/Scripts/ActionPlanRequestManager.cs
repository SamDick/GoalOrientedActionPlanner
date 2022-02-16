using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ActionPlanRequestManager : MonoBehaviour
{
    Queue<PlanRequest> planRequestQueue = new Queue<PlanRequest>();
    PlanRequest currentPlanRequest;
    public int queueLength;
    public static ActionPlanRequestManager instance;
    float planDelay;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {

    }
    private void FixedUpdate()
    {
        queueLength = planRequestQueue.Count;
    }
    public void RequestPlan(AIActor actor, Action initializationMethod)
    {
        PlanRequest newRequest = new PlanRequest(actor, initializationMethod);
        instance.planRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }
    

    void TryProcessNext()
    {
        currentPlanRequest = planRequestQueue.Dequeue();
        GoalOrientedActionPlanner.ActionPlanner(currentPlanRequest, null,currentPlanRequest.actor.currentGoal);
    }

    public void FinishedProcessingPlan()
    {
        currentPlanRequest.callBack();
    }

    public struct PlanRequest
    {
        public AIActor actor;
        public Action callBack;
        public PlanRequest(AIActor _actor, Action _callBack)
        {
            callBack = _callBack;
            actor = _actor;
        }
    }
}
