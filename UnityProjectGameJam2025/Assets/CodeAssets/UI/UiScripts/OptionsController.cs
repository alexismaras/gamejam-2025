using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
public class OptionsController : MonoBehaviour
{
    [SerializeField] private GameObject _optionsCanvas;
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private AudioMixer _audioMixer;
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

    public void SetVolume()
    {
        float volume = _volumeSlider.value;
        _audioMixer.SetFloat("output", Mathf.Log10(volume)*20);
    }
}
