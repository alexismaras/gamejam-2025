using UnityEngine;
using System;

public class HandEnemyController : MonoBehaviour
{
    public static event Action OnHandSmash;
    [SerializeField] private GameObject _visualHand;

    [SerializeField] private float _handAttackTime;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private float _playerLocationCheckThreshold;

    private float _initialPlayerPositionX;
    private float _initialPlayerPositionZ;

    private float _currentPlayerPositionX;
    private float _currentPlayerPositionZ;

    private float _elapsedTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _visualHand.SetActive(false);
        _initialPlayerPositionX = _playerController.PlayerRigidbody.position.x;
        _initialPlayerPositionZ = _playerController.PlayerRigidbody.position.z;

        _currentPlayerPositionX = _initialPlayerPositionX;
        _currentPlayerPositionZ = _initialPlayerPositionZ;
    }

    // Update is called once per frame
    void Update()
    {
        HandChecking();
    }

    void HandChecking()
    {
        _currentPlayerPositionX = _playerController.PlayerRigidbody.position.x;
        _currentPlayerPositionZ = _playerController.PlayerRigidbody.position.z;

        if (Mathf.Abs(_currentPlayerPositionX - _initialPlayerPositionX) <= _playerLocationCheckThreshold && Mathf.Abs(_currentPlayerPositionZ - _initialPlayerPositionZ) <= _playerLocationCheckThreshold)
        {
            _elapsedTime += Time.deltaTime;
        }
        else
        {
            _elapsedTime = 0;
            _visualHand.SetActive(false);
            _initialPlayerPositionX = _playerController.PlayerRigidbody.position.x;
            _initialPlayerPositionZ = _playerController.PlayerRigidbody.position.z;
        }

        if (_elapsedTime >= _handAttackTime * 0.5f)
        {
            _visualHand.SetActive(true);
            transform.position = new Vector3(_initialPlayerPositionX,  _playerController.PlayerRigidbody.position.y, _initialPlayerPositionZ);
        }

        if (_elapsedTime >= _handAttackTime)
        {
            OnHandSmash?.Invoke();
        }


    }
}
