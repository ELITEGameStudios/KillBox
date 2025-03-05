using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnPlayer : MonoBehaviour
{
    public GameObject Player, PlayerCam, SliderObj;
    public Transform Spawnpoint, CamSpawnpoint, sliderSpawnPoint;

    public void SpawnPlayerEvent()
    {
        Instantiate(Player, Spawnpoint.position, Spawnpoint.rotation);
        Instantiate(PlayerCam, CamSpawnpoint.position, CamSpawnpoint.rotation);
    }
    public void SpawnHealthbarEvent()
    {
        Instantiate(SliderObj, sliderSpawnPoint.position, sliderSpawnPoint.rotation);
    }
}
