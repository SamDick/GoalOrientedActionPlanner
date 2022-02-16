using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionNode : MonoBehaviour
{   
    private VisionNodeHandler handler;
    private Transform[] subnodes;
    public float offset;
    public int subnodesWithVision;
    Transform target;
    
    LayerMask layerMask;
    Vector3 position;
    int scanTimer;
    LayerMask mask;
    public string targetName = "Player";
    void Start()
    {
        mask = LayerMask.GetMask("Player","Environment");
        position = transform.position;
        subnodes = GetComponentsInChildren<Transform>();
        handler = GetComponentInParent<VisionNodeHandler>();
        handler.visionNodes.Add(this);
    }
    void LateUpdate(){ 
        if(scanTimer<0){
            scanTimer = handler.resfreshRate;
            subnodesWithVision -= subnodesWithVision>0?1:0;
            for (int i = 0; i < subnodes.Length; i++){
                Debug.DrawRay(subnodes[i].position, target.position - subnodes[i].position,Color.red);

            if(Physics.Raycast(subnodes[i].position, target.position - subnodes[i].position, out var hit, handler.range, mask))
                if (hit.transform.name != targetName ||(subnodes[i].position-hit.point).magnitude>handler.maximumDist)   
                    continue;
                else
                    subnodesWithVision++;
            }
            scanTimer--;
        }
    }
    public void Initialize(VisionNodeHandler handler){
        this.handler = handler;
        target = handler.target;
        Physics.Raycast(transform.position, Vector3.down,out var hit,10,LayerMask.GetMask("Environment"));
        transform.position = hit.point + offset * Vector3.up;
        foreach (Transform t in GetComponentsInChildren<Transform>())
        {
            if (t == transform || t.gameObject.name.Equals("Sphere"))
                continue;
            if(Physics.Raycast(t.position  + (offset * t.forward), Vector3.down,out hit))
                t.position = hit.point + offset * Vector3.up;
            else
                t.localPosition = offset * Vector3.up;
            
        }
    }
    public void Scan(){
        subnodesWithVision = 0;
        
        for(int i = 0; i < subnodes.Length; i++){

        if(Physics.Raycast(subnodes[i].position, target.position - subnodes[i].position, out var hit ,handler.range,mask,QueryTriggerInteraction.Ignore)){
            
            Debug.DrawRay(subnodes[i].position, hit.point - subnodes[i].position,Color.red);
            if (hit.transform.name != "Player" || (subnodes[i].position - hit.point).magnitude>handler.maximumDist)   
                continue;
            else
                subnodesWithVision++;
            }    
        }   
    }
    //A normalized representation of the subnodes with vision belonging to this VisionNode

    public bool NodeInRange(Vector3 pos, float min, float max){
        var D = (pos-transform.position).magnitude;
        return (D > min && D < max);
    }    
    public float GetVisibility(){
        return Mathf.Round(subnodesWithVision)/Mathf.Round(subnodes.Length);
        
    }
    public Vector3 GetPosition(){
        return position;
    }
    public void SetTarget(Transform target){
        this.target = target;
    }
}
public struct ScanResultsContainer{

    public float distance;
    public ScanResultsContainer(float distance)
    {
        this.distance = distance;
    }
}
