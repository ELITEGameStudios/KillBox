using UnityEngine;
using UnityEngine.UI;

// This class is responsible for initiating new question events in the game
// Handling the UI in the event of questions and mid game displays
// If and when the player can initiate a new question at a given time

public class QuestionInitiator : MonoBehaviour
{

    // The gameobjects for the main display panels of the question UI
    [SerializeField] 
    private GameObject mainPanel, correctDisplay, incorrectDisplay, portalObject, endGameScreen, hintPanel;
    
    // Represents the index of the current question and the current hint index to display
    private int currentQuestion = -1, currentHint; 
    
    // Controls which bonus question appears on the bonus trial
    public int bonusQuestionIndex {get; private set;}

    [SerializeField]
    private float displayTimer, displayTimeElapsed;
    
    // The timer for when the user can initiate another question. 
    // This is accessible to other scripts but can only be set by this one
    public float initiationTimerElapsed {get; private set;}

    // The text fields for question and and answers
    [SerializeField]
    private Text questionDisplay, hintDisplay;
    [SerializeField]
    private Text[] answerDisplays;
    
    // The button objects for the multiple choice answer fields
    [SerializeField]
    private Button[] answerButtons;
    
    // General status bools determining the state of a question the player is in
    private bool active = false, finishedQuestion, isBonus;

    // Allows or prevents the user to start a new question
    public bool canInitiate {get; private set;}

    // The text field for displaying the time until a new question can be initiated
    [SerializeField]
    private Text timerDisplay;


    // The reference to the main question manager
    [SerializeField]
    private MainQuestionManager manager;

    // The reference to the last trial script who called this script
    private EscapeRoomMainTrialScript currentTrialScript;

    // Start is called before the first frame update
    void Awake()
    {
        // Initially disables the UI to ensure unwanted UI is not open when the game starts
        mainPanel.SetActive(false);

        // Initially sets these indexes to a random value to incorporate some
        // Form of randomness in an otherwise predictable pattern 
        // For the use of these variables when they are modified later in the script
        currentHint = Random.Range(0, 7);
        bonusQuestionIndex = Random.Range(0, 4);
    }

    // Initiates a new question for the player to solve
    // Operates given the target question index, if this is a bonus question,
    // And the trial script that referenced this method 
    public void InitiateQuestion(int targetQuestion, bool bonus, EscapeRoomMainTrialScript trialScript){
        isBonus = bonus;
        currentTrialScript = trialScript;

        if(!isBonus){
            // Setting the current question index to retrieve data with
            if(currentQuestion == -1) { currentQuestion = targetQuestion;}

            // Setting text to appropriate question and answer
            questionDisplay.text = manager.mainQuestions[currentQuestion];
            for (int i = 0; i < answerDisplays.Length; i++){
                answerDisplays[i].text = manager.mainAnswers[currentQuestion][i];
            }
        }

        else{

            // Setting the bonus question index to retrieve data with
            // Increments the index by one each time. if the index is greater than
            // The length of bonus questions, cycle back to 0
            bonusQuestionIndex++;
            if(bonusQuestionIndex == manager.bonusQuestions.Length){bonusQuestionIndex = 0;}
            
            // Sets the current question index to the bonus question index
            currentQuestion = bonusQuestionIndex;
            
            // Setting text to appropriate question and answer
            questionDisplay.text = manager.bonusQuestions[currentQuestion];
            for (int i = 0; i < answerDisplays.Length; i++){
                answerDisplays[i].text = manager.bonusAnswers[currentQuestion][i];
            }
        }

        // Enabling the buttons to interact with the player
        for (int i = 0; i < answerButtons.Length; i++){
            answerButtons[i].interactable = true;
        }

        // Enabling UI
        mainPanel.SetActive(true);
        correctDisplay.SetActive(false);
        incorrectDisplay.SetActive(false);

        // Declares the question as active and freezes the game
        active = true;
        Time.timeScale = 0;
    }

