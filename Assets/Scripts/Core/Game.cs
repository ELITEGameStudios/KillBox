

[System.Serializable]
public class Game
{
    // Will store the initial settings and setup for the game being played and track player performance throughout

    // Undecided wether the Game class will store the current round and be the primary counter for score
    // Or instead if game just tracks this data for statistics purposes
    
    // Constant values pulled from via difficulty index
    public static readonly string[] difficultyNames = new string[]{"Easy", "Standard", "Extreme"};
    public static readonly float[] difficultyCoefficients = new float[]{0.65f, 1.0f, 2.0f};
    
    
    public bool freeplay;
    public int difficultyIndex;
    public string difficulty;
    public float difficultyCoefficient;
    public int round;
    public int[] upgradesPurchased; // Indexed in order of upgrade type sorted in Upgrade Manager scripts
    public int tokensCollectedOverall;
    public int tokensUsed;
    public int tokens;
    public Lifetime lifetime;
    public bool started {get {return lifetime == Lifetime.STARTED || lifetime == Lifetime.FINISHED;}}
    
    public enum Lifetime{
        INITIALIZED,
        STARTED,
        FINISHED
    }

    public Game(int difficultyIndex){
        this.difficultyIndex = difficultyIndex;
        difficulty = difficultyNames[difficultyIndex];
        difficultyCoefficient = difficultyCoefficients[difficultyIndex];

        round = 1;
        upgradesPurchased = new int[5];
        tokens = 0;
        tokensCollectedOverall = 0;
        lifetime = Lifetime.INITIALIZED;
    }

    public void AdvanceLevel(){
        round++;
    }

    public void AddToken(){
        tokens++;
        tokensCollectedOverall++;
    }

    public void LogUsedTokens(int tokensUsed){
        this.tokensUsed += tokensUsed;
        // tokens--;
    }
}
