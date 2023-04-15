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

    [SerializeField]private Volume v;
    private Vignette vg;
    private ColorAdjustments ca;
    [SerializeField] private ParticleSystem bubble;

    private bool RunOutOfOxygen = false;

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
