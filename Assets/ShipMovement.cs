using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    public float moveSpeed = 0;
    public float rotationSpeed = 0;

    public Rigidbody2D rb;
    public Camera cam;

    Vector2 movement;
    float angle;
    Vector2 mousePos;

    // Update is called once per frame
    void Update()
    {
        //movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        angle = Input.GetAxisRaw("Horizontal");

        //mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
   
        //Vector2 lookDir = mousePos - rb.position;
        //float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;

        rb.MoveRotation(rb.rotation + angle * rotationSpeed * Time.fixedDeltaTime);
    }
}
