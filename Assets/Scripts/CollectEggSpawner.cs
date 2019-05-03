using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectEggSpawner : MonoBehaviour {

    [SerializeField] GameObject exit;
    [SerializeField] AdvancedPathFinding ai;
    [SerializeField] GameObject Maze;
    DataEggSpawner eggs;
    ShortestPath shortest;

    void Start()
    {
        eggs = Maze.GetComponent<DataEggSpawner>();
        shortest = Maze.GetComponent<ShortestPath>();
    }

	// Update is called once per frame
	void Update () {
        if (ai.GetCurrentState() == AdvancedPathFinding.States.Stop)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SelectDataEgg();
            }
        }
	}

    private void SelectDataEgg()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        int layerMaskEgg = 1 << 10;
        Physics.Raycast(ray, out hit, 20f, layerMaskEgg);

        if (hit.collider != null)
        {
            Instantiate(exit, hit.collider.gameObject.transform.position, Quaternion.identity);
            eggs.ResetEggData();
            eggs.UpdateEggData(hit.collider.transform);
            shortest.SetDataEgg(null);
            hit.collider.gameObject.GetComponent<Renderer>().material.color = Color.cyan;
            ai.ChangeState(AdvancedPathFinding.States.Calculate);
        }
    }
}
