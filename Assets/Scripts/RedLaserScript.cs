using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedLaserScript : MonoBehaviour
{
    public GameManager manager;
    public LvlStarter starter;
    public EnemyCounter e_counter;

    [SerializeField]
    private GameObject active_object;
    private Transform player;
    [SerializeField]
    private GameObject[] laser_prefabs;

    [SerializeField]
    private float timer, interval;

    [SerializeField]
    private Color[] colors;

    [SerializeField]
    AudioClip[] clips;

    void Start()
    {
        interval = timer;
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(interval <= 0)
        {
            if(manager.LvlCount >= 21 && e_counter.enemiesInScene > 0)
            {
                active_object = laser_prefabs[Random.Range(0, laser_prefabs.Length)];
                active_object.transform.position = player.position;
                StartCoroutine("LaserEvent");
            }

            interval = timer;
        }
        else
        {
            interval -= Time.deltaTime;
        }
        
    }

    IEnumerator LaserEvent()
    {
        float windup = 0, winddown = 2;
        AudioSource audio = active_object.GetComponent<AudioSource>();
        active_object.GetComponent<SpriteRenderer>().color = colors[0];

        audio.clip = clips[0];
        audio.Play();

        while (windup < 1){
            active_object.transform.localScale = new Vector3(windup * 5, 100, 1);
            windup += 0.5f * Time.deltaTime;
            yield return null;
        }

        audio.clip = clips[1];
        audio.Play();

        active_object.transform.localScale = new Vector3(6, 100, 1);
        active_object.GetComponent<Collider2D>().enabled = true;
        active_object.GetComponent<EnemyDamage>().enabled = true;
        active_object.GetComponent<SpriteRenderer>().color = colors[1];

        while (winddown > 0)
        {

            active_object.transform.localScale = new Vector3(winddown * 3, 100, 1);
            winddown -= 2 * Time.deltaTime;
            yield return null;
        }

        active_object.GetComponent<Collider2D>().enabled = false;
        active_object.GetComponent<EnemyDamage>().enabled = false;
        active_object.GetComponent<SpriteRenderer>().color = colors[2];
        //ResetEvent();
    }
}
