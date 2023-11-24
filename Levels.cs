using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Levels : MonoBehaviour
{
    [SerializeField]
    private Text levelName;

    [SerializeField]
    private Text score;

    [SerializeField]
    private Text difficult;

    [SerializeField]
    private Image image;

    private int levelID;

    private string level = "";

    public int getLevelID() =>
        levelID;

    public string getLevel() =>
        level;

    public static bool isSelected = false;

    public static Levels selectedLevelObject;

    public void createLevelExample(in int ID, in string name, in int difficulty, in int score, in int maxScore)
    {
        level = name;
        levelID = ID;
        levelName.text = name;
        this.score.text = score.ToString() + "/" + maxScore.ToString();
        difficult.text = difficulty.ToString();
        if (!isSelected)
        {
            isSelected = true;
            image.color = new Color32(150,255,150,255);
            selectedLevelObject = this;
        }
    }

    public void eventToGameManager()
    {
        selectedLevelObject.image.color = new Color32(255,255,255,255);
        image.color = new Color32(150, 255, 150, 255);
        selectedLevelObject = this;
    }
}
