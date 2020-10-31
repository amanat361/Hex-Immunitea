using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class menu : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;
    public Text progressText;

    public void LoadLevel (int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously (int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        loadingScreen.SetActive(true);

        while (operation.progress < 0.9f)
        {
            yield return null;
            var scaledPerc = 0.7f * operation.progress / 0.9f;
            progressText.text = "Loading: " + (100f * scaledPerc).ToString("F0") + "%";
            slider.value = scaledPerc;
        }

        operation.allowSceneActivation = true;
        float perc = 0.7f;
        while (!operation.isDone)
        {
            yield return null;
            perc = Mathf.Lerp(perc, 1f, 0.05f);
            progressText.text = "Loading: " + (100f * perc).ToString("F0") + "%";
            slider.value = perc;
        }
    }
    
    public void Quit()
    {
        Application.Quit();
    }
}
