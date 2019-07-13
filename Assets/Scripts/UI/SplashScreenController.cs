using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Core;

public class SplashScreenController : MonoBehaviour
{
    private bool loadNextScene;

    void Update()
    {
        if (Input.anyKey)
        {
            StartCoroutine(FadeOut());
        }

        if (loadNextScene)
        {
            SceneManager.LoadScene(Constants.Scene_MissionSelection);
        }
    }

    IEnumerator FadeOut()
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= 0.025f;
            yield return new WaitForSeconds(.05f);
        }
        loadNextScene = true;
    }
}

