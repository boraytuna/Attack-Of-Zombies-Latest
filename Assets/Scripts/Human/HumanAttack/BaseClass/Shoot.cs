using UnityEngine;

public abstract class Shoot : MonoBehaviour, IAttacker
{   
    [SerializeField] protected AttackerDamageManager attackerDamageManager;
    [SerializeField] protected float baseDamage;
    [SerializeField] protected LayerMask zombieLayer;   // Layer mask to detect zombies
    [SerializeField] protected LayerMask obstacleLayer; // Layer mask to detect obstacles
    [SerializeField] protected Transform shootPoint;    // Point from which the raycast will be shot
    [SerializeField] protected float attackRange;
    [SerializeField] protected float shootingInterval;

    protected float lastAttackTime;
    protected AudioManager audioManager;

    protected virtual void Start()
    {
        if (shootPoint == null)
        {
            // If no shoot point is specified, use the middle of the attacker
            shootPoint = transform;
        }
        lastAttackTime = -1f;
        PlayIdleAnimation();
        attackerDamageManager = FindObjectOfType<AttackerDamageManager>();
    }

    protected abstract void PlayAttackAnimation();
    protected abstract void PlayIdleAnimation();

    // Change PlayShootingSound to virtual
    protected abstract void PlayShootingSound();

    public void Attack(Collider targetCollider)
    {
        // Check if enough time has passed since the last attack
        if (Time.time >= lastAttackTime + shootingInterval)
        {
            
            Vector3 targetPosition = targetCollider.bounds.center; // Aim at the center of the target's collider
            Vector3 direction = (targetPosition - shootPoint.position).normalized;

            Debug.DrawRay(shootPoint.position, direction * attackRange, Color.red, 1f);  // Draw the ray for debugging

            // Check if the raycast hits an obstacle first
            if (Physics.Raycast(shootPoint.position, direction, out RaycastHit hit, attackRange, zombieLayer | obstacleLayer))
            {
                if ((zombieLayer.value & (1 << hit.collider.gameObject.layer)) != 0)
                {
                    // Hit a zombie
                    //Debug.Log("Raycast hit: " + hit.collider.gameObject.name);
                    PlayAttackAnimation(); // Play attack animation
                    
                    if(AttackerDamageManager.Instance != null)
                    {
                        float multiplier = attackerDamageManager.GetMultiplier();
                        //Debug.Log("Multiplier = " + multiplier);
                        float adjustedDamage = baseDamage * multiplier;

                        // Debug.Log("Current multiplier: " + multiplier);
                        // Debug.Log("Base damage: " + baseDamage);
                        // Debug.Log("Adjusted damage: " + adjustedDamage);

                        IDamagable damagable = targetCollider.GetComponent<IDamagable>();
                        if (damagable != null)
                        {
                            damagable.TakeDamage(adjustedDamage);
                            //Debug.Log("Dealt " + adjustedDamage + " damage to " + targetCollider.gameObject.name);
                        }
                        else
                        {
                            Debug.LogWarning("No IDamagable component found on " + targetCollider.gameObject.name);
                        }

                        lastAttackTime = Time.time;
                    }
                    else
                    {
                        Debug.LogError("AttackerDamager is null");
                    }
                   
                }
                else
                {
                    // Hit an obstacle
                    Debug.Log("Raycast hit an obstacle before hitting a zombie.");
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
            //Debug.Log("Attack on cooldown.");
            PlayIdleAnimation(); // Play idle animation when attack is on cooldown
        }
    }
}
