using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EndGameController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject endGameCam;
    [SerializeField] private PlayerController playerController;
    [SerializeField]private Volume v;
    [SerializeField] private GameObject bubbleEmitter;


    private ColorAdjustments ca;

    private Color FilterColor;
    [SerializeField]private Color FilterColorNew;
    // Start is called before the first frame update
    void Start()
    {
        v.profile.TryGet(out ca);
    }

    // Update is called once per frame
    void Update()
    {
        if(bubbleEmitter.activeSelf == true && Vector3.Distance(this.transform.position, player.transform.position) < 8f){
            
            Invoke("MyFunction", 3);
            playerController.enabled = false;
            StartCoroutine(colorLerp());
            bubbleEmitter.SetActive(false);
            StartCoroutine(playerMovement());
            StartCoroutine(playerMovementXto0());

        }
    }

    void MyFunction(){
        endGameCam.SetActive(true);
    }

    IEnumerator colorLerp(){
        while(true){
            FilterColor = ca.colorFilter.value;
            FilterColor = Color.Lerp(FilterColor, FilterColorNew, 0.05f);
            ca.colorFilter.value = FilterColor;
            yield return new WaitForSeconds(0.1f);
        }
    }


    IEnumerator playerMovement(){
        while(true){
            player.transform.position -= new Vector3 (0.0f, 0.02f, 0.0f);
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator playerMovementXto0(){
        while(Mathf.Abs(player.transform.position.x) > 0.02f ){
            player.transform.position -= new Vector3 (0.02f * Mathf.Sign(player.transform.position.x), 0.0f, 0.0f);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
