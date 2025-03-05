using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is responsible for
// Spawning enemies periodically into the scene
// Adjusting spawnrates and which enemies spawn given a difficulty level for the game
// Limits the enemies which can spawn into the scene at once based on the difficulty 

public class EscapeRoomSpawnSystem : MonoBehaviour
{
    // A list of positions of the spawns in the map
    public List<Transform> active_spawns;
    
    // The current list of enemy prefabs the spawner will spawn in the game
    [SerializeField]
    private List<GameObject> currentEnemyTable;
   
    // The lists of enemy prefabs the spawner should spawn at set difficulties 
    [SerializeField]
    private List<GameObject> introEnemeyTable, easyEnemyTable, normalEnemyTable, hardEnemyTable, extremeEnemyTable;
    
    // The time offset between periodic spawning and the max enemies allowed on the map
    [SerializeField]
    private float spawnRate, enemyCap;

    // The script for determining how many enemies are in the scene
    [SerializeField]
    private EnemyCounter enemyCounter;
    
    // The index of the current enemy list the spawner will pull from next
    [SerializeField]
    private int enemyIndex;

    // Called before the first frame of the game
    void Awake(){
        // Sets the initial settings for the spawner
        SetSpawnDifficulty(0);
        enemyIndex = 0;
    }

    // Adjusts the settings of the spawner given the difficulty of the game
    // Called by the main question manager when a main question is answered correctly
    public void SetSpawnDifficulty(int difficulty){
        switch (difficulty){
            // Each case sets the corresponding enemy table depending on the difficulty
            // To the current enemy table
            // Also sets the unique spawnrate and enemy cap for each difficulty
            case 0:
                // The easiest settings
                currentEnemyTable = introEnemeyTable;
                spawnRate = 3;
                enemyCap = 10;
                break;
            
            case 1:
                // After one question is solved
                currentEnemyTable = easyEnemyTable;
                spawnRate = 1.35f;
                enemyCap = 20;
                break;
            
            case 2:
                // After two questions are solved
                currentEnemyTable = normalEnemyTable;
                spawnRate = 0.8f;
                enemyCap = 30;
                break;
            
            case 3:
                // After three questions are solved
                currentEnemyTable = hardEnemyTable;
                spawnRate = 0.65f;
                enemyCap = 40;
                break;
            
            case 4:
                // After the final main question is solved
                spawnRate = 1.5f;
                currentEnemyTable = extremeEnemyTable;
                enemyCap = 30;
                break;
        }
    }

    // Starts the main spawning loop
    // Called by the game manager of the main game when the play button is pressed
    public void StartSpawnSequence()
    {
        StartCoroutine(spawning());
    }

    // The main spawning coroutine the script operates on
    // Using this as a coroutine allows accurate time offsets and skips frames when not needed
    // The only place using a while loop is viable in unity, since otherwise the usage  
    // Of while loops would freeze the game! 
    IEnumerator spawning()
    {
        while(true) {

            // Iterates through each active spawnpoint
            for (int i = 0; i < active_spawns.Count; i++)
            {
                
                if(enemyCounter.enemiesInScene < enemyCap)
                {
                    // Waits for the given spawnrate time and spawns an enemy
                    // at the position and rotation of the spawn used 
                    yield return new WaitForSeconds(spawnRate);
                    Instantiate(currentEnemyTable[enemyIndex], active_spawns[i].position, active_spawns[i].rotation);
                    // Updates the index of the list to use, or sets it to  
                    // Zero if the index goes beyond the variable table length 
                    enemyIndex++;
                    if(enemyIndex == currentEnemyTable.Count){enemyIndex = 0;}
                }

                // Pauses execution until the next runtime frame  
                // If the enemies are capped 
                yield return null;
            }
            
        }
    }
}