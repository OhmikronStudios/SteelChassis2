using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float timeCount = 0.0f;
    //public float damage = 50.0f;
    public float angle;
    public float velocity;

    public float xVel;
    public float yVel;
    public float zVel;

    //float initialXpos;
    float Xpos;
    float Ypos;
    float Zpos;
    
    
    // Start is called before the first frame update
    void Start()
    {
        timeCount = 0.0f;
        Xpos = transform.position.x;
        Ypos = transform.position.y;
        Zpos = transform.position.z;
        Destroy(gameObject, 8.0f);
    }

    // Update is called once per frame
    void Update()
    {
        timeCount += Time.deltaTime/4;        
        Vector3 moveVector = MoveProjectile();
        transform.LookAt(moveVector);
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, 1.0f))
        {
            if (hit.collider.gameObject.tag == "Enemy")
            {
                Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.yellow);
                int damage = 50;
                float damageFactor = Mathf.Cos(Vector3.Angle(transform.forward, hit.normal));
                if(damageFactor < 0)
                { damage *= -1; }
                hit.collider.gameObject.GetComponent<PatrolBot>().ModifyHealth(Mathf.RoundToInt(-damage*damageFactor));
                Debug.Log(Mathf.RoundToInt(damage * damageFactor) + " Damage done to: " + hit.collider.gameObject.name); 
            }
            else if (hit.collider.gameObject.tag == "Player")
            {
                int damage = 50;
                float damageFactor = Mathf.Cos(Vector3.Angle(transform.forward, hit.normal));
                if (damageFactor < 0)
                { damage *= -1; }
                hit.collider.gameObject.GetComponent<TankMovement>().ModifyHealth(Mathf.RoundToInt(-damage * damageFactor));
            }
            else if (hit.collider.gameObject.tag == "Ground" || hit.collider.gameObject.tag == "Wall")
            {
                Destroy(gameObject);
            }
        }
        
        transform.position = moveVector;
    }

    public void Launch(Vector3 fireDirection, float projSpeed)
    {
        xVel = fireDirection.x * projSpeed;
        yVel = fireDirection.y * projSpeed;
        zVel = fireDirection.z * projSpeed;
    }

    Vector3 MoveProjectile()
    {
        return new Vector3(CalculateX(), CalculateY(), CalculateZ());
    }
    
    float CalculateX()
    {
        return Xpos += + xVel * timeCount;
    }

    float CalculateY()
    {
        return Ypos += (yVel * timeCount + (-9.8f * timeCount * timeCount / 2));
    }
    
    float CalculateZ()
    {
        return Zpos += + zVel * timeCount;
    }
}
