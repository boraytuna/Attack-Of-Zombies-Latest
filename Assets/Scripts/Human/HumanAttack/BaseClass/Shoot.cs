using UnityEngine;

public abstract class Shoot : MonoBehaviour, IShoot
{   
    [SerializeField] protected AttackerDamageManager attackerDamageManager;
    [SerializeField] protected float baseDamage;
    [SerializeField] protected LayerMask zombieLayer;
    [SerializeField] protected LayerMask obstacleLayer;
    [SerializeField] protected Transform shootPoint;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float shootingInterval;
    [SerializeField] protected float rotationSpeed = 5f; // Speed at which the attacker rotates towards the target

    protected float lastAttackTime;
    private RaycastHit[] hits = new RaycastHit[100];

    protected AudioManager audioManager;

    protected virtual void Start()
    {
        if (shootPoint == null)
        {
            shootPoint = transform;
        }
        lastAttackTime = -1f;
        attackerDamageManager = GameObject.FindWithTag("HumanManager").GetComponent<AttackerDamageManager>();
    }

    protected abstract void PlayAttackAnimation();
    protected abstract void PlayIdleAnimation();
    protected abstract void PlayShootingSound();

    public void Attack(Collider targetCollider)
    {
        if (Time.time >= lastAttackTime + shootingInterval)
        {
            Vector3 targetPosition = targetCollider.bounds.center;
            Vector3 direction = (targetPosition - shootPoint.position).normalized;

            transform.LookAt(targetCollider.transform);

            int hitCount = Physics.RaycastNonAlloc(shootPoint.position, direction, hits, attackRange, zombieLayer | obstacleLayer);

            if (hitCount > 0)
            {
                for (int i = 0; i < hitCount; i++)
                {
                    RaycastHit hit = hits[i];
                    if ((zombieLayer.value & (1 << hit.collider.gameObject.layer)) != 0)
                    {
                        HandleHit(targetCollider);
                        break;
                    }
                    else if ((obstacleLayer.value & (1 << hit.collider.gameObject.layer)) != 0)
                    {
                        Debug.Log("Raycast hit an obstacle before hitting a zombie.");
                        break;
                    }
                }
            }
            else
            {
                Debug.Log("Raycast did not hit any zombie.");
            }

            lastAttackTime = Time.time;
        }
        else
        {
            PlayIdleAnimation();
        }
    }

    public void LookAtTarget(Vector3 targetPosition)
    {
        // Implementation of looking at the target
        Vector3 direction = (targetPosition - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // Adjust the speed if necessary
    }   

    private void HandleHit(Collider targetCollider)
    {
        PlayShootingSound();
        PlayAttackAnimation();

        if (attackerDamageManager != null)
        {
            float multiplier = attackerDamageManager.GetMultiplier();
            float adjustedDamage = baseDamage * multiplier;

            IDamagable damagable = targetCollider.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(adjustedDamage);
            }
            else
            {
                Debug.LogWarning("No IDamagable component found on " + targetCollider.gameObject.name);
            }
        }
        else
        {
            Debug.LogError("AttackerDamageManager is null");
        }
    }
}
