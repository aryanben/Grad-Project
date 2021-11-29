using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public GameObject fireballPE;
    public ParticleSystem footprintPE;
    public Transform spellSP;
    public Transform footSP;

    Animator anim;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        anim = GetComponent<Animator>();   
    }

    // Update is called once per frame
    void Update()
    {
        mH = Input.GetAxis("Horizontal");
        mV = Input.GetAxis("Vertical");
        rb.velocity = transform.TransformDirection(mH * speed, rb.velocity.y, mV * speed);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpSpeed);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // rb.AddForce(Vector3.forward * dashSpeed);
            StartCoroutine(Dash());
        }
        if (Input.GetKey(KeyCode.W))
        {
            anim.SetBool("isRunning",true);
        }
         if (!Input.GetKey(KeyCode.W))
        {
            anim.SetBool("isRunning", false);
        }
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("fastSpell");
           
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
        
    }
    IEnumerator Dash() 
    {
        float startTime = Time.time;
        dashPE.Play();
        while (Time.time<startTime+dashTime)
        {
            dashV = new Vector3(rb.velocity.x,0, rb.velocity.z);
            rb.AddForce(dashV * dashSpeed * Time.deltaTime);
            
            yield return null;
        }
    
    
    }
    public void FireSpell() 
    {
        Instantiate(fireballPE, spellSP.position, transform.rotation);

    }
    public void FootPrint() 
    {
        Instantiate(footprintPE, footSP.position, transform.rotation);

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag=="Ground")
        {
            onGround = true;
           // footprintPE.enableEmission = false;
            footprintPE.Stop();
          //  footprintPE.SetActive(false);
        }
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
    }
}
