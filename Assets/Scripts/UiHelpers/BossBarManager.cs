using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class BossBarManager : MonoBehaviour
{

    [SerializeField] private GameObject boss_bar;
    [SerializeField] private Text name_txt, health_txt;
    [SerializeField] private Slider slider;
    [SerializeField] private List<BossDisplayObj> bossList;
    [SerializeField] private Volume boss_volume;
    [SerializeField] private float volume_weight;
    [SerializeField] private Graphic sliderFillGraphic;
    [SerializeField] private Image bossImage, glow, glow2;

    public static BossBarManager Instance {get; private set;}
    void Awake()
    {
        bossList = new List<BossDisplayObj>();

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Update is called once per frame
    public void AddToQueue(GameObject boss_input, string name, Color color, Sprite sprite){

        BossDisplayObj new_object = new BossDisplayObj(boss_input, name, color, sprite);
        // MainAudioSystem.main.TriggerBossMusic(name);

        bossList.Add(new_object);

    }

    void ManageVolume(){
        // print("LLLL");
        // if(QualityControl.main.BossShaderVolume.isOn){
        // if(QualityControl.main.BossShaderVolume.isOn){

            // if(bossList.Count > 0 && volume_weight < 1 || PortalScript.main.Mode == 3 && volume_weight < 1 && PortalScript.main.gameObject.activeInHierarchy){ volume_weight += 0.25f * Time.deltaTime; }
            // else if (bossList.Count == 0 && volume_weight > 0){ volume_weight -= 0.25f * Time.deltaTime; } 
            // else if (bossList.Count > 0){ volume_weight = 1; }
            // else{ volume_weight = 0; }

            // boss_volume.weight = volume_weight;
        // }
        // else{
        //     boss_volume.weight = 0;
        // }
    }

    void Update(){

        for(int i = 0; i < bossList.Count; i++){
            if(bossList[i]._object == null || bossList[i].health.CurrentHealth <= 0){
                bossList.RemoveAt(i);
            }
        }

        ManageVolume();

        if (bossList.Count > 0){
            BossDisplayObj displayObj = bossList[0];
            foreach (BossDisplayObj item in bossList){
                if (
                    Vector2.Distance(item._object.transform.position, Player.main.tf.position) <
                    Vector2.Distance(displayObj._object.transform.position, Player.main.tf.position)
                )
                {
                    displayObj = item;
                }
            }

            if (!boss_bar.activeInHierarchy)
            {
                boss_bar.SetActive(true);
            }

            // Setting information
            health_txt.text =   displayObj.health.CurrentHealth.ToString() + " | " + bossList[0].health.maxHealth.ToString();
            name_txt.text = displayObj._name;
            slider.maxValue =   displayObj.health.maxHealth;
            slider.value =   displayObj.health.CurrentHealth;

            // Setting colors
            sliderFillGraphic.color =   displayObj.color;
            bossImage.color =   displayObj.color;
            bossImage.sprite =   displayObj.sprite;
            name_txt.color =   displayObj.color;
            glow.color =   Color.Lerp(Color.clear, displayObj.color, 0.57f);
            glow2.color =   Color.Lerp(Color.clear, displayObj.color, 0.57f);
        }
        else{
            if(boss_bar.activeInHierarchy){
                boss_bar.SetActive(false);
            }

            //if(BossAudio.Instance.Check){
            //    BossAudio.Instance.Check = false;
            //}
            // if(MainAudioSystem.main.exterior_track){
            //     MainAudioSystem.main.PlayMainLoop();
            // }
        }
    }
}

public class BossDisplayObj{

    public GameObject _object {get; private set;}
    public Sprite sprite {get; private set;}
    public Color color {get; private set;}
    public string _name {get; private set;}

    public EnemyHealth health {get; private set;}
    public BossDisplayObj(GameObject obj, string name, Color color, Sprite sprite){
        _object = obj;
        _name = name;
        this.color = color;
        this.sprite = sprite;
        health = obj.GetComponent<EnemyHealth>();
    }
}