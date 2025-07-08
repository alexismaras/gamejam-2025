using UnityEngine;

public class FootstepPlayer : MonoBehaviour
{
    [SerializeField] AudioSource[] _footSteps;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnFootstep()
    {
        _footSteps[Random.Range(0, _footSteps.Length)].Play();

    }
}
