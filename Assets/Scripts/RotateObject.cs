using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private float rotatinoSpeed;


    public void OnMouseDrag()
    {
        if (StateManager.currentState == StateManager.productState)
        {
            
            
            float rotX = Input.GetAxis("Mouse X") * rotatinoSpeed * Mathf.Rad2Deg;
            float rotY = Input.GetAxis("Mouse Y") * rotatinoSpeed * Mathf.Rad2Deg;
            
            gameObject.transform.RotateAround(-Vector3.up, rotX);
            
        }
    }
    
    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
    
}
