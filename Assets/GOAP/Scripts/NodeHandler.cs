using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeHandler : MonoBehaviour
{
    public VisionNodeHandler vision;  
    public ActionNodeHandler action;

    public VisionNode[] GetVisionNodes(float minRange, float maxRange){
        var selection = new List<VisionNode>();
        for(int i = 0; i < vision.visionNodes.Count;i++)
            if(vision.visionNodes[i].NodeInRange(transform.position,minRange,maxRange))
                selection.Add(vision.visionNodes[i]);
        return selection.ToArray();
    }
    public VisionNode[] GetVisionNodes(float minRange, float maxRange, float visionThreshold){
        var selection = new List<VisionNode>();
        foreach(VisionNode node in vision.visionNodes)
            if(node.NodeInRange(transform.position,minRange,maxRange) && node.subnodesWithVision == visionThreshold)
                selection.Add(node);
        return selection.ToArray();
    }
    public ActionNode GetActionNode(AIActor actor, GOAP_Action.Goals goal){
        var N = 0;
        var D = Mathf.Infinity;
        var distance = 0f;
        for(int i = 0; i < action.actionNodes.Length; i++){
            if(action.actionNodes[i].GetGoal().Equals(goal) && action.actionNodes[i].isAvailable){
                distance = action.actionNodes[i].GetDistance(actor.transform.position);
                if(distance < D){
                    D = distance;
                    N = i;            
                }
            }
        }
        return action.actionNodes[N];
    }
    public ActionNode[] GetActionNode(Vector3 position, float minRange, float maxRange){
        var A = new List<ActionNode>();
        var D = maxRange;
        var distance = minRange;
        for(int i = 0; i < action.actionNodes.Length; i++){
            if(action.actionNodes[i].isAvailable){
                distance = action.actionNodes[i].GetDistance(position);
                if(distance > minRange && distance < maxRange)
                    A.Add(action.actionNodes[i]);
            }
        }
        return A.ToArray();
    }
}
