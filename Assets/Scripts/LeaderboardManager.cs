using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;

using Newtonsoft.Json;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField]
    private string standardId = "standard";
    private string extremeId = "extreme";
    private string easyId = "easy";

    private string currentId = "";
    private string displayId= "";

    [SerializeField]
    private InputField Name;
    
    //0 = top 10, 1 = player pos
    private int lock_to = 0, top_pos = 0, disp_lbd = 1;




    [SerializeField]
    private Text[] names_txt, pos_txt, scores_txt;
    
    [SerializeField]
    private Text placeholder, yourPosText;

    private int[] pos = new int[10], scores = new int[10];

    private string[] names = new string[10];
    private string[] tiers = new string[10];

    [SerializeField]
    private Image[] share_mode_color, pos_panels;

    [SerializeField]
    private Color[] leaderboard_colors, tier_colors;

    public static LeaderboardManager main {get; private set;}

    // Start is called before the first frame update
    void Awake()
    {
        
        if(main == null){
            main = this;
            currentId = standardId;
            displayId = standardId;
        }
        else{
            Destroy(this);
        }

        InitLbd();
    }

    public async void InitLbd()
    {
        await UnityServices.InitializeAsync();

        //only if i want to create a new account
        //AuthenticationService.Instance.ClearSessionToken();
        
        //await AuthenticationService.Instance.SignInAnonymouslyAsync(new SignInOptions(CreateAccount : true));
        await SignInAnonymously();
        await GetBestScores();
        await GetPlayerScore(true);
    }

    public async Task GetBestScores(){

        Debug.Log("Getting scores");

        var scoresResponse = await LeaderboardsService.Instance.GetScoresAsync(displayId);

        Debug.Log(scoresResponse.Results.Count);

        if(scoresResponse.Results != null){
            
            top_pos = 0;

            for (int i = 0; i < 10; i++)
            {
                if(scoresResponse.Results.Count <= i){
                
                    pos[i] = -1;
                    names[i] = "";
                    scores[i] = -1;
                    tiers[i] = "";

                    pos_txt[i].text = "";
                    names_txt[i].text = "";
                    scores_txt[i].text = "";

                    // pos_panels[i].color = leaderboard_colors[disp_lbd];
                }
                else{
                    LeaderboardEntry entry = scoresResponse.Results[i];

                    pos[i] = (int)entry.Rank+1;
                    names[i] = entry.PlayerName;
                    scores[i] = (int)entry.Score;
                    tiers[i] = entry.Tier;

                    pos_txt[i].text = pos[i].ToString();
                    names_txt[i].text = names[i];
                    scores_txt[i].text = scores[i].ToString();
                    // pos_panels[i].color = leaderboard_colors[disp_lbd];
                }
            }
        }
        Debug.Log("got scores");
    }
    public async Task GetRemoteScores(){

        Debug.Log("Getting scores");

        var scoresResponse = await LeaderboardsService.Instance.GetScoresAsync(displayId,new GetScoresOptions{Offset = top_pos, Limit = 10});

        Debug.Log(scoresResponse.Results.Count);

        if(scoresResponse.Results != null){
            for (int i = 0; i < 10; i++)
            {
                if(scoresResponse.Results.Count <= i){
                
                    pos[i] = -1;
                    names[i] = "";
                    scores[i] = -1;
                    tiers[i] = "";

                    pos_txt[i].text = "";
                    names_txt[i].text = "";
                    scores_txt[i].text = "";
                    // pos_panels[i].color = leaderboard_colors[disp_lbd];

                }
                else{
                    LeaderboardEntry entry = scoresResponse.Results[i];

                    pos[i] = (int)entry.Rank+1;
                    names[i] = entry.PlayerName;
                    scores[i] = (int)entry.Score;
                    tiers[i] = entry.Tier;

                    pos_txt[i].text = pos[i].ToString();
                    names_txt[i].text = names[i];
                    scores_txt[i].text = scores[i].ToString();
                    // pos_panels[i].color = leaderboard_colors[disp_lbd];
                }
            }
        }
        Debug.Log("got scores");
    }
    

    public async Task GetPlayerScore(bool forMainPosDisplay = false){
        
        var player_pos_entry = await LeaderboardsService.Instance.GetPlayerScoreAsync(displayId);
        int player_pos = player_pos_entry.Rank;
        yourPosText.text = (player_pos+1).ToString();
        
        if(!forMainPosDisplay){
            if(player_pos < 10){
                GetBestScores();
            }
            else{

                Debug.Log("Getting player scores");

                var scoresResponse = await LeaderboardsService.Instance.GetPlayerRangeAsync(displayId, new GetPlayerRangeOptions{ RangeLimit = 5 });

                Debug.Log(scoresResponse.Results.Count);

                if(scoresResponse.Results != null){
                    top_pos = scoresResponse.Results[0].Rank;
                    for (int i = 0; i < 10; i++)
                    {
                        if(scoresResponse.Results.Count <= i){
                        
                            pos[i] = -1;
                            names[i] = "";
                            scores[i] = -1;
                            tiers[i] = "";

                            pos_txt[i].text = "";
                            names_txt[i].text = "";
                            scores_txt[i].text = "";
                            // pos_panels[i].color = leaderboard_colors[disp_lbd];


                        }
                        else{
                            LeaderboardEntry entry = scoresResponse.Results[i];

                            pos[i] = (int)entry.Rank+1;
                            names[i] = entry.PlayerName;
                            scores[i] = (int)entry.Score;
                            tiers[i] = entry.Tier;

                            pos_txt[i].text = pos[i].ToString();
                            names_txt[i].text = names[i];
                            scores_txt[i].text = scores[i].ToString();
                            // pos_panels[i].color = leaderboard_colors[disp_lbd];
                        }
                    }
                }
            }
            Debug.Log("got scores");
        }
    }

    public async void SendScore(){

        if(!GameManager.main.freeplay){

            Debug.Log("Sending scores");
            LeaderboardEntry response = await LeaderboardsService.Instance.AddPlayerScoreAsync(currentId, GameManager.main.LvlCount);
            Debug.Log("Sent scores");
        }
        else{
            Debug.Log("This game is in freeplay mode. Can't send scores");
        }
    }

    async Task SignInAnonymously()
    {
        bool signed_in = AuthenticationService.Instance.IsSignedIn;

        AuthenticationService.Instance.SignedIn += () => { 
            Debug.Log("Signed in as: " + AuthenticationService.Instance.PlayerId);
            signed_in = true;
        };

        AuthenticationService.Instance.SignInFailed += s => { Debug.Log(s); };


        if(!signed_in){
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            signed_in = true;
        }

        string name = AuthenticationService.Instance.PlayerName;

        placeholder.text = "Change Name? (Currently: " + name + ")";
    }

    public async void ChangeName(){
        string name = Name.text;
        await AuthenticationService.Instance.UpdatePlayerNameAsync(name);
    }

    public void Scroll(int number){
        if(top_pos + number < 0){
            top_pos = 0;
        }
        else{
            top_pos += number;
        }

        GetRemoteScores();
    }    

    public void SetMode(int mode){
        switch(mode){
            case 0:
                currentId = easyId;
                break;
            case 1:
                currentId = standardId;
                break;
            case 2:
                currentId = extremeId;
                break;
        }
    }
    
    public void SetDisplayMode(int mode){
        switch(mode){
            case 0:
                displayId = easyId;
                disp_lbd = 0;
                foreach (Image item in share_mode_color)
                { item.color = leaderboard_colors[0]; }

                break;
            case 1:
                displayId = standardId;
                disp_lbd = 1;

                foreach (Image item in share_mode_color)
                { item.color = leaderboard_colors[1];}

                break;
            case 2:
                displayId = extremeId;
                disp_lbd = 2;

                foreach (Image item in share_mode_color)
                { item.color = leaderboard_colors[2];}
                
                break;
        }

        ChangeDisplay();
    }

    public void ChangeDisplayLock(int mode){
        lock_to = mode;
        ChangeDisplay();
    } 

    async void ChangeDisplay(){

        await GetPlayerScore(true);

        if(lock_to == 0){

            await GetBestScores();
        } 
        else if(lock_to == 1){

            await GetPlayerScore();
        }
    }

    Color SetColorByTier(string tier){
        switch(tier){
            case "the_elite":
                return tier_colors[4];
            case "aetherial":
                return tier_colors[3];
            case "prismatic":
                return tier_colors[2];
            case "pro":
                return tier_colors[1];
            case "novice":
                return tier_colors[0];
        }

        return leaderboard_colors[disp_lbd];
    } 

    //public async void GetTop10()
    //{
    //    await LeaderboardsService.Instance.GetScoresAsync(standardId, 102);
    //}
    //public async void AddScore()
    //{
    //    await UnityServices.InitializeAsync();
    //    await AuthenticationService.Instance.SignInAnonymouslyAsync();
    //    var scoresResponse = await LeaderboardsService.Instance.GetScoresAsync("standard");
    //    Debug.Log(scoresResponse);
    //    await LeaderboardsService.Instance.AddPlayerScoreAsync(standardId, 102);
    //}

    // Update is called once per frame
    void Update()
    {
        
    }
}
