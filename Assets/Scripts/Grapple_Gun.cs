using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Grapple_Gun : MonoBehaviour
{
    private LineRenderer lr;
    private Vector3 grapplePoint;
    Transform gunTip, player;
    GameObject nearestTarget;
    private SpringJoint joint;
    bool grapling;
    Move_Player move_Player;
    Animator anim;
    int count, force;

    void Awake()
    {
        count = 0;
        force = 800;
        anim = GameObject.Find("Player").transform.GetChild(0).GetComponent<Animator>();
        gunTip = transform.GetChild(0).transform;
        player = GameObject.Find("Player").transform;
        grapling = false;
        move_Player = player.GetComponent<Move_Player>();
        lr = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (Input.touchCount > 0 && grapling != true)
        {
            move_Player.PlayerSpeedValue(0);
            GameObject[] gmm = GameObject.FindGameObjectsWithTag("Shoot_Target");
            nearestTarget = null;
            for (int i = 0; i < gmm.Length; i++)
            {
                Debug.Log(gmm[i].name);
                if (nearestTarget == null && gmm[i].transform.position.z > player.transform.position.z + 5)
                    nearestTarget = gmm[i];
                else if(nearestTarget != null)
                {
                    if (gmm[i].transform.position.z < nearestTarget.transform.position.z && gmm[i].transform.position.z > player.transform.position.z + 5)
                        nearestTarget = gmm[i];
                }
            }
            if (nearestTarget != null)
            {
                StartGrapple();
                grapling = true;
            }
        }
        else if (Input.touchCount == 0 && grapling == true)
        {
            StopGrapple();
            grapling = false;
        }
        if (grapling)
        {
            checkPosition();
            if (player.GetComponent<Rigidbody>().velocity.z > 0)
                player.GetComponent<Rigidbody>().AddForce(Vector3.forward * force * Time.deltaTime);
        }
    }

    private void checkPosition()
    {
        if (player.transform.position.z < nearestTarget.transform.position.z + 10)
            force = 800;
        else
            force = 0;
    }

    //Called after Update
    void LateUpdate()
    {
        if(grapling == true)
            DrawRope();
    }

    /// <summary>
    /// Call whenever we want to start a grapple
    /// </summary>
    void StartGrapple()
    {
        if (count != 0)
            anim.SetTrigger("Grapple");
        count++;
        grapplePoint = nearestTarget.transform.position;
        joint = player.gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = grapplePoint;

        float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

        //The distance grapple will try to keep from grapple point. 
        joint.maxDistance = distanceFromPoint * 0.8f;
        joint.minDistance = distanceFromPoint * 0.25f;

        //Adjust these values to fit your game.
        joint.spring = 4.5f;
        joint.damper = 7f;
        joint.massScale = 4.5f;

        lr.positionCount = 2;
        currentGrapplePosition = gunTip.position;
    }


    /// <summary>
    /// Call whenever we want to stop a grapple
    /// </summary>
    void StopGrapple()
    {
        anim.SetTrigger("Grapple_Release");
        lr.positionCount = 0;
        Destroy(joint);
    }

    private Vector3 currentGrapplePosition;

    void DrawRope()
    {
        //If not grappling, don't draw rope
        if (!joint) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 8f);

        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, currentGrapplePosition);
    }

    public bool IsGrappling()
    {
        return joint != null;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }
}