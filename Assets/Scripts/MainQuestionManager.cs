using UnityEngine;
using UnityEngine.UI;

// This class stores the main data for all questions and answers
// This class also checks for correct or wrong answers, 
// Updates difficulty of the game accordingly and handles summoning rewards for bonus questions
public class MainQuestionManager : MonoBehaviour
{

    // Reference to the spawner script
    [SerializeField]
    private EscapeRoomSpawnSystem spawnScript;
    
    // Reference to the player movement system
    [SerializeField]
    private TwoDPlayerController playerController;
    
    // List of reward drops, each index represents tier of gun
    // Bosses List holds the boss object that should spawn on round 2 and 3
    [SerializeField]
    private GameObject[] rewards, bosses;

    // The text field for displaying amount of main questions solved ingame
    [SerializeField]
    private Text solvedQuestionsCounter;   

    // The script which controls the color of the floor 
    [SerializeField]
    private FloorColorScript floor_color;

    // The texts for the four main questions
    public string[] mainQuestions {get; private set;} = new string[]{
        "Find and simplify the derivative of \nf(x) = (3x^5 + 4x) / 5x^3",
        "How far away is point \nP(5, 3, 2) from plane 3x + 3z = 7 + 5y",
        "An airplane's speed is 150 km/h heading due North, and is pushed by a wind coming from N45degW at 30 km/h. How fast, and at what angle, is the airplane traveling?",
        "Determine the Solution to the planes \n6x+4y+3z=0, and 2x+5y+7z-3=0",
    };

    // The texts for the four bonus questions
    public string[] bonusQuestions {get; private set;} = new string[]{
        "Determine the derivative of \nf(x) = 2x^3+5x",
        "Which part of your computer is responsible for processing the main tasks and instructions of your computer?",
        "What is the powerhouse of the cell?",
        "What is the atomic number of Lithium?",
    };
    
    // The texts for the four sets of four answers for the main questions
    public string[][] mainAnswers {get; private set;} = new string[][]{
        // Four answers for main question 0
        new string[]{
            "((15x^4+4)(5x^3)-(15x^2)(3x^5+4x)) / (5x^3)^2",
            "(6x^4 - 8) / 5x^3",
            "(15x^4 + 4 ) / 15x^2",
            "(30x^7-40x^3) / (5x^6)"
        },

        // Four answers for main question 1
        new string[]{
            "19/sqrt(43)",
            "sqrt(19)",
            "7sqrt(32)",
            "7"
        },

        // Four answers for main question 2
        new string[]{
            "the ground speed of the plane is traveling 172.5km/h at a bearing of N7.06degW.",
            "the ground speed of the plane is traveling 130.5km/h at a bearing of N9.4degE.",
            "the ground speed of the plane is traveling 125km/h at a bearing of N11.9degW.",
            "the ground speed of the plane is traveling 202.7km/h at a bearing of N5.4degE."
        },

        // Four answers for main question 3
        new string[]{
            "L: (-4, 0, 6) + s(-1, 0, 1)",
            "L: x=6, y=2t, z=4t+9",
            "L: x= -4, y=t, z=-t+6 ",
            "L: x=-3, y=t, z=-t+2",
        },
    };
    
    // The texts for the four sets of four answers for the bonus questions
    public string[][] bonusAnswers {get; private set;} = new string[][]{
        
        // Four answers for bonus question 0
        new string[]{
            "6x^4+5x^2",
            "2x^2+5",
            "6x^2+5",
            "7",
        },

        // Four answers for bonus question 1
        new string[]{
            "GPU (Graphics Processing Unit)",
            "RAM (Random Access Memory)",
            "The Motherboard",
            "CPU (Central Processing Unit)",
        },

        // Four answers for bonus question 2
        new string[]{
            "The Nucleus",
            "The Mitochondria",
            "Photosynthesis",
            "DNA",
        },

        // Four answers for bonus question 3
        new string[]{
            "3",
            "6",
            "2",
            "21",
        },
    };
    
