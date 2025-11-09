using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayPortalUI : MonoBehaviour
{
    public ElementProfile[] elements;
    public GameObject killboxEmblem;
    public GameObject freeplayEmblem;
    public GameObject bossrushEmblem;

    [System.Serializable]
    public struct ElementProfile
    {
        public Color[] killboxDifficultyColors;
        public Color freeplayColor;
        public Color bossRushColor;
        public Graphic[] graphics;
    }

    // Start is called before the first frame update
    public void SetGraphics()
    {  
        freeplayEmblem.SetActive(MainMenuManager.instance.selectedGamemode == MainMenuManager.Gamemode.FREEPLAY);
        bossrushEmblem.SetActive(MainMenuManager.instance.selectedGamemode == MainMenuManager.Gamemode.BOSSRUSH);
        killboxEmblem.SetActive(MainMenuManager.instance.selectedGamemode == MainMenuManager.Gamemode.KILLBOX);


        foreach (ElementProfile element in elements)
        {
            Color targetColor;
            switch (MainMenuManager.instance.selectedGamemode)
            {
                case MainMenuManager.Gamemode.BOSSRUSH:
                    targetColor = element.bossRushColor;
                    break;
                case MainMenuManager.Gamemode.FREEPLAY:
                    targetColor = element.freeplayColor;
                    break;
                default:
                    targetColor = element.killboxDifficultyColors[MainMenuManager.instance.selectedDifficulty];
                    break;
            }
                
            foreach (Graphic graphic in element.graphics)
            {
                float alpha = graphic.color.a;
                graphic.color = new Color(
                    targetColor.r,
                    targetColor.g,
                    targetColor.b,
                    alpha
                );
            }
        }   
    }
}
