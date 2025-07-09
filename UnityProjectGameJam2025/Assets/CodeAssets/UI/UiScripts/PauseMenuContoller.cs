using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuContoller : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenuCanvas;
    [SerializeField] private GameObject _optionsCanvas;
    [SerializeField] private LoadingScreenController _sceneLoader;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            _pauseMenuCanvas.SetActive(true);
        }
    }

    public void Resume()
    {
        _pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1;
    }

    public void Restart()
    {
        _sceneLoader.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        _sceneLoader.LoadScene(0);
    }

    public void Options()
    {
        _optionsCanvas.SetActive(true);
    }
}
