using UnityEngine;

public class StoryTextScript : MonoBehaviour
{

    [SerializeField] private GameObject _uiText;

    private bool _isTextActive;
    void Start()
    {
        Activate();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && _isTextActive)
        {
            Deactivate();
        }
    }

    private void Activate()
    {
        Time.timeScale = 0;
        _isTextActive = true;
        _uiText.gameObject.SetActive(true);
    }

    private void Deactivate()
    {
        Time.timeScale = 1;
        _isTextActive = false;
        _uiText.gameObject.SetActive(false);
    }
}
