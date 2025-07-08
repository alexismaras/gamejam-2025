using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverMenuCanvas;
    [SerializeField] private LoadingScreenController _sceneLoader;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        _gameOverMenuCanvas.SetActive(true);
    }

    public void Restart()
    {
        _sceneLoader.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void Quit()
    {
        _sceneLoader.LoadScene(0);
        Time.timeScale = 1;
    }

}
