using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QualityControl : MonoBehaviour
{
    public int ShadowIndex {get; private set;}
    public int hqVolumeIndex {get; private set;}
    public int bossShaderIndex {get; private set;}
    public int csVolumeIndex {get; private set;}
    public int damageVolumeIndex {get; private set;}
    
    [SerializeField] private Toggle shadowToggle, hqVolumeToggle, dmgVolumeToggle, csVolumeToggle, bossShaderVolume;
    [SerializeField] private Slider particleSlider;

    public static QualityControl main {get; private set;}
    public Toggle BossShaderVolume { get => bossShaderVolume; }
    public Toggle ShadowToggle { get => shadowToggle; private set => shadowToggle = value; }
    public Toggle HqVolumeToggle { get => hqVolumeToggle; private set => hqVolumeToggle = value; }
    public Toggle DmgVolumeToggle { get => dmgVolumeToggle; private set => dmgVolumeToggle = value; }
    public Toggle CsVolumeToggle { get => csVolumeToggle; private set => csVolumeToggle = value; }

    // Start is called before the first frame update


    void Awake()
    {
        if(main == null){ main = this; }
        else if(main != this){ Destroy(this); }
    }
    void Start()
    {
        damageVolumeIndex = PlayerPrefs.GetInt("dmg_volume", 1);
        ShadowIndex = PlayerPrefs.GetInt("Shadows", 1);
        hqVolumeIndex = PlayerPrefs.GetInt("quality_index", 1);
        bossShaderIndex = PlayerPrefs.GetInt("boss_shader_index", 1);
        csVolumeIndex = PlayerPrefs.GetInt("camera_shake", 1);

        hqVolumeToggle.isOn = hqVolumeIndex == 1;
        bossShaderVolume.isOn = bossShaderIndex == 1;
        csVolumeToggle.isOn = csVolumeIndex == 1;

        ToggleVolumes();
        Shadows(ShadowIndex == 1);
    }

    public void ToggleCameraShake(bool inputBool){
        csVolumeIndex = inputBool ? 1 : 0;
        PlayerPrefs.SetInt("camera_shake", csVolumeIndex);
        PlayerPrefs.Save();
    }

    public void ChangeVolumeQuality(bool inputBool) {
        hqVolumeIndex = HqVolumeToggle.isOn ? 1 : 0;
        PlayerPrefs.SetInt("quality_index", hqVolumeIndex);
        ToggleVolumes();
    }

    public void ChangeShadowQuality(){
        Shadows(ShadowToggle.isOn);
    }

    public void ChangeBossShaderQuality()
    {
        bossShaderIndex = bossShaderVolume.isOn ? 1 : 0;
        PlayerPrefs.SetInt("boss_shader_index", bossShaderIndex);
        ToggleVolumes();
    }

    // Update is called once per frame
    public void ToggleVolumes()
    {
        PostProcessManager.instance.SetQuality(hqVolumeIndex == 1);
    }

    public void ToggleDmgVolume(bool inputBool)
    {
        damageVolumeIndex = inputBool ? 1 : 0;
        PlayerPrefs.SetInt("dmg_volume", damageVolumeIndex);
        PlayerPrefs.Save();
        ToggleVolumes();
    }
    public void Quality(int QIndex)
    {
        switch (QIndex)
        {
            case 0:
                HqVolumeToggle.isOn = true;
                ChangeVolumeQuality(true);

                ShadowToggle.isOn = true;
                DmgVolumeToggle.isOn = true;
                CsVolumeToggle.isOn = true;
                bossShaderVolume.isOn = true;
                particleSlider.value = 60;
                ChangeShadowQuality();
                ChangeBossShaderQuality();
                break;

            case 1:
                HqVolumeToggle.isOn = true;
                ChangeVolumeQuality(true);

                ShadowToggle.isOn = false;
                DmgVolumeToggle.isOn = true;
                bossShaderVolume.isOn = true;
                CsVolumeToggle.isOn = true;
                particleSlider.value = 30;
                ChangeShadowQuality();
                ChangeBossShaderQuality();
                break;

            case 2:
                HqVolumeToggle.isOn = false;
                ChangeVolumeQuality(false);

                ShadowToggle.isOn = false;
                bossShaderVolume.isOn = false;
                DmgVolumeToggle.isOn = true;
                CsVolumeToggle.isOn = false;
                particleSlider.value = 15;
                ChangeShadowQuality();
                ChangeBossShaderQuality();
                break;
        }

        PlayerPrefs.Save();
        // Index = QIndex;
        // PPRBool();
    }

    public void Shadows(bool inputBool)
    {
        ShadowIndex = inputBool ? 1 : 0;
        PlayerPrefs.SetInt("Shadows", ShadowIndex);
        PlayerPrefs.Save();
        GameManager.main.GetCurrentMap().UpdateShadows();
    }
}
