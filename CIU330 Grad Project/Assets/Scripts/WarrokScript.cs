using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarrokScript : MonoBehaviour
{
    public Transform leftPoint;
    public Transform rightPoint;
    Vector3 leftV;
    Vector3 rightV;
    bool leftP;
    public float speed;
    public float activeRange;
    public GameObject player;
    Animator anim;
    bool attack;
    bool playerNear;
    bool shotReady;
    float playerNearTimer;

    public Transform spellSP;
    public GameObject spellGO;
    void Start()
    {
        leftP = true;
        attack = false;
        playerNear = false;
        playerNear = false;
        shotReady = false;

        leftV = leftPoint.position;
        rightV = rightPoint.position;
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        anim.SetBool("Right", true);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.LookAt(player.transform.position);
        if (Vector3.Distance(transform.position, player.transform.position) < activeRange)
        {
            playerNear = true;
        }
        else
        {
            playerNear = false;

        }

        if (playerNear)
        {
            if (!shotReady)
            {
                Patrol();
            }
            else
            {
                transform.LookAt(player.transform.position);
                anim.SetBool("Shoot", true);
            }
            playerNearTimer += Time.deltaTime;
            if (playerNearTimer > 5)
            {
                shotReady = true;

            }
        }
    }
    public void SpellInstan()
    {
        GameObject temp = Instantiate(spellGO, spellSP.position, transform.rotation);
        temp.transform.LookAt(player.transform);
    }
    public void ShotEnd()
    {
        anim.SetBool("Shoot", false);
        shotReady = false;
        playerNearTimer = 0;
        transform.rotation = Quaternion.identity;
    }
    public void Patrol()
    {
        if (leftP)
        {
            transform.position = Vector3.MoveTowards(transform.position, rightV, (speed * Time.deltaTime));

            if (Vector3.Distance(transform.position, rightV) < 2)
            {
                leftP = false;

                anim.SetBool("Left", true);
                anim.SetBool("Right", false);
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, leftV, (speed * Time.deltaTime));

            if (Vector3.Distance(transform.position, leftV) < 2)
            {
                leftP = true;
                anim.SetBool("Left", false);
                anim.SetBool("Right", true);

            }
        }

    }
}
