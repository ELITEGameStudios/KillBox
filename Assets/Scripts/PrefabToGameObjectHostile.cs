using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PrefabToGameObjectHostile : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public GameManager manager;
    public string TargetTag;
    public GameObject player, gameManager;

    public EnemyHealth enemyHealth;
    public EnemyDamage enemyDamage;

    [SerializeField]
    private bool no_pathfinding, no_health;

    public AIDestinationSetter DestSetter;

    // Start is called before the first frame update
    void Start()
    {
        

        if (!no_pathfinding)
             DestSetter = GetComponent<AIDestinationSetter>();

        gameManager = GameObject.Find("Manager");
        manager = gameManager.GetComponent<GameManager>();
        player = Player.main.obj;
        playerHealth = Player.main.health;

        if(!no_pathfinding)
            DestSetter.target = Player.main.tf;

        enemyDamage.playerHealthScript = playerHealth;

        if(!no_health)
            enemyHealth.manager = manager;
    }
}
