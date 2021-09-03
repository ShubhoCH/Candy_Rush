
using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    int enemyIndex;
    Transform player;
    float maxIgnorance;
    bool startInteraction, tooClose;
    Animator animEnemy;
    public GameObject childItem;
    private void Awake()
    {
        tooClose = false;
        maxIgnorance = 0;
        enemyIndex = int.Parse(transform.name);
        player = GameObject.Find("Player").transform;
        if(enemyIndex != 6)
            animEnemy = GetComponent<Animator>();
    }
    private void Update()
    {
        if(startInteraction != true)
        {
            if(Mathf.Abs(Vector3.Distance(transform.position, player.transform.position)) > maxIgnorance)
                startInteraction = true;
        }
        else
            Interact(enemyIndex);     
    }

    private void Interact(int index)
    {
        if (index == 4)
        {
            if (Vector3.Distance(player.position, transform.position) < 50f)
                animEnemy.SetTrigger("isScared");
        }
        //5:Carrot
        if (index == 5)
        {
            if (transform.position.z < player.transform.position.z + 20 && tooClose != true)
            {
                tooClose = true;
                transform.parent.parent.gameObject.AddComponent<Rigidbody>();
                transform.parent.parent.gameObject.GetComponent<Rigidbody>().freezeRotation = true;
                transform.parent.parent.GetComponent<Rigidbody>().AddForce(Vector3.up * 600);
            }
            if(tooClose == true)
                transform.parent.parent.position = Vector3.MoveTowards(transform.parent.parent.position, player.transform.position, 7 * Time.deltaTime);
        }
        //6: Egg
        if (index == 6)
        {
            if (transform.position.z < player.transform.position.z + 80)
            {
                Debug.Log("TooClose");
                animEnemy = transform.parent.gameObject.GetComponent<Animator>();
                animEnemy.SetTrigger("TooClose");
                animEnemy.speed = 2;
                Destroy(GetComponent<Enemy>());
            }
        }
        //7: Pebble Throw:
        if(index == 7)
        {
            transform.GetChild(0).localPosition = -transform.GetChild(0).GetChild(1).localPosition + new Vector3(0,.5f,0);
            transform.LookAt(player);
            if(Vector3.Distance(player.transform.position, transform.position) < 25 && tooClose != true)
            {
                transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("Throw");
                Invoke("Throw", 2.4f);
                tooClose = true;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (enemyIndex == 1 && collision.transform.name == "Player")
            animEnemy.SetTrigger("isKilled");
        if (enemyIndex == 5 && collision.transform.name == "Player")
            collision.gameObject.GetComponent<Move_Player>().Dead(true);
        if (enemyIndex == 6 && collision.transform.name == "Player")
            collision.gameObject.GetComponent<Move_Player>().Dead(true);
    }
    void Throw()
    {
        GameObject gm = Instantiate(childItem);
        gm.transform.position = transform.GetChild(0).GetChild(1).GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).position;
        gm.AddComponent<Shoot_Item_Behaviour>().targetTransform = player;
    }
}
