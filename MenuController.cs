using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> windows;

    [SerializeField]
    protected SQLiteControll sqlLite;

    private int lastPositionWindow = 0;

    private int positionWindow = 0;

    protected int user_ID;

    private void Start()
    {
#if !UNITY_EDITOR
Resolution resolution = Screen.currentResolution;
Application.targetFrameRate = Mathf.RoundToInt((float)resolution.refreshRate);
#endif
    }

    public void changeWin(int indexWin)
    {
        positionWindow = indexWin;
        StartCoroutine(ScreenMove());
    }

    private IEnumerator ScreenMove()
    {
        windows[lastPositionWindow].LeanScale(Vector3.zero, 0.5f).setEaseInExpo();
        yield return new WaitForSeconds(0.5f);
        lastPositionWindow = positionWindow;
        windows[positionWindow].LeanScale(Vector3.one, 0.5f).setEaseInExpo();
    }
    public static void moveScene(string sceneName)
    {
        PlayerPrefs.SetString(SQLiteControll.sceneNameKey, sceneName);
        SceneManager.LoadScene("Load");
    }
}
