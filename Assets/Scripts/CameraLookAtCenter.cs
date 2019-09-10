using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookAtCenter : MonoBehaviour
{
    [SerializeField] private float
        minDistance
        , maxDistance
        , minHeight
        , maxHeight
        , zoomSpeed
        , rotateSpeed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {
        if (Input.GetAxis("Horizontal") != 0) // move cam left or right
        {
            transform.parent.transform.Rotate(Vector3.up * Input.GetAxis("Horizontal") * rotateSpeed * -1);
        }

        if (Input.GetAxis("Vertical") < 0 && Vector3.Distance(transform.position, transform.parent.transform.position) < maxDistance) // zoom out
        {
            transform.Translate(Vector3.back * zoomSpeed);
        }
        else if (Input.GetAxis("Vertical") > 0 && Vector3.Distance(transform.position, transform.parent.transform.position) > minDistance) // zoom in
        {
            transform.Translate(Vector3.forward * zoomSpeed);
        }
    }
}
