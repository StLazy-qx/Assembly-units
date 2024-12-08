using UnityEngine;

public abstract class Spawner<T,N> : MonoBehaviour where T : Pool<N> where N : ObjectablePoll
{
    [SerializeField] protected T Pool;
    [SerializeField] protected Transform SpawnPlace;

    protected float MinAreaX;
    protected float MaxAreaX;
    protected float MinAreaZ;
    protected float MaxAreaZ;
    protected float Half = 0.5f;
    protected float DistanceBetweenPoint;

    protected abstract void OutputObjects();

    protected N GetObject(Vector3 position)
    {
        return Pool.GetObject(position);
    }
}