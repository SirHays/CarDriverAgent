using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float smoothTime;
    private Vector3 velocity = Vector3.zero;
    [SerializeField] private Transform lookat;
    private float lookAtSmoothTime =0f;
    

    
    void LateUpdate()
    {
        // Define a target position above and behind the target transform
        Vector3 targetPosition = target.TransformPoint(new Vector3(0, 4, -10));
        Vector3 lookAtTargetPosition = target.TransformPoint(new Vector3(0, 1, 0));
        // Smoothly move the camera towards that target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        lookat.transform.position = Vector3.SmoothDamp(lookat.transform.position, lookAtTargetPosition, ref velocity, lookAtSmoothTime);
        
    
        transform.LookAt(lookat,target.up);
        
        
    }
}
