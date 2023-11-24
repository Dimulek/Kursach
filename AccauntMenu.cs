using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AccauntMenu : MenuController
{
    [SerializeField]
    private Transform Leaders;

    [SerializeField]
    private GameObject leaderExample;

    [SerializeField]
    private Transform LevelsContent;

    [SerializeField]
    private GameObject levelExample;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey(SQLiteControll.userIDKey))
            exit();
        sqlLite.getLeaders(ref Leaders, leaderExample);
        sqlLite.getLevels(ref LevelsContent, levelExample);
    }

    public void infinityMove() =>
        moveScene("InfinityGame");

    public void levelMove()
    {
        if (Levels.selectedLevelObject.getLevel() == "")
            return;
        PlayerPrefs.SetInt(SQLiteControll.levelIDKey, Levels.selectedLevelObject.getLevelID());
        moveScene(Levels.selectedLevelObject.getLevel());
    }

    public void exit()
    {
        PlayerPrefs.DeleteKey(SQLiteControll.userIDKey);
        moveScene("MainMenu");
    }
}
