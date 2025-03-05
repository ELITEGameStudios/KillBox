using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TooltipGrabber : MonoBehaviour
{
    [SerializeField]
    private Text text;

    void Start()
    {
        // text.text = Tooltips.tips[Random.Range(0, Tooltips.tips.Length)];
        text.text = Tooltips.tips[0];
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
            text.text = Tooltips.tips[Random.Range(0, Tooltips.tips.Length)];
    }
}
