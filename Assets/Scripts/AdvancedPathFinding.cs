using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AdvancedPathFinding : MonoBehaviour
{
    private bool isTurning = true;
    private bool isMoving = true;
    private bool turnFinish = false;
    private Path path;

    [SerializeField] float speed = .1f;
    [SerializeField] float rotationSpeed = 4f;
    [SerializeField] GameObject lastDataEgg = null;

    private HashSet<GameObject> openEggs = new HashSet<GameObject>();
    private HashSet<GameObject> touchedEggs = new HashSet<GameObject>();

    private Vector3 target;
    private Vector3 targetDir;

    // Use this for initialization
    void Start()
    {
        Debug.Log("I'm going to find my way out!");
        GetComponent<Renderer>().material.color = new Color(0, 0, 255);
    }
    private bool destory = true;
    void Update()
    {
        
        if (!turnFinish)
        {
            Turn(AdvancedPath());
        }
        else
        {
            MoveForward();
        }
        
        foreach (var egg in openEggs)
        {
            egg.GetComponent<Renderer>().material.color = new Color(255, 0, 0);
        }
    }

    void MoveForward()
    {
        if (isMoving)
        {
            isMoving = false;
            target = transform.position + transform.forward;
            Debug.LogError("I have a target destination!");
            //Debug.Log(target);
        }

        // Check if the position of the cube and sphere are approximately equal.
        if (target != null && Vector3.Distance(transform.position, target) > 0.001f)
        {
            // Move our position a step closer to the target.
            float step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, target, step);
           // Debug.Log("I'm moving!");
        }
        else
        {
            turnFinish = false;
            isMoving = true;
            isTurning = true;
            Debug.LogError("I reached my destination!");
        }
    }

    void Turn(Path path)
    {
        if (isTurning)
        {
            isTurning = false;
            switch (path)
            {
                case Path.Back:
                    targetDir = -transform.forward;
                    break;
                case Path.Forward:
                    targetDir = transform.forward;
                    break;
                case Path.Left:
                    targetDir = -transform.right;
                    break;
                case Path.Right:
                    targetDir = transform.right;
                    break;
                case Path.Return:
                    transform.position = openEggs.First().GetComponent<PathData>().GetLastDataEgg().transform.position;
                    lastDataEgg = openEggs.First().GetComponent<PathData>().GetLastDataEgg().GetComponent<PathData>().GetLastDataEgg();
                    targetDir = openEggs.First().transform.position - transform.position;
                    openEggs.Remove(openEggs.First());
                    break;
                default:
                    targetDir = transform.forward;
                    break;
            }
            Debug.LogError("I have a target direction!");
            Debug.LogError(path);
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        Debug.Log("I'm rotating");

        if (targetDir != null && Vector3.Distance(transform.forward, targetDir) < 0.1f)
        {
            transform.rotation = targetRotation;
            turnFinish = true;
            Debug.LogError("I'm done rotating!");
        }
    }

    Path AdvancedPath()
    {
        RaycastHit forwardRay;
        RaycastHit leftRay;
        RaycastHit rightRay;

        Physics.Raycast(transform.position, transform.forward, out forwardRay, 1f);
        Physics.Raycast(transform.position, -transform.right, out leftRay, 1f);
        Physics.Raycast(transform.position, transform.right, out rightRay, 1f);

        int rightCost = GetCost(rightRay);
        int forwardCost = GetCost(forwardRay);
        int leftCost = GetCost(leftRay);

        if (rightCost == 0)
        {
            rightCost = 100000;
        } else if (rightCost == -1)
        {
            return Path.Right;
        } 

        if (forwardCost == 0)
        {
            forwardCost = 100000;
        } else if (forwardCost == -1)
        {
            return Path.Forward;
        }

        if (leftCost == 0)
        {
            leftCost = 100000;
        } else if (leftCost == -1)
        {
            return Path.Left;
        }

        int min = 0;

        min = Mathf.Min(Mathf.Min(rightCost, forwardCost), leftCost);

        int openEggCost = 10000;
        if (openEggs.Count > 0)
        {
            openEggCost = openEggs.First().GetComponent<PathData>().GetHCost() + lastDataEgg.GetComponent<PathData>().GetGCost();
            min = Mathf.Min(Mathf.Min(Mathf.Min(rightCost, forwardCost), leftCost), openEggCost);
        }

        Debug.Log("right " + rightCost);
        Debug.Log("left  " + leftCost);
        Debug.Log("forwd " + forwardCost);
        Debug.Log("open  " + openEggCost);
        Debug.Log("min   " + min);


        if (min == rightCost)
        {
            if (forwardCost < 100000)
            {
                AddOpenEgg(forwardRay);

            }
            if (leftCost < 100000)
            {
                AddOpenEgg(leftRay);
            }
            return Path.Right;
        }

        if (min == forwardCost)
        {
            if (rightCost < 100000)
            {
                AddOpenEgg(rightRay);
            }
            if (leftCost < 100000)
            {
                AddOpenEgg(leftRay);
            }
            return Path.Forward;
        }

        if (min == leftCost)
        {
            if (forwardCost < 100000)
            {
                AddOpenEgg(forwardRay);
            }
            if (rightCost < 100000)
            {
                AddOpenEgg(rightRay);
            }
            return Path.Left;
        }

        if (openEggCost == min)
        {
            if (forwardCost < 100000)
            {
                AddOpenEgg(forwardRay);
            }
            if (rightCost < 100000)
            {
                AddOpenEgg(rightRay);
            }
            if (leftCost < 100000)
            {
                AddOpenEgg(leftRay);
            }
            return Path.Return;
        }

        return Path.Back;

    }

    private void AddOpenEgg(RaycastHit ray)
    {
        GameObject dataEgg = ray.transform.gameObject;
        if (!touchedEggs.Contains(dataEgg))
        {
            PathData openEgg = dataEgg.GetComponent<PathData>();
            openEgg.UpdateLastDataEgg(lastDataEgg);
            openEgg.UpdateGCost(lastDataEgg.GetComponent<PathData>().GetGCost());
            openEggs.Add(dataEgg);
            SortOpenEgg();
        }
    }

    private int GetCost(RaycastHit ray)
    {
        if (ray.collider != null)
        {
            if (ray.collider.tag == "Maze") { }
            else if (ray.collider.name == "Exit") { return -1; }
            else if (ray.collider.tag == "DataEgg")
            {
                if (ray.transform.gameObject.GetComponent<PathData>().GetIsDeadEnd()) { return 1; }
                if (lastDataEgg != null)
                {
                    return ray.transform.GetComponent<PathData>().GetHCost() + lastDataEgg.GetComponent<PathData>().GetGCost();
                }
                else
                {
                    return ray.transform.GetComponent<PathData>().GetHCost();
                }
            }
        }
        return 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DataEgg"))
        {
            PathData eggData = other.GetComponent<PathData>();
        
            if (!touchedEggs.Contains(other.gameObject))
            {
                touchedEggs.Add(other.gameObject);
            }
            other.GetComponent<Renderer>().material.color = new Color(0, 255, 0);

            if (lastDataEgg == null)
            {
                eggData.UpdateGCost(0);
            }
            else
            {
                if (eggData.GetGCost() > lastDataEgg.GetComponent<PathData>().GetGCost())
                {
                    eggData.UpdateLastDataEgg(lastDataEgg);
                } else if (eggData.GetGCost() == 0)
                {
                    eggData.UpdateGCost(lastDataEgg.GetComponent<PathData>().GetGCost());
                    eggData.UpdateLastDataEgg(lastDataEgg);
                }
            }
            lastDataEgg = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("DataEgg"))
        {
            if (other.gameObject.GetComponent<PathData>().GetIsDeadEnd())
            {
                StartCoroutine(DestroyEgg(other.gameObject.GetComponent<PathData>().GetLastDataEgg()));
                Debug.LogError("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
                Destroy(other.gameObject);                   
            }
        }

    }
    //TODO: delete all the dead end eggs that AI has already seen

    IEnumerator DestroyEgg(GameObject dataEgg)
    {
        EggEndCheck();
        yield return new WaitForSeconds(1f);
        PathData pathData = dataEgg.GetComponent<PathData>();
        while(pathData.GetHits() < 1)
        {
            Debug.LogError("ddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddd");
            pathData = pathData.GetLastDataEgg().GetComponent<PathData>();
            Destroy(pathData.gameObject);
        }
    }


    void EggEndCheck()
    {
        GameObject[] eggs = GameObject.FindGameObjectsWithTag("DataEgg");
        foreach (var egg in eggs)
        {
            egg.GetComponent<PathData>().WallCheck();
        }
    }
    private void SortOpenEgg()
    {
        if (openEggs != null)
        {
            HashSet<GameObject> sortedEggs = new HashSet<GameObject>(openEggs.OrderBy(o => o.GetComponent<PathData>().GetHCost()));
            openEggs = sortedEggs;
        }
    }
}
