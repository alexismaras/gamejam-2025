using UnityEngine;

public class HandEnemyController : MonoBehaviour
{
    public enum HandState { Checking, Attacking, }
    public HandState CurrentGameState;

    [SerializeField] private GameObject _visualHand;

    [SerializeField] private float _handAttackTime;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private float _playerLocationCheckThreshold;

    private float _initialPlayerPositionX;
    private float _initialPlayerPositionY;

    private float _currentPlayerPositionX;
    private float _currentPlayerPositionY;

    private float _elapsedTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _visualHand.SetActive(false);
        _initialPlayerPositionX = _playerController.PlayerRigidbody.position.x;
        _initialPlayerPositionY = _playerController.PlayerRigidbody.position.y;

        _currentPlayerPositionX = _initialPlayerPositionX;
        _currentPlayerPositionY = _initialPlayerPositionY;
    }

    // Update is called once per frame
    void Update()
    {
        switch (CurrentGameState)
        {
            case HandState.Checking:
                HandChecking();
                break;


            case HandState.Attacking:

                break;
        }
    }

    void HandChecking()
    {
        _currentPlayerPositionX = _playerController.PlayerRigidbody.position.x;
        _currentPlayerPositionY = _playerController.PlayerRigidbody.position.y;

        if (Mathf.Abs(_currentPlayerPositionX - _initialPlayerPositionX) <= _playerLocationCheckThreshold && Mathf.Abs(_currentPlayerPositionY - _initialPlayerPositionY) <= _playerLocationCheckThreshold)
        {
            _elapsedTime += Time.deltaTime;
        }
        else
        {
            _elapsedTime = 0;
            _visualHand.SetActive(false);
            _initialPlayerPositionX = _playerController.PlayerRigidbody.position.x;
            _initialPlayerPositionY = _playerController.PlayerRigidbody.position.y;
        }

        if (_elapsedTime >= _handAttackTime * 0.5f)
        {
            _visualHand.SetActive(true);
            transform.position = new Vector3(_initialPlayerPositionX, _initialPlayerPositionY, _playerController.PlayerRigidbody.position.z);
        }

        if (_elapsedTime >= _handAttackTime)
        {
            _playerController.KillPlayer();
        }


    }
}
