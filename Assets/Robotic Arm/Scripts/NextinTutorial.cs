using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NextinTutorial : MonoBehaviour
{
    public GameObject panel;
    public GameObject oriImage;
    public GameObject newImage;
    private int activeButtonIndex = 0;
    Button[] buttons;

    // Color of the buttons
    private Color normalButtonColor;

    public void NextImage()
    {
        oriImage.SetActive(false);
        newImage.SetActive(true);
    }

    void Start()
    {
        buttons = panel.GetComponentsInChildren<Button>();
        ColorBlock colors = buttons[0].colors;
        colors.normalColor = Color.yellow;
        buttons[0].colors = colors;
    }
    void Update()
    {
        if (Input.GetButtonDown("pivotControl"))
        {
            activeButtonIndex++;
            if (activeButtonIndex >= buttons.Length)
            {
                activeButtonIndex = 0;
            }

            // Highlight the selected button by changing its color
            ColorBlock colors = buttons[activeButtonIndex].colors;
            colors.normalColor = Color.yellow; // Set the highlight color
            buttons[activeButtonIndex].colors = colors;

            // Reset the color of the previously selected button
            int previousButtonIndex = activeButtonIndex - 1;
            if (previousButtonIndex < 0)
            {
                previousButtonIndex = buttons.Length - 1; // Wrap around to the last button if at the beginning
            }

            colors.normalColor = Color.white; // Reset the color to the normal color
            buttons[previousButtonIndex].colors = colors;
        }

        if (Input.GetButtonDown("ButtonChoice"))
        {
            
            buttons[activeButtonIndex].onClick.Invoke();
        }
    }
}
