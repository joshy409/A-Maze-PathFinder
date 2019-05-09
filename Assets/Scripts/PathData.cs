using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathData : MonoBehaviour {

    public const int STEP = 10;

    [SerializeField] int hCost; //how far it is from destination
    [SerializeField] int gCost; //how far it travelled;
    [SerializeField] int fCost; //how far it travelled;

    [SerializeField] Transform destination;
    [SerializeField] GameObject lastDataEgg = null;

    [SerializeField] bool isShortestPath;

    public bool GetIsShortestPath()
    {
        return isShortestPath;
    }

    public void SetIsShortestPath(bool b)
    {
        isShortestPath = b;
    }


    public void ResetEggData()
    {
        hCost = 0;
        gCost = 0;
        fCost = 0;
        destination = null;
        lastDataEgg = null;
        isShortestPath = false;
    }

    public void UpdateHCost()
    {
        hCost = (int)Mathf.Abs(transform.position.x - destination.position.x) * 100 / 10 + (int)Mathf.Abs(transform.position.z - destination.position.z) * 100 / 10;
        SetFCost();
    }

    public void UpdateGCost(int previousGCost)
    {
        gCost = STEP+previousGCost;
        SetFCost();
    }

    public void UpdateLastDataEgg(GameObject previousEgg)
    {
        lastDataEgg = previousEgg;
    }

    public void UpdateLastDataEgg()
    {
        lastDataEgg = null;
    }

    private void SetFCost()
    {
        fCost = hCost + gCost;
    }

    public int GetHCost()
    {
        return hCost;
    }
    public int GetGCost()
    {
        return gCost;
    }
    public int GetFCost()
    {
        return fCost;
    }

    public GameObject GetLastDataEgg()
    {
        return lastDataEgg;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.magenta;
    //    if (GetLastDataEgg() != null)
    //    {
    //        Gizmos.DrawLine(transform.position, GetLastDataEgg().transform.position);
    //    }
    //}

    public void SetDestination(Transform newDestination)
    {
        destination = newDestination;
    }

    public Transform GetDestination()
    {
        return destination;
    }

    //public void WallCheck()
    //{
    //    hits = 0;
    //    RaycastHit forwardRay;
    //    RaycastHit leftRay;
    //    RaycastHit rightRay;
    //    RaycastHit backRay;

    //    Physics.Raycast(transform.position, transform.forward, out forwardRay, 1f);
    //    Physics.Raycast(transform.position, -transform.right, out leftRay, 1f);
    //    Physics.Raycast(transform.position, transform.right, out rightRay, 1f);
    //    Physics.Raycast(transform.position, -transform.forward, out backRay, 1f);

    //    if (forwardRay.collider == null) { }
    //    else if (forwardRay.collider.tag == "Maze") { }
    //    else if (forwardRay.collider.tag == "DataEgg" || forwardRay.collider.tag == "Exit")
    //    {
    //        hits++;
            
    //    }
    //    if (leftRay.collider == null) {  }
    //    else if (leftRay.collider.tag == "Maze") {  }
    //    else if (leftRay.collider.tag == "DataEgg" || leftRay.collider.tag == "Exit")
    //    {
    //        hits++;

    //    }
    //    if (rightRay.collider == null) {  }

    //    else if (rightRay.collider.tag == "Maze") { }
    //    else if (rightRay.collider.tag == "DataEgg" || rightRay.collider.tag == "Exit")
    //    {
    //        hits++;

    //    }
    //    if (backRay.collider == null) {}
    //    else if (backRay.collider.tag == "Maze") { }
    //    else if (backRay.collider.tag == "DataEgg" || backRay.collider.tag == "Exit")
    //    {

    //        hits++;
    //    }


    //    if (hits <= 1)
    //    {
    //        isDeadEnd = true;
    //    }
        
    //}
}
