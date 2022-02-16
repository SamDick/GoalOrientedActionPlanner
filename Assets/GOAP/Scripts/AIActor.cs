using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// Makes calls to the ActionPlanRequestManager and recieves stacks of actions (Plans) to initialize
/// </summary>
public class AIActor : MonoBehaviour
{
    public GOAP_Action.Goals currentGoal;
    public float planCost;
    public float speed;
    public float baseSpeed;
    public Animator animator;
    public bool animLock;
    public AudioSource audioSource;
    internal GOAP_Action currentGOAP_Action;
    public Stack<GOAP_Action> GActionPlan;
    public List<GOAP_Action> actionLibrary;
    public NavMeshAgent navigationAgent;
    public WorldStateHandler worldstateHandler;
    private NodeHandler nodes;
    public Transform target;
    internal float distanceToTarget;
    internal float animationTimeN;
    public float count;
    bool fired;
    private void Start(){
        nodes = GetComponent<NodeHandler>();
        count = UnityEngine.Random.Range(10,50);
        navigationAgent.speed = speed;
    }
    private void Update(){
        if(target!=null)
            distanceToTarget = (target.position - transform.position).magnitude;
        else    
            distanceToTarget = Mathf.Infinity;
        if (count>0)
            count--;
        else if (!fired){
            fired = true;
            Initialize();
        }
        
        
    }
    public void Initialize(){
        actionLibrary = new List<GOAP_Action>(GetComponentsInChildren<GOAP_Action>());
        GActionPlan = new Stack<GOAP_Action>();
        RequestNewPlan();
    }
    public void SetGoal(GOAP_Action.Goals goal)
    {
        currentGoal = goal;
    }
    internal void RequestNewPlan()
    {   
        speed = baseSpeed;
        ActionPlanRequestManager.instance.RequestPlan(this, InitializeNextAction);
    }
    public void InitializeNextAction()
    {  
        if (GActionPlan != null && GActionPlan.Count == 0)
        {
            planCost = 0;
            RequestNewPlan();
            
        } else {
            
            GActionPlan.Pop().InitializeAction(this);
        }
    }
    
    public GOAP_Action.Goals GetGoal(){
        return currentGoal;
    }
    public NodeHandler GetNodeHandler(){
        return nodes;
    }
    public bool GetAnimationLock(){
        return animLock;
    }
    public void SetAnimationLock(bool b){
        animLock = b;
    }
}