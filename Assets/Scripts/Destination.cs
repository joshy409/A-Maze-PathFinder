using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "AdvanceAI")
        {
            Debug.LogError("AI found the Exit");
            AdvancedPathFinding ai = other.GetComponent<AdvancedPathFinding>();
            ai.ChangeState(AdvancedPathFinding.States.Stop);
            ai.ResetEggs();
            other.transform.position = transform.position;
            Destroy(gameObject);
        }
    }
}
