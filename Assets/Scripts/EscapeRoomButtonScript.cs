using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is attached to every trial button instance
// Responsible for starting the challenge of the main trial object 
// Upon the buttons interaction
public class EscapeRoomButtonScript : MonoBehaviour
{
    // Reference to the trial that this button represents
    // This variable is set automatically by the trial script itself
    public EscapeRoomMainTrialScript summoner;

    // Starts the trials challenge upon the user clicking this button
    public void ClickEvent()
    {
        summoner.StartChallenge();
    }
}
