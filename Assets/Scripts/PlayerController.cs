using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector2 direction;
    public float oxygen = 100f;
    public int pearlNumber = 0;

   [SerializeField]
    private float maxSpeed = 5f;
    [SerializeField]
    private float lerpRate = 0.1f;
    [SerializeField]
    private float oxygenRate = 0.2f;
    [SerializeField]
    private float currentSpeed;

    private CharacterController characterController;
    private bool isAlive = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if(oxygen <= 0)
        {
            isAlive = false;
            return;
        }
        GetInput();
        PlayerMove();
        oxygen -= Time.deltaTime * oxygenRate;
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
            currentSpeed = Mathf.Lerp(currentSpeed, 0, lerpRate);
            characterController.Move(transform.right * currentSpeed * Time.deltaTime);
            return;
        }
        characterController.Move(direction * maxSpeed * Time.deltaTime);
        currentSpeed = maxSpeed;
    }
}
