using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    public bool onGround;
    public float speed;
    public float jumpSpeed;
    public float dashSpeed;
    public float dashTime;
    Rigidbody rb;
    public float mH;
    public float mV;
    Vector3 dashV;
    public ParticleSystem dashPE;
    public ParticleSystem eSparkPE;
    public ParticleSystem longSpellPE;
    public ParticleSystem portalFirePE;
    public GameObject fireballPE;
    public ParticleSystem footprintPE;
    public Transform spellSP;
    public Transform footSP;
    public float slowerFloat;
    Animator anim;
    bool isShooting;
    public int speckCount;
    public int speckGoal;
    public bool nearPortal;
  
    public Text speckCountText;
    private void Awake()
    {
        isShooting = false;
        nearPortal = false;
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        anim = GetComponent<Animator>();
        speckCountText.text = $"Portal Specks : {speckCount}/{speckGoal}";
    }

    // Update is called once per frame
    void Update()
    {
      
        mV = Input.GetAxis("Vertical");

        if (mV < 0.5f && mV > -0.5f)
        {
            mH = Input.GetAxis("Horizontal");
        }
        else
        {
            mH = 0;
        }

        if (!isShooting)
        {
            rb.velocity = transform.TransformDirection(mH * speed, rb.velocity.y, mV * speed);
        }






        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // rb.AddForce(Vector3.forward * dashSpeed);
            StartCoroutine(Dash());
        }
        if (Input.GetKey(KeyCode.W))
        {
            anim.SetBool("isRunning", true);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (onGround)
                {
                    anim.SetTrigger("Jump");
                }
                else if (Physics.Raycast(transform.position, -Vector3.up,.1f))
                {
                   // onGround = true;
                    anim.SetTrigger("Jump");
                   
                }
               
            }
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
        if (Input.GetKey(KeyCode.S))
        {
            anim.SetBool("backRun", true);
        }
        else
        {
            anim.SetBool("backRun", false);
        }
        if (Input.GetKey(KeyCode.A))
        {
            anim.SetBool("leftRun", true);
        }
        else
        {
            anim.SetBool("leftRun", false);
        }
        if (Input.GetKey(KeyCode.D))
        {
            anim.SetBool("rightRun", true);
        }
        else
        {
            anim.SetBool("rightRun", false);
        }
        // if (!Input.GetKey(KeyCode.W))
        //{
        //    anim.SetBool("isRunning", false);
        //}
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("fastSpell");
            isShooting = true;

        }
        if (nearPortal)
        {
            if (Input.GetKeyUp(KeyCode.F))
            {
                if (speckCount>=speckGoal)
                {
                    anim.SetBool("longSpell", true);
                    isShooting = true;
                }
                else
                {
                    //not enough specks brh
                }
                
            }
        }
        //var emm = footprintPE.emission;
        //if (onGround)
        //{
        //    emm.rateOverDistanceMultiplier = 0;
        //}
        //else 
        //{
        //    emm.rateOverDistanceMultiplier =1;
        //}

    }
    private void FixedUpdate()
    {
        if (isShooting)
        {
            // rb.velocity.Set(rb.velocity.x* slowerFloat, rb.velocity.y * slowerFloat, rb.velocity.z * slowerFloat);
            rb.velocity = rb.velocity * slowerFloat;
            // rb.velocity = Vector3.zero;
        }
    }
    IEnumerator Dash()
    {
        float startTime = Time.time;
        dashPE.Play();
        while (Time.time < startTime + dashTime)
        {
            dashV = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(dashV * dashSpeed * Time.deltaTime);

            yield return null;
        }


    }
    public void FireSpell()
    {
        Instantiate(fireballPE, spellSP.position, transform.rotation);

    }
    public void FireSpellEnd()
    {

        isShooting = false;
    }
    public void LongSpell()
    {
        longSpellPE.Play();
        portalFirePE.Play();
    }
    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

       
    }
        public void LongSpellEnd()
    {
        anim.SetBool("longSpell", false);
        longSpellPE.Stop();
        isShooting = false;
    }
    public void JumpEvent()
    {
        
        rb.AddForce(Vector3.up * jumpSpeed);
    }
    public void FootPrint()
    {
        Instantiate(footprintPE, footSP.position, transform.rotation);

    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            footprintPE.Play();
            // footprintPE.enableEmission = true;
            // footprintPE.SetActive(true);
            onGround = false;
        }
        if (collision.gameObject.tag == "Portal")
        {
            nearPortal = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            onGround = true;
            // footprintPE.enableEmission = false;
            footprintPE.Stop();
            //  footprintPE.SetActive(false);
        }
        if (collision.gameObject.tag == "PlasmaBall")
        {
            eSparkPE.Play();
        }
        if (collision.gameObject.tag == "Speck")
        {
            Destroy(collision.gameObject);
            speckCount++;
            speckCountText.text = $"Portal Specks : {speckCount}/{speckGoal}";
        }
        if (collision.gameObject.tag == "Portal")
        {
            nearPortal = true;
        } 
        if (collision.gameObject.tag == "PortalFire")
        {
            if (portalFirePE.isPlaying)
            {
                SceneManager.LoadScene(2);
            }
            
        }

    }
   
   
}
