using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeControl : MonoBehaviour
{
    public Slider VolumeSlider, SFXSlider;
    public float music_value, sfx_value;

    [SerializeField]
    private PlayerHealth p_health;
    public GameManager gameManager;

    [SerializeField]
    private AudioMixerSnapshot[] snapshots;

    [SerializeField] private AudioMixer m_mixer;

    [SerializeField]
    private int active_snapshot = 0;

    [SerializeField]
    private float time = 1, threshold = 0.35f, normalized_health, c_health, m_health;

    public static VolumeControl main {get; private set;}




    // Start is called before the first frame update
    void Awake()
    {
        if(main == null){
            main = this;
        }
        else{
            Destroy(this);
        }
    }

    void Start()
    {
        music_value = PlayerPrefs.GetInt("Volume", 100);
        sfx_value = PlayerPrefs.GetInt("SFXVolume", 100);
        VolumeSlider.value = music_value;
        SFXSlider.value = sfx_value;
        p_health = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();

        m_mixer.TransitionToSnapshots(snapshots, new float[]{1f, 0f, 0f}, time);
        active_snapshot = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //gameManager.audio.volume = (float)VolumeSlider.value / 100;
        PlayerPrefs.SetInt("Volume", (int)VolumeSlider.value);
        PlayerPrefs.SetInt("SFXVolume", (int)SFXSlider.value);

        music_value = VolumeSlider.value;
        sfx_value = SFXSlider.value;

        if(p_health != null && active_snapshot != 2){

            c_health = p_health.CurrentHealth;
            m_health = p_health.GetMaxHealth();

            normalized_health = c_health / m_health;

            //if(active_snapshot )
            // Will implement snapshot code for resting action (pre round);

            if(normalized_health <= threshold && active_snapshot == 0){
                m_mixer.TransitionToSnapshots(snapshots, new float[]{0f, 1f, 0f}, time);
                active_snapshot = 1;
            }
            else if(normalized_health > threshold && active_snapshot == 1){
                m_mixer.TransitionToSnapshots(snapshots, new float[]{1f, 0f, 0f}, time);
                active_snapshot = 0;
            }

        }
    }

    public void SetSilentSnapshot(bool status, float timer){
        if(status){
            m_mixer.TransitionToSnapshots(snapshots, new float[]{0f, 0f, 1f}, timer);
            active_snapshot = 3;
        }
        else{
            active_snapshot = 0;
            m_mixer.TransitionToSnapshots(snapshots, new float[]{1f, 0f, 0f}, timer);
        }
    }
}
