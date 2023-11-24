using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScript : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private Text loadingText;

    [SerializeField]
    private Text loadingPercent;

    [SerializeField]
    private Image loadingImage;

    private AsyncOperation asyc;

    void Start() {
        asyc = SceneManager.LoadSceneAsync(PlayerPrefs.GetString("windowName"));
    }

    void Update()
    {
        if (isCanAddPoint)
            StartCoroutine(createNewPoint());
        loadingImage.fillAmount = asyc.progress;
        loadingPercent.text = Mathf.RoundToInt(asyc.progress * 100).ToString() + "%";
    }

    private int pointCount = 0;
    private bool isCanAddPoint = true;

    private IEnumerator createNewPoint()
    {
        isCanAddPoint = false;
        if (pointCount < 3)
        {
            loadingText.text += ".";
            ++pointCount;
        }
        else
        {
            loadingText.text = "Loading ";
            pointCount = 0;
        }
        yield return new WaitForSeconds(0.5f);
        isCanAddPoint = true;
    }
}
