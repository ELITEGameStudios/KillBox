using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayLevel : MonoBehaviour
{
    private GameManager manager;

    [SerializeField]
    private Text text;
    public string pre_statement, post_statement;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("Manager").GetComponent<GameManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        text.text = pre_statement + " " + manager.LvlCount + " " + post_statement;
    }
}
