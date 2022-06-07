using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class misc_buttons : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleActive(GameObject _tog)
    {
        _tog.SetActive(!_tog.activeSelf);
    }
}
