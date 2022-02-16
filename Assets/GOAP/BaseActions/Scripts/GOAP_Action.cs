using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
/// <summary>
/// POssible subclasses? For movement options, defensive options, and attack options?
/// </summary>
///
[RequireComponent(typeof(ActionAnimatorInterface))]
public class GOAP_Action : MonoBehaviour
{

    internal AIActor myActor;
    public string actionName;
    public enum Goals { Idle, Die, Speak, Approach, Flee, Attack, Patrol, Spawn, Relocate };
    public Goals goal;
    public Transform target;
    public float heuristicCost;
    public float baseCost;
    public bool inPlan;
    public float maxRange = 100;
    public float minRange = 1;
    internal GOAP_Action nextAction;
    [Header("Conditions\n")]
    public WorldStateHandler.Move move;
    public WorldStateHandler.Health health;
    public WorldStateHandler.Awareness awareness;
    public WorldStateHandler.Hostility hostility;
    [Header("\n\nEffects\n")]    
    public WorldStateHandler.Move moveEffect;
    public WorldStateHandler.Health healthEffect;
    public WorldStateHandler.Awareness awarenessEffect;
    public WorldStateHandler.Hostility hostilityEffect;
    public WorldStateHandler.WORLD_STATE conditions;
    public WorldStateHandler.WORLD_STATE effects;
    internal ActionAnimatorInterface animInterface;
    
    private void Awake(){
        SetWorldStates();
    }
    private void Start(){
        SetActor(GetComponentInParent<AIActor>());
        animInterface = GetComponent<ActionAnimatorInterface>();
    }

    public virtual void InitializeAction(AIActor actor)
    {
        animInterface.Initialize();
        SetActor(actor);
        if(actor.GActionPlan.Count > 0)
            actor.GActionPlan.TryPeek(out nextAction);
        actor.currentGOAP_Action = this;
        inPlan = false;
        GetActor().worldstateHandler.SetWorldState(effects);
        PerformAction();
    }
    public virtual void PerformAction(){

    }   
    public virtual void FinishAction(){
        nextAction = null;
        
    }
    public virtual void SetWorldStates(){
        conditions = new WorldStateHandler.WORLD_STATE(new Enum[WorldStateHandler.WORLD_STATE_SIZE]{move,health,awareness,hostility});
        effects = new WorldStateHandler.WORLD_STATE(new Enum[WorldStateHandler.WORLD_STATE_SIZE]{moveEffect,healthEffect,awarenessEffect,hostilityEffect});
    }
    public virtual Enum[] GetEffects(){
        return effects.values;
    }
    public virtual bool GetProceduralConditions(){
        return false;
    }
    public float GetHeuristicCost()
    {
        return heuristicCost;
    }
    public virtual void SetHeuristicCost()
    {
        heuristicCost = baseCost;// plus something TODO
    }

    public void CostUp()
    {
        //TODO
    }
    public void CostChange(int i)
    {
        //TODO
    }
    public void CostDown()
    {
        //TODO        
    }
    public void EvaluateAttackSuccess(bool success)
    {
        if (success)
            CostDown();
        else
            CostUp();
        
    }
    public virtual Goals GetGoal()
    {
        return goal;
    }
    public string GetName()
    {
        return actionName;
    }
    public AIActor GetActor()
    {
        return myActor;
    }
    public GOAP_Action SetActor(AIActor newActor)
    {
        myActor = newActor;
        return this;
    }
}