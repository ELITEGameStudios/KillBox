using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotshotScript : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        //rb.AddForce(player.transform.up * 50);
        StartCoroutine("TimedDestruction");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator TimedDestruction()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }
}
