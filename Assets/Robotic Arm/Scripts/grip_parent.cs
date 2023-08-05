using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grip_parent : MonoBehaviour
{
    public GameObject childObject;
    public GameObject rightTwezer;
    public GameObject targetCube;
    public Transform cubeParent;

    private grip_object childScript;
    private right_grip rightTouched;
    bool haveGrip = false;

    // Start is called before the first frame update
    void Start()
    {
        childScript = childObject.GetComponentInChildren<grip_object>();
        rightTouched = rightTwezer.GetComponentInChildren<right_grip>();
    }

    // Update is called once per frame
    void Update()
    {
        if (childScript != null && childScript.collidedWithTarget && rightTouched != null && rightTouched.rightCollidedWithTarget)
        {
            // Code to execute when the child object has collided with the target object
            //haveGrip = true;
            targetCube.GetComponent<Rigidbody>().useGravity = false;
            targetCube.transform.SetParent(transform, true);

        }
        //else if (haveGrip == true && !childScript.collidedWithTarget && !controllerScript.moving)
        else 
        {
            //haveGrip = false;
            targetCube.GetComponent<Rigidbody>().useGravity = true;
            targetCube.transform.SetParent(cubeParent, true);
        }
    }
}