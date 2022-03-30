using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public GameObject Health;
    public GameObject Health2;
    public GameObject trigger;
    public GameObject R;
    
    void OnTriggerStay(Collider other)
    {
        R.SetActive(true);

        if (Input.GetKeyDown(KeyCode.R)) { Health.SetActive(false); Health2.SetActive(true); trigger.SetActive(false); R.SetActive(false); } 
       
    }

    
    void OnTriggerExit(Collider other)
    {
        R.SetActive(false);
    }
}
