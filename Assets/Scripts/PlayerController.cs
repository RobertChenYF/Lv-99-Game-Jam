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
    private float horizon;
    private float vertical;


    //these parameter controls the scene depends on player depth

    [SerializeField] private Light sunLight;
    [SerializeField] private Color cameraHighColor;
    [SerializeField] private Color cameraLowColor;
    [SerializeField] private Camera mainCamera;

    [SerializeField] private Color CAHighColor;
    [SerializeField] private Color CALowColor;

    [SerializeField] private LineRenderer softTube;

    public int TubeCount = 1;




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


        RenderSettings.fogColor = Color.Lerp(cameraHighColor, cameraLowColor,Mathf.Abs(transform.position.y/300.0f));
        mainCamera.backgroundColor = Color.Lerp(cameraHighColor, cameraLowColor,Mathf.Abs(transform.position.y/300.0f));
        sunLight.intensity = Mathf.Abs(transform.position.y/300.0f) * -1f + 1.0f;
        ca.colorFilter.value = Color.Lerp(CAHighColor, CALowColor,Mathf.Abs(transform.position.y/300.0f));
        
        if(transform.position.y > -100 * TubeCount && Mathf.Abs(transform.position.x) < 3){
            oxygen = 100;
            softTube.positionCount = 5;
            maxSpeed = 6;
        }
        else{
            softTube.positionCount = 0;
            maxSpeed = 3;
        }

        Vector3 midPoint1 = new Vector3(transform.position.x/2.0f,transform.position.y-1,0);
        Vector3 midPoint2 = new Vector3(midPoint1.x /2.0f,transform.position.y-0.6f,0);
        Vector3 midPoint3 = new Vector3(midPoint1.x *1.5f,transform.position.y-0.8f,0);
        softTube.SetPositions(new Vector3[] {transform.position,midPoint3,midPoint1,midPoint2,new Vector3(0,transform.position.y+2,0)});


    }
    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    void GetInput()
    {
        horizon = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
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
        playerAnima.SetFloat("X", horizon * currentSpeed / maxSpeed);
        playerAnima.SetFloat("Y", vertical);
    }
}
