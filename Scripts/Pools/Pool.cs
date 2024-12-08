using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pool<T> : MonoBehaviour where T : ObjectablePoll
{
    [SerializeField] protected T Template;
    [SerializeField] protected Transform Container;
    [SerializeField] protected int Capacity;

    protected List<T> ObjectsPool = new List<T>();

    public int Count => ObjectsPool.Count;

    protected T CreateNewObject()
    {
        T newObj = Instantiate(Template, Container);

        newObj.Deactivate();
        ObjectsPool.Add(newObj);

        return newObj;
    }

    public void Initialize()
    {
        for (int i = 0; i < Capacity; i++)
            CreateNewObject();
    }

    public T GetObject(Vector3 position)
    {
        T newObj = ObjectsPool.FirstOrDefault(obj => !obj.IsActive);

        newObj.Activate();
        newObj.transform.position = position;

        return newObj;
    }

    public int CountActivatedObjects()
    {
        return ObjectsPool.Count(obj => obj.IsActive);
    }

    public bool IsAllObjectsActive()
    {
        return ObjectsPool.All(target => target.IsActive);
    }
}
