using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class Human : MonoBehaviour
{

   
    private Transform homePos;
    private bool goingHome = false;
    HumanSpawner hs;
    private AIDestinationSetter aids;
    Robot robot;
    Transform escapeTarget;
    private Animator anim;
    private void OnEnable()
    {
        anim = GetComponent<Animator>();
        hs = FindObjectOfType<HumanSpawner>();
        aids = GetComponent<AIDestinationSetter>();
        aids.target = FindEscapeTarget();
        
    }
    private void FixedUpdate()
        {
            if(goingHome) return;
            StartCoroutine(CheckDirection(transform.position));
        }
    
        IEnumerator CheckDirection(Vector3 first)
        {
            yield return new WaitForSeconds(0.1f);
            Vector3 second = transform.position;
            if (second.y >first.y)
            {
                anim.SetTrigger("WalkBack");
            }
            else if (second.y < first.y)
            {
                anim.SetTrigger("WalkFront");
            }
        }

    private Transform FindEscapeTarget()
    {
        float minDistance = Mathf.Infinity;
        foreach (EscapeTarget go in FindObjectsOfType<EscapeTarget>())
        {
            float distance = Vector3.Distance(transform.position, go.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                escapeTarget = go.transform;
            }
        }
        return escapeTarget;
    }
    public void GotCaught()
    {
        GetComponent<AIPath>().canMove = false;
        aids.target = null;
        gameObject.layer = LayerMask.NameToLayer("HumanCaught");
        goingHome = true;
    }

    public Transform SetHomePos(Transform other)
    {
        homePos = other;
        return homePos;
    }
    public Transform GetHomePos()
    {
        return homePos;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Robot"))
        {
            robot = other.GetComponent<Robot>();
        }
        if (other.CompareTag("SpawnPos") && goingHome)
        {
            hs.HumanReturned(robot);
            Destroy(gameObject);
        }

        if (other.CompareTag("EscapePos"))
        {
            hs.HumanEscaped(robot);
            Destroy(gameObject);
        }
    }
    private void OnMouseDown()
    {
        robot = hs.GetRobot();
        robot.gameObject.layer = LayerMask.NameToLayer("Robot");
        robot.ChaseHuman(this);
    }
}
