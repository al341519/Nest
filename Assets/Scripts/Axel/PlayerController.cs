using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 0.1f;
    public float jumpSpeed = 0.07f;
    public bool grounded;
    private Vector3 velocity = Vector3.zero;
    protected Rigidbody rb;
    //private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        grounded = true;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        velocity.x = 0f;
        Vector3 move = Vector3.zero;

        
        if (Input.GetButtonDown("Jump") && grounded)
        {
            grounded = false;
            velocity.y = jumpSpeed;
        }
        Vector3 desiredPosition = transform.position;
        desiredPosition.x += Input.GetAxis("Horizontal") * speed;
        StartCoroutine(MoveFromTo(transform, transform.position, desiredPosition, 10));

        rb.position = rb.position + velocity;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.position.y < this.transform.position.y)
        {
            velocity.y = 0f;
            grounded = true;
        }
    }
    IEnumerator MoveFromTo(Transform objectToMove, Vector3 a, Vector3 b, float speed)
    {
        float step = (speed / (a - b).magnitude) * Time.fixedDeltaTime;
        float t = 0;
        while (t <= 1.0f)
        {
            t += step; // Goes from 0 to 1, incrementing by step each time
            objectToMove.position = Vector3.Lerp(a, b, t); // Move objectToMove closer to b
            yield return new WaitForFixedUpdate();         // Leave the routine and return here in the next frame
        }
        objectToMove.position = b;

    }
}
