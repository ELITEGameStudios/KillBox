using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonFunctions
{
    public static bool reduceFlashes {get; private set;}
    // Start is called before the first frame update
    public static float WarningIndicator(float x, float flashesPerSecond = 1)
    {
        if(!reduceFlashes){
            return 0.5f * Mathf.Sin(2*Mathf.PI * (flashesPerSecond * x - 0.25f)) + 0.5f;
        }
        else{
            return -Mathf.Pow(0.007f, x)+1;
        }
    }

    public static float SineEase(float x, float peak = 1, float startOffset = 0.5f)
    {
        return 0.5f * Mathf.Sin(Mathf.PI* (x / peak  - startOffset)) + 0.5f;
    }

    public static float SineNormalize(float x, float period = 1)
    {
        return 0.5f * Mathf.Sin(2*Mathf.PI * ((x / period) - 0.25f)) + 0.5f;
    }

    public static float CosineEase(float x, float peak = 1, float startOffset = 0.5f)
    {
        return 0.5f * Mathf.Cos(Mathf.PI* (x / peak  - startOffset)) + 0.5f;
    }

    public static List<T> RandomizeList<T>(List<T> values)
    {
        List<T> oldPool = values;
        List<T> newPool = values;
        for (int i = oldPool.Count-1; i > 0; i--){
            int random = Random.Range(0, oldPool.Count);
            newPool.Add(oldPool[random]);
            oldPool.RemoveAt(random);
        }
        return newPool;
    }
}
