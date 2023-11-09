using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{

    public int rotatrionScale = 10;

    
    void Update()
    {
        transform.Rotate(rotatrionScale * Time.deltaTime, 0, 0);
    }
}
