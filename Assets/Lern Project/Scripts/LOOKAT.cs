using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOOKAT : MonoBehaviour
{

    public GameObject Player;

    
    void Update()
    {
        gameObject.transform.LookAt(Player.transform.position); 
    }
}
