using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Vector2 direction;
    public float oxygen = 100f;
    public int pearlNumber = 0;
    public bool isEnd = false;

    public float maxOxygen = 100f;
    public int pipeLevel = 1;

    private float horizontal;
    private float vertical;
    private Vector2 moveDirection = Vector2.zero;
    private AudioSource breathAudio;
    [SerializeField]private AudioSource BGM;

    [SerializeField] private AudioClip oneBreathe;
    [SerializeField] private AudioClip longBreath;
    [SerializeField] private AudioClip diveBGM;
    [SerializeField] private AudioClip storeBGM;



    [SerializeField] private ParticleSystem bubble;
    [SerializeField] private GameObject store;
    [SerializeField] private GameObject birthP;
    [SerializeField] private Image traMask;


    
    private Vignette vg;
    private ColorAdjustments ca;

    [SerializeField] private Transform head;

    private bool RunOutOfOxygen = false;

    [SerializeField]private Volume v;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float lerpRate = 0.1f;
    [SerializeField] private float oxygenRate = 0.2f;
    [SerializeField] private float currentSpeed;
    [SerializeField] private Animator playerAnima;

    private CharacterController characterController;
    private bool isAlive = true;
    private Vector2 lastDirection;//最后一次不为0的 输入的方向


    //these parameter controls the scene depends on player depth

    [SerializeField] private Light sunLight;
    [SerializeField] private Color cameraHighColor;
    [SerializeField] private Color cameraLowColor;
    [SerializeField] private Camera mainCamera;

    [SerializeField] private Color CAHighColor;
    [SerializeField] private Color CALowColor;
    [SerializeField] private LineRenderer softTube;

   

    void Start()
    {
        v.profile.TryGet(out vg);
        v.profile.TryGet(out ca);
        characterController = GetComponent<CharacterController>();
        breathAudio = GetComponent<AudioSource>();
        birthP = GameObject.Find("BirthPoint");
        transform.position = birthP.transform.position;
        oxygen = maxOxygen;
    }

    void Update()
    {
        if(isEnd) return;

        if(transform.position.y > 7)
        {
            GoBoat();
            traMask.enabled = true;
            traMask.color = Color.white;
            StopCoroutine("FadeOut");
            StartCoroutine("FadeOut", Color.white);
        }
        if(Input.GetKeyDown(KeyCode.R)){
            transform.position = birthP.transform.position;
            return;
        }
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
                breathAudio.PlayOneShot(oneBreathe);
            }
            //change vignette
            maxSpeed = 2 + (oxygen/30.0f);
            vg.intensity.value = (0.0f - oxygen)/30.0f * 0.4f + 0.1f;
            ca.saturation.value = oxygen * 2.8f;

        }
        if(oxygen <=  -30){
            
            isAlive = false;
            PlayerDead();
            return;
        }

        if(oxygen >0 ){
            if(RunOutOfOxygen == true)
            {
                
            RunOutOfOxygen = false;
            //breathAudio.PlayOneShot(longBreath);
            var em = bubble.emission;
            em.enabled = true;
                        maxSpeed = 2;
            vg.intensity.value =  0.1f;
            ca.saturation.value = 0;
            }
            if(softTube.positionCount == 0){
                vg.intensity.value = Mathf.Sin(Time.time*2.0f) * 0.1f + 0.1f;
            }
            
        }
        GetInput();
        
        if(isAlive)
        {
            PlayerMove();
            PlayerAnimation();
        }
        oxygen -= Time.deltaTime * oxygenRate;


        RenderSettings.fogColor = Color.Lerp(cameraHighColor, cameraLowColor,Mathf.Abs(transform.position.y/300.0f));
        mainCamera.backgroundColor = Color.Lerp(cameraHighColor, cameraLowColor,Mathf.Abs(transform.position.y/300.0f));
        sunLight.intensity = Mathf.Abs(transform.position.y/300.0f) * -1f + 1.0f;
        ca.colorFilter.value = Color.Lerp(CAHighColor, CALowColor,Mathf.Abs(transform.position.y/300.0f));
        
        if(transform.position.y > -100 * pipeLevel + 8 && Mathf.Abs(transform.position.x) < 3){
            oxygen = maxOxygen;
            softTube.positionCount = 8;
            maxSpeed = 6;
        }
        else{
            softTube.positionCount = 0;
            maxSpeed = 3;
        }

        Vector3 midPoint1 = new Vector3(head.position.x/2.0f,head.position.y-1,0);
        Vector3 midPoint2 = new Vector3(midPoint1.x /2.0f,head.position.y-0.6f,0.3f);
        Vector3 midPoint3 = new Vector3(midPoint1.x *1.5f,head.position.y-0.8f,0);
        Vector3 midPoint4 = (head.position + midPoint3)/2 - new Vector3(0,0.2f,0);
        Vector3 midPoint5 = (midPoint1 + midPoint3)/2 - new Vector3(0,0.1f,0);
        Vector3 midPoint6 = (midPoint1 + midPoint2)/2 - new Vector3(0,0.1f,0);

        softTube.SetPositions(new Vector3[] {head.position,midPoint4,midPoint3,midPoint5,midPoint1,midPoint6,midPoint2,new Vector3(Mathf.Sign(head.position.x)*0.01f, head.position.y+0.8f, 1)});


    }
    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    void GetInput()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        direction = new Vector2(horizontal, vertical);
        moveDirection = Vector2.Lerp(moveDirection, direction, 0.015f);
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
            PlayerRotate();
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
            //PlayerRotate();
            playerAnima.SetBool("isMoving", true);
        }
        playerAnima.SetFloat("X", moveDirection.x * currentSpeed / maxSpeed);
        playerAnima.SetFloat("Y", moveDirection.y * currentSpeed / maxSpeed);
    }

    void PlayerRotate()
    {
        if(horizontal > 0.3)
        {   
            transform.localScale = new Vector3(-0.1f, transform.localScale.y, transform.localScale.z);
        }
        else if(horizontal < -0.3)
        {
            transform.localScale = new Vector3(0.1f, transform.localScale.y, transform.localScale.z);
        }
    }
    
    void PlayerDead()
    {
        GoBoat();

        //死亡转场
        StopCoroutine("FadeOut");
        traMask.color = Color.black;
        StartCoroutine("FadeOut", Color.black);

        //play sound
    }

    private void GoBoat()
    {
        GlobalData.Instance.pearls = pearlNumber;
        isAlive = false;
        mainCamera.gameObject.SetActive(false);
        store.SetActive(true);
        transform.position = birthP.transform.position;
        oxygen = maxOxygen;
        RenderSettings.fog = false;
        breathAudio.Pause();
        BGM.clip = storeBGM;
        BGM.Play();
    }

    public void Dive()
    {
        //下潜转场
        StopCoroutine("FadeOut");
        traMask.color = Color.white;
        StartCoroutine("FadeOut", Color.white);

        breathAudio.PlayDelayed(2);
        BGM.clip = diveBGM;
        BGM.Play();
        isAlive = true;
        mainCamera.gameObject.SetActive(true);
        RenderSettings.fog = true;
        store.SetActive(false);
        //play sound
    }

    IEnumerator FadeOut(Color initalColor)
    {
        float t = 0;
        if(initalColor == Color.white)
            yield return new WaitForSeconds(0.3f);
        else
            yield return new WaitForSeconds(1f);
        while(t < 2)
        {
            t += Time.deltaTime;
            traMask.color = Color.Lerp(initalColor, Color.clear, t/2);
            yield return null;
        }
    }
    public void EndGame()
    {
        playerAnima.SetBool("isEnd", true);
    }
}
