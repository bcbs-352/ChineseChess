using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class ArmController : MonoBehaviour
{
    public GameObject followCamera;

    public Transform revoluteJointA, revoluteJointB, liftPair;
    public Slider[] sliders;

    public bool isChecking = false;

    void Start()
    {
        if (followCamera == null)
            followCamera = transform.GetChild(transform.childCount - 1).gameObject;
    }


    void Update()
    {
        if (isChecking)
            CameraMoving();
    }

    void CameraMoving()
    {

    }
}
