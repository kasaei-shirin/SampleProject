using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    float inputX, inputZ,inputY;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        inputX = Input.GetAxis("Horizontal");
        inputZ = Input.GetAxis("Vertical");
       
        // inputY=Input.GetButtonDown.
        if (inputX != 0)
            Rotate();
        if (inputZ != 0)
            Move();
       
    }

    private void Move()
    {
     //transform.position += transform.forward * inputZ * Time.deltaTime*5;
        transform.position += transform.up * inputZ * Time.deltaTime * 5;
    }

    private void Rotate()
    {
        transform.Rotate(new Vector3(0, inputX * Time.deltaTime*5, 0));
    }
}
