using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceScanner : MonoBehaviour
{
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private float _radius;
    [SerializeField] private float _interval;

    private Queue<Coin> _discoveredObjects = new Queue<Coin>();
    private HashSet<Coin> _uniqueObjects = new HashSet<Coin>();
    private Coroutine _scanningCoroutine;
    private WaitForSeconds _delay;
    private bool _isMapScanning;
    private int _maxUniqueObjects = 15;

    public int ResourceCount => _discoveredObjects.Count;

    public event Action<int> ResourcesCounting;

    private void Awake()
    {
        _delay = new WaitForSeconds(_interval);
    }

    private void Start()
    {
        BeginScanning();
    }

    public bool HaveResource()
    {
        return _discoveredObjects.Count > 0;
    }

    public Coin DequeueResource()
    {
        if (HaveResource() == false)
            return null;

        return _discoveredObjects.Dequeue();
    }

    private void BeginScanning()
    {
        if (_scanningCoroutine != null)
        {
            StopCoroutine(_scanningCoroutine);
        }

        _isMapScanning = true;
        _scanningCoroutine = StartCoroutine(CheckRoutine());
    }

    private IEnumerator CheckRoutine()
    {
        while (_isMapScanning)
        {
            CheckupMap();

            yield return _delay;
        }
    }

    private void CheckupMap()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 
            _radius, _targetLayer);

        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent(out Coin resource))
            {
                if (_uniqueObjects.Add(resource))
                {
                    _discoveredObjects.Enqueue(resource);
                }
            }
        }

        ResourcesCounting?.Invoke(_discoveredObjects.Count);
        CleanListUniqueObjects();
    }

    private void CleanListUniqueObjects()
    {
        if (_uniqueObjects.Count > _maxUniqueObjects)
            _uniqueObjects.Clear();
    }
}
