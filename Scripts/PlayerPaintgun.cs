using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace paintSystem
{
    public class PlayerPaintgun : MonoBehaviour
    {
        PlayerMovement player;

        //public PaintCursor playerCursor;
        //public Camera playerCam;
        public GameObject originPaint;
        GameObject tempPaint;

        CircleCollider2D box;

        public float paintDistance = 15.0f;

        public PAINT_TYPE currentPaint = PAINT_TYPE.NONE;
        public PAINT_TYPE nextPaint = PAINT_TYPE.NONE;

        ////public AudioSource variables
        public AudioSource paint1;
        public AudioSource paint2;
        public AudioSource paint3;
        public AudioSource erasePaint;

        public ParticleSystem particles;

        public bool playAudio = false;
        bool playParticles = false;

        // Start is called before the first frame update
        void Start()
        {
            player = GetComponent <PlayerMovement>();
            box = GetComponent<CircleCollider2D>();
        }

        Color GetColorOfPaint(PAINT_TYPE paintType)
        {
            switch(paintType)
            {
                case PAINT_TYPE.JUMP:
                    return paintInfo.jump;
                case PAINT_TYPE.SPEED:
                    return paintInfo.speed;
                case PAINT_TYPE.STICK:
                    return paintInfo.stick;
            }

            return paintInfo.none;
        }

        int GetIndexFromCurrentPaint()
        {
            switch(currentPaint)
            {
                case PAINT_TYPE.JUMP:
                    return 0;
                case PAINT_TYPE.SPEED:
                    return 1;
                case PAINT_TYPE.STICK:
                    return 2;
            }

            return 0; // return jump if all else fails
        }

        PAINT_TYPE GetPaintFromIndex(int index)
        {
            if (index == 0) return PAINT_TYPE.JUMP;
            else if (index == 1) return PAINT_TYPE.SPEED;
            else if (index == 2) return PAINT_TYPE.STICK;

            // no paint type without a valid index
            return PAINT_TYPE.NONE;
        }

        void SwithPaint(int index)
        {
            // check if we have the paint at that index, and if we do then switch to it
            if (player.enabledPaints[index])
                currentPaint = GetPaintFromIndex(index);
        }

        void ColorPlayer(Color paintColor)
        {
            player.GetComponent<SpriteRenderer>().color = paintColor;
        }

        // handles input when the player tries to switch paint
        void CheckSwitchPaints()
        {
            if(currentPaint == PAINT_TYPE.NONE)
            {
                for(int i = 0; i < player.enabledPaints.Count; i++)
                {
                    if (player.enabledPaints[i])
                        currentPaint = GetPaintFromIndex(i);
                }
            }

            if(Input.GetKeyDown(KeyCode.UpArrow) || Input.mouseScrollDelta.y > 0)
            {
                int nI = GetIndexFromCurrentPaint() - 1;
                if (nI == -1) nI = 2;

                SwithPaint(nI);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.mouseScrollDelta.y < 0)
            {
                int nI = GetIndexFromCurrentPaint() + 1;
                if (nI == 3) nI = 0;

                SwithPaint(nI);
            }

            //ColorPlayer(GetColorOfPaint(currentPaint));
        }

        // we use -16 for the offset, as the paint is 32 tall
        Vector2 DeterminePointWithPaintOffset(Vector2 point, Vector3 normal)
        {
            if(normal.x != 0)
                return new Vector2(point.x, point.y - 1.6f);
            else if (normal.y != 0)
                return new Vector2(point.x - 1.6f, point.y);

            // return the point if we have nothing to change
            return point;
        }

        void SprayPaint()
        {
            // cant paint without paint
            if (currentPaint == PAINT_TYPE.NONE)
                return;

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mouseDir = (mousePos - transform.position);
            Vector3 playerPos = player.transform.position;

            mouseDir.Normalize();
            playerPos.z = 0;

            // set particle colors
            // switch case

            // start dstop particles in update();
            
            RaycastHit2D hit = Physics2D.Raycast(playerPos, mouseDir, paintDistance);

            if (hit.collider != null)
            {
                //Debug.Log("Target Position: " + hit.point);

                string objTag = hit.collider.gameObject.tag;

                if (objTag == paintInfo.jumpTag || 
                    objTag == paintInfo.speedTag || 
                    objTag == paintInfo.stickTag ||
                    objTag == paintInfo.defaultTag ||
                    objTag == "Unpaintable" ||
                    objTag == "Enemy" ||
                    objTag == "Door" ||
                    objTag == "paintCan")
                    return; // dont paint a paint

                tempPaint = Instantiate(originPaint,
                hit.point,
                hit.collider.gameObject.transform.rotation, null);

                tempPaint.GetComponent<paint>().paintType = currentPaint;
            }
        }
        
        void ErasePaint()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mouseDir = (mousePos - transform.position);
            Vector3 playerPos = player.transform.position;

            mouseDir.Normalize();
            playerPos.z = 0;

            RaycastHit2D hit = Physics2D.Raycast(playerPos, mouseDir, paintDistance);

            if (hit.collider != null)
            {
                string objTag = hit.collider.gameObject.tag;

                if (objTag == paintInfo.jumpTag ||
                    objTag == paintInfo.speedTag ||
                    objTag == paintInfo.stickTag ||
                    objTag == paintInfo.defaultTag)
                {
                    Destroy(hit.collider.gameObject);
                }
            }
        }
        
        void TracePaintSurface()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mouseDir = (mousePos - transform.position);
            Vector3 playerPos = player.transform.position;

            mouseDir.Normalize();
            playerPos.z = 0;

            //Debug.DrawRay(playerPos, mouseDir, Color.red, .01f, false);
        }

        void Controls()
        {
            if(Input.GetKey(KeyCode.Mouse0))
                SprayPaint();

            if (Input.GetKey(KeyCode.Mouse1))
                ErasePaint();
        }

        // Update is called once per frame
        void Update()
        {   
            if (Manager.gGameManager.state == GameManager.GAMESTATE.PAUSED)
                return;

            if (!player)
                return;

            var emission = particles.emission; // Stores the module in a local variable
            var main = particles.main;

            if (Input.GetKey(KeyCode.Mouse0))
            {
                switch (currentPaint)
                {
                    case PAINT_TYPE.JUMP:
                        paint1.Play();
                        main.startColor = paintInfo.jump;
                        break;
                    case PAINT_TYPE.SPEED:
                        paint2.Play();
                        main.startColor = paintInfo.speed;
                        break;
                    case PAINT_TYPE.STICK:
                        paint3.Play();
                        main.startColor = paintInfo.stick;
                        break;
                    default:
                        paint1.Play();
                        main.startColor = new Color(0,0,0,0);
                        break;
                }
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                main.startColor = paintInfo.none;
                erasePaint.Play();
            }

            // "pain particles" - John O'Leske, Trello, 2021
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mouseDir = (mousePos - transform.position);
            mouseDir.Normalize();

            // set dir for the particles
            particles.transform.up = -mouseDir;

            if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1))
            {
                if (!playParticles)
                    particles.Play();

                emission.enabled = true;
            }            
            else
            {
                if (emission.enabled) 
                    emission.enabled = false;
            }
            

            CheckSwitchPaints(); // this will set out currentPaint

            // left mouse button
            TracePaintSurface();
            Controls();
            //TracePaintSurface();

        }

    }

}
