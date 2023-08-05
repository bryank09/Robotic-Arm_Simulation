using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class skipInTutorial : MonoBehaviour
{

    public void skip()
    {
        SceneManager.LoadSceneAsync(2);
    }
}
