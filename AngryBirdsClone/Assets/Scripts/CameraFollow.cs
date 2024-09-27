using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform objE, objD, bird;

    // Update is called once per frame
    void Update()
    {
        Vector3 posCam = transform.position;
        posCam.x = bird.position.x;
        posCam.x = Mathf.Clamp(posCam.x, objE.position.x, objD.position.x);
        transform.position = posCam;
    }
}
