using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetScoreText : MonoBehaviour
{

    [SerializeField] private Text text;

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        text.text = GameManager.main.ScoreCount.ToString();
        
    }
}
