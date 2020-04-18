using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float lerp_value = 0.01f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector3 new_pos = new Vector3(
                target.position.x,
                0,
                transform.position.z
            );
        transform.position = Vector3.Lerp(transform.position, new_pos, lerp_value);        
    }
}
