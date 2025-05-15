using UnityEngine;

public class HurtBoxPositionConstraint : MonoBehaviour
{
    [SerializeField] GameObject _positionConstraintGameObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetPositionToGameObject();
    }

    void SetPositionToGameObject()
    {
        transform.position = _positionConstraintGameObject.transform.position;
        transform.rotation = _positionConstraintGameObject.transform.rotation;
    }
}
