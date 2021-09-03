using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    // Global Variables:
    #region
    int newGame;
    public float speedBee;
    bool inFlight, inProgress, isWaiting, inProcessing;
    public GameObject Player, Bee, platform;
    //For Automation:
    public GameObject[] Hurdles;
    Vector3 startPosition, dropPosition, endPosition;
    Animator anim_Bee, anim_Player;
    #endregion
    private void Awake()
    {
        Animator trampoline = null;
        GameObject[] gmm = GameObject.FindGameObjectsWithTag("Spring");
        for (int i = 0; i < gmm.Length; i++)
        {
            if (i == 0 && gmm[i].transform.position.z > transform.position.z)
                trampoline = gmm[0].GetComponent<Animator>();
            else
            {
                if (gmm[i].transform.position.z < trampoline.transform.position.z && gmm[i].transform.position.z > transform.position.z)
                    trampoline = gmm[i].GetComponent<Animator>();
            }
        }
        if (trampoline != null)
            Player.GetComponent<Move_Player>().spring = trampoline;
    }
    private void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex > 23)
            SetLevel_Automated();
        //PlayerPrefs.SetInt("NEWGAME", 0);
        //newGame = PlayerPrefs.GetInt("NEWGAME", 0);
        newGame = 1;

        //Assign Player Transform to the Cinemachine cameras:
        transform.GetChild(1).GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = Player.transform;
        transform.GetChild(1).GetComponent<Cinemachine.CinemachineVirtualCamera>().LookAt = Player.transform;
        //Is new Game?
        NewGame();
        //else
        //    NewGame();
    }
    private void Update()
    {
        if(newGame == 0)
        {
            if (isWaiting)
                Bee.transform.RotateAround(endPosition, Vector3.up, 30 * Time.deltaTime);
            else if (inFlight)
            {
                Bee.transform.position = Vector3.MoveTowards(Bee.transform.position, dropPosition, speedBee * Time.deltaTime);
                Player.transform.position = Bee.transform.position - new Vector3(.3f, 3, .1f);
                if (Bee.transform.position == dropPosition)
                {
                    //Drop Player:
                    anim_Bee.SetTrigger("DROP");
                    Player.GetComponent<Rigidbody>().useGravity = true;
                    anim_Player.SetTrigger("START");
                    anim_Player.SetBool("isJumping", true);
                    inFlight = false;
                    inProcessing = true;
                }
            }
            else if (inProcessing == true)
            {
                if (Player.transform.position.y - startPosition.y < 1)
                {
                    inProcessing = false;
                    inProgress = true;
                    Player.GetComponent<Move_Player>().enabled = true;
                    GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>().Priority = 0;
                    PlayerPrefs.SetInt("NEWGAME", 1);
                }
            }
            else if (inProgress == true)
            {
                Bee.transform.position = Vector3.MoveTowards(Bee.transform.position, endPosition, speedBee * Time.deltaTime);
                if (Bee.transform.position == endPosition)
                {
                    inProgress = false;
                    isWaiting = true;
                    endPosition += new Vector3(0, 0, 10);
                }
            }
        }
        else
        {
            if (inProgress == true)
            {
                Bee.transform.position = Vector3.MoveTowards(Bee.transform.position, endPosition, speedBee * Time.deltaTime);
                if (Bee.transform.position == endPosition)
                {
                    inProgress = false;
                    endPosition += new Vector3(0, 0, 10);
                }
            }
            else
                Bee.transform.RotateAround(endPosition, Vector3.up, 30 * Time.deltaTime);
        }
    }
    void NotNewGame()
    {
        Bee = Instantiate(Bee);
        inFlight = true;
        inProgress = isWaiting = inProcessing = false;
        startPosition = GameObject.Find("Platform").transform.GetChild(0).Find("StartPoint").position;
        dropPosition = startPosition + new Vector3(0, 20, 0);
        Bee.transform.position = dropPosition + new Vector3(-25, 40, -150);
        Bee.transform.rotation = Quaternion.Euler(0, 0, 0);
        Player.transform.position = Bee.transform.position - new Vector3(.3f, 3, .1f);
        endPosition = GameObject.Find("Platform").transform.Find("END").transform.Find("PickUpPoint").position;
        endPosition += new Vector3(0, 35, -10);
        anim_Bee = Bee.GetComponentInChildren<Animator>();
        anim_Player = Player.GetComponentInChildren<Animator>();
        Player.GetComponent<Rigidbody>().useGravity = false;
        Player.GetComponent<Move_Player>().enabled = false;
        inFlight = true;
    }
    void NewGame()
    {
        Bee = Instantiate(Bee);
        Player.GetComponent<Move_Player>().enabled = true;
        Player.transform.position = GameObject.Find("Platform").transform.GetChild(0).Find("StartPoint").position + Vector3.up;
        endPosition = GameObject.Find("Platform").transform.Find("END").transform.Find("PickUpPoint").position;
        endPosition += new Vector3(0, 35, -10);
        Bee.transform.position = endPosition;
        endPosition += new Vector3(0, 0, 10);
        Player.GetComponent<Rigidbody>().useGravity = true;
        GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>().Priority = 0;
    }
    void SetLevel_Automated()
    {
        platform = Instantiate(platform);
        platform.transform.position = Vector3.zero;
        Transform Parent = GameObject.Find("Obstacle").transform;
        for(int i = 0; i < 4; i++)
        {
            GameObject gmm = Instantiate(Hurdles[Random.Range(0, Hurdles.Length)]);
            gmm.transform.SetParent(Parent);
            if (i != 0)
                gmm.transform.position = Parent.GetChild(Parent.childCount - 1).Find("Anchor").position;
            else
                gmm.transform.position = Parent.position;
        }
        platform.transform.GetChild(1).transform.position = Parent.GetChild(Parent.childCount - 1).Find("Anchor").position - new Vector3(0, 0, 6.25f);
    }
}
