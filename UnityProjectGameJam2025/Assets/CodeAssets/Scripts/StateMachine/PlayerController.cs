using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class PlayerController : MonoBehaviour
{
    private IState currentState;
    public Rigidbody PlayerRigidbody { get; private set; }
    public Transform PlayerTransform { get; private set; }
    public Animator PlayerAnimator { get; private set; }
    public Collider PlayerCollider { get; private set; }

    public bool IsGrounded { get; private set; }


    [SerializeField] private Camera _playerCamera; // Assign in Inspector
    public Camera PlayerCamera => _playerCamera;

    [SerializeField] private float _jumpForce;
    public float JumpForce => _jumpForce;
    [SerializeField] private float _jumpMaxHeight;
    public float JumpMaxHeight => _jumpMaxHeight;
    [SerializeField] private float _jumpMaxAirTime;
    public float JumpMaxAirTime => _jumpMaxAirTime;

    private bool _chargingUpPunch;
    private float _punchChargeValue;

    [SerializeField] float _maxPunchCharge;
    [SerializeField] private Slider _punchChargeSlider;
    [SerializeField] private Image _punchChargeSliderFillImage;
    private Color _punchChargeSliderInitialFillColor;
    private Color _punchChargeSliderTargetFillColor;

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

    }
    void Start()
    {
        ChangeState(new IdleState(this));
        _punchChargeSliderInitialFillColor = _punchChargeSliderFillImage.color;
        _punchChargeSliderTargetFillColor = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerTransform = this.gameObject.transform;
        GroundCheck();
        currentState?.Update();

        ChargePunch();
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
            _punchChargeSlider.gameObject.SetActive(true);
        }

        if (Input.GetKeyUp(KeyCode.Mouse0) && currentState is not FightState)
        {
            _chargingUpPunch = false;
            _punchChargeSlider.gameObject.SetActive(false);
            ChangeState(new FightState(this));
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
    }

    public void ChangeState(IState newState)
    {
        currentState?.Exit(); // Clean up previous state
        currentState = newState;
        currentState.Enter(); // Initialize new state
    }

    void GroundCheck()
    {
        RaycastHit hit;
        Vector3 raycastStart = transform.position;
        if (Physics.Raycast(raycastStart, Vector3.down, out hit, 100f))
        {
            Debug.DrawRay(raycastStart, Vector3.down * 100f, Color.green, 0.02f, false);
        }

        if (hit.distance <= 0.01f)
        {
            Debug.Log("yyy");
            IsGrounded = true;
        }
        else
        {
            IsGrounded = false;
        }
    }
}
