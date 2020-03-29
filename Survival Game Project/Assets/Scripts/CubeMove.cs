using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMove : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }
    public void Movement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(new Vector3(0.0f, moveSpeed * Time.deltaTime, 0.0f)); //move up
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(new Vector3(0.0f, -moveSpeed * Time.deltaTime, 0.0f)); //move down
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(new Vector3(-moveSpeed * Time.deltaTime, 0.0f, 0.0f)); //move left
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(new Vector3(moveSpeed * Time.deltaTime, 0.0f, 0.0f)); //move right
        }
    }
}
