using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintCursor : MonoBehaviour
{
    public Camera playerCam;
    public CircleCollider2D circle;
    public Vector3 paintTarget = new Vector3(0, 0, 0);
    public bool isSnapped = false;

    // Update is called once per frame
    void Update()
    {
        Vector3 nP = playerCam.ScreenToWorldPoint(Input.mousePosition);
        nP.z = 0f;

        transform.position = nP;

        if (!isSnapped)
            GetComponent<SpriteRenderer>().transform.position = nP;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Hit " + collider);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Surface");
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "paintable")
        {
            isSnapped = true;
            paintTarget = new Vector3(collision.collider.transform.position.x + 1.6f, transform.position.y, 0);
            GetComponent<SpriteRenderer>().transform.position = paintTarget;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        isSnapped = false;
    }
}
