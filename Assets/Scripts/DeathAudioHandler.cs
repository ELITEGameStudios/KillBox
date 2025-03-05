using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAudioHandler : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    // Start is called before the first frame update
    void Awake()
    {
        audioSource.pitch = Random.Range(0.8f, 1.2f);
    }
    void OnEnable(){
        audioSource.pitch = Random.Range(0.8f, 1.2f);
    }
    
    void Start(){
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
