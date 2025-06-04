using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestScript : MonoBehaviour
{
    [SerializeField]
    private GameObject child_button_obj;
    private Button child_button;

    [SerializeField] private PortalScript portal;

    [SerializeField]
    private GameObject player, vfx_prefab, button_prefab;

    public GameObject[] rewards_list;

    private bool player_is_nearby = false;
    private float dist;

    private GameManager manager;

    public int cost;

    public float[] size;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        manager = GameObject.Find("Manager").GetComponent<GameManager>();

        portal = Resources.FindObjectsOfTypeAll<PortalScript>()[0];

        child_button_obj = Instantiate(button_prefab, transform);

        child_button_obj.SetActive(false);
        child_button_obj.transform.SetParent(GameObject.Find("WorldUiCanvas").transform);
        child_button_obj.transform.Translate(0, 1, 0);
        child_button_obj.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        child_button = child_button_obj.GetComponent<Button>();
        child_button.interactable = false;
        child_button_obj.GetComponent<ChestButtonScript>().chest = this;
        child_button_obj.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Open Chest\n-" + cost.ToString() + " tokens";
    }

    void Update()
    {
        dist = Vector3.Distance(player.transform.position, gameObject.transform.position);
        if (dist < 2 && !player_is_nearby)
        {
            player_is_nearby = true;
            OnPlayerApproach();
        }

        else if (dist > 2 && player_is_nearby)
        {
            player_is_nearby = false;
            OnPlayerLeave();
        }

        else if (portal.loadingScene && child_button_obj.activeInHierarchy)
        {
            OnPlayerLeave();
        }

    }

    void OnPlayerApproach()
    {
        if (manager.ScoreCount >= cost)
        {
            child_button.interactable = true;
        }
        child_button_obj.SetActive(true);
    }

    void OnPlayerLeave()
    {
        child_button.interactable = false;
        child_button_obj.SetActive(false);
    }

    public void GrantChest()
    {
        vfx_prefab.SetActive(true);
        vfx_prefab.transform.SetParent(null);
        vfx_prefab.transform.localScale = new Vector3 (1, 1, 1);

        int index = Random.Range(0, rewards_list.Length);
        GameObject reward = Instantiate(rewards_list[index], vfx_prefab.transform);
        
        reward.transform.SetParent(null);

        Transform grid = GameObject.Find("Grid").transform;

        for (int i = 0; i < grid.childCount; i++)
        {
            if (grid.GetChild(i).gameObject.activeInHierarchy)
            {
                reward.transform.SetParent(grid.GetChild(i));
                reward.transform.localScale = new Vector3(0.2f, 0.2f, 1f);
                break;
            }
        }



        Destroy(child_button_obj);
        Destroy(gameObject);
    }
}
