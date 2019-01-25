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
        velocity.x = Input.GetAxis("Horizontal") * speed;

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
}
