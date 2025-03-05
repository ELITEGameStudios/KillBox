using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicideScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        GameObject[] tokensInScene = GameObject.FindGameObjectsWithTag("DroppedToken");

        if(GameManager.main.EscapeRoom() || BossRoundManager.main.isBossRound || GameManager.main.tokensThisRound + tokensInScene.Length >= GameManager.main.maxTokensPerRound ){
            Destroy(gameObject);
        }
    }
}
