using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuContoller : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenuCanvas;
    [SerializeField] private GameObject _optionsCanvas;
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void Quit()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void Options()
    {
        _optionsCanvas.SetActive(true);
    }
}
