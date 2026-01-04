using System.Collections.Generic;
using UnityEngine;

public class EnemyList : MonoBehaviour
{
    [SerializeField]
    private GameManager manager; 

    [SerializeField]
    private Spawn spawn; 
    [SerializeField]
    private GameObject[] fire_enemy_types, fire_mini_bosses;
    public EnemyEntry[] enemyEntries;
    
    [SerializeField]
    private List<int> bossMaps;
    public List<int> BossMaps { get => bossMaps; private set => bossMaps = value; }

    [SerializeField]
    private int boss_round_counter = 11;
    public static EnemyList instance { get; private set; }

    [System.Serializable]
    public struct EnemyEntry
    {
        public int debutDif;
        public int retireDif;
        public int spawnTickets;
        public GameObject enemyObject;
    }

    void Awake()
    {

        if (instance == null) { instance = this; }
        else if (instance != this) { Destroy(this); }
    }

    public List<GameObject> UpdatedList
    {
        get // New Implementation
        {
            float difficulty = GameManager.main.Difficulty;
            List<GameObject> result = new List<GameObject>();

            if (GameManager.main.LvlCount == 1)
            {
                result.Add(enemyEntries[0].enemyObject);
                return result;
            }

            for (int i = 0; i < enemyEntries.Length; i++)
            {
                EnemyEntry candaditeEntry = enemyEntries[i];
                if (candaditeEntry.debutDif <= difficulty && (candaditeEntry.retireDif >= difficulty || candaditeEntry.retireDif == -1))
                {
                    for(int j = 0; j < candaditeEntry.spawnTickets; j++) result.Add(candaditeEntry.enemyObject);
                }
            }
            return result;
        }
    }




    // Useless for now, might need as reference for the future

    [SerializeField]
    public List<GameObject> FireList
    {
        get
        {
            //float difficulty = manager.Difficulty;
            List<GameObject> result = new List<GameObject>();
            for(int i = 0; i < fire_enemy_types.Length; i++) { result.Add(fire_enemy_types[i]); }
            return result;
        }
    }

    public GameObject MiniFireBoss{
        get
        {
            List<GameObject> valid_boses = new List<GameObject>();
            for(int i = 0; i < fire_mini_bosses.Length; i++){}

            if(valid_boses.Count > 0) {
                int choice = Random.Range(0, valid_boses.Count);
                return valid_boses[choice];
            }

            else {return null;}
        }
    }
}
