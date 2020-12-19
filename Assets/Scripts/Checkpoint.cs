using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Transform cp_loc;
    public float timeSinceCheckIn;

    // Start is called before the first frame update
    void Start()
    {
        cp_loc = gameObject.transform;
        timeSinceCheckIn = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceCheckIn += Time.deltaTime;
    }

    public Vector3 getPosition()
    {
        return cp_loc.position;
    }
    
    public void printPosition()
    {
        Debug.Log(getPosition().ToString());
    }
}
