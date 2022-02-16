using UnityEngine;

public class TranslateOverTime : MonoBehaviour
{
    public float rotationsPerSecond;
    public Vector3 axis;
    private void Update(){
        axis.Normalize();
        transform.Rotate(axis*rotationsPerSecond);
    }
}
