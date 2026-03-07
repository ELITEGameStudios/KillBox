using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class EnemyProfile : MonoBehaviour
{
    private static float necroSelfDamageMultiplier = 2f;

    [SerializeField] private string enemyName;
    [SerializeField] private int maxHealth, damage, limit;
    [SerializeField] private float speed, acceleration, necroHitInterval = 0.5f, currentNecroHitTimer;
    [SerializeField] private bool boss;
    [SerializeField] private Animator animator;
    [SerializeField] private AIDestinationSetter destinationSetter;
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private EnemyDamage enemyDamage;
    [SerializeField] private PlayerDamage playerDamage;
    [SerializeField] private AIPath pathfinding;
    [SerializeField] private GameObject animationObject, tokenParticleObject;
    [SerializeField] private Collider2D mainCollider;
    public bool hasDrop;
    public bool animEnable;

    public bool canBeNecro => destinationSetter != null && !bypassNecro && !boss;
    public bool bypassNecro, isNecro;
    public bool canHitAsNecro => isNecro && currentNecroHitTimer <= 0;

    public string EnemyName { get => enemyName; private set => enemyName = value; }
    public int MaxHealth { get => maxHealth; private set => maxHealth = value; }
    public int Damage { get => damage; private set => damage = value; }
    public int Limit { get => limit; private set => limit = value; }
    public float Speed { get => speed; private set => speed = value; }
    public float Acceleration { get => acceleration; private set => acceleration = value; }
    public bool Boss { get => boss; private set => boss = value; }


    // Start is called before the first frame update
    void Awake()
    {

        pathfinding = gameObject.GetComponent<AIPath>();
        animator = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
        playerDamage = GetComponent<PlayerDamage>();
        enemyDamage = GetComponent<EnemyDamage>();

        if (pathfinding != null)
        {
            destinationSetter = GetComponent<AIDestinationSetter>();
            speed = pathfinding.maxSpeed;
            acceleration = pathfinding.maxAcceleration;
        }

        if (boss) { EnemyCounter.main.AddBoss(this); }

        EnemyCounter.main.AddEnemy(this);

        if (animEnable)
        {
            DisableEnemy();
            Invoke(nameof(EnableEnemy), 0.75f);
        }
    }

    public void Update()
    {
        if (animator != null)
        {
            animator.SetFloat("Blend", (float)enemyHealth.CurrentHealth / enemyHealth.maxHealth);
        }
        if (tokenParticleObject != null) { tokenParticleObject.SetActive(hasDrop); }

        if (isNecro)
        {
            NecroUpdate();
        }

    }

    public void EnableEnemy()
    {
        animationObject.SetActive(false);
        mainCollider.enabled = true;
        enemyHealth.enabled = true;
        enemyDamage.enabled = true;
        playerDamage.enabled = true;
        pathfinding.enabled = true;
    }

    public void SetAsNecro()
    {
        if(!canBeNecro){return;}
        isNecro = true;
        enemyHealth.CurrentHealth = enemyHealth.maxHealth;
    }

    public void NecroUpdate()
    {
        if(currentNecroHitTimer > 0){currentNecroHitTimer -= Time.deltaTime;}

        // Find closest enemy
        if(EnemyCounter.main.enemyProfiles.Count == 0){return;}
        EnemyProfile targetEnemy = EnemyCounter.main.enemyProfiles[0];
        float distance = Vector2.Distance(targetEnemy.transform.position, transform.position);
        for (int i = 1; i < EnemyCounter.main.enemyProfiles.Count; i++)
        {
            EnemyProfile obj = EnemyCounter.main.enemyProfiles[i];
            if(obj == gameObject){continue;}

            float newDist = Vector2.Distance(obj.transform.position, transform.position);
            if (distance < newDist)
            {
                targetEnemy = obj;
                distance = newDist;
            }
        }

        // Pathfind to closest enemy
        destinationSetter.target = targetEnemy.transform;
    }

    public void HitAsNecro(EnemyProfile otherProfile)
    {
        currentNecroHitTimer = necroHitInterval;
        otherProfile.enemyHealth.TakeDmg(enemyDamage.damage);
        if(enemyDamage.destroyOnHit){enemyHealth.Die();}
        else
        {
            enemyHealth.TakeDmg((int)(enemyDamage.damage * necroSelfDamageMultiplier));
        }
    }

    public void DisableEnemy()
    {
        animationObject.SetActive(true);
        mainCollider.enabled = false;
        enemyHealth.enabled = false;
        enemyDamage.enabled = false;
        playerDamage.enabled = false;
        pathfinding.enabled = false;
    }

    public void Retire()
    {
        PulseEffectManager.instance.AddEffect(transform.position, strength: 0.01f, widthFactor: 0.12f);
        EnemyCounter.main.RemoveEnemy(this);
    }
    
    public void AddDrop()
    {
        hasDrop = true;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyProfile otherProfile = collision.gameObject.GetComponent<EnemyProfile>();
        if(otherProfile != null)
        {
            if (canHitAsNecro)
            {
                HitAsNecro(otherProfile);
            }
        }
    }
}
