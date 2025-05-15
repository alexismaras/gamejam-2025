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
    public static event Action<Vector3, GameObject> OnAttackStart;
    public static event Action OnAttackEnd;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Gets Invoked by Attack Animation
    // attackType is the bodypart that makes the attack movement
    // Sends out static event to all AttackReceivers

    void HandleStartAttackFrame(int attackType)
    {
        GameObject attackBodyPart = AttackTypeGameObject(attackType);
        Vector3 attackStartVector = attackBodyPart.transform.position;
        
        OnAttackStart?.Invoke(attackStartVector, attackBodyPart);
    }

    // Gets Invoked at end of Attack Animation
    // Sends out static event to all AttackReceivers
    void HandleEndAttackFrame(int attackType)
    {
        OnAttackEnd?.Invoke();
    }   
    
    // here, attackType is mapped to the corresponding child gameObject
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
