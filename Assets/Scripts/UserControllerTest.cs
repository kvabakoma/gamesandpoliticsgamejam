using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserControllerTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Input.GetAxis("Horizontal") * .3f, transform.position.y, Input.GetAxis("Vertical") * .3f);
    }
}
