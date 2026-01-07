using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCluster : MonoBehaviour
{
    [SerializeField]
    private AIShooterScript[] shootSources;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private float distance, angle, colorChange, timeElapsed, xPos, yPos;
    [SerializeField]
    private Color startColor, endColor;
    [SerializeField]
    private SpriteRenderer starSprite;
    void OnEnable(){
        starSprite.color = startColor;
        timeElapsed = 0;
        angle = Random.Range(0f, 6.28f);
        xPos = Player.main.tf.position.x + distance*Mathf.Cos(angle);
        yPos = Player.main.tf.position.y + distance*Mathf.Sin(angle);
    }
    void Update(){
        transform.position = new Vector2(xPos, yPos);
        timeElapsed = timeElapsed + Time.deltaTime;
        starSprite.color = Color.Lerp(startColor, endColor, timeElapsed * colorChange);
        if (starSprite.color == endColor){
            Fire();
        }
    }
    void Fire(){
        foreach (AIShooterScript source in shootSources)
        {
            source.Shoot();
        }
        bullet.SetActive(false);
    }
}