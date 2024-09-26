using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TCount : MonoBehaviour
{
    public Text txt;
    public int toques;

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0)
        {
            toques += Input.touchCount;
            txt.text = toques.ToString();
        }
    }
}
