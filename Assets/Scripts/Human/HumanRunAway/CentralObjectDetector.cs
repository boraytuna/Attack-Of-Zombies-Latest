using UnityEngine;

public abstract class CentralObjectDetector : MonoBehaviour
{
    private float _detectionRange = 10f;  // Default value

    public float DetectionRange
    {
        get => _detectionRange;
        set => _detectionRange = value;
    }

    [SerializeField] private LayerMask detectionLayer;
    public int maxColliders = 10;

    private Collider[] hitColliders;

    private void Awake()
    {
        hitColliders = new Collider[maxColliders];
    }

    private void Update()
    {
        DetectObjects();
    }

    private void DetectObjects()
    {
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, _detectionRange, hitColliders, detectionLayer);
        for (int i = 0; i < numColliders; i++)
        {
            Collider hitCollider = hitColliders[i];
            // Check for IHuman or IAttacker interfaces
            if (hitCollider.TryGetComponent(out IHuman human))
            {
                OnHumanDetected(human);
            }
            else if (hitCollider.TryGetComponent(out IShoot shoot))
            {
                OnAttackerDetected(shoot);
            }
        }
    }

    protected abstract void OnHumanDetected(IHuman human);
    protected abstract void OnAttackerDetected(IShoot shoot);
}
