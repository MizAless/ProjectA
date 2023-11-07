using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;
    public float speed = 2f;
    private Vector3 moveVector;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        moveVector.x = Input.GetAxis("Horizontal");
        moveVector.z = Input.GetAxis("Vertical");

        //animator.SetFloat("speed", Vector3.ClampMagnitude(moveVector, 1).magnitude);

        rb.transform.localPosition += transform.forward * moveVector.z * speed * Time.deltaTime;
        rb.transform.localPosition += transform.right * moveVector.x * speed * Time.deltaTime;
    }
}
