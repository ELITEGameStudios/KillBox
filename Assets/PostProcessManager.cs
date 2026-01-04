using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static EquipmentManager;

public class PostProcessManager : MonoBehaviour
{
    [Header("Quality")]
    [SerializeField] private Volume currentQualityVolume;
    [SerializeField] private Volume lowQualityVolume, highQualityVolume;

    [Header("Equipment")]
    [SerializeField] private Volume[] equipmentVolumes;

    [Header("Other")]
    [SerializeField] private Volume damageVolume;
    [SerializeField] private Volume bossVolume;
    [SerializeField] private Material wallMat;
    [SerializeField] private AnimationCurve wallEffectCurve;
    [SerializeField] public Volume DamageVolume {get { return damageVolume; }}



    public static PostProcessManager instance { get; private set; }

    void Awake()
    {
        if (instance == null) { instance = this; }
        else if(instance != this){ Destroy(this); }
    }

    void Update()
    {
        if(wallMat != null)
        {
            PlayerHealth health = Player.main.health;
            if(health != null)
            {
                wallMat.SetFloat("_effectSlider", 1f - (health.CurrentHealth / (float)( health.netMaxHealth ))); 
            }
            else
            {
                wallMat.SetFloat("_effectSlider", 0 ); 
            }
        }
    }
    
    public Volume GetEquipmentVolume(EquipmentType equipmentType)
    {
        return equipmentVolumes[(int)equipmentType];
    }

    public Volume GetBossVolume(EquipmentType equipmentType)
    {
        return bossVolume;
    }

    public void SetQuality(bool highQuality)
    {
        currentQualityVolume = highQuality ? highQualityVolume : lowQualityVolume;
        lowQualityVolume.enabled = !highQuality;
        highQualityVolume.enabled = highQuality;
        bossVolume.enabled = QualityControl.main.bossShaderIndex == 1;
        damageVolume.enabled = QualityControl.main.damageVolumeIndex == 1;
    }
    
}
