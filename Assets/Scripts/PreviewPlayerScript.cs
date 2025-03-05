using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PreviewPlayerScript : MonoBehaviour
{
    public VideoClip[] clips;
    [SerializeField]
    private VideoPlayer screen;

    public void ChangeClips(int index)
    {
        screen.clip = clips[index];
        screen.Play();
    }
}
