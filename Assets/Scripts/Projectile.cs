using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public Vector3 target;   //要到达的目标
    public float horizontalDirection = 1;  //水平方向的方向，1为向右，-1为向左
    public float speed = 5;    //速度
    private float distanceToTarget;   //两者之间的距离
    private bool move = true;

    void Start()
    {
        target = new Vector3(transform.position.x + (5*horizontalDirection), transform.position.y-1, transform.position.z);
        target.z = transform.position.z;

        //计算两者之间的距离
        distanceToTarget = Vector3.Distance(this.transform.position, target);
        StartCoroutine(StartShoot());
        Destroy(gameObject, 20);
    }
    
    IEnumerator StartShoot()
    {
 
        while (move)
        {
            Vector3 targetPos = target;
 
            //让始终它朝着目标
            this.transform.LookAt(targetPos);
 
            //计算弧线中的夹角
            float angle = Mathf.Min(1, Vector3.Distance(this.transform.position, targetPos) / distanceToTarget) * 45;
            this.transform.rotation = this.transform.rotation * Quaternion.Euler(Mathf.Clamp(-angle, -42, 42), 0, 0);
            float currentDist = Vector3.Distance(this.transform.position, target);
            if (currentDist < 0.01f)
                move = false;
            this.transform.Translate(Vector3.forward * Mathf.Min(speed * Time.deltaTime, currentDist));
            yield return null;
        }
    }
}
