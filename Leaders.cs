using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Leaders : MonoBehaviour
{
    [SerializeField]
    private Text nickName;

    [SerializeField]
    private Text score;

    public void changeNickAndScore(in string nick, in int score)
    {
        nickName.text = nick;
        this.score.text = score.ToString();
    }
}
