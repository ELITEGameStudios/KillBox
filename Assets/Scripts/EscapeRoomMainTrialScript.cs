using UnityEngine;
using UnityEngine.UI;

// Class attached to every physical trial object int the scene
// Responsible for the behaviour of the trial objects
// The players access and interaction of trial objects
// Allows player to initiate questions if the player is permitted by the initiator
public class EscapeRoomMainTrialScript : MonoBehaviour
{
    // The gameobject of the child button for this trial
    private GameObject child_button_obj;

    // The button UI component (script) of the child button
    private Button child_button;

    // The gameobject of the player + the object template to clone into the child button gameobject 
    [SerializeField]
    private GameObject player, button_prefab;

    // Keeps track of if the child button is enabled for the player 
    private bool buttonIsEnabled = false;
    
    // Does this trial object represent bonus trials? 
    [SerializeField]
    private bool bonusTrial, isHint;

    // distance from the player to this gameobject
    private float playerDistance;
    
    // Reference to the main initiator script for questions 
    [SerializeField]
    private QuestionInitiator initiator;

    [SerializeField]
    private int questionIndex;

    void Start()
    {
        // Gets the player gameobject
        player = Player.main.obj;

        /* Setting up the button that appears when the player gets close to the challenge
        It must be set via script instead of the editor due to the limitations of Unity's
        World UI Canvas system. */

        // Creates a new preset button into the scene as a child of this object
        child_button_obj = Instantiate(button_prefab, transform);
        // Disbles the button
        child_button_obj.SetActive(false);

        // Transforms the child to its appropriate position, and parents to the UI Canvas
        child_button_obj.transform.SetParent(GameObject.Find("WorldUiCanvas").transform);
        child_button_obj.transform.Translate(0, 1, 0);
        child_button_obj.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

        // Gets the button component of the object and temporarily disables its function
        child_button = child_button_obj.GetComponent<Button>();
        child_button.interactable = false;

        // Gets a script from the object and references a reqired variable to this script
        child_button_obj.GetComponent<EscapeRoomButtonScript>().summoner = this;
        
        // Sets the text of the button
        if(bonusTrial) {child_button_obj.transform.GetChild(0).gameObject.GetComponent<Text>().text = "INITIATE A\nBONUS QUESTION";}
        else if(!isHint){ child_button_obj.transform.GetChild(0).gameObject.GetComponent<Text>().text = "INITIATE A\nMAIN QUESTION"; }
        else { child_button_obj.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Need a HINT?"; }
    }

    // Enables and disables the trial's button depending on
    // The players distance and if a trial can be initiated
    // Called every runtime frame
    void Update()
    {
        // Calculates the distance between the player and this gameobject
        playerDistance = Vector3.Distance(player.transform.position, gameObject.transform.position);
        
        // Enables the button if the player is close enough and a question can be asked
        if (playerDistance < 2 && !buttonIsEnabled && initiator.canInitiate)
        { OnPlayerApproach(); }

        // Disables the button if the player is too far and/or a question cannot be asked
        else if (playerDistance > 2 && buttonIsEnabled || !initiator.canInitiate)
        { OnPlayerLeave(); }

    }

    // Enables the trial's UI button 
    // Called when the player leaves the range of the trial 
    void OnPlayerApproach()
    {
        child_button.interactable = true;
        child_button_obj.SetActive(true);
        buttonIsEnabled = true;
    }

    // Disables the trial's UI button 
    // Called when the player leaves the range of the trial
    // Or if the user can't initiate a question, this blocks access to questions
    void OnPlayerLeave()
    {
        child_button.interactable = false;
        child_button_obj.SetActive(false);
        buttonIsEnabled = false;
    }


    // Destroy the trial object and button if it represents a main question
    // Called when it's initiated question was answered correctly
    public void EndSequence(){
        if(!bonusTrial) {
            Destroy(child_button_obj);
            Destroy(gameObject);
        }
    }

    // Initiates a new question for the player to solve
    // Question is determined through the questionIndex provided by this object 
    // Or if it is a bonus question, the specific index is handled by the initiator itself 
    public void StartChallenge()
    {
        if(!isHint){
            initiator.InitiateQuestion(questionIndex, bonusTrial, this);
        }
        else{
            initiator.InitiateHint();
        }
    }

}
