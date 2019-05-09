using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataEggSpawner : MonoBehaviour {

    [SerializeField] GameObject dataEgg;
    private List<PathData> eggs;

	// Use this for initialization
	void Start () {
        GameObject instantiatedEgg = null;
        eggs = new List<PathData>();

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                instantiatedEgg = (GameObject)Instantiate(dataEgg, new Vector3(4.5f - 1f * i, .5f, -4.5f + 1f * j), Quaternion.identity);
                instantiatedEgg.name = "Sphere " + i + j;
                eggs.Add(instantiatedEgg.GetComponent<PathData>());
            }
        }       
    }
	
    public void ResetEggData()
    {
        foreach (var egg in eggs)
        {
            egg.ResetEggData();
            egg.GetComponent<Renderer>().material.color = Color.white;
            if (egg.GetLastDataEgg() != null)
            {
                Debug.LogWarning(egg.name);
            }
        }
    }

    public void UpdateEggData(Transform newDestination)
    {
        foreach (var egg in eggs)
        {
            egg.SetDestination(newDestination);
            egg.UpdateHCost();
        }
    }
}
