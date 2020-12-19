using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAI : MonoBehaviour
{
   
    public float viewRadius;
    [Range(0, 360)] public float viewAngle;

    public GameObject seekObject;
    GameObject turret;



    // Start is called before the first frame update
    void Start()
    {
        turret = GetComponent<PatrolBot>().turret;
        seekObject = GetComponent<PatrolBot>().playerObject;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += turret.transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
    public bool TargetIsVisible()
    {
        Vector3 dirToTarget = (seekObject.transform.position - turret.transform.position).normalized;
        float dstToTarget = Vector3.Distance(turret.transform.position, seekObject.transform.position);
        return (Vector3.Angle(turret.transform.forward, dirToTarget) < viewAngle / 2 && dstToTarget < viewRadius);
    }

    public bool Attack()
    {
        //Vector3 dirToTarget = (seekObject.transform.position - transform.position).normalized;
        float dstToTarget = Vector3.Distance(transform.position, seekObject.transform.position);
        return (dstToTarget < viewRadius / 3);
    }
    
}
