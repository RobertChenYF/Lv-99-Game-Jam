using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pearl : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<PlayerController>().pearlNumber++;
            SoundManager.Instance.PlayGetPearl();
            Destroy(gameObject);
        }
    }
}
