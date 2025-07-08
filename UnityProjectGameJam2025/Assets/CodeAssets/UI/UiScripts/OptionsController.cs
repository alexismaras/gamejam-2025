using UnityEngine;

public class OptionsController : MonoBehaviour
{
    [SerializeField] private GameObject _optionsCanvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Back()
    {
        _optionsCanvas.SetActive(false);
    }
}
