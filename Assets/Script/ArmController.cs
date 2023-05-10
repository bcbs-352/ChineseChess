using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class ArmController : MonoBehaviour
{
    public GameObject followCamera;
    float rotX = 0f, rotY = 30f * Mathf.PI / 180, distance = 30f;
    public float rotSpeed = 0.25f, zoomSpeed = 2f, liftSpeed = 0.5f;

    public Transform revoluteJointA, revoluteJointB, liftPair;
    public Transform zoomTarget, endPoint;
    public Slider[] sliders;

    public bool isChecking = false;

    const float L1 = 0.21f, L2 = 0.2f, s = 0.005f;

    void Start()
    {
        if (followCamera == null)
            followCamera = transform.GetChild(transform.childCount - 1).gameObject;
        //ArmMoving(2 / 15, 0.1f, 0);
    }


    void Update()
    {
        //Debug.Log(endPoint.localPosition.z + " " + endPoint.localPosition.x);
        ArmMoving(endPoint.localPosition.z, endPoint.localPosition.x, -endPoint.localPosition.y);

        if (isChecking)
            CameraMoving();
    }

    void ArmMoving(float x, float y, float z)
    {
        float theta1, theta2, theta3;

        float alpha = Mathf.Atan2(y, x);
        float cosTheta2 = (x * x + y * y - L1 * L1 - L2 * L2) / (2 * L1 * L2);
        float sinTheta2 = Mathf.Sqrt(1 - cosTheta2 * cosTheta2);

        theta1 = alpha - Mathf.Atan2(L2 * sinTheta2, L1 + L2 * cosTheta2);
        theta2 = Mathf.Atan2(sinTheta2, cosTheta2);
        theta3 = 2 * Mathf.PI / s;
        if (float.IsNaN(theta1) || float.IsNaN(theta2) || float.IsNaN(theta3) || float.IsInfinity(theta1) || float.IsInfinity(theta2) || float.IsInfinity(theta3))
            return;

        revoluteJointA.localRotation = Quaternion.Euler(0, theta1 * Mathf.Rad2Deg, 180);
        revoluteJointB.localRotation = Quaternion.Euler(0, theta2 * Mathf.Rad2Deg, 0);
        //liftPair.localPosition = new Vector3(0, z, 0);
        //Debug.Log(theta1 * Mathf.Rad2Deg + " " + theta2 * Mathf.Rad2Deg);

        sliders[0].value = theta1 * Mathf.Rad2Deg;
        sliders[1].value = theta2 * Mathf.Rad2Deg;
        sliders[2].value = z;
    }

    void CameraMoving()
    {
        if (followCamera == null || !followCamera.activeSelf)
            return;
        Debug.Log("ÒÆ¶¯Ïà»ú");
        Vector3 targetPos = zoomTarget.position;
        float xyDistance = distance * Mathf.Cos(rotY), zHeight = distance * Mathf.Sin(rotY);
        Vector3 camPos = new Vector3(targetPos.x + xyDistance * Mathf.Cos(rotX), targetPos.y + zHeight, targetPos.z + xyDistance * Mathf.Sin(rotX));
        followCamera.transform.position = camPos;

        if (Input.GetMouseButton(1))
        {
            rotX -= Input.GetAxis("Mouse X") * rotSpeed;
            rotY -= Input.GetAxis("Mouse Y") * rotSpeed;
        }
        if (Input.GetMouseButton(2))
        {
            zoomTarget.position += new Vector3(0, Input.GetAxis("Mouse Y") * liftSpeed, 0);
        }
        distance += Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
    }

    public void OnSliderValurChanged()
    {
        revoluteJointA.localRotation = Quaternion.Euler(0, sliders[0].value, 180);
        revoluteJointB.localRotation = Quaternion.Euler(0, sliders[1].value, 0);
        liftPair.localPosition = new Vector3(0, sliders[2].value, 0);
    }

    public void SwitchView()
    {
        followCamera.SetActive(!isChecking);
        isChecking = !isChecking;
    }

    public void MoveToCoor()
    {
        int x = (int)sliders[3].value, y = (int)sliders[4].value;
        ArmMoving(0.133333f - x * 0.1f / 3, 0.066666f + y * 0.1f / 3, 0);
    }

    public void SetEndPointPos(Vector3 pos)
    {
        endPoint.localPosition = pos;
        ArmMoving(endPoint.localPosition.z, endPoint.localPosition.x, -endPoint.localPosition.y);
    }
}
