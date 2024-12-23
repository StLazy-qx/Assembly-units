using UnityEngine;

public abstract class Spawner<T> : MonoBehaviour where T: ObjectablePoll
{
    [SerializeField] protected Pool<T> PoolObjects;

    [SerializeField] private Transform _spawnPlace;

    private float _minAreaX;
    private float _maxAreaX;
    private float _minAreaZ;
    private float _maxAreaZ;
    private const float Half = 0.5f;

    private void Awake()
    {
        InitializeAreaBounds();
        PoolObjects.Initialize();
        OnAwake();
    }

    protected Vector3 DetermineSpawnCoordinate()
    {
        return new Vector3(
            Random.Range(_minAreaX, _maxAreaX),
            _spawnPlace.position.y,
            Random.Range(_minAreaZ, _maxAreaZ));
    }

    protected virtual void OnAwake() { }

    protected abstract void OutputObjects();

    private void InitializeAreaBounds()
    {
        Vector3 spawnScale = _spawnPlace.localScale;
        Vector3 spawnPosition = _spawnPlace.position;
        _minAreaX = spawnPosition.x - (spawnScale.x * Half);
        _maxAreaX = spawnPosition.x + (spawnScale.x * Half);
        _minAreaZ = spawnPosition.z - (spawnScale.z * Half);
        _maxAreaZ = spawnPosition.z + (spawnScale.z * Half);
    }
}