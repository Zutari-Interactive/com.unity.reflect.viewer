using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lookat : MonoBehaviour
{
    public Transform cam;
    public float turnSpeed;
    float x = 0f;
    float z = 0f;

    //public GameObject[] cams;

    private void Start()
    {
        //if(cams.Length == 0)
        //{
        //    Debug.LogError("no cameras on lookat");
        //}
        //else
        //{
        //    cam = cams[0].transform;
        //}

        cam = Camera.main.transform;
    }

    void FixedUpdate()
    {

        var lookPos = cam.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(-lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * turnSpeed);

    }

    public void SwitchCamera(int i)
    {
        //cam = cams[i].transform;
    }
}
