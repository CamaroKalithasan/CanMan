using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehavior : MonoBehaviour
{
    //public ToggleableOject target;

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player") || collider.gameObject.CompareTag("Box"))
        {
            //if(target.locked) target.locked = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //target.locked = true;
    }
}
