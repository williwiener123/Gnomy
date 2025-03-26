using UnityEngine;
using System.Collections;

public class UFOPatrol : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;
    public float waitTime = 1f; // Time to wait at each point

    private Vector3 target;
    private bool isWaiting = false;

    void Start()
    {
        target = pointA.position;
    }

    void Update()
    {
        if (!isWaiting)
        {
            // Move towards the target position
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

            // Check if the UFO has reached the target position
            if (Vector3.Distance(transform.position, target) < 0.05f)
            {
                StartCoroutine(WaitAndSwitch());
            }
        }
    }

    IEnumerator WaitAndSwitch()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime); // Wait at the point
        target = (target == pointA.position) ? pointB.position : pointA.position; // Switch target
        isWaiting = false;
    }
}
