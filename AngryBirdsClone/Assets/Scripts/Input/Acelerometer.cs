using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acelerometer : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Input.acceleration.x, Input.acceleration.y, 0);
    }
}
