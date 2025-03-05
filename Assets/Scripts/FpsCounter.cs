using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FpsCounter : MonoBehaviour
{
    [SerializeField] public int Fps { get; private set; }
    [SerializeField] private float frequency;
    [SerializeField] private Text text;

    private void Start()
    {
        StartCoroutine(FramesPerSecondNumerator());
    }

    private IEnumerator FramesPerSecondNumerator()
    {
        while(true){
            int frameCountOffset = Time.frameCount;
            float timeOffset = Time.realtimeSinceStartup;
            yield return new WaitForSeconds(frequency);

            float timeSpan = Time.realtimeSinceStartup - timeOffset;
            int frameCount = Time.frameCount - frameCountOffset;

            Fps = Mathf.RoundToInt(frameCount / timeSpan);
            text.text = "FPS: " + Fps.ToString();
        }
    }
    
    // [SerializeField] private float fps, targetFps;

    // // Update is called once per frame
    // void Update()
    // {
    //         targetFps = Application.targetFrameRate;
    //         // fps = ((float)1 / Time.unscaledDeltaTime);
    //         fps = (float)Time.captureFramerate;

    //         Debug.Log(Time.);

    //         text.text = "FPS: " + ((int)fps).ToString();
    // }
}
