using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    string bossName;
    Transform Player;
    bool Interact, dead, hit;
    public Animator animBoss;
    public GameObject childItems;
    float maxTime, curTime, Health;
    private void Start()
    {
        bossName = transform.name;
        if (transform.name == "1")
            maxTime = 1;
        else
            maxTime = 5;
        curTime = 0;
        if (transform.name == "1")
            Health = 50;
        else if (transform.name == "2")
            Health = 60;
        else if (transform.name == "3")
            Health = 70;
        Interact = hit = dead = false;
        Player = GameObject.Find("Player").transform;
    }
    private void Update()
    {
        if (Interact && !dead)
        {
            if (bossName == "1")
                KillOrbeKilled();
            else if (bossName == "2")
                StartSmashing();
            else if (bossName == "3")
                StartSpitting();
        }
    }

    private void KillOrbeKilled()
    {
        if(!hit)
            curTime += Time.deltaTime;
        if (curTime > maxTime)
        {
            curTime = 0;
            var gmm = Instantiate(childItems);
            gmm.transform.position = transform.GetChild(1).position;
            gmm.transform.localScale = new Vector3(.3f, .3f, .3f);
            var x = gmm.transform.Find("Mushroom").GetChild(0).gameObject.GetComponent<ParticleSystem>().main;
            x.startLifetime = 1;
            x.startSpeed = 1;
            var y = gmm.transform.Find("Mushroom").GetChild(0).gameObject.GetComponent<ParticleSystem>().emission;
            y.rateOverTime = 10;
            gmm.AddComponent<Destroy>();
            gmm.transform.GetChild(4).GetChild(1).localScale = new Vector3(4, 4, 4);
        }
    }
    void StartSmashing()
    {
        transform.GetChild(0).GetChild(1).transform.localPosition = new Vector3(0, transform.GetChild(0).GetChild(1).transform.localPosition.y, 0);
        if(!hit)
            curTime += Time.deltaTime;
        if (curTime < maxTime - .5f)
            transform.GetChild(0).LookAt(Player.transform);
        if (Vector3.Distance(Player.transform.position, transform.position) < 12)
            animBoss.SetTrigger("TooClose"); 
        else if (curTime > maxTime)
        {
            curTime = 0;
            animBoss.SetTrigger("Fight");
        }
    }
    void StartSpitting()
    {
        transform.GetChild(0).GetChild(1).transform.localPosition = new Vector3(0, transform.GetChild(0).GetChild(1).transform.localPosition.y, 0);
        if (!hit)
            curTime += Time.deltaTime;
        if (curTime < maxTime - 1f)
            transform.GetChild(0).LookAt(Player.transform);
        if (Vector3.Distance(Player.transform.position, transform.position) < 11)
        {
            animBoss.SetTrigger("Attack");
        }
        else if (curTime > maxTime && hit!= true)
        {
            curTime = 0;
            animBoss.SetTrigger("Spit");
            Invoke("Spit", 1.4f);
        }
    }
    public void Kill()
    {
        animBoss.SetTrigger("Fight");
        Interact = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
            Player.GetComponent<Move_Player>().Die(true);
    }
    public void Damage()
    {
        Health -= 10;
        hit = true;
        Debug.Log(Health);
        if (Health <= 0 && dead != true)
        {
            dead = true;
            animBoss.SetTrigger("Dead");
            Player.GetComponent<Move_Player>().Won();
            GameObject.Find("Canvas_Joystick").SetActive(false);
        }
        else
            animBoss.SetTrigger("Hit");
        Invoke("Recover", 3);
    }
    bool Recover() => hit = false;
    void Spit()
    {
        GameObject gm = Instantiate(childItems);
        gm.transform.position = transform.GetChild(0).Find("Instantiate_here").position;
        gm.GetComponent<Rigidbody>().AddForce(transform.GetChild(0).Find("Instantiate_here").forward * 1500);
    }
}
