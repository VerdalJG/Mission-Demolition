using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode
{
    idle,
    playing,
    levelEnd
}

public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S; // A private singleton -- Singleton is a class that allows only one instance of an object to be created

    [Header("Set in Inspector")]
    public Text uitLevel; // Reference to UIText_Level
    public Text uitShots; // Reference to UIText_Shots
    public Text uitButton; // Reference to UIText_Button's text component
    public Text uitBestScores; // Reference to UIText_BestScores
    public Vector3 castlePos; // The place to put the castles
    public GameObject[] castles; // An array of castles


    [Header("Set Dynamically")]
    public int level; // The current level
    public int levelMax; // The max level count
    public int shotsTaken; // The shot count
    public GameObject castle; // The current castle
    public GameMode mode = GameMode.idle; // Reference to enumerator to set an idle state for the game
    public string showing = "Show Slingshot"; // FollowCam mode
    public static int bestScoreL1 = 10; // Best Score Level 1
    public static int bestScoreL2 = 10; // Best Score Level 2
    public static int bestScoreL3 = 10; // Best Score Level 3
    public static int bestScoreL4 = 10; // Best Score Level 4
    [HideInInspector] public GameObject slingshot; // Reference to slingshot object in scene
    private Slingshot shotScript; // Reference to slingshot script

    void Awake()
    {
        //If the high scores already exists, read it, else - Assign the high scores to their respective levels
        if (PlayerPrefs.HasKey("Level1BestScore")) // If the key 'Level1BestScore' exists
        {
            bestScoreL1 = PlayerPrefs.GetInt("Level1BestScore"); // Read whatever value is stored with this key
        }
        else // If the key doesn't exist
        {
            PlayerPrefs.SetInt("Level1BestScore", bestScoreL1); // Create the key and set the value from the variable bestScoreL1
        }

        if (PlayerPrefs.HasKey("Level2BestScore"))
        {
            bestScoreL2 = PlayerPrefs.GetInt("Level2BestScore");
        }
        else
        {
            PlayerPrefs.SetInt("Level2BestScore", bestScoreL2);
        }

        if (PlayerPrefs.HasKey("Level3BestScore"))
        {
            bestScoreL3 = PlayerPrefs.GetInt("Level3BestScore");
        }
        else
        {
            PlayerPrefs.SetInt("Level3BestScore", bestScoreL3);
        }

        if (PlayerPrefs.HasKey("Level4BestScore"))
        {
            bestScoreL4 = PlayerPrefs.GetInt("Level4BestScore");
        }
        else
        {
            PlayerPrefs.SetInt("Level4BestScore", bestScoreL4);
        }
    }

    void Start()
    {
        S = this; //Define the singleton
        level = 0; // Set level to 0 on start
        levelMax = castles.Length; // The maximum amount of levels is the length of the array of castles, which is 4, unless we add more castles.
        StartLevel(); // Call start level
        uitBestScores.text = "Level " + (level + 1) + " Best Score: " + PlayerPrefs.GetInt("Level1BestScore"); // Set level 1 best score text
    }

    // Called on level complete
    void NextLevel()
    {
        level++; // Add +1 to level
        Goal.goalMet = false; // When we start the next level, reset the goalMet boolean to false
        if (level == levelMax) // If we pass the 4th level
        {
            level = 0; // Restart the level number to 0
        }
        StartLevel(); // Call function StartLevel
    }

    //Called on start and after NextLevel()
    void StartLevel()
    {
        //Destroy old castles
        if (castle != null) // Check if castle exists - if game object castle is NOT null, hence if castle exists
        {
            Destroy(castle); // Destroy castle
        }

        //Destroy old projectiles
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile"); // Create an array of gameobjects, save projectiles in the array
        foreach (GameObject pTemp in gos) // For each projectile in gos
        {
            Destroy(pTemp); // Destroy projectiles
        }

        //Instantiate new castle
        castle = Instantiate(castles[level]); // Create current castle using information from the array of castles, picking the appropriate level
        castle.transform.position = castlePos; // Place current castle at fixed castle position
        shotsTaken = 0; // Set shot counter to 0

        //Reset the camera
        SwitchView("Show Both");

        Goal.goalMet = false; // Reset the goal's status

        UpdateGUI(); //Call UpdateGUI every frame

        mode = GameMode.playing;
        
        // Update the best score text on level start
        switch (level)
        {
            case 0:
                uitBestScores.text = "Level " + (level + 1) + " Best Score: " + bestScoreL1; // Set text based on level
                 break;

            case 1:
                uitBestScores.text = "Level " + (level + 1) + " Best Score: " + bestScoreL2;
                 break;

            case 2:
                uitBestScores.text = "Level " + (level + 1) + " Best Score: " + bestScoreL3;
                 break;

            case 3:
                uitBestScores.text = "Level " + (level + 1) + " Best Score: " + bestScoreL4;
                 break;
        }
        slingshot = GameObject.Find("SlingShot"); //Find slingshot in scene
        shotScript = slingshot.GetComponent<Slingshot>(); //Get slingshot script component from the slingshot game object
        shotScript.powerUp = 0; // Reset PowerUps
    }

    // Update UI Information Every Frame - Called in Update.
    void UpdateGUI()
    {
        //Show the data in GUITexts in the canvas
        uitLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        uitShots.text = "Shots Taken: " + shotsTaken;

        if (Goal.goalMet) // Did we win?
        {
            switch (level) // What level are we on?
            {
                case 0: // Level 1
                    if (shotsTaken < PlayerPrefs.GetInt("Level1BestScore")) // Is shotsTaken less than our high score?
                    {
                        bestScoreL1 = shotsTaken; // Modify Variable
                        PlayerPrefs.SetInt("Level1BestScore", bestScoreL1); // Save the high score
                    }
                    uitBestScores.text = "Level " + (level + 1) + " Best Score: " + bestScoreL1; // Update text
                    break;

                case 1: // Level 2
                    if(shotsTaken < PlayerPrefs.GetInt("Level2BestScore"))
                    {
                        bestScoreL2 = shotsTaken;
                        PlayerPrefs.SetInt("Level2BestScore", shotsTaken);
                    }
                    uitBestScores.text = "Level " + (level + 1) + " Best Score: " + bestScoreL2;
                    break;

                case 2:
                    if (shotsTaken < PlayerPrefs.GetInt("Level3BestScore"))
                    {
                        bestScoreL3 = shotsTaken;
                        PlayerPrefs.SetInt("Level3BestScore", shotsTaken);
                    }
                    uitBestScores.text = "Level " + (level + 1) + " Best Score: " + bestScoreL3;
                    break;

                case 3:
                    if (shotsTaken < PlayerPrefs.GetInt("Level4BestScore"))
                    {
                        bestScoreL4 = shotsTaken;
                        PlayerPrefs.SetInt("Level4BestScore", shotsTaken);
                    }
                    uitBestScores.text = "Level " + (level + 1) + " Best Score: " + bestScoreL4;
                    break;

            }
        }
    }

    void Update()
    {
        UpdateGUI(); //Update GUI every frame (could be optimized?)

        if ((mode == GameMode.playing) && Goal.goalMet) //Check for level end
        {
            mode = GameMode.levelEnd; //Set mode to level end, which also causes it to stop checking if it has ended

            SwitchView("Show Both"); // Zoom out

            Invoke("NextLevel", 2f); //Start next level in 1 second
        }
    }

    public void SwitchView(string eview = "") // Function that also declares a string variable 'eview' that starts as null
    {
        if (eview == "") // If eview is null
        {
            eview = uitButton.text; // Set eview to whatever text is displayed on the button
        }

        showing = eview; // Set showing variable to the text string currently on eview so that it can work as a cycle
        switch (showing) // Set the camera point of interest to the respective case based on the cycle
        {
            case "Show Slingshot":
                FollowCam.POI = null; // Reset camera back to starting position
                uitButton.text = "Show Castle";  // Set button text to cycle with eview and showing
                break;

            case "Show Castle":
                FollowCam.POI = S.castle; // Set camera to the current castle
                uitButton.text = "Show Both"; // Text cycle
                break;

            case "Show Both":
                FollowCam.POI = GameObject.Find("ViewBoth"); // Set camera to 'ViewBoth's' position
                uitButton.text = "Show Slingshot"; // Text cycle
                break;


        }

    }

    // Called when we release a projectile
    public static void ShotFired()
    {
        S.shotsTaken++; // Add +1 to shots taken variable.
    }
    
    // Called from the button in canvas
    public void ResetLevel()
    {
        StartLevel(); // Starts the level again
    }
}