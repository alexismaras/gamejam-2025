using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
public class PlayerController : MonoBehaviour
{
    private IState currentState;
    public Rigidbody PlayerRigidbody { get; private set; }
    public Transform PlayerTransform { get; private set; }
    public Animator PlayerAnimator { get; private set; }
    public Collider PlayerCollider { get; private set; }


    [SerializeField] private Camera _playerCamera; // Assign in Inspector
    public Camera PlayerCamera => _playerCamera;

    [SerializeField] private GameOverController _gameOverController;

    [Header("Movement Settings")]
    [SerializeField] private float _rotationSpeed;
    public float RotationSpeed => _rotationSpeed;
    [SerializeField] private float _walingAcceleration;
    public float WalkingAcceleration => _walingAcceleration;

    [Header("Jump Settings")]
    [SerializeField] private float _jumpForce;
    public float JumpForce => _jumpForce;
    [SerializeField] private float _jumpMaxHeight;
    public float JumpMaxHeight => _jumpMaxHeight;
    [SerializeField] private float _jumpMaxAirTime;
    public float JumpMaxAirTime => _jumpMaxAirTime;

    [Header("Attack Settings")]
    private bool _chargingUpPunch;
    private float _punchChargeValue;

    [SerializeField] private float _maxPunchCharge;
    [SerializeField] private int _regularPunchPoints = 1;
    [SerializeField] private int _chargedPunchPoints = 2;
    [SerializeField] private AttackSource _attackSource;
    [SerializeField] private Slider _punchChargeSlider;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private Image _punchChargeSliderFillImage;
    private Color _punchChargeSliderInitialFillColor;
    private Color _punchChargeSliderTargetFillColor;

    [Header("Speed Power Up")]
    [SerializeField] private float _speedPowerUpTime;

    private bool _speedPowerUp;
    public bool SpeedPowerUp => _speedPowerUp;

    
    private float _elapsedSpeedPowerUpTime;


    private int _playerScore;

    public Coroutine StartStateCoroutine(IEnumerator coroutine)
    {
        return StartCoroutine(coroutine);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        PlayerRigidbody = GetComponent<Rigidbody>();
        PlayerTransform = this.gameObject.transform;
        PlayerAnimator = GetComponent<Animator>();
        PlayerCollider = GetComponent<Collider>();
        IceCubeUnit.OnAssignPointsToPlayer += HandleAssignPointsToPlayer;
        DeadZoneScript.OnDeadZoneEntered += HandleDeadZoneEntered;
        _attackSource = GetComponent<AttackSource>();

    }
    void Start()
    {
        ChangeState(new IdleState(this));
        _punchChargeSliderInitialFillColor = _punchChargeSliderFillImage.color;
        _punchChargeSliderTargetFillColor = Color.red;
        _scoreText.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        PlayerTransform = this.gameObject.transform;
        GroundCheck();
        currentState?.Update();

        ChargePunch();

        WhileSpeedPowerup();

        Debug.Log(_playerScore);
    }

    void ChargePunch()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && currentState is not FightState)
        {
            _chargingUpPunch = true;
            _punchChargeValue = 0;
            _punchChargeSlider.value = 0;
            _punchChargeSlider.maxValue = _maxPunchCharge;
            _punchChargeSliderFillImage.color = _punchChargeSliderInitialFillColor;
            // _punchChargeSlider.gameObject.SetActive(true);
        }

        if (Input.GetKeyUp(KeyCode.Mouse0) && currentState is not FightState)
        {
            _chargingUpPunch = false;
            _punchChargeValue = 0;
            _punchChargeSlider.value = 0;
            // _punchChargeSlider.gameObject.SetActive(false);
            ChangeState(new FightState(this));
            _attackSource.AttackChargeStrength = _punchChargeValue >= _maxPunchCharge ? _chargedPunchPoints : _regularPunchPoints;
        }

        if (_chargingUpPunch)
        {
            if (_punchChargeValue > _maxPunchCharge)
            {
                _punchChargeSliderFillImage.color = _punchChargeSliderTargetFillColor;
                return;
            }

            _punchChargeValue += Time.deltaTime;
            _punchChargeSlider.value = _punchChargeValue;

            // Gradual Green To Red while Charging up
            // _punchChargeSliderFillImage.color = Color.Lerp(_punchChargeSliderInitialFillColor, _punchChargeSliderTargetFillColor, _punchChargeValue / _maxPunchCharge);
        }

        else
        {
            _punchChargeSliderFillImage.color = _punchChargeSliderInitialFillColor;
        }
    }

    void HandleAssignPointsToPlayer(int points)
    {
        _playerScore += points;
        _scoreText.text = _playerScore.ToString();
    }

    void HandleDeadZoneEntered()
    {
        KillPlayer();

    }

    public void KillPlayer()
    {
        _gameOverController.GameOver();
    }

    public void ChangeState(IState newState)
    {
        currentState?.Exit(); // Clean up previous state
        currentState = newState;
        currentState.Enter(); // Initialize new state
    }

    public bool GroundCheck()
    {
        RaycastHit hit;
        Vector3 raycastStart = new Vector3 (transform.position.x, transform.position.y + 0.1f, transform.position.z);
        if (Physics.Raycast(raycastStart, Vector3.down, out hit, 1f))
        {
            Debug.DrawRay(raycastStart, Vector3.down * 100f, Color.green, 0.02f, false);
        }

        if (hit.distance <= 0.2f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AsyncGroundCheck()
    {
        StartCoroutine(AwaitGrounded());
    }

    IEnumerator AwaitGrounded()
    {
        while (!GroundCheck())
        {
            yield return null;
        }
        Debug.Log("IsGroundedAgain");

        ChangeState(new IdleState(this));
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnergyDoseItem"))
        {
            Destroy(other.gameObject);
            _speedPowerUp = true;
            _elapsedSpeedPowerUpTime = 0;

        }
    }

    void WhileSpeedPowerup()
    {
        if (_speedPowerUp)
        {

            _elapsedSpeedPowerUpTime += Time.deltaTime;

            if (_elapsedSpeedPowerUpTime >= _speedPowerUpTime)
            {
                _speedPowerUp = false;
            }
        }
    }

    void OnDestroy()
    {
        IceCubeUnit.OnAssignPointsToPlayer -= HandleAssignPointsToPlayer;        
    }
}
