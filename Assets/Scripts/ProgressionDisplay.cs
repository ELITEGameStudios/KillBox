using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressionDisplay : MonoBehaviour
{
    [SerializeField] private Text currentLevelXpText, targetXpText, playerXpText, levelNumberDisplay, nextLevelNumberDisplay;
    [SerializeField] private Slider xpSlider;
    [SerializeField] private GameObject[] rewardsImages;
    [SerializeField] private ProgressionArrowGraphicAnimation arrowGraphic;
    [SerializeField] private float normalizedXpToNext;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDisplays(){
        currentLevelXpText.text = ProgressionSystem.currentLevelXp.ToString();
        targetXpText.text = ProgressionSystem.nextLevelXp.ToString();
        playerXpText.text = ProgressionSystem.playerXp.ToString();
        levelNumberDisplay.text = ProgressionSystem.playerLevel.ToString();
        nextLevelNumberDisplay.text = (ProgressionSystem.playerLevel+1).ToString();

        xpSlider.maxValue = ProgressionSystem.nextLevelXp;
        xpSlider.minValue = ProgressionSystem.currentLevelXp;
        xpSlider.value = ProgressionSystem.playerXp;

        for (int i = 0; i < rewardsImages.Length; i++)
        { rewardsImages[i].SetActive(false); }

        if( (ProgressionSystem.playerLevel+1) % 5 == 0 && (ProgressionSystem.playerLevel+1) <= 25){
            rewardsImages[ (ProgressionSystem.playerLevel+1) / 5].SetActive(true);
        } 
        else{
            rewardsImages[0].SetActive(true);
        }

        normalizedXpToNext = (float)(ProgressionSystem.playerXp - ProgressionSystem.currentLevelXp) / (float)(ProgressionSystem.nextLevelXp - ProgressionSystem.currentLevelXp); 

        if(arrowGraphic != null){
            arrowGraphic.SetTimings(Mathf.Lerp(2f, 0f, normalizedXpToNext) );
        }
    }
}
