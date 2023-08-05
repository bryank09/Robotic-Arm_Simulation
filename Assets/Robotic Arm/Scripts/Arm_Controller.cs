using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class Arm_Controller : MonoBehaviour
{
    public GameObject targetCube;
    //public grip_parent controllerScript;
    //public Transform cubeParent;
    //public bool moving=false;
    public Rigidbody targetCubeRb;

    public Slider baseSlider;
    public Slider armSlider;
    public Slider lowerArmSlider;
    public Slider endRingSlider;
    public Slider endGearSlider;

    // slider value for base platform that goes from -1 to 1.
    private float baseSliderValue = 0.0f;
    private float upperArmSliderValue = 0.0f;
    private float lowerArmSliderValue = 0.0f;
    private float endRingSliderValue = 0.0f;
    private float endGearSliderValue = 0.0f;

    // These slots are where you will plug in the appropriate arm parts into the inspector.
    public Transform robotBase;
    public Transform upperArm;
    public Transform lowerArm;
    public Transform endRing;
    public Transform endGear;

    // Allow us to have numbers to adjust in the inspector the speed of each part's rotation.
    private float baseTurnRate = 1.0f;
    private float upperArmTurnRate = 1.0f;
    private float lowerArmTurnRate = 1.0f;
    private float endRingTurnRate = 1.0f;
    private float endGearTurnRate = 1.0f;

    //rotation limits
    private float baseYRot = 0.00f;
    private float baseYRotMin = -180.0f;
    private float baseYRotMax = 180.0f;

    private float upperArmXRot = 0.00f;
    private float upperArmXRotMin = -45.0f;
    private float upperArmXRotMax = 45.0f;

    private float lowerArmXRot = 0.00f;
    private float lowerArmXRotMin = -45.0f;
    private float lowerArmXRotMax = 45f;

    private float endRingXRot = -70.0f;
    private float endRingXRotMin = -89.0f;
    private float endRingXRotMax = 20.0f;

    private float endGearYRot = 0.00f;
    private float endGearYRotMin = -90.0f;
    private float endGearYRotMax = 90f;

    // For highlighting Effect
    public Material highlightMaterial;
    public Material originalMaterial1, originalMaterial2;
    private Renderer renderer;

    // For Gripper Control
    public Slider gripperSlider;
    public Transform endSlider;
    private Animator anim;
    private List<Pivots> savedPivots = new List<Pivots>();
    string[] pivotType = { "lowerArm", "upperArm", "endRing", "endGear", "base", "gripper" };
    bool initialSaved = true;

    // For storing the buttons
    private Button[] buttons;
    private int activeButtonIndex = 0;
    private bool isSelectingButton = true;
    private bool isSelectingSliders = true;

    // Color of the buttons
    private Color normalButtonColor;

    // Active Slider Name
    private float sliderNameOffset = 0.5f;

    // Gripper Animation Control
    public void moveGrip()
    {
        anim.Play("gripper", -1, gripperSlider.normalizedValue);
    }

    void Start()
    {
        //cubeParent = targetCube.transform.parent;
        //targetCube.transform.SetParent(cubeParent, true);

        /* Set default values to that we can bring our UI sliders into negative values */
        baseSlider.minValue = -1;
        armSlider.minValue = -1;
        baseSlider.maxValue = 1;
        armSlider.maxValue = 1;
        lowerArmSlider.minValue = -1;
        lowerArmSlider.maxValue = 1;
        endRingSlider.minValue = -1;
        endRingSlider.maxValue = 1;
        endGearSlider.minValue = -1;
        endGearSlider.maxValue = 1;


        anim = GetComponent<Animator>();
        anim.speed = 0;
        //targetCube = controllerScript.targetCube;

        // Get all the buttons in the canvas
        buttons = GameObject.FindObjectsOfType<Button>();

        // Set the text for active slider to false (Hide)
        savedPivots.Add(new Pivots(-11.10f, 38.10f, -26.07f, 14.42f, 79.42f, 0.45f));
        savedPivots.Add(new Pivots(-7.61f, 17.55f, -30.24f, 14.42f, -21.54f, 0f));
        resetPivots();
    }
    
    public void Dropcube()
    {
        StartCoroutine(UnfreezeYAxis());
    }

    private IEnumerator UnfreezeYAxis()
    {
        // Unfreeze the Y axis
        targetCubeRb.constraints &= ~RigidbodyConstraints.FreezePositionY;
        targetCubeRb.constraints &= ~RigidbodyConstraints.FreezePositionZ;
        targetCubeRb.constraints &= ~RigidbodyConstraints.FreezePositionX;

        // Wait for the specified time before freezing the Y axis again
        yield return new WaitForSeconds(2f);

        // Freeze the Y axis again
        targetCubeRb.constraints |= RigidbodyConstraints.FreezePositionY;
        targetCubeRb.constraints |= RigidbodyConstraints.FreezePositionZ;
        targetCubeRb.constraints |= RigidbodyConstraints.FreezePositionX;
    }

    public void automaticPivot()
    {
        Debug.Log("execute the command");
        StartCoroutine(IterateSavedPivots());
    }

    private IEnumerator IterateSavedPivots()
    {
        foreach (Pivots savedPivot in savedPivots)
        {
            int i = 0;
            Type type = savedPivot.GetType();
            FieldInfo[] pivots = type.GetFields(BindingFlags.Instance | BindingFlags.Public);

            // Use a flag to check if the inner loop is completed
            bool innerLoopFinished = false;

            StartCoroutine(PivotComponents(pivots, savedPivot, () =>
            {
                // Inner loop is completed
                innerLoopFinished = true;
            }));

            // Wait until the inner loop is finished before moving to the next iteration of the outer loop
            while (!innerLoopFinished)
            {
                yield return null;
            }
        }
    }

    private IEnumerator PivotComponents(FieldInfo[] pivots, Pivots savedPivot, System.Action onComplete)
    {
        int i = 0;

        foreach (FieldInfo pivot in pivots)
        {
            float pivotValue = Convert.ToSingle(pivot.GetValue(savedPivot));

            if (pivotType[i] == "gripper")
            {
                yield return StartCoroutine(delayGripperAnim(pivotValue));
            }
            else
            {
                yield return StartCoroutine(RotateTowardsSavePivot(pivotValue, pivotType[i]));
            }

            i++;
        }

        // Call the onComplete action to signal the inner loop is completed
        onComplete?.Invoke();
    }

    IEnumerator RotateTowardsSavePivot(float savedPivot, string pivotType)
    {
        float rotationSpeed = 0.3f;
        float tolerance = 0.5f;

        int i = 0;
        switch (pivotType)
        {
            case "lowerArm":
                while (Mathf.Abs(lowerArmXRot - savedPivot) > tolerance && i < 200000)
                {
                    if (savedPivot < lowerArmXRot)
                    {
                        lowerArmXRot -= rotationSpeed;
                    }
                    else
                    {
                        lowerArmXRot += rotationSpeed;
                    }

                    i++;
                    yield return null;  // Wait for the next frame
                }

                lowerArmXRot = savedPivot;
                break;
            case "upperArm":
                while (Mathf.Abs(upperArmXRot - savedPivot) > tolerance && i < 200000)
                {
                    if (savedPivot < upperArmXRot)
                    {
                        upperArmXRot -= rotationSpeed;
                    }
                    else
                    {
                        upperArmXRot += rotationSpeed;
                    }

                    i++;
                    yield return null;
                }

                upperArmXRot = savedPivot;
                break;
            case "endRing":
                while (Mathf.Abs(endRingXRot - savedPivot) > tolerance && i < 200000)
                {
                    if (savedPivot < endRingXRot)
                    {
                        endRingXRot -= rotationSpeed;
                    }
                    else
                    {
                        endRingXRot += rotationSpeed;
                    }

                    i++;
                    yield return null;  // Wait for the next frame
                }
                endRingXRot = savedPivot;
                break;
            case "endGear":
                while (Mathf.Abs(endGearYRot - savedPivot) > tolerance && i < 200000)
                {
                    if (savedPivot < endGearYRot)
                    {
                        endGearYRot -= rotationSpeed;
                    }
                    else
                    {
                        endGearYRot += rotationSpeed;
                    }

                    i++;
                    yield return null;  // Wait for the next frame
                }

                endGearYRot = savedPivot;
                break;
            case "base":
                while (Mathf.Abs(baseYRot - savedPivot) > tolerance && i < 200000)
                {
                    if (savedPivot < baseYRot)
                    {
                        baseYRot -= rotationSpeed;
                    }
                    else
                    {
                        baseYRot += rotationSpeed;
                    }

                    i++;
                    yield return null;
                }

                baseYRot = savedPivot;
                break;
        }
    }

    IEnumerator delayGripperAnim(float savedPivot)
    {
        //yield return new WaitForSeconds(5);
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        float currentTime = stateInfo.normalizedTime;
        int i = 0;

        while (Mathf.Abs(currentTime - savedPivot) > 0.05f && i < 200000)
        {
            if (savedPivot < currentTime)
            {
                currentTime -= 0.01f;
            }
            else
            {
                currentTime += 0.01f;
            }

            anim.Play("gripper", -1, currentTime);
            i++;

            yield return null; // Wait for the next frame
        }

        currentTime = savedPivot;
    }

    public void resetPivots()
    {
        float[] initialPivot = { 0.00f, 0.00f, -70.0f, 0.00f, 0.00f, 0f };
        int i = 0;
        foreach (float pivotValue in initialPivot)
        {
            StartCoroutine(RotateTowardsSavePivot(pivotValue, pivotType[i]));
            i++;
        }
        //targetCube.transform.SetParent(cubeParent, true);
        //UnityEngine.Debug.Log("check");
        targetCube.transform.localRotation = Quaternion.Euler(0.0f, -180.0f, 0.0f);
        //targetCube.localEulerAngles = new Vector3(0.0f, -180.0f, 0.0f);
        targetCube.transform.localPosition = new Vector3(-0.1885648f, 0.01859072f, 0.06483545f);
        StartCoroutine(unFreezeCube());

    }

    private IEnumerator unFreezeCube()
    {
        // Unfreeze the Y axis
        targetCubeRb.constraints &= ~RigidbodyConstraints.FreezePositionY;
        targetCubeRb.constraints &= ~RigidbodyConstraints.FreezePositionZ;
        targetCubeRb.constraints &= ~RigidbodyConstraints.FreezePositionX;

        // Wait for the specified time before freezing the Y axis again
        yield return new WaitForSeconds(1f);

        // Freeze the Y axis again
        targetCubeRb.constraints |= RigidbodyConstraints.FreezePositionY;
        targetCubeRb.constraints |= RigidbodyConstraints.FreezePositionZ;
        targetCubeRb.constraints |= RigidbodyConstraints.FreezePositionX;
    }

    public void addSavedPivot()
    {
        if (initialSaved == true)
        {
            savedPivots.Clear();
            initialSaved = false;
        }
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        float currentTime = stateInfo.normalizedTime;
        Pivots currentPivot = new Pivots(upperArmXRot, lowerArmXRot, endRingXRot, endGearYRot, baseYRot, currentTime);
        savedPivots.Add(currentPivot);
    }

    public void deleteSavedPivots()
    {
        initialSaved = true;
        savedPivots.Clear();
        savedPivots.Add(new Pivots(-11.10f, 38.10f, -26.07f, 14.42f, 79.42f, 0.5f));
        savedPivots.Add(new Pivots(-7.61f, 17.55f, -30.24f, 14.42f, -21.54f, 0f));
    }
    void CheckInput()
    {
        //getting real time value from robot armature
        baseSliderValue = baseSlider.value;
        upperArmSliderValue = armSlider.value;
        lowerArmSliderValue = lowerArmSlider.value;
        endRingSliderValue = endRingSlider.value;
        endGearSliderValue = endGearSlider.value;
    }

    public void ProcessMovement()
    {
        //rotating our base of the robot here around the Y axis and multiplying
        //the rotation by the slider's value and the turn rate for the base.
        baseYRot += baseSliderValue * baseTurnRate;
        //clamp so the rotation cant go max/min of our set 
        baseYRot = Mathf.Clamp(baseYRot, baseYRotMin, baseYRotMax);
        //alow on which xyz plane the parts of the robot can rotate on
        robotBase.localEulerAngles = new Vector3(robotBase.localEulerAngles.x, baseYRot, robotBase.localEulerAngles.z);

        //rotating our upper arm of the robot here around the X axis and multiplying
        //the rotation by the slider's value and the turn rate for the upper arm.
        upperArmXRot += upperArmSliderValue * upperArmTurnRate;
        upperArmXRot = Mathf.Clamp(upperArmXRot, upperArmXRotMin, upperArmXRotMax);
        upperArm.localEulerAngles = new Vector3(upperArmXRot, upperArm.localEulerAngles.y, upperArm.localEulerAngles.z);

        lowerArmXRot += lowerArmSliderValue * lowerArmTurnRate;
        lowerArmXRot = Mathf.Clamp(lowerArmXRot, lowerArmXRotMin, lowerArmXRotMax);
        lowerArm.localEulerAngles = new Vector3(lowerArmXRot, lowerArm.localEulerAngles.y, lowerArm.localEulerAngles.z);

        endRingXRot += endRingSliderValue * endRingTurnRate;
        endRingXRot = Mathf.Clamp(endRingXRot, endRingXRotMin, endRingXRotMax);
        endRing.localEulerAngles = new Vector3(endRingXRot, endRing.localEulerAngles.y, endRing.localEulerAngles.z);

        endGearYRot += endGearSliderValue * endGearTurnRate;
        endGearYRot = Mathf.Clamp(endGearYRot, endGearYRotMin, endGearYRotMax);
        endGear.localEulerAngles = new Vector3(endGear.localEulerAngles.x, endGear.localEulerAngles.y, endGearYRot);

    }
    public void ResetSliders()
    {
        //resets the sliders back to 0 when you lift up on the mouse click down (snapping effect)
        baseSliderValue = 0.0f;
        upperArmSliderValue = 0.0f;
        baseSlider.value = 0.0f;
        armSlider.value = 0.0f;
        lowerArmSliderValue = 0.0f;
        lowerArmSlider.value = 0.0f;
        endRingSliderValue = 0.0f;
        endRingSlider.value = 0.0f;
        endGearSliderValue = 0.0f;
        endGearSlider.value = 0.0f;
        //gripperSlider.value = 0.0f;
    }
    void Update()
    {
        //CheckInput();
        CheckKeyboardInput();
        //ChekJoystickInput();
        ProcessMovement();
        //targetCube = controllerScript.targetCube;
    }
    // Code added for keyboard/Joystick Input
    //void ChekJoystickInput()
    void CheckKeyboardInput()
    {
        // Get the joystick input values for the horizontal axis
        //float horizontal = Input.GetAxis("HorizontalControl");
        float vertical = Input.GetAxis("VerticalControl");

        // Obtain the values from robot armature first
        CheckInput();

        // Check for keyboard input to select active slider
        if (Input.GetButtonDown("pivotControl"))  // { joystick button 0} --> Positive Button 
        {
            // Check if sliders are being selected
            if (isSelectingSliders)
            {
                SelectNextSlider();
            }
            else // Buttons are being selected
            {
                SelectNextButton();
            }
        }

        // Check for keyboard input to select active slider or button
        //if (Input.GetKeyDown(KeyCode.DownArrow))  // {KeyCode.DownArrow} --> For Keyboard
        if (Input.GetButtonDown("sliderButtonChoice"))  // { joystick button 1} --> Positive Button 
        {
            // Toggle between selecting buttons or sliders
            isSelectingButton = !isSelectingButton;

            // Highlight the active slider or button based on the current mode
            if (isSelectingButton)
            {
                SelectNextButton();
            }
            else
            {
                // Reset the highlight of all buttons back to normal 
                foreach (Button button in buttons)
                {
                    ColorBlock colors = button.colors;
                    colors.normalColor = Color.white;
                    button.colors = colors;
                }
                SelectNextSlider();
            }
        }
        // Check for keyboard input to control active slider's value
        Slider activeSlider = GetActiveSlider();

        // Check for keyboard input to control active slider's value
        //if (Input.GetKey(KeyCode.LeftArrow)) // {KeyCode.LeftArrow} --> For Keyboard
        if (vertical == -1)
        {
            // Decrease the value of the active slider
            DecreaseSliderValue();
        }
        else if (vertical == 1)
        {
            // Increase the value of the active slider
            IncreaseSliderValue();
        }
        else if (Input.GetButtonDown("ButtonChoice")) //{ joystick button 3} --> Positive Button
        {
            Debug.Log("TESTING button");
            // Perform the action of the active button
            buttons[activeButtonIndex].onClick.Invoke();
        }
        else
        {
            // Snap the active slider value back to 0
            ResetSliders();
        }

    }

    private int activeSliderIndex = -1;
    void SelectNextSlider()
    {
        // Determine the index of the currently active slider
        Slider[] sliders = { baseSlider, armSlider, lowerArmSlider, endRingSlider, endGearSlider, gripperSlider };
        Transform[] pivots = { robotBase, upperArm, lowerArm, endRing, endGear, endSlider };

        // Reset the active button index to the first button
        activeButtonIndex = 0;

        // selecting slider flag set to true
        isSelectingSliders = true;

        //// Disable all buttons
        //foreach (Button button in buttons)
        //{
        //    button.gameObject.SetActive(false);
        //}

        //Check which slider is currently active for the (activeSliderIndex)
        for (int i = 0; i < sliders.Length; i++)
        {
            if (sliders[i].gameObject.activeInHierarchy)
            {
                activeSliderIndex = i;
                break;
            }
        }

        // Disable all slider
        sliders[0].gameObject.SetActive(false);
        sliders[1].gameObject.SetActive(false);
        sliders[2].gameObject.SetActive(false);
        sliders[3].gameObject.SetActive(false);
        sliders[4].gameObject.SetActive(false);
        sliders[5].gameObject.SetActive(false);

        // Enable the next slider in the array
        int nextSliderIndex = activeSliderIndex + 1;
        if (nextSliderIndex >= sliders.Length)
        {
            nextSliderIndex = 0; // Wrap around to the first slider if at the end
        }
        // Activating the next slider
        sliders[nextSliderIndex].gameObject.SetActive(true);

        // Getting the original render of the current pivot
        renderer = pivots[nextSliderIndex].GetComponent<Renderer>();

        // Chaning to highlighted material
        renderer.material = highlightMaterial;

        // Removing the highlight from the rest of the pivots
        foreach (Transform t in pivots)
        {
            if (t != pivots[nextSliderIndex])
            {
                if (t != endRing || t != endGear)
                    t.GetComponent<Renderer>().material = originalMaterial1;
                else
                    t.GetComponent<Renderer>().material = originalMaterial2;
            }
        }

        //// Update the Text element to display the name of the currently active slider
        //if (sliderNameText != null)
        //{
        //    sliderNameText.text = sliders[nextSliderIndex].name;

        //    // Enable the UI text object 
        //    sliderNameText.gameObject.SetActive(true);
        //    sliderNameText.enabled = true;  // Enable the Text (Script) ~ Content of the text UI element 

        //    // Position the UI Text slightly above the active slider's handle
        //    RectTransform handleRectTransform = sliders[nextSliderIndex].transform.Find("Handle Slide Area/Handle").GetComponent<RectTransform>();
        //    Vector3 handlePosition = handleRectTransform.position;
        //    if (sliders[nextSliderIndex].name == "gripper Slider")
        //    {
        //        // Offset the gripper slider by 40 from the left (Different slider Handle position)
        //        sliderNameText.transform.position = handlePosition + new Vector3(0f, sliderNameOffset, 0f);
        //    }
        //    else
        //    {
        //        sliderNameText.transform.position = handlePosition + new Vector3(0f, sliderNameOffset, 0f);
        //    }

        //}


    }

    void SelectNextButton()
    {
        Transform[] pivots = { robotBase, upperArm, lowerArm, endRing, endGear, endSlider };

        // Reset the active slider index to the first slider
        activeSliderIndex = 0;

        // Deactivate the slider choice
        isSelectingSliders = false;

        // Disable all slider
        baseSlider.gameObject.SetActive(false);
        armSlider.gameObject.SetActive(false);
        lowerArmSlider.gameObject.SetActive(false);
        endRingSlider.gameObject.SetActive(false);
        endGearSlider.gameObject.SetActive(false);
        gripperSlider.gameObject.SetActive(false);

        // Removing the highlight element from the pivots
        foreach (Transform pivot in pivots)
        {
            Renderer pivotRenderer = pivot.GetComponent<Renderer>();
            if (pivotRenderer != null)
            {
                pivotRenderer.material = originalMaterial1; // Change to the appropriate original material here
            }
        }

        // Activate the next button in the array
        activeButtonIndex++;
        if (activeButtonIndex >= buttons.Length)
        {
            activeButtonIndex = 0; // Wrap around to the first button if at the end
        }
        // Activating the next button
        buttons[activeButtonIndex].gameObject.SetActive(true);

        // Store the normal color of the selected button
        normalButtonColor = buttons[activeButtonIndex].colors.normalColor;

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
        colors = buttons[previousButtonIndex].colors;
        colors.normalColor = normalButtonColor; // Reset the color to the normal color
        buttons[previousButtonIndex].colors = colors;
        Debug.Log(colors);
    }

    void DecreaseSliderValue()
    {
        Slider activeSlider = GetActiveSlider();
        Debug.Log(activeSlider);

        if (activeSlider != null && activeSlider != gripperSlider)
        {
            activeSlider.value -= Time.deltaTime * 1.10f;
        }
        else
        {
            activeSlider.value -= 0.05f;
        }
    }

    void IncreaseSliderValue()
    {
        Slider activeSlider = GetActiveSlider();
        Debug.Log(activeSlider);

        if (activeSlider != null && activeSlider != gripperSlider)
        {
            activeSlider.value += Time.deltaTime * 1.10f;
        }
        else
        {
            activeSlider.value += 0.05f;
        }
    }

    Slider GetActiveSlider()
    {
        Slider[] sliders = { baseSlider, armSlider, lowerArmSlider, endRingSlider, endGearSlider, gripperSlider };

        foreach (Slider slider in sliders)
        {
            if (slider.gameObject.activeInHierarchy)
            {
                return slider;
            }
        }
        return null;
    }
}

public class Pivots
{
    public float upperArmPivot, lowerArmPivot, endRingPivot, endGearPivot, basePivot, gripperAnim;
    public Pivots(float lowerArmXRot, float upperArmXRot, float endRingXRot, float endGearYRot, float baseYRot, float gripperanim)
    {
        this.lowerArmPivot = lowerArmXRot;
        this.upperArmPivot = upperArmXRot;
        this.endRingPivot = endRingXRot;
        this.endGearPivot = endGearYRot;
        this.basePivot = baseYRot;
        this.gripperAnim = gripperanim;
    }

}
