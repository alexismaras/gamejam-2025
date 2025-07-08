using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuController : MonoBehaviour
{

    [SerializeField] private GameObject _creditsCanvas;
    [SerializeField] private GameObject _menuCanvas;
    [SerializeField] private GameObject _optionsCanvas;

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenOptions()
    {
        _optionsCanvas.SetActive(true);
        _menuCanvas.SetActive(false);
    }

    public void CloseOptions()
    {
        _optionsCanvas.SetActive(false);
        _menuCanvas.SetActive(true);
    }

    public void OpenCredits()
    {
        _creditsCanvas.SetActive(true);
        _menuCanvas.SetActive(false);
    }

    public void CloseCredits()
    {
        _creditsCanvas.SetActive(false);
        _menuCanvas.SetActive(true);
    }
}
