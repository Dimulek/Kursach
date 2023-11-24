using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SQLiteControll : MonoBehaviour
{
    #region keysOfMemory
    public const string userIDKey = "User_ID";
    public const string levelIDKey = "levelID";
    public const string sceneNameKey = "windowName";
    #endregion

    public static SqliteConnection dbConnetction;
    private string path;
    private static SqliteDataReader reques;

    private const string dbName = "Villangeon.bytes.db";
    private static string connextionLine;
    //Start is called before the first frame update

    #region проверка наличия БД

    public static void myDBPath()
    {
        connextionLine = GetDatabasePath();
    }

    public static string GetDatabasePath()
    {
#if UNITY_EDITOR
        string filePath = Path.Combine(Application.streamingAssetsPath, dbName);
        if (!File.Exists(filePath)) 
            UnpackDatabase(filePath);
        return filePath;
#elif UNITY_STANDALONE
        string filePath = Path.Combine(Application.streamingAssetsPath, dbName);
        if(!File.Exists(filePath)) 
            UnpackDatabase(filePath);
            return filePath;
#elif UNITY_ANDROID
        string filePath = Path.Combine(Application.persistentDataPath, dbName);
        if (!File.Exists(filePath)) 
            UnpackDatabase(filePath);
            return filePath;
#endif
    }

    private static void UnpackDatabase(in string filePath)
    {
        string fromPath = Path.Combine(Application.streamingAssetsPath, dbName);

        WWW reader = new WWW(fromPath);
        while (!reader.isDone) { }
        
        File.WriteAllBytes(filePath, reader.bytes);
        addTables();
        insertTables(); 
        addViews();
    }
    #endregion

    #region Создание БД

    private static void createTable(in string tableName, in string tableValues)
    {
        string query = "Create table " + tableName + " (" + tableValues + ");";
        try
        {
            setConnection(query);
        }
        catch(Exception ex)
        {
            Debug.Log("Table - " + ex.Message);
        }
    }
    private static void addTables()
    {
        createTable("User", "ID_User integer NOT NULL PRIMARY KEY AUTOINCREMENT,\r\n  Login_User TEXT(50) NOT NULL UNIQUE,\r\n  Password_User TEXT(50) NOT NULL,\r\n  NickName_User TEXT(50) NOT NULL UNIQUE,\r\n  CONSTRAINT Login_Check CHECK (LENGTH(Login_User) >= 5),\r\n  CONSTRAINT Password_Check CHECK (LENGTH(Password_User) >= 5),\r\n  CONSTRAINT NickName_Check CHECK (LENGTH(NickName_User) >= 5)");
        createTable("User_InfinityMode", "ID_User_InfinityMode integer NOT NULL PRIMARY KEY AUTOINCREMENT,\r\n  CountPoints_User_InfinityMode integer NOT NULL,\r\n\tDate_User_InfinityMode REAL NOT NULL DEFAULT(julianday('now')),\r\n\tUser_ID integer NOT NULL,\r\n  CONSTRAINT CountPoints_User_InfinityMode_Check CHECK (CountPoints_User_InfinityMode >= 0),\r\n\tFOREIGN KEY(User_ID) REFERENCES User(ID_User)");
        createTable("Difficult", "ID_Difficult integer NOT NULL PRIMARY KEY AUTOINCREMENT,\r\n\tCountStars_Difficult integer NOT NULL UNIQUE");
        createTable("Level", "ID_Level integer NOT NULL PRIMARY KEY AUTOINCREMENT,\r\n\tName_Level Text(50) NOT NULL UNIQUE,\r\n\tDifficult_ID integer NOT NULL,\r\n\tFOREIGN KEY(Difficult_ID) REFERENCES Difficult(ID_Difficult)");
        createTable("Enemy", "ID_Enemy integer NOT NULL PRIMARY KEY AUTOINCREMENT,\r\n\tIndex_Enemy integer NOT NULL UNIQUE");
        createTable("Level_Enemy", "ID_Level_Enemy integer NOT NULL PRIMARY KEY AUTOINCREMENT,\r\n\tEnemy_ID integer NOT NULL,\r\n\tLevel_ID integer NOT NULL,\r\n\tWaveNumber_Level_Enemy integer NOT NULL,\r\n\tFOREIGN KEY(Enemy_ID) REFERENCES Enemy(ID_Enemy),\r\n\tFOREIGN KEY(Level_ID) REFERENCES Level(ID_Level)");
        createTable("User_Level", "ID_User_Level integer NOT NULL PRIMARY KEY AUTOINCREMENT,\r\n\tUser_ID integer NOT NULL,\r\n\tLevel_ID integer NOT NULL,\r\n\tWaveStop_User_Level integer NOT NULL,\r\n\tFOREIGN KEY(User_ID) REFERENCES User(ID_User),\r\n\tFOREIGN KEY(Level_ID) REFERENCES Level(ID_Level)");
    }
    private static void createView(in string viewLine)
    {
        try
        {
            setConnection(viewLine);
        }
        catch (Exception ex)
        {
            Debug.Log("Insert - " + ex.Message);
        }
    }
    private static void addViews()
    {
        createView("create VIEW Find_Levels(\"КодПользователя\", \"КодУровня\",\r\n\"Название\", \"Сложность\", \"ПоследняяВолна\", \"Последняя\")\r\nas\r\n\tselect ID_User, \r\n\tID_Level,\r\n\tName_Level, \r\n\tCountStars_Difficult,\r\n\tWaveStop_User_Level,\r\n\tmax(WaveNumber_Level_Enemy) from Level\r\n\tleft join Difficult on ID_Difficult = Difficult_ID\r\n\tleft JOIN Level_Enemy on Level_Enemy.Level_ID = ID_Level\r\n\tLEFT join User_Level on User_Level.Level_ID = ID_Level\r\n\tLEFT JOIN User on ID_User = User_ID\r\n\tGroup by ID_User, ID_Level;");
        createView("create VIEW Find_User(\"Код\", \"Логин\", \"Ник\", \"Пароль\")\r\nas\r\n\tselect ID_User, Login_User, NickName_User, Password_User from User;");
        createView("create VIEW LeaderBoard(\"Логин\", \"Никнейм\", \"Очки\")\r\nas\r\n\tselect Login_User, NickName_User, CountPoints_User_InfinityMode from User_InfinityMode inner JOIN User on ID_User = User_ID;");
    }
    private static void insertIntoTable(in string tableName, in string insertLine)
    {
        string query = "Insert into " + tableName + " " + insertLine + ";";
        try
        {
            setConnection(query);
        }
        catch (Exception ex)
        {
            Debug.Log("Insert - " + ex.Message + "\n" + query + "\n");
        }
    }
    private static void insertTables()
    {
        insertIntoTable("User", "(Login_User, Password_User, NickName_User)\r\nvalues('dimulek', 'dimulek', 'dimulek')");
        insertIntoTable("Difficult", "(CountStars_Difficult)\r\nvalues (1), (2), (3)");
        insertIntoTable("Level", "(Name_Level, Difficult_ID)\r\nvalues ('Level1',1), ('Level2',2), ('Level3',3)");
        insertIntoTable("Enemy", "(Index_Enemy)\r\nvalues (0),(1),(2),(3),(4),(5),(6),(7),(8),(9),(10),\r\n(11),(12),(13),(14),(15),(16),(17),(18),(19),(20),\r\n(21),(22),(23),(24),(25),(26),(27),(28),(29),(30)");
        insertIntoTable("Level_Enemy", "(Level_ID, Enemy_ID, WaveNumber_Level_Enemy)\r\nvalues\r\n(2, 1, 0),\r\n\r\n(3, 1, 0),\r\n\r\n(1, 1, 0),(1, 1, 0),(1, 1, 0),(1, 1, 0),(1, 1, 0),(1, 1, 0),(1, 1, 0),(1, 1, 0),(1, 1, 0),(1, 1, 0),\r\n(1, 3, 1), (1, 3, 1),(1, 3, 1),(1, 3, 1),(1, 3, 1),(1, 3, 1),(1, 3, 1),\r\n(1, 4, 2), (1, 4, 2),(1, 4, 2),(1, 4, 2),(1, 4, 2),\r\n(1, 5, 3),(1, 5, 3),(1, 5, 3),(1, 5, 3),(1, 5, 3),\r\n(1, 4, 4),(1, 4, 4),(1, 4, 4),(1, 4, 4),(1, 4, 4),(1, 6, 4),(1, 6, 4),(1, 6, 4),\r\n(1, 6, 5),(1, 6, 5),(1, 6, 5),(1, 6, 5),(1, 6, 5), (1, 8, 5),\r\n(1, 10, 6),(1, 10, 6),(1, 10, 6), (1, 7, 6),(1, 7, 6),(1, 7, 6),(1, 7, 6),(1, 7, 6),\r\n(1, 11, 7),(1, 11, 7),(1, 11, 7),\r\n(1, 12, 8),(1, 12, 8),\r\n(1, 13, 9)");
    }

    #endregion

    public static bool setConnection(in string act)
    {
        try
        {
            dbConnetction = new SqliteConnection("URI=file:" + connextionLine);
            dbConnetction.Open();
            if (ConnectionState.Open == dbConnetction.State)
            {
                SqliteCommand cmd = new SqliteCommand();
                cmd.Connection = dbConnetction;
                cmd.CommandText = act;
                reques = cmd.ExecuteReader();
            }

        }
        catch
        {
            myDBPath();
            PlayerPrefs.DeleteKey(SQLiteControll.userIDKey);
            PlayerPrefs.SetString(SQLiteControll.sceneNameKey, "MainMenu");
            SceneManager.LoadScene("Load");
            return false;
        }
        return true;
    }

    #region Запросы
    public void updateScoreInfinity(in int User_ID, in int waveCount)
    {
        if (!setConnection("Select CountPoints_User_InfinityMode from User_InfinityMode Where User_ID = " + User_ID))
            return;
        int count = -1;
        while (reques.Read())
        {
            count = Convert.ToInt32(reques[0]);

        }
        if (count == -1)
            setConnection(String.Format("insert into User_InfinityMode (CountPoints_User_InfinityMode, User_ID) values('{0}', '{1}')", waveCount, User_ID));
        else if (count < waveCount)
            setConnection(String.Format("update User_InfinityMode set CountPoints_User_InfinityMode = {0} Where User_ID == {1}", waveCount, User_ID));
    }

    public int checkUser(in string login, in string password)
    {
        int user_ID = -1;
        if(!setConnection(String.Format("Select Код from Find_User Where Логин == '{0}' and Пароль == '{1}'", login, password)))
            return -100;
        while (reques.Read())
            user_ID = Convert.ToInt32(reques[0]);
        return user_ID;
    }

    public bool registerUser(in string nickName, in string login, in string password, ref string exeption)
    {
        try
        {
            if (login == "" || nickName == "")
            {
                exeption = "Поля не должны быть пустыми!";
                return false;
            }
            if (login.Length < 5 || nickName.Length < 5 | password.Length < 5)
            {
                exeption = "Длина логина, пароля и ника должны быть длинее 5 символов!";
                return false;
            }
            if (login.Length > 50 || nickName.Length > 50 | password.Length > 50)
            {
                exeption = "Длина логина, пароля и ника не должны быть длинее 50 символов!";
                return false;
            }
            if (!CheckLoginAndNickName(login, nickName))
            {
                exeption = "Аккаунт таким логином или ником уже существует!";
                return false;
            }
            setConnection(String.Format("insert into User (Login_User, Password_User, NickName_User) values ('{0}', '{1}', '{2}')", login, password, nickName));

            updateUser_Level(checkUser(login, password));
        }
        catch (Exception ex) 
        {
            exeption = ex.Message;
        }
        return true;
    }
    
    public void updateUser_Level(in int user_id)
    {
        insertIntoTable("User_Level", $"(User_ID, Level_ID, WaveStop_User_Level)\r\nvalues ({user_id}, 1, 0)");
        insertIntoTable("User_Level", $"(User_ID, Level_ID, WaveStop_User_Level)\r\nvalues ({user_id}, 2, 0)");
        insertIntoTable("User_Level", $"(User_ID, Level_ID, WaveStop_User_Level)\r\nvalues ({user_id}, 3, 0)");
    }

    public List<int> getEnemyIDsForLevelAndWave(in int level_ID, in int waveNumber)
    {
        List<int> list = new List<int>();
        if(!setConnection(String.Format("select ID_Level_Enemy, Index_Enemy from Level_Enemy inner join Enemy on ID_Enemy = Enemy_ID where Level_ID = {0} and WaveNumber_Level_Enemy = {1} order by ID_Level_Enemy ASC", level_ID, waveNumber)))
            return list;

        while (reques.Read())
            list.Add(Convert.ToInt32(reques[1]));
        return list;
    }

    public void saveResultLevel(in int userID, in int levelID, in int waveCount)
    {
        if(!setConnection("Select ПоследняяВолна from Find_Levels Where КодПользователя = " + userID + " and КодУровня = " + levelID))
            return;
        int count = -1;
        while (reques.Read())
        {
            count = Convert.ToInt32(reques[0]);
        }
        if (count < waveCount)
        {
            setConnection(String.Format("update User_Level set WaveStop_User_Level = {0} Where User_ID = {1} and Level_ID = {2}", waveCount, userID, levelID));
        }
    }

    public bool CheckLoginAndNickName(in string login, in string nickName)
    {
        if(!setConnection(String.Format("Select Код from Find_User Where Логин == '{0}' or Ник == '{1}'", login, nickName)))
            return false;
        int countFinds = 0;
        while (reques.Read())
            countFinds++;
        return countFinds == 0;
    }

    public void getLeaders(ref Transform leaderList, in GameObject leaderExample)
    {
        if(!setConnection("Select Никнейм, Очки from LeaderBoard ORDER BY Очки DESC LIMIT(10);"))
            return;

        while (reques.Read())
        {
            GameObject leader = Instantiate(leaderExample, leaderList.position, leaderList.rotation, leaderList);
            Leaders lead = leader.GetComponent<Leaders>();
            lead.changeNickAndScore(reques[0].ToString(), Convert.ToInt32(reques[1]));
        }
    }

    public void getLevels(ref Transform levelsList, in GameObject levelExample)
    {
        if (!setConnection("Select КодУровня, Название, Сложность, ПоследняяВолна, Последняя from Find_Levels where КодПользователя = " + PlayerPrefs.GetInt("User_ID").ToString() + " ORDER BY Сложность ASC;"))
            return;
        while (reques.Read())
        {
            GameObject leader = Instantiate(levelExample, levelsList.position, levelsList.rotation, levelsList);
            Levels lead = leader.GetComponent<Levels>();
            lead.createLevelExample(Convert.ToInt32(reques[0]), reques[1].ToString(), Convert.ToInt32(reques[2]), Convert.ToInt32(reques[3]), Convert.ToInt32(reques[4]) + 1);
        }
        Levels.isSelected = false;
    }

    public bool checkUserByID(int userID)
    {
        if(!setConnection(String.Format("Select * from Find_User Where Код == {0}", userID)))
            return false;
        while (reques.Read())
            return true;
        return false;
    }
    #endregion

}