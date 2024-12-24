using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pool<T> : MonoBehaviour where T : PoolableObject
{
    [SerializeField] protected T Template;
    [SerializeField] protected Transform Container;
    [SerializeField] protected int Capacity;

    protected List<T> ObjectsPool = new List<T>();

    public int Count => ObjectsPool.Count;

    protected virtual T CreateNewObject()
    {
        T newObject = Instantiate(Template, Container);

        newObject.Deactivate();
        ObjectsPool.Add(newObject);

        return newObject;
    }

    private void ActivateObject(T subject, Vector3 position)
    {
        subject.transform.position = position;

        subject.Activate();
    }

    public void Initialize()
    {
        for (int i = 0; i < Capacity; i++)
            CreateNewObject();
    }

    public T GetObject(Vector3 position)
    {
        T newObject = ObjectsPool.FirstOrDefault(subject => subject.IsActive == false);

        if (newObject == null)
        {
            newObject = CreateNewObject();
        }

        ActivateObject(newObject, position);

        return newObject;
    }

    public List<T> GetListActiceObjects()
    {
        List<T> tempList = new List<T>();

        foreach (T subject in ObjectsPool)
        {
            if (subject.IsActive)
            {
                tempList.Add(subject);
            }
        }
        return tempList;
    }

    public T GetFirstActiveObject()
    {
        return ObjectsPool.FirstOrDefault(subject => subject.IsActive == true);
    }

    public int CountActivatedObjects()
    {
        return ObjectsPool.Count(subject => subject.IsActive);
    }

    public bool IsAllObjectsActive()
    {
        return ObjectsPool.All(subject => subject.IsActive);
    }
}
