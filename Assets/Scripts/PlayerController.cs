using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maxSpeed = 5f;

    public Vector2 direction;
    public float lerpRate = 0.2f;
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
    }
    void FixedUpdate()
    {
        GetInput();
        PlayerMove();
    }

    void GetInput()
    {
        float horizon = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        direction = new Vector2(horizon, vertical);
        direction.Normalize();
    }
    void PlayerMove()
    {
        if(direction == Vector2.zero)
        {
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, lerpRate);
            return;
        }
        rb.velocity = direction * maxSpeed;
    }
}
