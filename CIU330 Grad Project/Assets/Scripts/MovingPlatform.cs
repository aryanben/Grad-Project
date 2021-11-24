using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private GameObject target = null;
    private Vector3 offset;
    void Start()
    {
        target = null;
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    target = other.gameObject;
    //    Debug.Log(target.name);
    //}
    void OnTriggerStay(Collider col)
    {
        
        target = col.gameObject;
       
        offset = target.transform.position - transform.position;
    }
    void OnTriggerExit(Collider col)
    {
        target = null;
    }
    void LateUpdate()
    {
        if (target != null)
        {
            target.transform.position = transform.position + offset;
        }
    }
}
