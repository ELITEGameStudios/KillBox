using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossTrialScript : MonoBehaviour
{
    [SerializeField]
    private GameObject child_button_obj;
    private Button child_button;

    [SerializeField]
    private GameObject player, button_prefab, cutter_boss, active_boss_obj, triad_boss;
    


    private bool player_is_nearby = false;
    private float dist;

    private GameManager manager;

    public int boss_index;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        manager = GameObject.Find("Manager").GetComponent<GameManager>();

        child_button_obj = Instantiate(button_prefab, transform);



        child_button_obj.SetActive(false);
        child_button_obj.transform.SetParent(GameObject.Find("WorldUiCanvas").transform);
        child_button_obj.transform.Translate(0, 1, 0);
        child_button_obj.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        child_button = child_button_obj.GetComponent<Button>();
        child_button.interactable = false;
        child_button_obj.GetComponent<BossTrialButtonScript>().summoner = this;
        child_button_obj.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Prove Your Worth";
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
    }

    void OnPlayerApproach()
    {

        child_button.interactable = true;
        child_button_obj.SetActive(true);
    }

    void OnPlayerLeave()
    {
        child_button.interactable = false;
        child_button_obj.SetActive(false);
    }

    public void SummonBoss()
    {

        StartCoroutine(BossSummon());

    }

    IEnumerator BossSummon()
    {
        gameObject.GetComponent<ParticleSystem>().Play();

        yield return new WaitForSeconds(2);

        if (boss_index == 0)
        {
            active_boss_obj = Instantiate(cutter_boss, transform.position, transform.rotation);
            active_boss_obj.transform.SetParent(null);
        }
        else if (boss_index == 1)
        {
            active_boss_obj = Instantiate(triad_boss, transform.position, transform.rotation);
            active_boss_obj.transform.SetParent(null);
        }

        Destroy(gameObject);


    }
}
