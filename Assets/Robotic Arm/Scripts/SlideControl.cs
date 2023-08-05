using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideControl : MonoBehaviour
{
    public Slider slider;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.speed = 0;
    }

    public void moveGrip()
    {
        anim.Play("gripper", -1, slider.normalizedValue);
    }
}