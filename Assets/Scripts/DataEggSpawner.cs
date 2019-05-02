using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataEggSpawner : MonoBehaviour {

    [SerializeField] GameObject dataEgg;
	// Use this for initialization
	void Start () {
        GameObject instantiatedEgg = null;
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                instantiatedEgg = (GameObject)Instantiate(dataEgg, new Vector3(4.5f - 1f * i, .5f, -4.5f + 1f * j), Quaternion.identity);
                instantiatedEgg.GetComponent<PathData>().UpdateHCost();
                instantiatedEgg.name = "Sphere " + i + j;
            }
        }       
    }
	

}
