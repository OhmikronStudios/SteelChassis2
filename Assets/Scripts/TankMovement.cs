using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : MonoBehaviour
{
    //General Parameters
    Rigidbody rb;
    [SerializeField] Transform body;
    [SerializeField] Transform fwd;
    [SerializeField] GameObject orbCam;

    
    //Movement Parameters
    Vector2 playerInput;
    Vector3 currentPos;
    Vector3 currentFacing;
    [SerializeField] float maxSpeed = 10.0f;
    [SerializeField] float rotSpeed = 10.0f;

    //Firing Parameters
    [SerializeField] GameObject Projectile;
    [SerializeField] GameObject ProjectileSpawn;
    [SerializeField] Transform Cannon;
    [SerializeField] GameObject Turret;

    bool canFire = true;


    //Projectile Parameters
    [SerializeField] float projSpeed = 1.0f;


    //Gameplay Parameters
    public int health = 1000;





    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        orbCam = FindObjectOfType<OrbitCamera>().gameObject;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && canFire)
        {
            Fire();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        InputMotion(); 
    }

    void InputMotion()
    {
        //Gather starting positional data
        currentPos = rb.position;
        currentFacing = rb.rotation.eulerAngles;
        playerInput.x = Input.GetAxisRaw("Horizontal");
        playerInput.y = Input.GetAxisRaw("Vertical");
       
        //First we will apply any rotation to the tank
        Vector3 rotation = new Vector3(currentFacing.x, (currentFacing.y + (rotSpeed * playerInput.x * Time.fixedDeltaTime)), currentFacing.z);
        Quaternion toRotation = Quaternion.Euler(rotation);
        rb.MoveRotation(toRotation);

        //Secondly, we will apply forward motion, related to the tank's new facing
        Vector3 VectorFacing = body.transform.forward * maxSpeed * playerInput.y * Time.fixedDeltaTime;
        rb.MovePosition(currentPos + VectorFacing);

        //Lastly, we want the cannon's rotation to mimic that of the camera
        
        Turret.transform.forward = orbCam.transform.forward;
        Turret.transform.Rotate(-30, 0, 0);
    }

    void Fire()
    {
        Debug.Log("Fire");
        Vector3 fireDirection = ProjectileSpawn.transform.forward;
        GameObject temp = Instantiate(Projectile, ProjectileSpawn.transform.position, Quaternion.identity);
            
        temp.GetComponent<Projectile>().Launch(fireDirection, projSpeed);
        canFire = false;
        StartCoroutine(Reload());
    }
    public void ModifyHealth(int v)
    {
        health += v;
        if (health <= 0)
        {
            
        }

    }
    IEnumerator Reload()
    {
        yield return new WaitForSeconds(5.0f);
        canFire = true;
    }

}
