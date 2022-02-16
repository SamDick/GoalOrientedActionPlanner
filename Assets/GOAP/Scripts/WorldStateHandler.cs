using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/**
    Actor state recording & comparing

    -HEIRARCHY of NEEDS-
    The array of enumerated values representing the minutae of State are ordered, from top to bottom, from most important to least
    
*/
public class WorldStateHandler : MonoBehaviour
{
    public const int WORLD_STATE_SIZE = 4;
    public enum Move {NA  = -1, Stand, GoTo, Fight};
    public enum Health {NA = -1, Alive, Dead};
    public enum Awareness {NA  = -1, Unaware, Alert, Aware, Afraid};
    public enum Hostility {NA  = -1, Friendly, Neutral, Hostile}
    public Move move;
    public Health health;
    public Awareness awareness;
    public Hostility hostility;
    Enum[] WorldState;
    Enum[] initWorldState;
    void Awake(){
        
    }
    private void Start(){
        WorldState = new Enum[4]{
            move,
            health,
            awareness,
            hostility
        };
        initWorldState = WorldState;
    }
    void LateUpdate(){
        move =      (Move)WorldState[0];
        health =    (Health)WorldState[1];
        awareness = (Awareness)WorldState[2];
        hostility = (Hostility)WorldState[3];
    }
    public void ResetWorldState(){
        WorldState = initWorldState;
    }
    public void SetWorldState(WORLD_STATE newWorldState){
        var condition = 0;
        for(int i = 0; i < WorldState.Length; i++){
            condition = Convert.ToInt32(newWorldState.values[i]);
            if(condition.Equals(Convert.ToInt32(this.WorldState[i])) && condition >= 0)
                WorldState[i] = newWorldState.values[i];
        }
    }
    public void SetWorldState(int index, Enum value){
        WorldState[index] = value;
    }
    public Enum[] GetState(){
        return WorldState;
    }
    /**
     * Compares against the Actor's WORLD_STATE
     * All condition parameters have a "Not Applicable" value at index -1
     */
    public bool CompareWorldState(Enum[] conditions){
        var condition = 0;
        for(int i = 0; i < conditions.Length; i++){
            condition = Convert.ToInt32(conditions[i]);
            if(condition.Equals(Convert.ToInt32(this.WorldState[i])) || condition < 0)
                continue;
            return false;
        }
        return true;
    }
    /**
     * Compares against the Actor's WORLD_STATE
     * All condition parameters have a "Not Applicable" value at index -1
     */
    public int CompareWorldState(Enum[] conditions, float varianceLimit){
        var differences = 0;
        var condition = 0;
        for(int i = 0; i < conditions.Length; i++){
            condition = Convert.ToInt32(conditions[i]);

            if(condition.Equals(Convert.ToInt32(this.WorldState[i])) || condition < 0)
                continue;
            differences++;
        }
        return differences;
    }
    /**
     * Returns - 
     * differences  : int value representing the number of differences between the two sets
     */
    public int CompareWorldState(Enum[] conditions, Enum[] effects){
        var differences = 0;
        var condition = 0;
        for(int i = 0; i < conditions.Length; i++){
            condition = Convert.ToInt32(conditions[i]);
            if(condition.Equals(Convert.ToInt32(effects[i])) || condition.Equals(Convert.ToInt32(this.WorldState[i]))  || condition < 0)
                continue;
            differences++;
        }
        return differences;
    }
    public struct WORLD_STATE{
        public Enum[] values;
        public WORLD_STATE(Enum[] _values){
            values = _values;
        }
    }
}
