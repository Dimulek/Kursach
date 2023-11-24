using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MenuController
{
    private void Awake()
    {
        SQLiteControll.myDBPath();
        if (PlayerPrefs.HasKey(SQLiteControll.userIDKey) && sqlLite.checkUserByID(userID: PlayerPrefs.GetInt(SQLiteControll.userIDKey)))
            moveScene("Accaunt");
    }

    private string exept = "";

    #region SignIn

    [SerializeField]
    private InputField AULoginText;

    [SerializeField]
    private InputField AUPasswordText;

    [SerializeField]
    private Text AUExeptionText;

    #endregion

    #region Register

    [SerializeField]
    private InputField ReNickText;

    [SerializeField]
    private InputField ReLoginText;

    [SerializeField]
    private InputField RePasswordText;

    [SerializeField]
    private Text ReExeptionText;

    #endregion

    public void authorithetion()
    {
        if ((user_ID = sqlLite.checkUser(AULoginText.text, AUPasswordText.text)) < 1)
        {
            Debug.Log(user_ID);
            if (user_ID < -5)
                AUExeptionText.text = "Не получилось отправить запрос в базу данных!";
            else
                AUExeptionText.text = "Логин или пароль не верный!";
            return;
        }
        Debug.Log(user_ID);
        PlayerPrefs.SetInt(SQLiteControll.userIDKey, user_ID);
        moveScene("Accaunt");
    }

    public void registration()
    {
        if (!sqlLite.registerUser(ReNickText.text, ReLoginText.text, RePasswordText.text, ref exept))
        {
            ReExeptionText.text = exept;
            return;
        }
    }
}
