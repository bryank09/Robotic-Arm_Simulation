using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grip_object : MonoBehaviour
{
    //public GameObject myHands; //reference to your hands/the position where you want your object to go
    public bool collidedWithTarget; //a bool to see if you can or cant pick up the item
    GameObject ObjectIwantToPickUp; // the gameobject onwhich you collided with
    public bool hasItem; // a bool to see if you have an item in your hand

    public bool CollidedWithTarget
    {
        get { return collidedWithTarget; }
    }

    void Start()
    {
        collidedWithTarget = false;    //setting both to false
        hasItem = false;
    }

    private void OnTriggerEnter(Collider other) // to see when the player enters the collider
    {
        if (other.gameObject.tag == "target") //on the object you want to pick up set the tag to be anything, in this case "object"
        { 
            collidedWithTarget = true;  //set the pick up bool to true
        }
    }

    private void OnTriggerExit(Collider other)
    {
        collidedWithTarget = false; //when you leave the collider set the collidedWithTarget bool to false
    }
}
