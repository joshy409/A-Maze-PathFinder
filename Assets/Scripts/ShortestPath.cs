using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortestPath : MonoBehaviour
{
    [SerializeField] Exit exit;
    private GameObject dataEgg;

    // Update is called once per frame
    void Update()
    {
        if (dataEgg == null)
        {
            dataEgg = exit.GetExitEgg();
        }
        else
        {
            dataEgg.GetComponent<Renderer>().material.color = new Color(0, 0, 255);
            dataEgg = dataEgg.GetComponent<PathData>().GetLastDataEgg();
        }
    }
}
