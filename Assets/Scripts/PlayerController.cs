using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector2 direction;
    public float oxygen = 100f;
    public int pearlNumber = 0;

    
    private Vignette vg;
    private ColorAdjustments ca;
    private bool RunOutOfOxygen = false;

    [SerializeField]private Volume v;
    [SerializeField] private ParticleSystem bubble;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float lerpRate = 0.1f;
    [SerializeField] private float oxygenRate = 0.2f;
    [SerializeField] private float currentSpeed;
    [SerializeField] private Animator playerAnima;

    private CharacterController characterController;
    private bool isAlive = true;
    private Vector2 lastDirection;

    void Start()
    {
        v.profile.TryGet(out vg);
        v.profile.TryGet(out ca);
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if(oxygen <= 0)
        {
            if(RunOutOfOxygen == false)
            {
                RunOutOfOxygen = true;
                //play animation
                bubble.Emit(200);

            var em = bubble.emission;
            em.enabled = false;
            
                //play sound
            }
            //change vignette
            maxSpeed = 2 + (oxygen/30.0f);
            vg.intensity.value = (0.0f - oxygen)/30.0f * 0.4f + 0.1f;
            ca.saturation.value = oxygen * 2.8f;

        }
        if(oxygen <=  -30){
            
            isAlive = false;
            return;
        }

        if(oxygen >0 ){
            if(RunOutOfOxygen == true)
            {
                
            RunOutOfOxygen = false;

            var em = bubble.emission;
            em.enabled = true;
                        maxSpeed = 2;
            vg.intensity.value =  0.1f;
            ca.saturation.value = 0;
            }
        }
        GetInput();
        PlayerMove();
        PlayerAnimation();
        oxygen -= Time.deltaTime * oxygenRate;
    }
    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
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
            characterController.Move(lastDirection * currentSpeed * Time.deltaTime);
        }
        else
        {
            if(lastDirection != Vector2.zero)
            {
                float angle = Vector2.Angle(lastDirection, direction);
                if(angle < 90)
                {
                    //Quaternion targetRotation = Quaternion.Euler(0, 0, -angle);
                    //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime);
                    //transform.Rotate(0, 0, -angle);
                    //Vector3.RotateTowards(transform.right, new Vector3(direction.x, direction.y, 0), 1.0f * Time.deltaTime, 0.0f);
                    
                    transform.rotation = Quaternion.FromToRotation(transform.right, direction);
                }
            }
            lastDirection = direction;
            characterController.Move(lastDirection * maxSpeed * Time.deltaTime);
            currentSpeed = maxSpeed;
        }
    }

    void PlayerAnimation()
    {
        if(currentSpeed <= 0.01f)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            playerAnima.SetBool("isMoving", false);
        }
        else
        {
            playerAnima.SetBool("isMoving", true);
        }
    }
}
