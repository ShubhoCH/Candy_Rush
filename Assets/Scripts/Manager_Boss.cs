using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager_Boss : MonoBehaviour
{
    bool started, moveToLocation, startSpanning, spanned;
    public GameObject[] Bosses;
    int bossIndex;
    float curTime, maxTime;
    public GameObject canvas1, canvas2, Bazuka;
    Transform battlePoint, Player;
    public Transform[] spawnPosition;
    void Start()
    {
        curTime = 0;
        maxTime = 10;
        moveToLocation = startSpanning = spanned = false;
        Player = GameObject.Find("Player").transform;
        started = false;
        bossIndex = PlayerPrefs.GetInt("BOSS", 0);
        bossIndex = bossIndex % 3;
        var x = Instantiate(Bosses[bossIndex], transform);
        x.transform.localPosition = new Vector3(0, 0, 5);
        bossIndex += 1;
        x.name = bossIndex.ToString();
        battlePoint = GameObject.Find("Boss_Level").transform.GetChild(GameObject.Find("Boss_Level").transform.childCount - 1);
    }
    private void Update()
    {
        if(moveToLocation == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, battlePoint.position, 15 * Time.deltaTime);
            if (transform.position == battlePoint.position)
            {
                moveToLocation = false;
                Player.gameObject.GetComponent<Move_Player>().PlayerSpeedValue(7);
                Player.SetParent(null); 
                canvas2.SetActive(true);
                transform.Find(bossIndex.ToString()).GetComponent<Boss>().Kill();
                startSpanning = true;
            }
        }
        if(startSpanning == true && spanned != true)
        {
            curTime += Time.deltaTime;
            if (curTime >= maxTime)
            {
                curTime = 0;
                spanned = true;
                GameObject gmm = Instantiate(Bazuka);
                gmm.name = "8";
                gmm.transform.position = spawnPosition[Random.Range(0, spawnPosition.Length)].position - (Vector3.up * 2);
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Player" && started != true)
        {
            started = true;
            moveToLocation = true;
            canvas1.SetActive(false);
            Player.SetParent(transform);
            Player.GetComponent<Move_Player>().BossLevel();
            Player.GetChild(0).GetComponent<Animator>().SetBool("isRunning", false);
            Player.gameObject.GetComponent<Move_Player>().PlayerSpeedValue(0);
            transform.Find("CM").GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority = 2;
            transform.Find("CM").GetComponent<Cinemachine.CinemachineVirtualCamera>().LookAt = transform.Find(bossIndex.ToString());
            transform.Find("CM").GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = Player.transform;
        }
    }
    public void Collected() => spanned = false;
}
