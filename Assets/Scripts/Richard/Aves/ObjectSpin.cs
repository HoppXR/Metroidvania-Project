using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ObjectSpin : MonoBehaviour
{

    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private Transform boss;
    
    void Update()
    {
        transform.RotateAround(boss.position, Vector3.forward, rotationSpeed + Time.deltaTime);
    }
}
