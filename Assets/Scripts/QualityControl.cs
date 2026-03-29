using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QualityControl : MonoBehaviour
{

    // Main Effects
    public bool MainPostProcessing {get {return PlayerPrefs.GetInt("quality_index", 1) == 1;}  set{PlayerPrefs.SetInt("quality_index", value ? 1 : 0); PlayerPrefs.Save(); hqVolumeToggle.isOn = value;}}
    public bool BossPostProcessing {get {return PlayerPrefs.GetInt("boss_shader_index", 1) == 1;}  set{PlayerPrefs.SetInt("boss_shader_index", value ? 1 : 0); PlayerPrefs.Save();}}
    public bool DamagePostProcessing {get {return PlayerPrefs.GetInt("dmg_volume", 1) == 1;}  set{PlayerPrefs.SetInt("dmg_volume", value ? 1 : 0); PlayerPrefs.Save(); dmgVolumeToggle.isOn = value;}}
    public bool RenderShadows {get {return PlayerPrefs.GetInt("Shadows", 1) == 1;}  set{PlayerPrefs.SetInt("Shadows", value ? 1 : 0); PlayerPrefs.Save(); shadowToggle.isOn = value;}}
    public bool CameraShake {get {return PlayerPrefs.GetInt("camera_shake", 1) == 1;}  set{PlayerPrefs.SetInt("camera_shake", value ? 1 : 0); PlayerPrefs.Save(); csVolumeToggle.isOn = value;}}
    
    // Render Texture Effects
    public bool PulseEffectShader {get { return PlayerPrefs.GetInt("pulse_effect", 1) == 1;} set{PlayerPrefs.SetInt("pulse_effect", value ? 1 : 0); PlayerPrefs.Save(); pulseEffectToggle.isOn = value;}} 
    public bool HealthEffectShader {get { return PlayerPrefs.GetInt("health_effect", 1) == 1;} set{PlayerPrefs.SetInt("health_effect", value ? 1 : 0); PlayerPrefs.Save(); healthEffectToggle.isOn = value;}} 
    public bool GridEffectShader {get { return PlayerPrefs.GetInt("grid_effect", 1) == 1;} set{PlayerPrefs.SetInt("grid_effect", value ? 1 : 0); PlayerPrefs.Save(); gridEffectToggle.isOn = value;}} 
    public bool NeedsRenderTextues => PulseEffectShader || HealthEffectShader;
    
    // UI Elements
    [SerializeField] private Toggle shadowToggle, hqVolumeToggle, dmgVolumeToggle, csVolumeToggle, bossShaderVolume;
    [SerializeField] private Toggle pulseEffectToggle, healthEffectToggle, gridEffectToggle;
    [SerializeField] private Slider particleSlider;

    public static QualityControl main {get; private set;}


    void Awake()
    {
        if(main == null){ main = this; }
        else if(main != this){ Destroy(this); }
    }
    void Start()
    {
        hqVolumeToggle.isOn = MainPostProcessing;
        bossShaderVolume.isOn = BossPostProcessing;
        csVolumeToggle.isOn = CameraShake;
        shadowToggle.isOn = RenderShadows;
        dmgVolumeToggle.isOn = DamagePostProcessing;

        pulseEffectToggle.isOn = PulseEffectShader;
        gridEffectToggle.isOn = GridEffectShader;
        healthEffectToggle.isOn = HealthEffectShader;

        ToggleVolumes();
        ChangeShadowQuality(RenderShadows);
    }



    // Screen space effects
    public void TogglePulseEffect(bool inputBool){
        PulseEffectShader = inputBool;
        SetRenderTextureSettings();
    }

    public void ToggleHealthEffect(bool inputBool){
        HealthEffectShader = inputBool;
        SetRenderTextureSettings();
    }

    public void ToggleGridEffect(bool inputBool){
        GridEffectShader = inputBool;
        SetRenderTextureSettings();
    }
    public void SetRenderTextureSettings()
    {
        // if (NeedsRenderTextues)
        // {
            
        // }
        // else
        // {
            
        // } 
    }

    // Main Effects
    public void ToggleCameraShake(bool inputBool){
        CameraShake = inputBool;
    }

    public void ChangeVolumeQuality(bool inputBool) {
        MainPostProcessing = inputBool;
        ToggleVolumes();
    }
    public void ChangeBossShaderQuality(bool inputBool)
    {
        BossPostProcessing = inputBool;
        ToggleVolumes();
    }
    public void ChangeShadowQuality(bool inputBool)
    {
        RenderShadows = inputBool;
        if(GameManager.main != null) GameManager.main.GetCurrentMap().UpdateShadows();
    }
    public void ToggleDmgVolume(bool inputBool)
    {
        DamagePostProcessing = inputBool;
        ToggleVolumes();
    }


    public void ToggleVolumes()
    {
        PostProcessManager.instance.SetQuality();
    }



    public void Quality(int QIndex)
    {
        switch (QIndex)
        {
            // High
            case 0:
                particleSlider.value = 60;

                ChangeVolumeQuality(true);
                ChangeShadowQuality(true);
                ToggleDmgVolume(true);
                ToggleCameraShake(true);
                ChangeBossShaderQuality(true);

                ToggleGridEffect(true);
                ToggleHealthEffect(true);
                TogglePulseEffect(true);
                break;

            // Medium
            case 1:
                particleSlider.value = 30;

                ChangeVolumeQuality(true);
                ToggleDmgVolume(true);
                ToggleCameraShake(true);
                ChangeShadowQuality(true);
                ChangeBossShaderQuality(true);

                ToggleGridEffect(true);
                ToggleHealthEffect(false);
                TogglePulseEffect(false);
                break;

            // Low
            case 2:
                particleSlider.value = 15;

                ToggleDmgVolume(false);
                ToggleCameraShake(true);
                ChangeShadowQuality(false);
                ChangeBossShaderQuality(false);
                ChangeVolumeQuality(true);
                
                ToggleGridEffect(true);
                ToggleHealthEffect(false);
                TogglePulseEffect(false);
                break;
        }

        // Index = QIndex;
        // PPRBool();
    }

}
