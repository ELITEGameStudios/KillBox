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
        AIPath pathfinding = gameObject.GetComponent<AIPath>();
        if(pathfinding != null){
            speed = pathfinding.maxSpeed;
            acceleration = pathfinding.maxAcceleration;
        }

        if(boss){ EnemyCounter.main.AddBoss(this); }
        
        EnemyCounter.main.AddEnemy(this);
    }

    public void Retire(){
        EnemyCounter.main.RemoveEnemy(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
