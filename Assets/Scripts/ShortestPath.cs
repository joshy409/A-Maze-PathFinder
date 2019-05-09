using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortestPath : MonoBehaviour
{
    [SerializeField] AdvancedPathFinding ai;
    [SerializeField] GameObject dataEgg;
    [SerializeField] GameObject Destination;


    // Update is called once per frame
    void FixedUpdate()
    {
        if (dataEgg != null)
        {
            dataEgg.GetComponent<Renderer>().material.color = new Color(0, 0, 255);
            dataEgg.GetComponent<PathData>().SetIsShortestPath(true);
            dataEgg = dataEgg.GetComponent<PathData>().GetLastDataEgg();
            if (dataEgg == null)
            {
                //ai.startingEgg.GetComponent<PathData>().SetIsShortestPath(false);
                ai.transform.position = ai.startingEgg.transform.position;
                Instantiate(Destination, ai.startingEgg.GetComponent<PathData>().GetDestination().position, Quaternion.identity);
                ai.ChangeState(AdvancedPathFinding.States.Collect);
                this.enabled = false;
            }
        }
    }

    public void SetDataEgg(GameObject newDataEgg)
    {
        dataEgg = newDataEgg;
        ai.startingEgg.GetComponent<PathData>().UpdateLastDataEgg();
    }
}
