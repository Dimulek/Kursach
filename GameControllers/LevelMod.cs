using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LevelMod : GameManager
{
    [SerializeField]
    private GameObject winInterface;

    private int level_ID;

    private void Awake()
    {
        level_ID = PlayerPrefs.GetInt(SQLiteControll.levelIDKey);
        InstanceManager = this;
        methodGetEnemies = new Action(getEnemyOfWave);
        deathMethod = new Action(saveResultOfLevel);
    }

    private void getEnemyOfWave()
    {
        enemyToSpawn = sqliteControll.getEnemyIDsForLevelAndWave(level_ID, waveCount);
        if (enemyToSpawn.Count == 0)
            winEvent();
    }

    private void winEvent()
    {
        openAndCloseInterfaceses(ref gameInterface, ref winInterface, true);
        saveResultOfLevel();
    }

    private void saveResultOfLevel()
    {
        sqliteControll.saveResultLevel(PlayerPrefs.GetInt(SQLiteControll.userIDKey), level_ID, wavesPassed);
    }
}
