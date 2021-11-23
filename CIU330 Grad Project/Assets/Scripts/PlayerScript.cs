using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
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
    public Transform spellSP;

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
}
