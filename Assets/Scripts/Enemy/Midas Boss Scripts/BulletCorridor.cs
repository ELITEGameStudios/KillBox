using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCorridor : MonoBehaviour
{
    [SerializeField] private AIShooterScript[] leftShooter, rightShooter;
    [SerializeField] private int indexLeft, indexRight;
    [SerializeField] private SpriteRenderer upWall, downWall;
    [SerializeField] private ParticleSystem upParticles, downParticles;
    [SerializeField] private Color start, end;
    [SerializeField] private float timeElapsed, currentFireInterval, fireRate, colorRate, dieRate, otherTimeElapsed;
    private bool particlePlayer, atPosition;

    void Start(){
        transform.position = Player.main.tf.position;
        upWall.color = start; downWall.color = start;
        particlePlayer = true; atPosition = false;

        currentFireInterval = fireRate;
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        if (upWall.color != end && !atPosition){
            FadeIn();
        }
        else{
            if (particlePlayer){PlayParticles();}
            FiringUpdate();
        }
        if (timeElapsed >= dieRate){
            otherTimeElapsed += Time.deltaTime;
            FadeOut();
        }
    }
    void FadeIn(){
        upWall.color = Color.Lerp(start, end, timeElapsed * colorRate); downWall.color = Color.Lerp(start, end, timeElapsed * colorRate); 
        transform.position = Player.main.tf.position;
    }
    void FiringUpdate(){
        if (currentFireInterval <= 0)
        {
            indexLeft = Random.Range(0, 5); indexRight = Random.Range(0, 5); 
            for (int i =0 ; i < 5; i++){
                if (i != indexLeft){leftShooter[i].Shoot();}
                if (i != indexRight){rightShooter[i].Shoot();}
            }
            currentFireInterval = fireRate;
        }
        else{
            currentFireInterval -= Time.deltaTime;
        }
    }
    void PlayParticles(){
        atPosition = true;
        upParticles.Play(); downParticles.Play();
        particlePlayer = false;
    }
    void FadeOut(){
        upWall.color = Color.Lerp(end, start, otherTimeElapsed* 2); downWall.color = Color.Lerp(end, start, otherTimeElapsed* 2); 
        if (upWall.color == start){
            Destroy(gameObject);
        }
    }
}