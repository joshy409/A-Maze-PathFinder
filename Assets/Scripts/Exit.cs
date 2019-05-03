using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour {

    [SerializeField] ShortestPath shortestPath;

    void Awake()
    {
        shortestPath = GameObject.Find("Maze").GetComponent<ShortestPath>();
    }
	void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "AdvanceAI")
        {
            Debug.LogError("AI found the Exit");
            AdvancedPathFinding ai = other.GetComponent<AdvancedPathFinding>();
            ai.ChangeState(AdvancedPathFinding.States.Stop);
            ai.ResetEggs();
            other.transform.position = transform.position;
            //other.gameObject.SetActive(false);
            shortestPath.enabled = true;
            shortestPath.SetDataEgg(ai.lastDataEgg);
            Destroy(gameObject);
        }
    }
    

     
    //public GameObject GetExitEgg()
    //{
    //    RaycastHit forwardRay;
    //    RaycastHit leftRay;
    //    RaycastHit rightRay;
    //    RaycastHit backRay;

    //    Physics.Raycast(transform.position, transform.forward, out forwardRay, 1f);
    //    Physics.Raycast(transform.position, -transform.right, out leftRay, 1f);
    //    Physics.Raycast(transform.position, transform.right, out rightRay, 1f);
    //    Physics.Raycast(transform.position, -transform.forward, out backRay, 1f);
        
    //    GameObject dataEgg = new GameObject();

    //    if (forwardRay.collider == null) { }
    //    else if (forwardRay.collider.tag == "Maze") { }
    //    else if (forwardRay.collider.tag == "DataEgg")
    //    {
    //        dataEgg = forwardRay.collider.gameObject;
    //    }

    //    if (leftRay.collider == null) { }
    //    else if (leftRay.collider.tag == "Maze") { }
    //    else if (leftRay.collider.tag == "DataEgg")
    //    {
    //        dataEgg = leftRay.collider.gameObject;
    //    }

    //    if (rightRay.collider == null) { }
    //    else if (rightRay.collider.tag == "Maze") { }
    //    else if (rightRay.collider.tag == "DataEgg")
    //    {
    //        dataEgg = rightRay.collider.gameObject;
    //    }

    //    if (backRay.collider == null) { }
    //    else if (backRay.collider.tag == "Maze") { }
    //    else if (backRay.collider.tag == "DataEgg")
    //    {
    //        dataEgg = backRay.collider.gameObject;
    //    }

    //    return dataEgg;
    //}
}
