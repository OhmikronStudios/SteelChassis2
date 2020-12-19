using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBot : MonoBehaviour, BotManager.NotifySink
{
    //Overseer & State Machine links
    public BotManager bm;
    public GameObject turret;
    public GameObject tempTarget;

    //AI Movement
    public AIHelper.MovementBehaviors activeMovementBehavior = AIHelper.MovementBehaviors.None;
    public GameObject targetObject = null;
    public GameObject playerObject;
    public float maxSpeed = 3.0f;
    [SerializeField] float checkRange = 5f;

    //Firing Parameters
    [SerializeField] GameObject Projectile;
    [SerializeField] GameObject ProjectileSpawn;
    [SerializeField] Transform Cannon;
    [SerializeField] GameObject Turret;
    public bool canFire = true;   
    [SerializeField] float projSpeed = 1.0f;


    int health;

    // Start is called before the first frame update
    void Start()
    {
        bm = FindObjectOfType<BotManager>();
        playerObject = FindObjectOfType<TankMovement>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        AIHelper.InputParameters inputData = new AIHelper.InputParameters(gameObject.transform, targetObject.transform, Time.deltaTime, maxSpeed);
        AIHelper.MovementResult movementResult = new AIHelper.MovementResult();

        switch (activeMovementBehavior)
        {
            case AIHelper.MovementBehaviors.FleeKinematic:
                AIHelper.FleeKinematic(inputData, ref movementResult);
                break;
            case AIHelper.MovementBehaviors.SeekKinematic:
                AIHelper.SeekKinematic(inputData, ref movementResult);
                break;
            case AIHelper.MovementBehaviors.WanderKinematic:
                AIHelper.WanderKinematic(inputData, ref movementResult);
                break;
            default:
                //AIHelpers.SeekKinematic(inputData, ref movementResult);
                movementResult.newPosition = transform.position;
                break;
        }
        
        gameObject.transform.position = movementResult.newPosition;
        gameObject.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        
        transform.LookAt(targetObject.transform.position);

        
    }

    //BotManager Functions
    void BotManager.NotifySink.OnEvent()
    {     
        
    }

    public void ModifyHealth(int v)
    {
        health += v;
        if(health <= 0)
        {
            Die();
        }
        
        
        gameObject.GetComponent<Animator>().SetBool("Alert", true);
    }

    void Die()
    {
        Debug.Log(name + " died");
        gameObject.SetActive(false);
    }

    public void Revive(Transform spawnPoint)
    {
        gameObject.SetActive(true);
        ;
        gameObject.transform.position = new Vector3(spawnPoint.position.x + Random.Range(-5, 5), spawnPoint.position.y, spawnPoint.position.z + Random.Range(-5, 5));
        gameObject.GetComponent<Animator>().SetBool("Alert", false);
        gameObject.GetComponent<Animator>().SetBool("Hostile", false);
        
        health = 100;
    }

    public void CheckIn()
    {
        if(targetObject.tag == "Checkpoint")
        {
            targetObject.GetComponent<Checkpoint>().timeSinceCheckIn = 0.0f;
        }
        targetObject = bm.AssignCheckpoint();
        Debug.Log(gameObject.name + " is in range of their target, new target: " + targetObject.name);
    }

    //AI Movement Patterns
    public void ActivateWander()
    {
        activeMovementBehavior = AIHelper.MovementBehaviors.WanderKinematic;
    }

    public void ActivateSeek()
    {
        
        activeMovementBehavior = AIHelper.MovementBehaviors.SeekKinematic;
    }

    public void ActivateLeave()
    {
        activeMovementBehavior = AIHelper.MovementBehaviors.FleeKinematic;
    }

    public void Fire()
    {
        Debug.Log("Fire");
        Vector3 fireDirection = ProjectileSpawn.transform.forward;
        GameObject temp = Instantiate(Projectile, ProjectileSpawn.transform.position, Quaternion.identity);

        temp.GetComponent<Projectile>().Launch(fireDirection, projSpeed);
        canFire = false;
        StartCoroutine(Reload());
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(5.0f);
        canFire = true;
    }


}
