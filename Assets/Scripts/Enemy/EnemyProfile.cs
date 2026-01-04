using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class EnemyProfile : MonoBehaviour
{
    [SerializeField] private string enemyName;
    [SerializeField] private int maxHealth, damage, limit;
    [SerializeField] private float speed, acceleration;
    [SerializeField] private bool boss;
    [SerializeField] private Animator animator;
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private EnemyDamage enemyDamage;
    [SerializeField] private PlayerDamage playerDamage;
    [SerializeField] private AIPath pathfinding;
    [SerializeField] private GameObject animationObject, tokenParticleObject;
    [SerializeField] private Collider2D mainCollider;
    public bool hasDrop;
    public bool animEnable;


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
}
