using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockField : MonoBehaviour
{
    // the time between shocks
    public float Interval = 5;
    // the bool to see if shock is active or not
    public bool ShockActive = true;
    //The particle 
    //public ParticleSystem muzzleFlash;
    // the kill trigger game object
    public GameObject KillTrigger;
    public GameObject ps;
    // the aimator for the shock
    public Animator Animator;

    //public SpriteRenderer sprite;
    public AudioSource audioSource;
    public AudioSource audioSource2;

    private void Start()
    {
        //sprite = GetComponent<SpriteRenderer>();
        
        InvokeRepeating("OnAndOff", 0, Interval);
        StartCoroutine("Inital");


    }
    private void Update()
    {
        if (ShockActive == true)
        {
            audioSource2.volume = 0;
            //Debug.Log("Not pls do something");
            ps.SetActive(false);
            
            //sprite.color = new Color(1, 1, 1, 1);
        }
      

    }
    private IEnumerator Inital()
    {
        
        yield return new WaitForSeconds(Interval);
        
       StartCoroutine("WindTimer");
       // InvokeRepeating("OnAndOffHalf", test, Interval / 2);
    }
  
    


    private IEnumerator WindTimer()
    {
        
        
        //Declare a yield instruction.
        WaitForSeconds wait = new WaitForSeconds(1);

        int i;
        for (i = 0; i < Interval; i++)
        {
            if (i >= Interval / 2)
            {
                //Debug.Log("halfway there");
                if (ShockActive == false)
                {
                    audioSource2.volume = 1;
                    //Debug.Log("pls do something");
                    ps.SetActive(true);
                   // sprite.color = new Color(1, 0, 0, 1);
                }
                else
                {
                    audioSource2.volume = 0;
                    //Debug.Log("Is active dont do anything");
                    ps.SetActive(false);
                    //sprite.color = new Color(1, 1, 1, 1);
                }
            }
            yield return wait;
            //Debug.Log(i);
        }
        i = 0;

        //float half = Interval / 2;
        // yield return new WaitForSeconds(windDuration);


        StartCoroutine("WindTimer");
    }
   

    void OnAndOff()
    {
       
        if (ShockActive == true)
        {
            audioSource.volume = 0;
            KillTrigger.SetActive(false);

            Animator.SetBool("ShockActive", false);

           
            ShockActive = false;
        }
        else
        {
            audioSource.volume = 1;
            KillTrigger.SetActive(true);
           
            Animator.SetBool("ShockActive", true);
           
           
            ShockActive = true;
        }

        //Debug.Log(ShockActive);
    }
  
}



