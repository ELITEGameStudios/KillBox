using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class KeyUnlock : MonoBehaviour
{
    private Transform player;
    private GameManager manager;

    [SerializeField]
    private GameObject key_outline;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        manager = GameObject.Find("Manager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (Vector3.Distance(key_outline.transform.position, player.position) < 2 && manager.HasKey)
        {
            manager.HasKey = false;
            StartCoroutine("Unlock");
        }
    }

    IEnumerator Unlock()
    {
        SpriteRenderer renderer = key_outline.GetComponent<SpriteRenderer>();
        Tilemap tilemapRenderer = gameObject.GetComponent<Tilemap>();

        float value = 1;
        Color color = renderer.color;

        while(value > 0)
        {
            value -= Time.deltaTime;
            color.a = value;

            renderer.color = color;
            yield return null;
        }

        value = 1;
        color = tilemapRenderer.color;
        while (value > 0)
        {
            value -= Time.deltaTime;
            color.a = value;

            tilemapRenderer.color = color;

            yield return null;
        }

        gameObject.SetActive(false);
    }
}
