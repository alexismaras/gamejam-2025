using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class AttackSource : MonoBehaviour
{
    [SerializeField] private GameObject _characterLeftHand;
    [SerializeField] private GameObject _characterRightHand;
    [SerializeField] private GameObject _characterLeftFoot;
    [SerializeField] private GameObject _characterRightFoot;
    [SerializeField] private GameObject _characterHead;
    public static event Action<Vector3, GameObject> OnAttack;
    private Vector3 _frameBeforeAttackVector;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void HandleBeforeAttackFrame(int attackType)
    {
        GameObject attackBodyPart = AttackTypeGameObject(attackType);
        _frameBeforeAttackVector = attackBodyPart.transform.position;
    }

    void HandleAttackFrame(int attackType)
    {
        GameObject attackBodyPart = AttackTypeGameObject(attackType);
        Vector3 attackDirection = attackBodyPart.transform.position - _frameBeforeAttackVector;
        Vector3 attackVector = attackDirection.normalized;
        OnAttack?.Invoke(attackVector, attackBodyPart);
    }

    GameObject AttackTypeGameObject(int attackType)
    {
        return attackType switch
        {
            0 => _characterLeftHand,
            1 => _characterRightHand,
            2 => _characterLeftFoot,
            3 => _characterRightFoot,
            4 => _characterHead,
            _ => null 
        };
    }
}
