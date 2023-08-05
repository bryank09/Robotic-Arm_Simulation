using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashing_Object : MonoBehaviour
{
    [SerializeField]
    public GameObject flashing_panel;

    // Start is called before the first frame update
    void Start()
    {
        FlashPanel();
    
    }
    private void FlashPanel()
    {
        InvokeRepeating("disableblink", 0.15f, 0.35f);
        InvokeRepeating("enableblink",0.15f, 0.15f);
        
    }

    private void enableblink()
    {
        flashing_panel.SetActive(true);
    }

        private void disableblink()
    {
        flashing_panel.SetActive(false);
    }
}
