using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile1 : MonoBehaviour
{
    /*public float speed = 10f; // 初始速度
    public float angle = 45f; // 发射角度
    public float gravity = -9.81f; // 重力加速度


    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {

        // 将角度转换为弧度
        float radianAngle = angle * Mathf.Deg2Rad;

        // 根据初始速度和角度计算水平和垂直速度
        float horizontalSpeed = speed * Mathf.Cos(radianAngle);
        float verticalSpeed = speed * Mathf.Sin(radianAngle);

        float flightTime = (-2f * verticalSpeed) / gravity;



        rb.AddForce(new Vector3(horizontalSpeed, verticalSpeed, 0));

        
        Destroy(gameObject, flightTime + 10);
    }*/

    public float speed = 5.0f;       // 抛射物的速度
    public float gravity = -9.8f;     // 抛射物的重力加速度
    public float lifeTime = 5.0f;     // 抛射物的生命周期
    public float launchAngle = 45.0f; // 发射角度

    private Rigidbody rb;            // 抛射物的Rigidbody组件

    void Start()
    {
        rb = GetComponent<Rigidbody>();   // 获取抛射物的Rigidbody组件
        //rb.useGravity = false;             // 禁用重力

        // 将发射角度转换为发射方向向量，并乘以速度得到初始速度
        Vector3 launchDirection = Quaternion.Euler(0, 0, launchAngle) * Vector3.right;
        rb.AddForce(launchDirection * speed, ForceMode.Impulse);

        // 在生命周期结束后自动销毁抛射物
        Destroy(gameObject, lifeTime);
    }

}
