using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCollision : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "target")
        {
            print("ENTER");
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "target")
        {
            print("STAY");
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "target")
        {
            print("EXIT");
        }
    }
}
