using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private KnightSpawner _spawner;
    [SerializeField] private ResourceScanner _scanner;
    [SerializeField] private Animator _animator;

    private void OnEnable()
    {
        _scanner.ResourcesCounting += OnResourcesCountingChanged;
    }

    private void OnDisable()
    {
        _scanner.ResourcesCounting -= OnResourcesCountingChanged;
    }

    private void OnResourcesCountingChanged(int resourceCount)
    {
        if (resourceCount > 0)
            AssignResourceKnight();
    }

    private void AssignResourceKnight()
    {
        if (_spawner.TryGetFreeUnit(out Knight freeKnight) == false)
            return;

        Coin coinTarget = _scanner.DequeueResource();

        if (coinTarget != null)
            _spawner.SendUnitToResource(freeKnight, coinTarget);
    }
}