    // The texts for the four main questions
    public string[] mainHints {get; private set;} = new string[]{
        "The Quotient rule in solving derivatives is crucial in a function with the form \nf(x) / g(x)\nThe derivative for this is determined as so:\nf'(x) = ( f'(x)g(x) - f(x)g'(x) )\n/ g(x)^2",
        "Remember the Power Rule in differentiating functions! This is the backbone of solving derivatives!!\nin any term of f(x) in the form 'ax^n', the derivative for this is\nf'(x) = anx^(n-1)\nFor example, if f(x) = 3x^2, f'(x)= (2)3x^(2-1) = 6x",
        "When finding the distance between a point and a plane in cartesian scalar form, the formula is as follows:\nd = (ax+by+cz+d) / sqrt(a^2+b^2+c^2)\nwhere xyz represent the coordinates of the point, and abcd represent the constants in the plane's cartesian/scalar equation",
        "Sine law is used to find unknown side lengths and angles based on known ones. \nSin law: sinA/a = sinB/b = sinC/c\nwhere ABC represent angles opposite from the side notated by their corresponding letter in lowercase.",
        "Cosine law is used to find an unknown side length based on its known opposite angle, and the other two side lengths. \nCos law: c = sqrt( a^2+b^2-2ab*cosC)\nwhere ab represent other side lengths and C represents the Angle opposite from the unknown side.",
        "To solve planes, you need to get xy and z in terms of t.\nif you eliminate one of the variables by subtracting a plane from a scalar multiple of another plane, cancelling out x, y, or z, you can set one of the other two as t\nfrom there solve the other variables with this newfound definition. You should end with a parametric equation for a line.",
    };
    // Indexes indicating which list element of the answers texts 
    // is the correct answer for each of the four questions 
    private int[] mainAnswerIndex = new int[]{
        1,
        0,
        1,
        2
    };
    private int[] bonusAnswerIndex = new int[]{
        2,
        3,
        1,
        0
    };


    // Stores how many questions of what type have been solved
    public int solvedMainQuestions {get; private set;} = 0;
    public int solvedBonusQuestions {get; private set;} = 0;

    // Checks the solution given a question and answer index, 
    // and a bool indicating main or bonus question
    // Updates difficulty and progression here if a question was correct
    // Returns a boolean indicating if the question was correct or wrong
    public bool CheckSolution(int question, int answer, bool bonus){
        if(mainAnswerIndex[question] == answer && !bonus){
            // If a main question is correct
            // Adds one to the counter of main questions solved, and updates the game difficulty 
            solvedMainQuestions++;
            UpdateDifficulty();
            return true;
        }
        else if(bonusAnswerIndex[question] == answer && bonus){
            // If a bonus question is correct
            // Adds one to the counter of bonus questions solved, and rewards the player 
            solvedBonusQuestions++;
            RewardPlayer();
            return true;
        }
        return false;
    }

    // Updates the difficulty of the game
    public void UpdateDifficulty(){

        // Sets the difficulty options for the enemy spawning system
        spawnScript.SetSpawnDifficulty(solvedMainQuestions);
        
        // Sets the health and speed of the player
        // Updates the UI counter for questions solved
        // Changes the floor color of the map
        // According to which stage of the game the player is on.
        switch (solvedMainQuestions){
            case 1:
                Player.main.health.SetMaxHealth(325, true);
                playerController.speed = 5.5f;
                solvedQuestionsCounter.text = "1";
                floor_color.ChangeColor(new Color(0f, 0.8f, 0f), Color.white);
                break;

            case 2:
                Player.main.health.SetMaxHealth(500, true);
                playerController.speed = 6f;
                solvedQuestionsCounter.text = "2";
                Instantiate(bosses[0], Vector3.zero, transform.rotation); 
                floor_color.ChangeColor(new Color(0.8f, 0, 1f), Color.white);
                break;

            case 3:
                Player.main.health.SetMaxHealth(750, true);
                playerController.speed = 6.5f;
                solvedQuestionsCounter.text = "3";
                Instantiate(bosses[1], Vector3.zero, transform.rotation); 
                floor_color.ChangeColor(new Color(1 , 0.3f, 0), Color.white);
                break;

            case 4:
                Player.main.health.SetMaxHealth(1450, true);
                playerController.speed = 8f;
                solvedQuestionsCounter.text = "4";
                floor_color.ChangeColor(Color.red, Color.white);
                break;
        }
    }

    // Rewards the player a balanced gun in the center of the map
    void RewardPlayer(){
        
        if(solvedMainQuestions == 0)
            // If this is the first stage of the game,
            // Limit the tier of rewards to tier 2 after 2 bonus questions solved
            if(solvedBonusQuestions < 2){
                Instantiate(rewards[0], Vector3.zero, transform.rotation);
            }
            else{ Instantiate(rewards[1], Vector3.zero, transform.rotation); }
        // Otherwise, make the rarity relative to the stage of the game 
        else if(solvedMainQuestions != 4){ Instantiate(rewards[solvedMainQuestions+1], Vector3.zero, transform.rotation); }
        
        // The secret weapon if do an extra bonus question after you've won
        else { Instantiate(rewards[solvedMainQuestions+1], new Vector3(1.5f, 1.5f, 0), transform.rotation); }
    }

    // Start is called before the first frame update
    void Awake()
    {
        // Sets the default floor color at the start of the game 
        floor_color.ChangeColor(Color.blue, Color.white);
    }
}
