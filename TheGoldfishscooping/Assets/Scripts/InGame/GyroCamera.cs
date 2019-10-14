using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroCamera : MonoBehaviour
{
    private readonly Quaternion _BASE_ROTATION = Quaternion.Euler(90, 0, 0);

    private Transform m_Transform;
    void Start()
    {
        Input.gyro.enabled = true;
        m_Transform = this.transform;
    }

    void Update()
    {
        // ジャイロの値を獲得する
        Quaternion gyro = Input.gyro.attitude;
 
        // 自信の回転をジャイロを元に調整して設定する
        //this.transform.localRotation =  _BASE_ROTATION * (new Quaternion(-gyro.x, -gyro.y, gyro.z, gyro.w));

        //Debug.LogError("attitude:" + gyro);
        // Debug.LogError("gravity:" + Input.gyro.gravity);
        // Debug.LogError("userAcceleration:" + Input.gyro.userAcceleration);
        // Vector3 calcAngle = Quaternion.Euler(m_Transform.localEulerAngles)* Input.gyro.userAcceleration;
        // Debug.LogError("calcAngle:" + calcAngle);
    }
}
