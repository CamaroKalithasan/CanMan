using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Created by Jay Land II - jaycland02@gmail.com
// =============================================

namespace paintSystem
{
    // types of paint
    public enum PAINT_TYPE
    {
        JUMP = 0,
        SPEED,
        STICK,
        NONE
    }

    // sizes of paint puddles
    public enum PAINT_SIZE
    {
        SMALL = 0,
        MEDIUM,
        LARGE
    }

    // used by paint tile to determine trigger offset on surfaces
    public enum ORIENTATION
    {
        WALL_L = 0,
        WALL_R,
        CEILING,
        FLOOR
    }

    // numbers we want to keep consistent throughout 
    public struct paintInfo
    {
        // colors
        public static Color jump = Color.green;
        public static Color speed = Color.red;
        public static Color stick = Color.blue;
        public static Color none = Color.gray;

        // tags
        public static string jumpTag = "paintJump";
        public static string speedTag = "paintSpeed";
        public static string stickTag = "paintStick";
        public static string defaultTag = "paintDefault"; // aka 'none'

        //scales
        public static float scaleSmall = 1.0f;
        public static float scaleMedium = 1.5f;
        public static float scaleLarge = 2.0f;
    }

    public class paint : MonoBehaviour
    {
        // no type by default
        public PAINT_TYPE paintType = PAINT_TYPE.NONE;
        public PAINT_SIZE paintSize = PAINT_SIZE.SMALL;
        public SpriteRenderer paintTex;

        // Start is called before the first frame update
        void Start()
        {
            DetermineColor(); // set our color from the type
            DetermineScale();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        // determines tag and color from the paint type
        private void DetermineColor()
        {
            switch(paintType)
            {
                case PAINT_TYPE.JUMP:
                    paintTex.color = paintInfo.jump; // set our color
                    tag = paintInfo.jumpTag; // set our tag for that type
                    break;
                case PAINT_TYPE.SPEED:
                    paintTex.color = paintInfo.speed;
                    tag = paintInfo.speedTag;
                    break;
                case PAINT_TYPE.STICK:
                    paintTex.color = paintInfo.stick;
                    tag = paintInfo.stickTag;
                    break;
                case PAINT_TYPE.NONE:
                    paintTex.color = paintInfo.none;
                    tag = paintInfo.defaultTag;
                    break;
            }
        }

        // determines the scale of the paint
        private void DetermineScale()
        {
            float ns = paintInfo.scaleSmall; // the new scale

            switch(paintSize)
            {
                case PAINT_SIZE.SMALL:
                    ns = paintInfo.scaleSmall;
                    break;
                case PAINT_SIZE.MEDIUM:
                    ns = paintInfo.scaleMedium;
                    break;
                case PAINT_SIZE.LARGE:
                    ns = paintInfo.scaleLarge;
                    break;
            }

            //transform.localScale = new Vector3(ns, transform.localScale.y, transform.localScale.z);
        }

        public void SetPaintType(PAINT_TYPE _type)
        {
            paintType = _type;
            DetermineColor(); // set our color and tag
        }

        public void SetPaintScale(PAINT_SIZE _size)
        {
            paintSize = _size;
            DetermineScale(); // set our scale 
        }
    }
}

