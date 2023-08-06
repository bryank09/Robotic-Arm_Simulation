# Robotic-Arm_Simulation
![image](https://github.com/bryank09/Robotic-Arm_Simulation/assets/107078925/0762d658-994d-4b75-9d56-62642d6981ce)

Table of Content

## About the Project
With Augmented Reality (AR) and Virtual Reality (VR) emerged, we can harness the functionality offered by them to enhance user experience when designing an application. In this project, those technology was used to develop a Robotic Arm Simulation Application. Through this application, users will be able to simulate the operations of a robotic arm, such as picking up objects and moving them to desired destination. 

This project mainly focus on achieving these 3 objectives,
- Simulate a real world operation of a Robotic Armature movement inside Unity.
- Saving the degree of freedom (DOF) or rotational points of the Robotic Armature on a series of task.
- Generate an immersive and user-friendly procedure during simulation.

By creating a simulation that doesn't need any component or tools, industrial activities that involves a robotic armature could be evaluated and tested of their possible action.

### Built With
- ![Unity](https://img.shields.io/badge/unity-%23000000.svg?style=for-the-badge&logo=unity&logoColor=white) __2018.4.36__
- ![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white)
- ![Google](https://img.shields.io/badge/google-4285F4?style=for-the-badge&logo=google&logoColor=white) __Google VR SDK__
- __Vuforia__
- ![Android Studio](https://img.shields.io/badge/Android%20Studio-3DDC84.svg?style=for-the-badge&logo=android-studio&logoColor=white)

## Getting Started
### User Prerequisites
- Android Phone
- Google VR Head Set
- Joystick Controller

### User Installation
- Install application APK: https://drive.google.com/drive/folders/1wU5zciZLA-gxMuRcDp46-2fCdCwtstsg?usp=sharing
> *I'm very sorry I couldn't find a way to upload the application to github as its over 25mb :(*

### Developer Prerequisites
- Install Unity v2018.4.36 at: https://unity.com/releases/editor/archive
- Install Vuforia SDK Unity at: https://developer.vuforia.com/downloads/sdk
- Install android studio at: https://developer.android.com/studio?gclid=CjwKCAjw5remBhBiEiwAxL2M94JO-4HcHwtSmyBrWxHAJMQr0hyDYNcXPbcOXV46PiKF2LmrLyWKZBoCj0sQAvD_BwE&gclsrc=aw.ds
- Install Google VR SDK at: https://github.com/googlevr/gvr-unity-sdk/releases

### Developer Installation
- Clone/download the files in the repo and open in unity as a project.
- Add your vuforia licence in Unity __Hierarchy>ARCamera>Inspector>Open Vuforia Engine Configuration
> **Side Note:** you might need to create and configure vuforia to your unity first: https://developer.vuforia.com/
> The GameObject declared inside the script might need to be reconfigure to the Arm_Controller inspector

## Usage
All the Scenes is located in Assets>Robotic Arm>Scenes
> **Side Note:** when navigating with a different type of controller, you might need to edit the input in Unity __Edit>Project Settings>Input__ and build the application again.

### Menu Scene ( Menu.unity )
![image](https://github.com/bryank09/Robotic-Arm_Simulation/assets/107078925/668f921a-2439-495b-8443-fc77cd301f2b)

- Menu scene is the first scene that the user will see and acts as a starting page for our application.
- Click any button to go to the tutorial Scene

### Tutorial Scene ( Sample_tutorial.unity )
![image](https://github.com/bryank09/Robotic-Arm_Simulation/assets/107078925/9f8fbb43-d796-4420-aefa-2f99d4a05c20)

- Tutorial Scene is where we would describe the functionalities and purpose of each object and components presented in the simulation.
- User could navigate through the tutorial or skip to the simulation using the buttons in the panel.

### Simulation Scene ( Sample.unity )
![image](https://github.com/bryank09/Robotic-Arm_Simulation/assets/107078925/7c027c44-99b5-4a86-9151-00ae9720d9b7)

There are 2 main controls inside the simulation scene, the sliders and button. Users could navigate between them by clicking a button. Both controls are used and controlled by the joystick to execute / adjust the robotic arm functionality to achieve the desired objective in the simulation.

### Sliders
The Sliders purpose is to control the robotic armature rotation. User navigates the slider by clicking a button in the controller.

Each slider represent a robotic arm component, which has been labeled in the picture above, to rotate
- Base: Base Arm
- Upper Arm: Upper Arm
- Lower Arm: Lower Arm
- End Rotation: End Ring
- Gear: End Gear
- Gripper: Gripper

### Buttons
The button has all the functionalities to achieve the objective of saving the rotational points of the robotic arm. Buttons are navigated the same way sliders are.

- RESET: Reset the whole scene to its original position.
- SAVE: Save current rotational points of the robotic arm component to a List.
- DELETE PIVOT: Delete the saved pivots that has been made by the user.
- EXECUTE: Rotate the robotic arm component to the saved position made.
- DROP: Drop the object that has been picked up.
- TUTORIAL: Go back to the tutorial scene.

> **Example/Base Usage: The user to adjust the robotic arm using the sliders and they click the save button, user could continue to save multiple saved points. User clicks the execute button to rotate each component to the saved points iteratively**

## Survey Result
The Robotic Arm Simulation Application was released to 10 users between the age of 17 - 23 with a deliberate selection of two distinct user groups. The first group comprised users who have real-life experience in using robotic arms, while the second group consisted of users with no prior experience in this field. By assessing the application's usability and user experience among users with varying backgrounds, we aimed to ensure that all users, regardless of their experience level, could interact with our application.


### Usability Results
![image](https://github.com/bryank09/Robotic-Arm_Simulation/assets/107078925/c16b2e8a-de0b-4ec9-ad9b-57e2484d8cd1)

### Ease of Learning Result
![image](https://github.com/bryank09/Robotic-Arm_Simulation/assets/107078925/4e3a6f3e-7440-4732-bbd9-58e829b160b9)

### Satisfaction Result
![image](https://github.com/bryank09/Robotic-Arm_Simulation/assets/107078925/fddc7aa6-acb9-43e0-b4ae-06ec7e1251c1)

## Limitation
We also found some limitations throughout the whole simulation; the first limitation is tracking accuracy of the robot gripper. The identification and synchronization of the AR markers with the real-word environment were not always accurate, which might cause the objects to move randomly or to go missing. Second the RigidBody object kept falling down the plane in an AR setting, that is why we freeze the target object so it only have the rigidbody effect when the gripper has the posession of the object.

## Project Contributors
- Bryan Keane
- Lim Zhi Min
- Osama Hisham Mosaad Ibrahim Abusetta
- Kong Ping Hao
- Ahmed Rafat Abdelaziz Soliman Elkilany
