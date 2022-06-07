using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace paintSystem
{
    public class PaintCanBehavior : MonoBehaviour
    {
        public PAINT_TYPE paintType = PAINT_TYPE.NONE;
        public SpriteRenderer canRenderer;

        // Start is called before the first frame update
        void Start()
        {
            switch (paintType)
            {
                case PAINT_TYPE.JUMP:
                    canRenderer.color = paintInfo.jump;
                    break;
                case PAINT_TYPE.SPEED:
                    canRenderer.color = paintInfo.speed;
                    break;
                case PAINT_TYPE.STICK:
                    canRenderer.color = paintInfo.stick;
                    break;
                case PAINT_TYPE.NONE:
                    canRenderer.color = paintInfo.none;
                    break;
                default:
                    break;
            }
        }
    }
}
