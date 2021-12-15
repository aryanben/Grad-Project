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
    public float shootRange;
    public GameObject player;
    Animator anim;
    bool attack;
    bool healing;
    bool isShooting;
    bool playerNear;
    bool shotReady;
    float playerNearTimer;
    public ParticleSystem fireSparks;
    public ParticleSystem healSparks;
    public Transform spellSP;
    public GameObject spellGO;
    bool initAnim;
    public float health;
    public float playerDmg;
    public Transform healPoint;
    float yCord;
    void Start()
    {
        yCord = transform.position.y;
        leftP = true;
        attack = false;
        healing = false;
        isShooting = false;
        playerNear = false;
        playerNear = false;
        shotReady = false;
        initAnim = false;

        health = 100;
        leftV = leftPoint.position;
        rightV = rightPoint.position;
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        anim.SetBool("Run", false);
        anim.SetBool("Shoot", false);
        anim.SetBool("Idle", true);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position.Set(transform.position.x, yCord, transform.position.z);
        //transform.LookAt(player.transform.position);
        if (Vector3.Distance(transform.position, player.transform.position) < activeRange)
        {
            playerNear = true;
        }
        else
        {
            playerNear = false;

        }


        if (health < 35 || healing)
        {
            SeekHeal();
        }
        else if (playerNear)
        {
            if (!shotReady)
            {

                if (!initAnim)
                {



                }
            }
            else
            {
                SeekPlayer();
                //if (true)
                //{
                //    transform.LookAt(player.transform.position);
                //    anim.SetBool("Shoot", true);
                //}

            }
            playerNearTimer += Time.deltaTime;
            if (playerNearTimer > 5)
            {
                shotReady = true;

            }
        }
        else
        {
            SeekHeal();
            //initAnim = false;
            
        }

    }
    public void SeekPlayer()
    {

        if (Vector3.Distance(transform.position, player.transform.position) <= shootRange)
        {
            transform.LookAt(player.transform.position);
            anim.SetBool("Shoot", true);
            isShooting = true;
        }
        else
        {
            transform.LookAt(player.transform.position);
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, (speed * Time.deltaTime));
            anim.SetBool("Run", true);
            anim.SetBool("Shoot", false);
            anim.SetBool("Idle", false);
        }

    }
    public void SeekHeal()
    {
        if (Vector3.Distance(transform.position, healPoint.position) < .3f)
        {
            //heal

            if (health < 100)
            {
                if (!healSparks.isPlaying)
                {
                    healSparks.Play();
                }

                healing = true;
                health += 2 * Time.deltaTime;

            }
            else if (health >= 100)
            {
                healing = false;
                healSparks.Stop();
            }

        }
        else
        {

            transform.LookAt(healPoint);
            transform.position = Vector3.MoveTowards(transform.position, healPoint.position, (speed * Time.deltaTime));
            anim.SetBool("Run", true);
            anim.SetBool("Shoot", false);
            anim.SetBool("Idle", false);
        }


    }
    public void Heal()
    {



    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "FireBall")
        {
            fireSparks.Play();
            health -= playerDmg;
            Destroy(collision.gameObject);
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
        isShooting = false;
        playerNearTimer = 0;
        //transform.rotation = Quaternion.identity;
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
