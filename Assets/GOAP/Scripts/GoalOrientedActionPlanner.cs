using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEditor;
using System;
using System.Threading;

public class GoalOrientedActionPlanner
{
    public static int VARIANCE = 88;
    public static void ActionPlanner(ActionPlanRequestManager.PlanRequest request, GOAP_Action initAction, GOAP_Action.Goals goal)
    {
        var actor = request.actor;
        var givenActions = actor.actionLibrary;
        List<GOAP_Action> possibleActions = new List<GOAP_Action>();
        if (givenActions != null && initAction == null){
            initAction = SelectPrimaryAction(actor, givenActions, goal);
            actor.target = initAction.target;
        }
        if(initAction == null){
            Debug.LogError("There are no actions available for '" + actor.name + "' to perform");
            return;
        }
        
        if(actor.worldstateHandler.CompareWorldState(initAction.conditions.values, VARIANCE) == 0 || SelectLinkActions(actor,givenActions,initAction))
            try{
                ActionPlanRequestManager.instance.FinishedProcessingPlan();
            }
            catch { //pop the plan entirely
                Debug.LogError("Unable to finish plan: the variance between action.conditions & actor.WorldState is too great\nLast action added: " + 
                (actor.GActionPlan.Count>0?actor.GActionPlan.Peek().gameObject.name:"None") + 
                "\n" + "Actor: " + actor.gameObject.name);
            }
    }

    static GOAP_Action SelectPrimaryAction(AIActor actor, List<GOAP_Action> givenActions, GOAP_Action.Goals goal)
    {

        List<GOAP_Action> possibleActions = new List<GOAP_Action>();

        foreach (GOAP_Action action in givenActions){
            if (!action.GetGoal().Equals(goal))
                continue;
            action.SetHeuristicCost();
            possibleActions.Add(action);
        }
        
        if(possibleActions.Count==0){
            Debug.LogWarning("\nThere are no actions available for '" + actor.name + "' with the goal '" +goal.ToString()+"'.");
            if(goal.Equals(GOAP_Action.Goals.Idle)){
                return null;
            }
            Debug.LogWarning("\n Actor entering idle state.");
            return SelectPrimaryAction(actor,givenActions,GOAP_Action.Goals.Idle);
        }

        int i = 0;
        GOAP_Action f = null;
        var h = Mathf.Infinity;
        foreach(GOAP_Action a in possibleActions){
            if (a.GetHeuristicCost() < h){
                h = a.GetHeuristicCost();
                f = a;
            }
            i++;

        }
        if ( f!=null){
            actor.planCost+=f.GetHeuristicCost();
            actor.GActionPlan.Push(f);
            return f;
        }
        else{
            Debug.Break();
            return null;
        }
            
    }
    /**
     * Compares against the in-focus Action's Effects.
     * Compares against the Actor's WORLD_STATE
     * 
     * Adds new actions that align with the first set and recursively adds new actions, updating the first set
     * 
     * Returns true when the last action added is aligned with the second set.
     */
    static bool SelectLinkActions(AIActor actor, List<GOAP_Action> givenActions, GOAP_Action initAction){
        var initialActionConditions = initAction.conditions;
        givenActions.Remove(initAction);
        for(int variance = 0; variance < initialActionConditions.values.Length; variance++){
            foreach(GOAP_Action action in givenActions){
                if(actor.worldstateHandler.CompareWorldState(initialActionConditions.values, action.GetEffects()) > variance)  
                    continue;
                actor.GActionPlan.Push(action);
                actor.planCost+=action.GetHeuristicCost();

                if(actor.worldstateHandler.CompareWorldState(action.conditions.values, VARIANCE)>0)
                    return SelectLinkActions(actor,givenActions,action);
                else
                    return true;
            }
        }
        return false;
    }
}