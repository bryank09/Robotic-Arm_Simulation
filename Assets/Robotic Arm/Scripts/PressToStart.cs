using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PressToStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("anyButton1")||Input.GetButtonDown("anyButton2")||Input.GetButtonDown("anyButton3")|| Input.GetButtonDown("pivotControl") || Input.GetButtonDown("sliderButtonChoice") || Input.GetButtonDown("buttonChoice"))
        {
            SceneManager.LoadSceneAsync(1);
        }
    }
}
