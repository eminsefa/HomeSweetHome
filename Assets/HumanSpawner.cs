using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class HumanSpawner : MonoBehaviour
{ 
    public Transform[] spawnPosArray;
    private Transform spawnPos;

    public GameObject humanPrefab;
    private Robot selectedRobot;

    public int numberOfRobotsInside = 1;
    public int numberOfRobots = 1;
    public int humanPoint = 0;

    public Text numberOfRobotsText;
    public Text humanPointText;
    public GameObject pausePanel;

    private void Start()
    {
        StartSpawn();
        SetTexts();
    }

    public void StartSpawn()
    {
        CancelInvoke("SpawnHuman");
        float spawnSpeed = 6f - numberOfRobots;
        if (numberOfRobots >= 6) spawnSpeed = 0.5f;
        InvokeRepeating("SpawnHuman",0f,spawnSpeed);
    }


    Transform RandomSpawnPos()
    {
        var i = Random.Range(0, spawnPosArray.Length);
        spawnPos = spawnPosArray[i];
        return spawnPos;
    }

    public void SpawnHuman()
    {
        GameObject human = Instantiate(humanPrefab, RandomSpawnPos().position, Quaternion.identity);
        human.GetComponent<Human>().SetHomePos(spawnPos);
    }

    public void RobotSelected(Robot robot)
    {
        selectedRobot = robot;
    }

    public Robot GetRobot()
    {
        return selectedRobot;
    }

    public void HumanReturned(Robot robot)
    {
        humanPoint++;
        SetTexts();
        if(robot!=null) robot.HumanReturned();
    }

    public void HumanEscaped(Robot robot)
    {
        if(robot!=null) robot.HumanEscaped();
        if (humanPoint == 0)
        {
            SceneManager.LoadScene(0);
        }
        humanPoint--;
        SetTexts();
    }

    public void SetTexts()
    {
        numberOfRobotsText.text = "Number Of Robots:" + numberOfRobots;
        humanPointText.text = "Sweet Home Point:" + humanPoint;
    }

    public void PauseButtonTapped()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }

    public void ResumeButtonTapped()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

    public void RestartButtonTapped()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void QuitButtonTapped()
    {
        Application.Quit();
    }
    
}
