using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class Robot : MonoBehaviour
{
    private bool humanCaptured = false;
    private bool returningHome = false;
    private AIDestinationSetter aids;
    public Transform robotHome;
    private HumanSpawner hs;
    public GameObject arrow;
    public Transform firstSpawnPos;
    private Animator anim;
    
    
    private void OnEnable()
    {
        anim = GetComponent<Animator>();
        hs = FindObjectOfType<HumanSpawner>();
        aids = GetComponent<AIDestinationSetter>();
        aids.target = firstSpawnPos;
    }
    private void FixedUpdate()
    {
        StartCoroutine(CheckDirection(transform.position));
    }

    IEnumerator CheckDirection(Vector3 first)
    {
        yield return new WaitForSeconds(0.3f);
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

    

    public void ChaseHuman(Human human)
    {
        if (humanCaptured) return;
        aids.target = human.transform;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Human") && !humanCaptured)
        {
            HumanCaptured(other.GetComponent<Human>());
        }
        else if (other.CompareTag("RobotHome") && returningHome)
        {
            hs.numberOfRobotsInside++;
            Destroy(gameObject);
        }
    }
    void HumanCaptured(Human human)
    {
        returningHome = false;
        gameObject.layer = LayerMask.NameToLayer("RobotBusy");
        human.GotCaught();
        humanCaptured = true;
        human.transform.parent = this.transform;
        SetDestination(human);
    }

    private void SetDestination(Human human)
    {
       Transform newDestination = human.GetHomePos();
       aids.target = newDestination;
    }

    public void HumanReturned()
    {
        aids.target = robotHome;
        humanCaptured = false;
        returningHome = true;

    }

    public void HumanEscaped()
    {
        aids.target = robotHome;
        humanCaptured = false;
        returningHome = true;
    }

    private void OnMouseDown()
    {
        foreach (Arrow go in FindObjectsOfType<Arrow>())
        {
            FindObjectOfType<Arrow>().gameObject.SetActive(false);
        }
        arrow.SetActive(true);
        hs.RobotSelected(this);
    }

    
}
