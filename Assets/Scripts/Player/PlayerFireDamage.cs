using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFireDamage : MonoBehaviour
{
    [SerializeField]
    private bool _on_fire;
    
    [SerializeField]
    private ParticleSystem _fire_particles;
    
    [SerializeField]
    private PlayerHealth _hp_script;

    [SerializeField]
    private float _timer = 0, deltatime, fixedtime;

    [SerializeField]
    private int _intensity = 50;

    // Update is called once per frame
    void FixedUpdate()
    {
        if(_timer >= 0 && _on_fire){
            _timer -= Time.fixedDeltaTime;
            _hp_script.TakeDmg(Mathf.RoundToInt(_intensity * Time.fixedDeltaTime), 0.5f);
        }   
        else if(_on_fire && _timer <= 0){
            _on_fire = false;
            _fire_particles.loop = false;
        }
        deltatime = Time.deltaTime;
        fixedtime = Time.fixedDeltaTime;
    }

    public void Ignite(float seconds, int intensity){
        _on_fire = true;
        _timer = seconds;
        _intensity = intensity;

        _fire_particles.loop = true;
        _fire_particles.Stop();
        _fire_particles.Play();
        

    }
}
