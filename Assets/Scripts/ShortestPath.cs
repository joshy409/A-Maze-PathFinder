using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortestPath : MonoBehaviour
{
    [SerializeField] Exit exit;
    private GameObject dataEgg;
    private bool once = true;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (dataEgg != null)
        {
            dataEgg.GetComponent<Renderer>().material.color = new Color(0, 0, 255);
            dataEgg = dataEgg.GetComponent<PathData>().GetLastDataEgg();
        }
    }

    public void SetDataEgg(GameObject newDataEgg)
    {
        dataEgg = newDataEgg;
    }
}
