using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

/**
    Collections of all states on each layer, subdivided into groups based on the first string before '_', adding
    each of those to their own list
*/
public class ActionAnimatorInterface : MonoBehaviour
{
    public List<string> animationStates;
    private GOAP_Action action;
    private string actionName;
    AnimatorController aniCon;
    public void Initialize(){
        if(action == null)
            action = GetComponent<GOAP_Action>();
        action.SetActor(action.GetComponentInParent<AIActor>());
        actionName = action.GetName();
        aniCon = action.GetActor().animator.runtimeAnimatorController as AnimatorController;
        animationStates = new List<string>();
        var states = aniCon.layers[0].stateMachine.states;
        foreach(ChildAnimatorState s in states){
            if(s.state.name.Contains(actionName))
                animationStates.Add(s.state.name);
        }
        var i = 0;
        for(int x = 0; x < animationStates.Count; x++){
            for(i = 0; i < animationStates.Count; i++){
                if(animationStates[x].Contains(i.ToString()) && x != i){
                    var temp = animationStates[i];
                    var t = x;
                    animationStates[i] = animationStates[x];
                    animationStates[t] = temp;
                }
            }
            i = 0;
        }
    }
    public void PlayAnimation(int index){
        action.GetActor().animator.Play(animationStates[index]);
    }
}
