using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace paintSystem
{
    public class PlayerScript : MonoBehaviour
    {
        // for input
        float horizontalMoving = 0f;
        float verticalMoving = 0f;

        // acceleration forces
        static float walkAcceleration = 4.0f;
        static float runAcceleration = 8.0f;
        static float stickAcceleration = 2.0f;

        // jump forces
        public float jumpHeight = 6.0f;
        static float jumpDampen = 1.5f;
        //static float jumpDampenSpeed = 2.0f;

        // speed caps
        static float walkCap = 3.0f; // walking speed cap
        static float runCap = 6.0f; // running speed cap
        static float stickWalkCap = 2.0f; // stick walking speed cap

        // friction forces for each situation
        static float deFriction = 0.3f; // default friction
        static float speedFriction = 0.1f;
        static float stickFriction = 0.5f;

        // keep track of ground normal
        static Vector3 groundNormal = new Vector3(0, 1.0f, 0);

        // duh
        bool flipRight = true;

        // checks for paint powers
        bool isGrounded = false;
        bool isOnPaint = false;
        bool isTouchingSurface = false;
        public bool isDead = false;
        public bool rainbowMan = true;
        public bool inAir = false;

        // axis enable / disable
        bool horizontalMove = false;
        bool verticalMove = false;

        // behavior enablers 
        public bool autoJump = false; // auto jump when we hit jump paint?
        bool tempAutoJump = false;

        // paint weapon enablers
        // 0 = Jump
        // 1 = Speed
        // 2 = Stick
        public List<bool> enabledPaints = new List<bool>();

        // misc
        public PhysicsMaterial2D physmat;
        public Rigidbody2D playerBody;
        public PAINT_TYPE paintPower = PAINT_TYPE.NONE;
        public Vector3 latestContactNormal = new Vector3(0, 1.0f, 0);

        // list of all stick paints we're touching
        List<Collider2D> stickPaints = new List<Collider2D>();

        // The Animator that controls the Animation States
        public Animator Animator;
        SpriteRenderer sp;

        void DisablePaints()
        {
            for (int i = enabledPaints.Count; i > 0; i--)
            {
                enabledPaints[i] = false;
            }
        }

        public void Kill()
        {
            isDead = true;
            DisablePaints();
        }

        void SurfaceRot()
        {
            transform.up = latestContactNormal;
        }

        // walking
        void Move()
        {
            float acceleration = walkAcceleration;
            float cap = walkCap;

            // use speed paint movement figures
            if (paintPower == PAINT_TYPE.SPEED)
            {
                acceleration = runAcceleration;
                cap = runCap;
            }

            horizontalMoving = Input.GetAxis("Horizontal");

            if (playerBody.velocity.magnitude < cap)
                playerBody.velocity = new Vector2(horizontalMoving, 0) * acceleration;

            Mathf.Clamp(playerBody.velocity.magnitude, 0, cap);
        }

        // if we have no movement input on stick paint, dont move
        void StickStop()
        {
            playerBody.velocity = new Vector2(0, 0);
        }

        void StickRotate()
        {
            transform.up = stickPaints[stickPaints.Count - 1].transform.up;
        }

        void StickMove()
        {
            StickRotate();

            if (horizontalMove) horizontalMoving = Input.GetAxis("Horizontal");
            if (verticalMove) verticalMoving = Input.GetAxis("Vertical");

            if (playerBody.velocity.magnitude < stickWalkCap)
                playerBody.velocity += new Vector2(horizontalMoving, verticalMoving) * stickAcceleration;

            Mathf.Clamp(playerBody.velocity.magnitude, 0, stickWalkCap);
        }

        // jumping 
        void Jump()
        {
            Animator.SetBool("IsJumping", true);
            if (autoJump || tempAutoJump)
            {
                float jumpAmp = jumpHeight;// * playerBody.velocity.x;

                playerBody.velocity = new Vector2(playerBody.velocity.x, jumpAmp);

                //playerBody.velocity = new Vector2(playerBody.velocity.x, playerBody.velocity.y) * 
            }
            else
            {
                // we use the jumpDampen to amplify, as the number works well
                playerBody.velocity = new Vector2(playerBody.velocity.x, jumpHeight);
            }

            tempAutoJump = false;
        }

        void PlayerOrient()
        {
            if (paintPower == PAINT_TYPE.STICK)
            {
                if (verticalMoving > 0)
                    sp.flipX = true;
                else
                    sp.flipX = false;

                if (latestContactNormal.y > 0)
                {
                    if (horizontalMoving > 0)
                        sp.flipX = true;
                    else
                        sp.flipX = false;
                }
                else
                {
                    if (horizontalMoving > 0)
                        sp.flipX = true;
                    else
                        sp.flipX = false;
                }
            }
            else
            {
                if (horizontalMoving > 0)
                    sp.flipX = true;
                else
                    sp.flipX = false;
            }

        }

        //Method for flipping the character right
        void FlipCharacter()
        {
            flipRight = !flipRight;

            if (paintPower == PAINT_TYPE.STICK)
            {
                if (Input.GetAxis("Vertical") < 0)
                {
                    sp.flipX = true;
                }
                if (Input.GetAxis("Vertical") > 0)
                {
                    sp.flipX = false;
                }
            }
            else
            {
                if (Input.GetAxis("Horizontal") < 0)
                {
                    sp.flipX = false;
                }
                if (Input.GetAxis("Horizontal") > 0)
                {
                    sp.flipX = true;
                }
            }
        }

        void Start()
        {
            sp = GetComponent<SpriteRenderer>();
            // setup the material for player friction
            playerBody.sharedMaterial = physmat;
            playerBody.sharedMaterial.friction = deFriction;

            // setup paint ammo ( start with no paints )
            for (int i = 0; i < 3; i++)
                enabledPaints.Add(false);
        }

        void EnableMovements()
        {
            // check if we need our gravity back
            if (paintPower != PAINT_TYPE.STICK && !isOnPaint)
                playerBody.gravityScale = 1;

            verticalMove = false;
            horizontalMove = false;

            for (int i = 0; i < stickPaints.Count; i++)
            {
                if (stickPaints[i].transform.up.x != 0) verticalMove = true;
                if (stickPaints[i].transform.up.y != 0) horizontalMove = true;
            }
        }

        bool CheckInputs()
        {
            if (Input.GetKey(KeyCode.W) ||
                Input.GetKey(KeyCode.A) ||
                Input.GetKey(KeyCode.S) ||
                Input.GetKey(KeyCode.D))
            {
                Animator.SetBool("Walking", true);
                return true;
            }
            Animator.SetBool("Walking", false);
            return false;
        }

        void WalkStop()
        {
            if (latestContactNormal == groundNormal)
                playerBody.velocity = new Vector2(0, playerBody.velocity.y);
        }

        bool IsPlayerGrounded()
        {
            if (latestContactNormal == groundNormal)
                return true;

            return false;
        }

        void RainbowCanMan()
        {
            //sp.color;
        }

        void Update()
        {
            // were dead
            if (isDead)
                return;

            EnableMovements();

            if (isTouchingSurface)
                Animator.SetBool("IsJumping", false);

            if (CheckInputs())
            {
                if (paintPower == PAINT_TYPE.STICK)
                    StickMove();
                else
                    Move();

                if (!autoJump && paintPower == PAINT_TYPE.JUMP)
                    if (Input.GetKeyDown(KeyCode.Space) && IsPlayerGrounded())
                        Jump();

                PlayerOrient();
            } 
            else
            {
                if (paintPower == PAINT_TYPE.STICK && !inAir)
                    StickStop();
                else if (latestContactNormal.y > 0)
                    WalkStop();
            }
        }

        void HandlePaint(Collider2D coll)
        {
            if (coll.gameObject.tag == "paintStick")
                stickPaints.Add(coll);
        }

        void PaintPlayerReset()
        {
            if (stickPaints.Count == 0)
            {
                paintPower = PAINT_TYPE.NONE;
                playerBody.sharedMaterial.friction = deFriction;
                isOnPaint = false;
            }
        }

        void DeterminePaintCanPower(Collider2D can)
        {
            SpriteRenderer canRenderer = can.GetComponent<SpriteRenderer>();

            if (canRenderer.color == paintInfo.jump)
                enabledPaints[0] = true;
            else if (canRenderer.color == paintInfo.speed)
                enabledPaints[1] = true;
            else if (canRenderer.color == paintInfo.stick)
                enabledPaints[2] = true;

            Debug.Log("Player touched " + canRenderer.color + " paint.");

            // make can invisible and set inactive
            canRenderer.color = new Color(0, 0, 0, 0);
            can.gameObject.SetActive(false);
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            switch (collider.gameObject.tag)
            {
                case "paintJump":
                    if (autoJump || tempAutoJump) Jump(); // auto jump on jump paint
                    paintPower = PAINT_TYPE.JUMP; // set after incase we enter with speed power
                    HandlePaint(collider);
                    break;

                case "paintSpeed":
                    paintPower = PAINT_TYPE.SPEED;
                    //playerBody.sharedMaterial.friction = speedFriction;
                    tempAutoJump = true;
                    HandlePaint(collider);
                    break;

                case "paintStick":

                    Debug.Log("Player touched stick paint.");

                    paintPower = PAINT_TYPE.STICK;
                    //playerBody.sharedMaterial.friction = stickFriction;
                    playerBody.gravityScale = 0;
                    HandlePaint(collider);
                    break;

                case "paintCan":
                    DeterminePaintCanPower(collider);
                    break;

                default:
                    PaintPlayerReset();
                    break;
            }
        }

        void OnTriggerExit2D(Collider2D collider)
        {
            // if it's stick paint, take it out of our list
            if (collider.gameObject.tag == "paintStick")
                for (int i = 0; i < stickPaints.Count; i++)
                    if (collider == stickPaints[i])
                        stickPaints.RemoveAt(i);

            if (stickPaints.Count == 0)
            {
                paintPower = PAINT_TYPE.NONE;
                isOnPaint = false;
                transform.rotation = new Quaternion(0, 0, 0, 0);
            }
        }

        void OnTriggerStay2D(Collider2D collider)
        {
            isOnPaint = true;
        }

        void CheckSurfaceNormal(Collision2D collision)
        {
            /*if (collision.gameObject.transform.up == groundNormal)
                latestContactNormal = groundNormal;*/

            if (collision.transform.up == groundNormal)
                latestContactNormal = groundNormal;

            if ((latestContactNormal.x <= 1 && latestContactNormal.x > 0) ||
                (latestContactNormal.y <= 1 && latestContactNormal.y > 0))
            {
                isGrounded = true;
            }

            if (latestContactNormal == groundNormal || isGrounded)
            {
                Animator.SetBool("IsJumping", false);
                //isGrounded = true;
            }
            else
            {
                Animator.SetBool("IsJumping", true);
                isGrounded = false;
            }

        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            inAir = false;

            CheckSurfaceNormal(collision);

            // this variable will be true whenever player is touching a surface
            isTouchingSurface = true;

            // stick to stick paint if we land on it
            if (paintPower == PAINT_TYPE.STICK)
                playerBody.velocity = new Vector2(0, 0);

            // get the latest collision
            if (collision.contactCount > 0)
            {
                latestContactNormal = collision.transform.up;//collision.contacts[collision.contactCount - 1].normal;
                Debug.Log("Latest contact normal: " + latestContactNormal);
            }

            if (paintPower == PAINT_TYPE.JUMP)
                Jump();
        }

        void OnCollisionStay2D(Collision2D collision)
        {
            CheckSurfaceNormal(collision);
        }

        void OnCollisionExit2D(Collision2D collision)
        {
            isGrounded = false;
            isTouchingSurface = false;
            inAir = true;
        }
    }
}


