using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Config")]
    public Transform target;
    public Vector3 offset;
    public float followSpeed;
    public float xMin = 0f;
    Vector3 velocity = Vector3.zero;
    
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    
    void FixedUpdate()
    {
        Vector3 targetPos = target.position + offset;
        Vector3 clampedPos = new Vector3(Mathf.Clamp(targetPos.x, xMin, float.MaxValue), targetPos.y, targetPos.z);
        Vector3 smoothPos = Vector3.SmoothDamp(transform.position, clampedPos, ref velocity, followSpeed * Time.fixedDeltaTime);

        transform.position = smoothPos;
    }

    
}
