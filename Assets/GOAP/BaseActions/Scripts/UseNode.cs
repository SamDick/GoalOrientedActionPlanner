using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

    //Nodes range from protective cover all the way to dogs that need to be pet
    //sometimes a node will call for an actor, other times an actor will seek a node
    //In all cases:

    //Nodes are transforms with the ability to make the actor do different things
    //An actor calls the node's UseNode function and in turn a dance plays out
    // between the two. Other actors may be called to the instance. More nodes
    // may be instantiated.
    //This action is very powerful, modular. Its depth is limited only by imagination.
public class UseNode : GOAP_Action
{

    private ActionNode node;
    /**
        UseNode, as a generic action, needs to be goal-agnostic
        As such, the action is entirely dependent on the Actors available ActionNodes
        Different action nodes affect this action per their parameters

        This happens after the plan is completed, but before the plan is initialized
    */
    public override void InitializeAction(AIActor actor){
        actionName = node.actionName;
        base.InitializeAction(actor);
    }
    public override void PerformAction(){
        
        StartCoroutine(node.UseNodeAction(this));
    }
/**
    UseNode, as a generic action, needs to be goal-agnostic.
    Therefore: When this goal is being considered for the Plan, this.goal is always equal to the Actor.goal
    Simultaneously, the action needs to reflect the chosen node

    A seperate method of evaluation needs to be made for LinkAction selection
*/
    public override Goals GetGoal(){
        node = GetActor().GetNodeHandler().GetActionNode(GetActor(), goal);
        goal = node.goal;
        target = node.transform;
        conditions = new WorldStateHandler.WORLD_STATE(node.GetConditions().GetState());
        return goal;
    }
    public override Enum[] GetEffects(){
        var nodes = GetActor().GetNodeHandler().GetActionNode(GetActor().transform.position,minRange,maxRange);
        for(int i = 0; i < nodes.Length; i++){
            if(GetActor().worldstateHandler.CompareWorldState(GetActor().GActionPlan.Peek().conditions.values,nodes[i].GetEffects().GetState()) == 0){
                node = nodes[i];
                effects = new WorldStateHandler.WORLD_STATE(node.GetEffects().GetState());
                break;
            }
        }
        return effects.values;
    }
    public override void SetHeuristicCost(){
        baseCost = node.GetBaseCost();
        base.SetHeuristicCost();
    }
    public override void FinishAction(){
        actionName = null;
    }
}
