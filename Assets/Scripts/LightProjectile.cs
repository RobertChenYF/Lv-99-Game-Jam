using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightProjectile : MonoBehaviour
{
    public Vector2 initialAim;
    public float speed = 5;
    public float gravity = 0.1f;

    private bool stop = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(stop == false){
            transform.Translate(initialAim * speed * Time.deltaTime);
            initialAim.y -= gravity * Time.deltaTime;

        }
    }

    private void OnCollisionEnter(Collision other) {

        //Debug.Log(other.gameObject.tag);
        if (other.gameObject.layer == LayerMask.NameToLayer("Rock")){
            Debug.Log("collision");
            stop = true;
        }
    }
}
