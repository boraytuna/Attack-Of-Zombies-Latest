using UnityEngine;

public abstract class Shoot : MonoBehaviour, IAttacker
{
    [SerializeField] protected float damage = 10f;      // Damage dealt by the attacker
    [SerializeField] protected float attackRange = 5f; // Range of the attack
    [SerializeField] protected float attackCooldown = 1f; // Cooldown period between attacks
    [SerializeField] protected LayerMask zombieLayer;   // Layer mask to detect zombies
    [SerializeField] protected Transform shootPoint;    // Point from which the raycast will be shot

    protected float lastAttackTime; // Time when the last attack occurred

    protected abstract void PlayAttackAnimation();
    protected abstract void PlayIdleAnimation();

    protected virtual void Start()
    {
        if (shootPoint == null)
        {
            // If no shoot point is specified, use the middle of the attacker
            shootPoint = transform;
        }
        lastAttackTime = -attackCooldown; // Initialize to allow immediate attack
        PlayIdleAnimation(); // Set the initial animation to idle
    }

    public void Attack(Collider targetCollider)
    {
        // Check if enough time has passed since the last attack
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            PlayAttackAnimation(); // Play attack animation
            Vector3 targetPosition = targetCollider.bounds.center; // Aim at the center of the target's collider
            Vector3 direction = (targetPosition - shootPoint.position).normalized;

            Debug.DrawRay(shootPoint.position, direction * attackRange, Color.red, 1f);  // Draw the ray for debugging

            if (Physics.Raycast(shootPoint.position, direction, out RaycastHit hit, attackRange, zombieLayer))
            {
                Debug.Log("Raycast hit: " + hit.collider.gameObject.name);  // Log the hit object name

                IDamagable damagable = hit.collider.GetComponent<IDamagable>();
                if (damagable != null)
                {
                    damagable.TakeDamage(damage);
                    Debug.Log("Attacked zombie: " + hit.collider.gameObject.name);
                }
                else
                {
                    Debug.Log("No IDamagable component found on hit object.");
                }
            }
            else
            {
                Debug.Log("Raycast did not hit any zombie.");
            }

            // Update the last attack time
            lastAttackTime = Time.time;
        }
        else
        {
            Debug.Log("Attack on cooldown.");
            PlayIdleAnimation(); // Play idle animation when attack is on cooldown
        }
    }
}
