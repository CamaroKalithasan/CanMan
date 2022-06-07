using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamControl : MonoBehaviour
{
    public Camera playerCam;

    // Update is called once per frame
    void Update()
    {
        playerCam.transform.position = new Vector3(transform.position.x, 
                                                    transform.position.y, 
                                                    playerCam.transform.position.z);
    }
}
