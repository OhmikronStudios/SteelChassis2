using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[ExecuteInEditMode]
public class WorldSnap : MonoBehaviour
{
    [SerializeField] TextMesh tileText = null;
    [SerializeField] bool showText = true;

    [SerializeField] [Range(1f, 100f)] int snapValue = 10;
    // Update is called once per frame
    void Update()
    {
        Vector3 snapPos;
        snapPos.x = Mathf.RoundToInt(transform.position.x / snapValue) * snapValue;
        snapPos.z = Mathf.RoundToInt(transform.position.z / snapValue) * snapValue;

        transform.position = new Vector3(snapPos.x, 0f, snapPos.z);

        if (showText) { tileText.text = snapPos.x / 10 + ", " + snapPos.z / 10; }
        else { tileText.text = " "; }
    }
}