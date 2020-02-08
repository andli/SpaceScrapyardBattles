using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    public float moveSpeed;
    public float acceleration;
    public float rotationSpeed;

    public Camera cam;
    public Rigidbody2D rb;

    Vector2 movement;
    float angleChange;
    //Vector2 mousePos;

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        angleChange = Input.GetAxisRaw("Horizontal");

        //mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate()
    {
        moveSpeed -= Mathf.Clamp(acceleration * movement.y, -5f, 5f);

        rb.MoveRotation(rb.rotation - angleChange * rotationSpeed * Time.fixedDeltaTime);
        rb.transform.Translate(Quaternion.Euler(0, 0, angleChange - 90f) * new Vector3(moveSpeed, 0, 0) * Time.deltaTime);
    }
}
