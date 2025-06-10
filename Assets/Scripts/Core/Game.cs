

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
    public int scoreCollectedOverall;
    public int tokensUsed;
    public int score;
    public Lifetime lifetime;
    public bool started {get {return lifetime == Lifetime.STARTED || lifetime == Lifetime.FINISHED;}}
    
    public bool hasUpgradedArsenal {get {return round > EnemyList.instance.bossRounds[0];}}
    public int specialUpgrade; // Representation of what special the player has. -1 is nothing, 0 = dual wield, 1 = gold weapons, 2 = necromancy, 3 = upgrades mastery
    
    public enum Lifetime{
        INITIALIZED,
        STARTED,
        FINISHED
    }

    public Game(int difficultyIndex, bool isFreeplay){
        this.difficultyIndex = difficultyIndex;
        difficulty = difficultyNames[difficultyIndex];
        difficultyCoefficient = difficultyCoefficients[difficultyIndex];

        round = 1;
        upgradesPurchased = new int[5];
        score = 0;
        scoreCollectedOverall = 0;
        specialUpgrade = -1;
        freeplay = isFreeplay;
        lifetime = Lifetime.INITIALIZED;
    }

    public void AdvanceLevel(){
        round++;
    }

    public void AddToken(int tokens){
        this.score++;
        scoreCollectedOverall++;
    }

    public void StartGame(){

        lifetime = Lifetime.STARTED;
        GameManager.main.StartGame();
    
    }

    public void LogUsedTokens(int tokensUsed){
        this.tokensUsed += tokensUsed;
        // tokens--;
    }
}
