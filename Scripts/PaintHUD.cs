using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace paintSystem
{
    public class PaintHUD : MonoBehaviour
    {
        public PlayerPaintgun paintgun;
        public Image img;

        PAINT_TYPE selectedPaint = PAINT_TYPE.NONE;

        void ChangeColor()
        {
            switch(selectedPaint)
            {
                case PAINT_TYPE.JUMP:
                    img.color = paintInfo.jump;
                    break;
                case PAINT_TYPE.SPEED:
                    img.color = paintInfo.speed;
                    break;
                case PAINT_TYPE.STICK:
                    img.color = paintInfo.stick;
                    break;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (paintgun.currentPaint != selectedPaint)
            {
                selectedPaint = paintgun.currentPaint;
                ChangeColor();
            }
        }
    }
}

