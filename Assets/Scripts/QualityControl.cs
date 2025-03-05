using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QualityControl : MonoBehaviour
{
    public GameObject[] PPR, Shadow;
    public GameObject LowVolume;
    public int ShadowIndex {get; private set;}
    public int hqVolumeIndex {get; private set;}
    public int bossShaderIndex {get; private set;}
    
    [SerializeField] private Toggle shadowToggle, hqVolumeToggle, dmgVolumeToggle, csVolumeToggle, bossShaderVolume;
    [SerializeField] private Slider particleSlider;

    public static QualityControl main {get; private set;}
    public Toggle BossShaderVolume { get => bossShaderVolume; }
    public Toggle ShadowToggle { get => shadowToggle; private set => shadowToggle = value; }
    public Toggle HqVolumeToggle { get => hqVolumeToggle; private set => hqVolumeToggle = value; }
    public Toggle DmgVolumeToggle { get => dmgVolumeToggle; private set => dmgVolumeToggle = value; }
    public Toggle CsVolumeToggle { get => csVolumeToggle; private set => csVolumeToggle = value; }

    // Start is called before the first frame update


    void Start()
    {
        if(main == null){
            main = this;
        }
        else if(main != this){
            Destroy(this);
        }
        ShadowIndex = PlayerPrefs.GetInt("Shadows", 1);
        hqVolumeIndex = PlayerPrefs.GetInt("quality_index", 1);
        bossShaderIndex = PlayerPrefs.GetInt("boss_shader_index", 1);
        
        if(ShadowIndex == 0) {ShadowToggle.isOn = false;}
        else {ShadowToggle.isOn = true;}
        
        if(hqVolumeIndex == 0) {HqVolumeToggle.isOn = false;}
        else {HqVolumeToggle.isOn = true;}

        if(bossShaderIndex == 0) {bossShaderVolume.isOn = false;}
        else {bossShaderVolume.isOn = true;}

        ToggleVolumes();

        if (ShadowIndex == 1)
        {
            for(int i = 0; i < Shadow.Length; i++)
            {
                Shadow[i].SetActive(true);
            }
        }
        else{
            for(int i = 0; i < Shadow.Length; i++)
            {
                Shadow[i].SetActive(false);
            }
        }
    }

    public void ChangeVolumeQuality(){
        if(HqVolumeToggle.isOn){hqVolumeIndex = 1;}
        else{hqVolumeIndex = 0;}
        PlayerPrefs.SetInt("quality_index", hqVolumeIndex);
        ToggleVolumes();
    }

    public void ChangeShadowQuality(){
        if(ShadowToggle.isOn){ShadowIndex = 1;}
        else{ShadowIndex = 0;}
        PlayerPrefs.SetInt("Shadows", ShadowIndex);
        Shadows(ShadowToggle.isOn);
    }

    public void ChangeBossShaderQuality(){
        if(bossShaderVolume.isOn){bossShaderIndex = 1;}
        else{bossShaderIndex = 0;}
        PlayerPrefs.SetInt("boss_shader_index", bossShaderIndex);
    }

    // Update is called once per frame
    public void ToggleVolumes()
    {
        for(int i = 0; i < PPR.Length; i++)
        {
            if (hqVolumeIndex == i)
                PPR[i].SetActive(true);
            else{
                PPR[i].SetActive(false);
            }

        }
    }

    public void Quality(int QIndex)
    {
        switch (QIndex){
            case 0:
                HqVolumeToggle.isOn = true;
                ShadowToggle.isOn = true;
                DmgVolumeToggle.isOn = true;
                CsVolumeToggle.isOn = true;
                bossShaderVolume.isOn = true;
                particleSlider.value = 60;
                ChangeShadowQuality();
                ChangeVolumeQuality();
                ChangeBossShaderQuality();
                break;

            case 1:
                HqVolumeToggle.isOn = true;
                ShadowToggle.isOn = false;
                DmgVolumeToggle.isOn = true;
                bossShaderVolume.isOn = true;
                CsVolumeToggle.isOn = true;
                particleSlider.value = 30;
                ChangeShadowQuality();
                ChangeVolumeQuality();
                ChangeBossShaderQuality();
                break;
                
            case 2:
                HqVolumeToggle.isOn = false;
                ShadowToggle.isOn = false;
                bossShaderVolume.isOn = false;
                DmgVolumeToggle.isOn = true;
                CsVolumeToggle.isOn = false;
                particleSlider.value = 15;
                ChangeShadowQuality();
                ChangeVolumeQuality();
                ChangeBossShaderQuality();
                break;
        }

        PlayerPrefs.Save();
        // Index = QIndex;
        // PPRBool();
    }

    public void Shadows(bool inputBool)
    {
        for(int i = 0; i < Shadow.Length; i++)
        {
            Shadow[i].SetActive(inputBool);
            Debug.Log("SHADOWOWOWOWOWOWOWOWOWOWOWOWOWOWOWO");
        }

        if(inputBool){
            PlayerPrefs.SetInt("Shadows", 1);
        }
        else{
            PlayerPrefs.SetInt("Shadows", 0);
        }
        
        PlayerPrefs.Save();
    }
}
