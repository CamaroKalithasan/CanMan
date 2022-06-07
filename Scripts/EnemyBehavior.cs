using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace paintSystem
{
    public class EnemyBehavior : MonoBehaviour
    {
        // Variable Declarations:
        // A variable which determines whether the enemy is facing left or right and returns true or false, respectively. False by default. 
        private bool facingLeft = false;
        // A variable which stores the starting point for the groundInfo ray cast. 
        public Transform groundDetectionPoint;
        // A variable which stores the starting point for the wallInfo ray cast.
        public Transform wallDetectionPoint;
        // A varaible which stores the starting point for the paintInfo ray cast.
        public Transform paintDetectionPoint;
        // A variable that acts as the enemy's speed limit, adjustable in its inspector. 
        public float enemySpeed;
        // A variable that acts as the enemy's rotation scale, adjustable in its inspector. 
        public float scaleForRotation;
        // A variable for the audio when the enemy eats the paint
        public AudioSource audioSource;

        // Update is called once per frame
        void Update()
        {
            // Let the enemy patrol during each frame.
            StartEnemyPatrol();
            // If EndOfGround returns true, meaning that there is no platform left to walk on, change the direction the enemy is facing.
            if (EndOfGround(1f))
            {
                ChangeEnemyDirection();
            }
            // If CollidedWithObject returns true, meaning that the enemy has hit an object that's not the player, change the direction the enemy is facing. 
            if (CollidedWithObject(0.35f))
            {
                ChangeEnemyDirection();
            }
            DestroyPaintedTile(5.0f);
        }

        // Function definitions:
        void StartEnemyPatrol()
        {
            if (!facingLeft)
            {
                transform.Translate(Vector2.right * enemySpeed * Time.deltaTime);
            }
            else if (facingLeft)
            {
                transform.Translate(Vector2.left * enemySpeed * Time.deltaTime);
            }
        }
        void ChangeEnemyDirection()
        {
            if (!facingLeft)
            {
                transform.localScale = new Vector2(-scaleForRotation, scaleForRotation);
                facingLeft = true;
            }
            else
            {
                transform.localScale = new Vector2(scaleForRotation, scaleForRotation);
                facingLeft = false;
            }
        }
        bool EndOfGround(float rayDistance)
        {
            bool returnValue = false;
            RaycastHit2D groundInfo = Physics2D.Raycast(groundDetectionPoint.position, Vector2.down, rayDistance);
            Debug.DrawRay(groundDetectionPoint.position, Vector2.down, Color.red, .01f, false);
            if (!groundInfo.collider)
            {
                returnValue = true;
                //Debug.Log("Enemy EndofGround = " + returnValue);
            }
            else if(groundInfo.collider.gameObject.layer == 7)
            {
                returnValue = true;
            }
            return returnValue;
        }
        bool CollidedWithObject(float rayDistance)
        {
            bool returnValue = false;
            Vector2 direction = Vector2.right;
            if (facingLeft)
            {
                direction = Vector2.left;
            }
            RaycastHit2D wallInfo = Physics2D.Raycast(wallDetectionPoint.position, direction, rayDistance);
            if (wallInfo.collider && !wallInfo.collider.gameObject.CompareTag("Player"))
            {
                if (!wallInfo.collider.gameObject.CompareTag("paintJump") && !wallInfo.collider.gameObject.CompareTag("paintSpeed") && !wallInfo.collider.gameObject.CompareTag("paintStick") && !wallInfo.collider.gameObject.CompareTag("paintCan"))
                {
                    returnValue = true;
                }
            }
            return returnValue;
        }
        void DestroyPaintedTile(float rayDistance)
        {
            //RaycastHit2D paintInfo = Physics2D.Raycast(paintDetectionPoint.position, /*new Vector2(0, -45)*/ Vector2.down, rayDistance);
            RaycastHit2D hit = Physics2D.Raycast(paintDetectionPoint.position, /*new Vector2(0, -45)*/ Vector2.down, rayDistance);
            if(hit.collider && !hit.collider.gameObject.CompareTag("Enemy"))
            {
                if (hit.collider.gameObject.CompareTag("paintJump") ||
                    hit.collider.gameObject.CompareTag("paintSpeed") ||
                    hit.collider.gameObject.CompareTag("paintStick"))
                {
                    Destroy(hit.collider.gameObject);
                    audioSource.Play();
                }
            }
        }
        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Player" && collision.gameObject.transform.up == new Vector3(0, 1f, 0))
                ChangeEnemyDirection();

            // destroy paint if we hit it, this will also prevent enemy getting stuck
            if (collision.gameObject.CompareTag("paintJump") ||
                collision.gameObject.CompareTag("paintSpeed") ||
                collision.gameObject.CompareTag("paintStick"))
            {
                if (!collision.gameObject)
                    return; // check if we already rid of the object

                Destroy(collision.gameObject);
                audioSource.Play();
            }
        }

        /*
        // Code for player agro:
        // A variable that contains the coordinate information of the player. 
        public Transform player;
        // A variable that acts as the enemy's agro range, adjustable in its inspector. 
        public float agroRange;
        // A variable which is used to store the enemy's RigidBody2D component. 
        Rigidbody2D enemyRB2D;

        Start()
        {
            enemyRB2D = GetComponent<Rigidbody2D>();
        }

        // Initiate a chase with the player.
        void StartChase()
        {
            // If the enemy is to the left of the player, then face and move right. 
            if (transform.position.x < player.position.x)
            {
                transform.localScale = new Vector2(1, 1);
                facingLeft = false;
                enemyRB2D.velocity = new Vector2(enemySpeed, 0);
            }
            // Else, if the enemy is to the right of the player, face and move left.
            else if (transform.position.x > player.position.x)
            {
                transform.localScale = new Vector2(-1, 1);
                facingLeft = true;
                enemyRB2D.velocity = new Vector2(-enemySpeed, 0);
            }
        }
        // End the chase with the player.
        void StopChase()
        {
            enemyRB2D.velocity = Vector2.zero;
        }
        // Use a ray cast to spot the player and return either true or false depending on whether or not the ray cast has done so. 
        bool PlayerSpotted(float distance)
        {
            bool returnValue = false;
            float castDistance = distance;

            if (facingLeft)
            {
                castDistance = -distance;
            }

            Vector2 endPosition = eyeSightPoint.position + Vector3.right * castDistance;
            RaycastHit2D hit = Physics2D.Linecast(eyeSightPoint.position, endPosition, 1 << LayerMask.NameToLayer("Default"));

            if (hit.collider != null)
            {
                // If the object that the ray cast has found has the tag Player, set returnValue to true.
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    returnValue = true;
                }
                // Else, set returnValue to false.
                else
                {
                    returnValue = false;
                }
            }
            return returnValue;
        }
        */
    }
}
