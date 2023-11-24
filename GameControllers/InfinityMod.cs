using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InfinityMod : GameManager
{
    private long scoreWave = 0;
    private long nowScoreWave = 0;

    private void Awake()
    {
        InstanceManager = this;
        methodGetEnemies = new Action(calculateNextWave);
        deathMethod = new Action(updateScore);
    }

    private void calculateNextWave()
    {
        calculateScoreWave();
        getAllEnemyWave();
    }

    private void calculateScoreWave()
    {
        if (long.MaxValue / 1.05f > scoreWave)
            scoreWave = Convert.ToInt32((scoreWave + 4) * 1.1f);
        nowScoreWave = scoreWave;
    }

    public void getAllEnemyWave()
    {
        List<int> list = new List<int>();
        while (nowScoreWave > 0)
        {
            Enemy enemy = enemyTypes.Where(p => (p.waveScoreCost - 1) * 3 <= nowScoreWave && p.minWaveCount - 1 < waveCount).OrderByDescending(p => p.waveScoreCost).First();

            list.Add(enemyTypes.IndexOf(enemy));
            nowScoreWave -= enemy.waveScoreCost;
        }
        list.Reverse();
        enemyToSpawn = list;
    }

    private void updateScore()
    {
        sqliteControll.updateScoreInfinity(PlayerPrefs.GetInt(SQLiteControll.userIDKey), wavesPassed);
    }
}
