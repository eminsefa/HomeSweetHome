using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotHome : MonoBehaviour
{
    public GameObject robotPrefab;

    private HumanSpawner hs;
    void Start()
    {
        hs = FindObjectOfType<HumanSpawner>();
    }
    private void OnMouseDown()
    {
        if (hs.numberOfRobotsInside > 0)
        {
            SpawnRobot();
            hs.numberOfRobotsInside--;
        }
        else if (hs.numberOfRobotsInside == 0)
        {
            if (hs.humanPoint >= 10)
            {
                SpawnRobot();
                hs.humanPoint -= 10;
                hs.numberOfRobots++;
                hs.StartSpawn();
                hs.SetTexts();
            }
            else
            {
                return;
            }
        }
    }

    private void SpawnRobot()
    {
        GameObject robot = Instantiate(robotPrefab, transform.position, Quaternion.identity);
        hs.RobotSelected(robot.GetComponent<Robot>());
        foreach (Arrow go in FindObjectsOfType<Arrow>())
        {
            go.gameObject.SetActive(false);
        }
        robot.GetComponent<Robot>().arrow.SetActive(true);
        
    }
}
