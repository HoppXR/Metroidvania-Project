using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private GameObject skip;
    
    void Start()
    {
        
        EventSystem.current.SetSelectedGameObject(skip);
    }
}
