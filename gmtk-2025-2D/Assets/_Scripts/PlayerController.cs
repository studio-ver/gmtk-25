using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public BoxCollider2D coll;
    public float speed = 250f;
    public float slideSpeed = 300f;
    public float jumpForce = 15f;
    private float direction = 1;
    public LayerMask groundLayer;
    private bool cooldown = false;

    private void Update()
    {
        bool grounded = IsGrounded();

        if (cooldown)
            return;

        if (!grounded)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            cooldown = true;
            StartCoroutine(Backflip());
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            cooldown = true;
            StartCoroutine(Slide());
        }
    }

    private IEnumerator Backflip()
    {
        direction = -1.5f;
        rb.linearVelocityY = jumpForce;
        yield return new WaitForSeconds(.4f);
        direction = 0;
        yield return new WaitForSeconds(.3f);
        direction = 1;
        cooldown = false;
    }

    private IEnumerator Slide()
    {
        float speedRef = speed;
        speed = slideSpeed;
        yield return new WaitForSeconds(.2f);
        speed = speedRef;
        yield return new WaitForSeconds(.15f);
        cooldown = false;
    }

    private void FixedUpdate()
    {
        rb.linearVelocityX = speed * direction * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 portalLocation = Vector3.zero;

        if (collision.collider.tag == "PortalA")
        {
            portalLocation = GameObject.FindWithTag("PortalB").transform.position;
            transform.position = new Vector3(portalLocation.x - 1.25f, transform.position.y, transform.position.z);
        } 
        else if (collision.collider.tag == "PortalB")
        {
            portalLocation = GameObject.FindWithTag("PortalA").transform.position;
            transform.position = new Vector3(portalLocation.x + 1.25f, transform.position.y, transform.position.z);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0, Vector2.down, .1f, groundLayer).collider != null;
    }
}
