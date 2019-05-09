using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour {
    private CollectEggSpawner isClickableRef;
    void Awake()
    {
         isClickableRef = GameObject.Find("Maze").GetComponent<CollectEggSpawner>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "AdvanceAI")
        {
            //Debug.LogError("AI found the Exit");
            AdvancedPathFinding ai = other.GetComponent<AdvancedPathFinding>();
            ai.ChangeState(AdvancedPathFinding.States.Stop);
            ai.ResetEggs();
            GameObject.Find("Maze").GetComponent<DataEggSpawner>().ResetEggDataNonColor();
            other.transform.position = transform.position;
            isClickableRef.isClickable = true;
            Destroy(gameObject);
        }
    }
}
