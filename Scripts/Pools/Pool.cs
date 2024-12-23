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

    protected virtual T CreateNewObject()
    {
        T newObj = Instantiate(Template, Container);

        newObj.Deactivate();
        ObjectsPool.Add(newObj);

        return newObj;
    }

    private void ActivateObject(T obj, Vector3 position)
    {
        obj.transform.position = position;

        obj.Activate();
    }

    public void Initialize()
    {
        for (int i = 0; i < Capacity; i++)
            CreateNewObject();
    }

    public T GetObject(Vector3 position)
    {
        T newObj = ObjectsPool.FirstOrDefault(obj => obj.IsActive == false);

        if (newObj == null)
        {
            newObj = CreateNewObject();
        }

        ActivateObject(newObj, position);

        return newObj;
    }

    public T GetFirstActiveObject()
    {
        return ObjectsPool.FirstOrDefault(obj => obj.IsActive == true);
    }

    public List<T> GetListActiceObjects()
    {
        List<T> tempList = new List<T>();

        foreach (T obj in ObjectsPool)
        {
            if (obj.IsActive)
            {
                tempList.Add(obj);
            }
        }
        return tempList;
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
