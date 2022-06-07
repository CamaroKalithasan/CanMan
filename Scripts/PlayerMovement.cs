using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace paintSystem
{
    public class PlayerMovement : MonoBehaviour
    {
        // for input
        float horizontalMoving = 0f;
        float verticalMoving = 0f;

        // acceleration forces
        static float stickAcceleration = 2.0f;

        // jump forces
        public float jumpHeight = 6.0f;
        static float stickWalkCap = 2.0f; // stick walking speed cap

        static Vector3 roofNormal = new Vector3(0, -1.0f, 0);
        static Vector3 floorNormal = new Vector3(0, 1.0f, 0);
        static Vector3 leftWallNormal = new Vector3(1.0f, 0, 0);
        static Vector3 rightWallNormal = new Vector3(-1.0f, 0, 0);

        // keep track of ground normal
        static Vector3 groundNormal = new Vector3(0, 1.0f, 0);

        public ORIENTATION playerOrientation = ORIENTATION.FLOOR;
        public ORIENTATION prevOrientation = ORIENTATION.FLOOR;

        // checks for paint powers
        public bool isGrounded = false;
        bool isTouchingSurface = false;
        public bool isLaunched = false;
        public bool isDead = false;
        public bool isOnPaint = false;

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
        //public PhysicsMaterial2D physmat;
        public Rigidbody2D playerBody;
        public PAINT_TYPE paintPower = PAINT_TYPE.NONE;
        public Vector3 latestContactNormal = new Vector3(0, 1.0f, 0);

        // list of all stick paints we're touching
        public List<Collider2D> stickPaints = new List<Collider2D>();
        public List<Collider2D> speedPaints = new List<Collider2D>();

        public float speedCooldown = 1.0f; // allows for speed jump

        // The Animator that controls the Animation States
        public Animator Animator;
        SpriteRenderer sp;

        public Vector3 jumpPaintDir = new Vector3(0, 1, 0);

        //audio SFXs for walking on regular and sticky surfaces
        public AudioSource walkingSource;
        public AudioSource jumpSource;
        public AudioSource impactSource;
        public AudioClip stickyWalk;
        public AudioClip regularWalk;

        //audio SFX for picking up cans
        public AudioSource pickupSource;

        void UpdateOrientation()
        {
            prevOrientation = playerOrientation;

            if (paintPower != PAINT_TYPE.STICK)
            {
                playerOrientation = ORIENTATION.FLOOR;
                return;
            }

            if (latestContactNormal == roofNormal)
                playerOrientation = ORIENTATION.CEILING;
            else if (latestContactNormal == floorNormal)
                playerOrientation = ORIENTATION.FLOOR;
            else if (latestContactNormal == leftWallNormal)
                playerOrientation = ORIENTATION.WALL_L;
            else if (latestContactNormal == rightWallNormal)
                playerOrientation = ORIENTATION.WALL_R;
        }

        void OrientPlayer()
        {
            if(stickPaints.Count == 0 && !isTouchingSurface)
            {
                transform.up = groundNormal;
                return;
            }
            Debug.Log("Orient Player");
        }

        void SpriteRot()
        {
            switch (playerOrientation)
            {
                case ORIENTATION.WALL_L:
                    if (verticalMoving > 0)
                    {
                        sp.flipX = false;
                    }
                    else
                    {
                        sp.flipX = true;
                    }
                    break;
                case ORIENTATION.WALL_R:
                    if (verticalMoving > 0)
                    {
                        sp.flipX = true;
                    }
                    else
                    {
                        sp.flipX = false;
                    }
                    break;
                case ORIENTATION.CEILING:
                    if (horizontalMoving < 0)
                    {
                        sp.flipX = false;
                    }
                    else
                    {
                        sp.flipX = true;
                    }
                    break;
                case ORIENTATION.FLOOR:
                    if (horizontalMoving > 0)
                    {
                        sp.flipX = true;
                    }
                    else
                    {
                        sp.flipX = false;
                    }
                    break;
            }
        }

        void DisablePaints()
        {
            for (int i = enabledPaints.Count; i > 0; i--)
            {
                enabledPaints[i] = false;
            }
        }

        Vector2 playerVel;

        public float airSpeed = 3.0f;
        public float walkSpeed = 4.0f;
        public float runSpeed = 9.0f;
        public float maxVelocity = 12.5f;
        public float gravity = 15.0f;

        float moveSpeed = 6.0f; // start at walk speed

        public bool wallJumping = false;

        // the new one
        void Move(bool bypassStick = false)
        {
            horizontalMoving = Input.GetAxis("Horizontal");
            //verticalMoving = Input.GetAxis("Vertical");

            if (stickPaints.Count > 0 && !bypassStick)
                return;

            if (isTouchingSurface && !isGrounded && (paintPower == PAINT_TYPE.JUMP || paintPower == PAINT_TYPE.STICK))
                wallJumping = true;

            float moving = horizontalMoving;
            
            /*if(isLaunched && wallJumping)
            {
                if (isGrounded)
                    moving = (playerBody.velocity.x + horizontalMoving) / moveSpeed;
                else
                    moving = playerBody.velocity.x / moveSpeed;
            }*/

            /*if (wallJumping)
            {
                if(latestContactNormal == rightWallNormal && horizontalMoving > 0)
                {
                    horizontalMoving *= -1;
                } else if (latestContactNormal == leftWallNormal && horizontalMoving < 0)
                {
                    horizontalMoving *= -1;
                }
                else
                {
                    wallJumping = false;
                }
            }*/

            if(isLaunched)
            {
                if (playerBody.velocity.magnitude < moveSpeed)
                    isLaunched = false;
            }

            if (jumpTimer <= 0.0f)
                moving = horizontalMoving;
            else
                moving = playerBody.velocity.x / moveSpeed;

            if (!isGrounded && horizontalMoving == 0)
                moving = playerBody.velocity.x / moveSpeed;

            Vector2 move = new Vector2(moving, playerBody.velocity.y / moveSpeed);
            playerVel = move;
            playerVel *= moveSpeed;

            Vector2.ClampMagnitude(playerVel, airSpeed);
            playerBody.velocity = playerVel;
        }

        // if we have no movement input on stick paint, dont move
        void StickStop()
        {
            //Debug.Log("Stick Stop.");
            if (isLaunched)
                isLaunched = false;
            playerBody.velocity = new Vector2(0, 0);
        }

        void StickRotate()
        {
            if (!isTouchingSurface && !isOnPaint)
                return;

            // new code
            transform.up = latestContactNormal; // :)
        }

        void StickMove()
        {
            /*if (!isTouchingSurface || latestContactNormal == groundNormal)
                playerBody.gravityScale = 1;
            else
                playerBody.gravityScale = 0;*/

            if (playerOrientation != ORIENTATION.FLOOR)
                playerBody.gravityScale = 0;

            if (horizontalMove) horizontalMoving = Input.GetAxis("Horizontal");
            if (verticalMove) verticalMoving = Input.GetAxis("Vertical");
            else verticalMoving = playerBody.velocity.y / moveSpeed;

            Vector2 newvel = new Vector2(horizontalMoving, verticalMoving) * stickAcceleration;

            //Mathf.Clamp(newvel.magnitude, 0, stickWalkCap);

            if (playerBody.velocity.magnitude <= stickWalkCap)
                playerBody.velocity = newvel;
        }

        static float jumpOnlyTime = 0.5f;
        float jumpTimer = 0.0f;

        // jumping 
        void Jump()
        {
            Animator.SetBool("IsJumping", true);
            if (autoJump || tempAutoJump)
            {
                //Debug.Log("Jumped.");

                if(jumpPaintDir == roofNormal)
                {
                    playerBody.velocity =
                        new Vector2(playerBody.velocity.x, -jumpHeight);
                }
                if (jumpPaintDir == floorNormal)
                {
                    playerBody.velocity = 
                        new Vector2(playerBody.velocity.x, jumpHeight );
                }
                if (jumpPaintDir == leftWallNormal)
                {
                    if (isGrounded)
                        playerBody.velocity =
                            new Vector2(jumpHeight, playerBody.velocity.y);
                            //new Vector2(jumpHeight + (playerBody.velocity.x * -1), playerBody.velocity.y);
                    else
                    {
                        //Debug.Log("Un-Pog.");

                        if (playerBody.velocity.y < 0)
                            playerBody.velocity =
                                new Vector2(jumpHeight, -playerBody.velocity.y - (jumpHeight / 2));
                        else
                            playerBody.velocity =
                                new Vector2(jumpHeight, playerBody.velocity.y + (jumpHeight / 2));
                    }

                    jumpTimer = jumpOnlyTime;
                }
                if (jumpPaintDir == rightWallNormal)
                {
                    if (isGrounded)
                        playerBody.velocity =
                            new Vector2(-jumpHeight, playerBody.velocity.y);
                            //new Vector2((-jumpHeight * 4) - playerBody.velocity.x, playerBody.velocity.y);
                    else
                    {
                        Debug.Log("Side Jump");

                        if (playerBody.velocity.y < 0)
                            playerBody.velocity =
                                new Vector2(-jumpHeight, jumpHeight);// - (jumpHeight / 2));
                        else
                            playerBody.velocity =
                                new Vector2(-jumpHeight, jumpHeight);// + (jumpHeight / 2));                        
                    }

                    jumpTimer = jumpOnlyTime;
                }
                // set jump anim
                Animator.SetBool("Jumping", true);
            }
            else
            {
                playerBody.velocity = new Vector2(playerBody.velocity.x, jumpHeight);
                // set jump anim
                Animator.SetBool("Jumping", true);
            }

            isLaunched = true;

            tempAutoJump = false;
            isGrounded = false;
        }

        void Start()
        {
            sp = GetComponent<SpriteRenderer>();

            // setup paint ammo ( start with no paints )
            for (int i = 0; i < 3; i++)
                enabledPaints.Add(false);
        }

        void EnableMovements()
        {
            // check if we need our gravity back
            if (paintPower != PAINT_TYPE.STICK && stickPaints.Count == 0)
                playerBody.gravityScale = 1;

            verticalMove = false;
            horizontalMove = false;

            if(paintPower == PAINT_TYPE.STICK && stickPaints.Count > 0)
            {
                for (int i = 0; i < stickPaints.Count; i++)
                {
                    if (stickPaints[i].transform.up.x != 0) verticalMove = true;
                    if (stickPaints[i].transform.up.y != 0) horizontalMove = true;
                }
            }

            if (paintPower == PAINT_TYPE.STICK && (playerOrientation == ORIENTATION.FLOOR || playerOrientation == ORIENTATION.CEILING))
            {
                verticalMove = false;
            }

            if (!isGrounded && isTouchingSurface && paintPower != PAINT_TYPE.STICK && !isLaunched)
            {
                horizontalMove = false;
            }
        }

        // check if we have ground physically below us
        bool CheckPhysicalGround()
        {
            Vector3 playerPos = playerBody.transform.position;
            playerPos.z = 0;

            RaycastHit2D hit = Physics2D.Raycast(playerPos, Vector2.down, 1.0f);
            //Debug.DrawRay(playerPos, Vector2.down, Color.red, .01f, false);

            if (hit.collider != null)
                return true;

            return false;
        }

        bool CheckInputs()
        {
            if (Input.GetKey(KeyCode.W) ||
                Input.GetKey(KeyCode.A) ||
                Input.GetKey(KeyCode.S) ||
                Input.GetKey(KeyCode.D))
            {
                /*if (isGrounded || stickPaints.Count > 0)
                    Animator.SetBool("Walking", true);
                else
                    Animator.SetBool("Jumping", true);*/

                return true;
            }
            //Animator.SetBool("Walking", false);
            return false;
        }
        
        void SetAnimState()
        {
            if(isGrounded)
            {
                Animator.SetBool("Jumping", false);
                
                if (CheckInputs())
                    Animator.SetBool("Walking", true);
                else
                    Animator.SetBool("Walking", false);
            } else
            {
                if(stickPaints.Count > 0)
                {
                    if (CheckInputs())
                        Animator.SetBool("Walking", true);
                    else
                        Animator.SetBool("Walking", false);
                } else
                {
                    Animator.SetBool("Jumping", true);
                }
            }
        }

        void WalkStop()
        {
            //Debug.Log("WalkStop();");

            if (isGrounded)
                playerBody.velocity = new Vector2(0, playerBody.velocity.y);

            Manager.gSFXmanager.StopLoop(); // stop walk sound
        }

        void Update()
        {
            if (Manager.gGameManager.state == GameManager.GAMESTATE.PAUSED)
                return;

            // were dead
            if (isDead)
                return;

            CheckInputs();
            SetAnimState();

            if (paintPower == PAINT_TYPE.STICK)
                walkingSource.clip = stickyWalk;
            else
                walkingSource.clip = regularWalk;

            if (!CheckInputs())
            {
                if (!isLaunched && paintPower == PAINT_TYPE.STICK)
                    StickStop();
                else if(isGrounded)
                    WalkStop();
            }

            // need for stick paint
            EnableMovements();

            if(CheckInputs())
            {
                if (!isLaunched && paintPower == PAINT_TYPE.STICK)
                    StickMove();
                else
                    Move();
            }   

            if (!autoJump && paintPower == PAINT_TYPE.JUMP)
                if (Input.GetKeyDown(KeyCode.Space))
                    Jump();

            if (!CheckInputs())
            {
                if (paintPower == PAINT_TYPE.STICK && isTouchingSurface)
                    StickStop();
                else if (isGrounded)
                    WalkStop();
            }

            /*if (isTouchingSurface)
                if (paintPower == PAINT_TYPE.STICK || isGrounded)
                    Animator.SetBool("IsJumping", false);*/

            // player orientation
            UpdateOrientation();
            OrientPlayer();
            SpriteRot();

            // animation setting
            if (isGrounded)
                Animator.SetBool("IsJumping", false);

            if (!isLaunched && isGrounded && speedPaints.Count == 0)
            {
                moveSpeed = walkSpeed;
            }

            jumpTimer -= Time.deltaTime;
            if (isGrounded)
                jumpTimer = 0.0f;
        }

        void HandlePaint(Collider2D coll)
        {
            isOnPaint = true;

            if (coll.gameObject.tag == "paintStick")
                stickPaints.Add(coll);
            else if (coll.gameObject.tag == "paintSpeed")
                speedPaints.Add(coll);
        }

        void PaintPlayerReset()
        {
            if (stickPaints.Count == 0 && speedPaints.Count == 0 && paintPower != PAINT_TYPE.JUMP)
            {
                paintPower = PAINT_TYPE.NONE;
                isOnPaint = false;
                playerBody.gravityScale = 1;
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

            //Debug.Log("Player touched " + canRenderer.color + " paint.");

            // make can invisible and set inactive
            canRenderer.color = new Color(0, 0, 0, 0);
            can.gameObject.SetActive(false);

            pickupSource.Play();
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            isOnPaint = true;

            switch (collider.gameObject.tag)
            {
                case "paintJump":
                    jumpPaintDir = collider.gameObject.transform.up;
                    if (paintPower == PAINT_TYPE.STICK) paintPower = PAINT_TYPE.JUMP; // set early for this case
                    if (autoJump || tempAutoJump) Jump(); // auto jump on jump paint
                    paintPower = PAINT_TYPE.JUMP; // set after incase we enter with speed power
                    HandlePaint(collider);
                    break;

                case "paintSpeed":
                    paintPower = PAINT_TYPE.SPEED;
                    tempAutoJump = true;
                    moveSpeed = runSpeed;
                    HandlePaint(collider);
                    break;

                case "paintStick":
                    paintPower = PAINT_TYPE.STICK;
                    //if(collider.gameObject.transform.up != groundNormal) playerBody.gravityScale = 0;
                    moveSpeed = walkSpeed;
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

            if(stickPaints.Count == 0 && speedPaints.Count == 0) {
                isOnPaint = false;
            }

            // if it's stick paint, take it out of our list
            if (collider.gameObject.tag == "paintStick")
                for (int i = 0; i < stickPaints.Count; i++)
                    if (collider == stickPaints[i])
                        stickPaints.RemoveAt(i);

            if (collider.gameObject.tag == "paintSpeed")
                for (int i = 0; i < speedPaints.Count; i++)
                    if (collider == speedPaints[i])
                        speedPaints.RemoveAt(i);

            if (stickPaints.Count == 0 && speedPaints.Count == 0)
                paintPower = PAINT_TYPE.NONE;
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            isTouchingSurface = true;

            // get the latest collision
            if (/*collision.gameObject.tag != "paintStick" &&*/ collision.contactCount > 0)
            {
                // absolute latest normal
                latestContactNormal = collision.contacts[collision.contactCount - 1].normal;
                //Debug.Log("Latest contact normal: " + latestContactNormal);
            }

            if (latestContactNormal == groundNormal)
                isGrounded = true;

            // stick to stick paint if we land on it
            if (paintPower == PAINT_TYPE.STICK && !wallJumping && isTouchingSurface)
                StickStop();

            if (paintPower == PAINT_TYPE.JUMP)
            {
                //Debug.Log("Surface Jump.");
                Jump();
            }

            // handle stick paint rotation
            if(paintPower == PAINT_TYPE.STICK && isTouchingSurface)// && 
                //latestContactNormal == (Vector3)collision.contacts[collision.contactCount - 1].normal)
                StickRotate();

            if (paintPower == PAINT_TYPE.STICK || isGrounded)
                Animator.SetBool("IsJumping", false);

            if (isGrounded && isLaunched)
            {
                isLaunched = false;
                moveSpeed = walkSpeed;
            }

            impactSource.Play();
        }

        void OnCollisionStay2D(Collision2D collision)
        {
            isTouchingSurface = true;
            latestContactNormal = collision.transform.up;

            // cant miss the ground now
            if (latestContactNormal == groundNormal)
                isGrounded = true;
        }

        void OnCollisionExit2D(Collision2D collision)
        {
            //if(collision.contactCount == 0) isGrounded = false;
            isGrounded = false;
            isTouchingSurface = false;
        }
    }
}