    // Initiates a hint for the player and its UI
    public void InitiateHint(){
        // Increments the hint index to display by 1
        currentHint++;
        // Sets the hint index to 0 if it is escaping the bounds of the hint list
        if(currentHint == manager.mainHints.Length) {currentHint = 0;}

        // Setting text to appropriate hint
        hintDisplay.text = manager.mainHints[currentHint];

        // Enabling Hint UI
        hintPanel.SetActive(true);

        // Declares the question as active and freezes the game
        active = true;
        Time.timeScale = 0;
    }

    // Closes the hint panel and sets the initiation countdown to 10 seconds
    public void EndHint(){
        hintPanel.SetActive(false);
        canInitiate = false;
        initiationTimerElapsed = 10;
        active = false;

        // Resumes game
        Time.timeScale = 1;
    }

    // Checks the answer the Player clicked by retrieving a bool from the main question manager
    // And adjusts the game UI accordingly
    // Operates given the index for the answer, provided by which button the player clicked
    public void CheckAnswer(int answer){

        // Ensures the player cannot initiate another question for a set amount of time
        canInitiate = false;
        if(isBonus){initiationTimerElapsed = 15;}
        else{initiationTimerElapsed = 30;}

        // Checking solution and enabling/disabling the display
        // telling you if you were correct or wrong
        if(manager.CheckSolution(currentQuestion, answer, isBonus)){ 
            correctDisplay.SetActive(true); 

            // If this was the last correct main question, set the initiation timer to 60
            // Instead of the default 30, not posing enough challenge for a final rush
            if(!isBonus && manager.solvedMainQuestions == 4)
            {initiationTimerElapsed = 60;}    

            // Calls the end sequence of the corresponding trial script
            // Only if the question is correct
            currentTrialScript.EndSequence();
        }
        else{ incorrectDisplay.SetActive(true); }

        // Disabling the buttons to interact witht the player
        for (int i = 0; i < answerButtons.Length; i++){
            answerButtons[i].interactable = false;
        }

        // Sets the display time until the frame dissapears
        displayTimeElapsed = displayTimer;
        finishedQuestion = true;


    }

    // Update is called once per frame
    void Update()
    {
        // Displays the timer for if the player can start a question or not
        // Done via changing the text and color of the display depending on the current scenario
        if(initiationTimerElapsed > 0){
            // The text is set to this during the countdown until a new question can be asked
            timerDisplay.text = (  (int)initiationTimerElapsed  ).ToString();
            timerDisplay.color = Color.red;
        }
        else if(manager.solvedMainQuestions == 4){
            // The text is set to this only when the final timer has finished
            timerDisplay.color = Color.yellow;
            timerDisplay.text = "ESCAPE";
        }
        else{
            // The text is set to this when the user can ask a new question
            timerDisplay.color = Color.green;
            timerDisplay.text = "CAN INITIATE";
        }

        // Deducts time every frame from the timer indicating when the display should
        // Dissapear after a question is answered
        if(active && displayTimeElapsed > 0 && finishedQuestion){
            displayTimeElapsed -= Time.unscaledDeltaTime;
        }
        else if(finishedQuestion && displayTimeElapsed <= 0 && active){    
            // Disables UI and re-enables normal gameplay loop once the timer expires
            mainPanel.SetActive(false);
            Time.timeScale = 1;
            finishedQuestion = false;
            active = false;
            currentQuestion = -1;
        }

        // Deducts time every frame from the timer indicating when a new
        // question can be solved by the player
        if(initiationTimerElapsed > 0 && !canInitiate){
            initiationTimerElapsed -= Time.deltaTime;
        }
        else if(initiationTimerElapsed <= 0 && !canInitiate){    
            if(manager.solvedMainQuestions == 4){
                // Activates the portal to escape once four main questions are answered
                portalObject.SetActive(true);
                // Still Allows the player to initiate another question
                // Theres an exclusive weapon for whoever does a bonus challenge on the last stage
                canInitiate = true;
            }
            else{
                canInitiate = true;
            }
        }
    }


    public void EndGame(){
        // Ends the game by displaying the end screen, and freezing the gameloop
        endGameScreen.SetActive(true);
        Time.timeScale = 0;
        return;
    }
}
