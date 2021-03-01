using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositiveCrystalEvent : MonoBehaviour
{
    public GameObject[] orbListToCreate;
    public float fallSpeed = 5f;
    public float moveMinX;
    public float moveMaxX;
    bool clicked = false;

    float tmpTime = 3f;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    } // Start()

    private void FixedUpdate()
    {
        tmpTime -= 1.5f * Time.fixedDeltaTime;

        if (rb.velocity.y >= 0.65f)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0.25f);
        }

        if (tmpTime <= 0f)
        {
            tmpTime = 3f;
            Vector2 vt = new Vector2(Random.insideUnitCircle.x * 1.5f, rb.velocity.y);
            rb.velocity = vt;
        }

        rb.position = new Vector2(Mathf.Clamp(rb.position.x, moveMinX, moveMaxX), rb.position.y);

    } // FixedUpdate()

    private void OnMouseDown()
    {
        if (!clicked)
        {
            clicked = true;
        }

    } // OnMouseDown()

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("LowerBoundry"))
        {
            GameManager._instance.createdPosiCrystalEncounterObj = null;
            Destroy(gameObject);
        }

    } // OnTriggerEnter2D()

} // class
