using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    public void tutorial()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void menu()
    {
        SceneManager.LoadSceneAsync(0);
    }

}
