using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ToggleChannelManager : MonoBehaviour
{
    [SerializeField] private int channelCount;
    [SerializeField] private int[] specialChannelIndexes;
    private static bool[] channels;
    // Start is called before the first frame update
    public static ToggleChannelManager main;

    void Awake()
    {
        if(main == null){ main = this; }
        else if(main != this){ Destroy(this); }
        
        channels = new bool[channelCount];

    }

    public void SetChannel(int channel, bool state){
        if(channel >= channelCount){return;}
        else{
            channels[channel] = state;
        }
    }

    public void ToggleChannel(int channel){
        if(channel >= channelCount){return;}
        else{
            channels[channel] = !channels[channel];
        }
    }

    public void ResetChannels(bool withSpecials = false){
        for (int i = 0; i < channels.Length; i++)
        {
            if(specialChannelIndexes.Contains(i) && !withSpecials){
                continue;
            }
            channels[i] = false;
        }
    }

    public bool GetChannel(int channel){
        if(channel >= channelCount){return false;}
        else{
            return channels[channel];
        }
    }
}
