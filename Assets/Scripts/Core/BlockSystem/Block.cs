using UnityEngine;

public abstract class Block : MonoBehaviour
{
    public Vector2 gridIndex;
    public Transform target;
    public abstract BlockTypes blockType { get;}
    public abstract Vector3 spriteSize { get; }
    
    public abstract void DoTappedActions();
    public abstract void SetupBlock();
    public abstract void MoveToTarget(float arriveTime);
    public abstract void UpdateSortingOrder();
    public abstract void SetSortingLayerName(string layerName);
    public abstract void SetSortingOrder(int index);
    
}