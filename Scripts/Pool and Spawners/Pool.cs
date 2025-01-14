using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pool<T> : MonoBehaviour where T : PoolableObject
{
    [SerializeField] private T _template;
    [SerializeField] private Transform _container;
    [SerializeField] private int _capacity;

    private List<T> _objectsPool = new List<T>();

    public int Count => _objectsPool.Count;

    protected T Template => _template;
    protected List<T> ObjectsPool => _objectsPool;
    protected Transform Container => _container;
    protected int Capacity => _capacity;

    public void Initialize()
    {
        for (int i = 0; i < _capacity; i++)
            CreateNewObject();
    }

    public T GetObject(Vector3 position)
    {
        T newObject = _objectsPool.FirstOrDefault(subject => subject.IsActive == false);

        if (newObject == null)
            newObject = CreateNewObject();

        ActivateObject(newObject, position);

        return newObject;
    }

    public List<T> GetListActiveObjects()
    {
        List<T> tempList = new List<T>();

        foreach (T subject in _objectsPool)
        {
            if (subject.IsActive)
            {
                tempList.Add(subject);
            }
        }

        return tempList;
    }

    public bool IsAllObjectsActive()
    {
        return _objectsPool.All(subject => subject.IsActive);
    }

    protected virtual T CreateNewObject()
    {
        T newObject = Instantiate(_template, _container);

        newObject.Deactivate();
        _objectsPool.Add(newObject);

        return newObject;
    }

    private void ActivateObject(T subject, Vector3 position)
    {
        subject.transform.position = position;

        subject.Activate();
    }
}
