using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
public class LoadingScreenController : MonoBehaviour
{
    [SerializeField] private GameObject _loadingCanvas;
    [SerializeField] private Slider _loadingBar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void LoadScene(int sceneId)
    {
        StartCoroutine(LoadSceneAsync(sceneId));
    }

    IEnumerator LoadSceneAsync(int sceneId)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

        _loadingCanvas.SetActive(true);

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);

            _loadingBar.value = progressValue;

            yield return null;
        }
    }
}
