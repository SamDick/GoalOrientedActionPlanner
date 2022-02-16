using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///Each area of navmesh requires its own node handler
//manually add the nodes to the visionNodes collection
//integer ID to tie actors to their respective node handlers
public class VisionNodeHandler : MonoBehaviour
{
    public int ID;
    public List<VisionNode> visionNodes;
    public List<VisionNode> activeNodes;
    public Transform target;
    public int resfreshRate = 60;
    public float maximumDist;
    public float range;
    float timer;
    void Awake(){
        visionNodes = new List<VisionNode>();
        activeNodes = new List<VisionNode>();
    }
    void Start()
    {
        InitializeNodes();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        
        timer++;
        if (timer > resfreshRate){
            StartCoroutine(RefreshNodes());
            timer = 0;
        }
    }
    public void InitializeNodes(){
        foreach(VisionNode node in visionNodes)
            node.Initialize(this);
    }
    public IEnumerator RefreshNodes(){
        var nodes = new List<VisionNode>();
        foreach(VisionNode node in visionNodes){
            if (node.subnodesWithVision >= 1)
                nodes.Add(node);
            
        }
        this.activeNodes = nodes;
        yield break;
    }
    public VisionNode GetNearestNode(Vector3 position){
        var dist = maximumDist;
        var nearest = -1;
        var newDist = 0f;
        for(int i = 0; i < visionNodes.Count;i++){
            newDist = (visionNodes[i].transform.position - position).magnitude;
            if (newDist < dist){
                nearest = i;
                dist = newDist;
            }
        }
        if (nearest>=0)
            return visionNodes[nearest];
        else
            return visionNodes[0];
    }
    public VisionNode GetNearestNodeToPlayer(){
        //timer+=6;///Cheap replacement for creating a request/callback architecture. Probably causes bugs;
        var dist = maximumDist;
        var nearest = -1;
        var newDist = 0f;
        for(int i = 0; i < activeNodes.Count;i++){
            if (activeNodes[i].subnodesWithVision < 3)
                continue;
            newDist = (activeNodes[i].transform.position - target.position).magnitude;
            if (newDist < dist && newDist < maximumDist){
                nearest = i;
                dist =  (activeNodes[i].transform.position - target.position).magnitude;
            }
        }
        if (activeNodes.Count>0 && nearest>0)
            return activeNodes[nearest];
        else
            return GetRandom();

    }
    public void GetFurthestNodeFromPlayer(){

    }
    public VisionNode GetRandom(){
        return visionNodes[Random.Range(0,visionNodes.Count)];
    }
        public VisionNode GetRandom(Vector3 pos){
        var x = GetNearestNode(pos);
        if(visionNodes[Random.Range(0,visionNodes.Count)].Equals(x))
            return GetRandom();
        return visionNodes[Random.Range(0,visionNodes.Count)];
    }
}
