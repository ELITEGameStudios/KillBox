using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOutOfMapGlitch : MonoBehaviour
{

    [SerializeField]
    private PortalScript portal;

    [SerializeField]
    private GameObject error_ui;

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Destructive")
        {
            transform.position = GameManager.main.GetMapByID(portal.currentMapIndex).Player.position;
            StartCoroutine("Event");
        }
    }

    IEnumerator Event()
    {
        error_ui.SetActive(true);

        yield return new WaitForSecondsRealtime(4);

        error_ui.SetActive(false);
    }
}
