using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }
    //public GameObject object1;
    //public int count = 0;


    // Update is called once per frame
    public void Loadscene()
    {
        SceneManager.LoadScene(2);
        //count += 1;

        //if (count % 2 == 0)
        //{
        //    object1.SetActive(false);
        //}
        //else
        //{
        //    object1.SetActive(true);
        //}
    }
}