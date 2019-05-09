using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AdvancedPathFinding : MonoBehaviour
{
    public bool isTurning = true;
    public bool isMoving = true;
    public bool turnFinish = false;
    private Path path;

    [SerializeField] float speed = .1f;
    [SerializeField] float rotationSpeed = 4f;
    
    public GameObject lastDataEgg = null;
    public GameObject startingEgg; 

    private HashSet<GameObject> openEggs = new HashSet<GameObject>();
    private HashSet<GameObject> touchedEggs = new HashSet<GameObject>();

    private Vector3 target;
    private Vector3 targetDir;
    public States currentState;

    public enum States {
        Stop,
        Calculate,
        Collect
    }

    // Use this for initialization
    void Start()
    {
        Debug.Log("I'm going to find my way out!");
        GetComponent<Renderer>().material.color = Color.black;
        currentState = States.Stop;
        startingEgg = lastDataEgg;
    }

    void FixedUpdate()
    {
        switch (currentState) {
            case States.Stop:
                break;
            case States.Calculate:
                Calculate();
                break;
            case States.Collect:
                Collect();
                break;
            default:
                Debug.LogError("Invalid State!");
                break;
        }

        foreach (var egg in openEggs)
        {
            egg.GetComponent<Renderer>().material.color = new Color(255, 0, 0);
        }

    }

    public void ChangeState(States newState) {
        currentState = newState;
        Debug.LogError(newState);
    }

    public States GetCurrentState()
    {
        return currentState;
    }

    void Calculate()
    {
        PathFind(AStar());
    }

    void Collect()
    {
        if (!turnFinish)
        {
            Turn(ShortestPath());
        }
        else
        {
            MoveForward();
        }
    }

    void MoveForward()
    {
        if (isMoving)
        {
            isMoving = false;
            target = transform.position + transform.forward;
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
        }
    }

    void PathFind(Path path)
    {
        SetTargetDir(path);
        Debug.LogError(path);

        target = transform.position + targetDir;
        gameObject.GetComponent<Rigidbody>().position = target;
    }


    void Turn(Path path)
    {
        if (isTurning)
        {
            isTurning = false;
            SetTargetDir(path);

            Debug.LogError(path);
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if (targetDir != null && Vector3.Distance(transform.forward, targetDir) < 0.1f)
        {
            transform.rotation = targetRotation;
            turnFinish = true;
        }
    }
    private void SetTargetDir(Path path)
    {
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
                if (openEggs.Count > 0)
                {
                    transform.position = openEggs.First().GetComponent<PathData>().GetLastDataEgg().transform.position;
                    lastDataEgg = openEggs.First().GetComponent<PathData>().GetLastDataEgg();
                    targetDir = openEggs.First().transform.position - transform.position;
                    openEggs.Remove(openEggs.First());
                }
                break;
            default:
                Debug.LogError("I have nowhere to go");
                break;
        }
    }

    Path AStar()
    {
        RaycastHit forwardRay;
        RaycastHit leftRay;
        RaycastHit rightRay;
        RaycastHit backRay;

        Physics.Raycast(transform.position, transform.forward, out forwardRay, 1f);
        Physics.Raycast(transform.position, -transform.right, out leftRay, 1f);
        Physics.Raycast(transform.position, transform.right, out rightRay, 1f);
        Physics.Raycast(transform.position, -transform.forward, out backRay, 1f);

        int rightCost = GetCost(rightRay);
        int forwardCost = GetCost(forwardRay);
        int leftCost = GetCost(leftRay);
        int backCost = GetCost(backRay);

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

        if (backCost == 0)
        {
            backCost = 100000;
        } else if (backCost == -1)
        {
            return Path.Back;
        }

        int min = 0;

        min = Mathf.Min(Mathf.Min(Mathf.Min(rightCost, forwardCost), leftCost),backCost);

        int openEggCost = 10000;
        if (openEggs.Count > 0)
        {
            openEggCost = openEggs.First().GetComponent<PathData>().GetHCost() + lastDataEgg.GetComponent<PathData>().GetGCost();
            min = Mathf.Min(Mathf.Min(Mathf.Min(Mathf.Min(rightCost, forwardCost), leftCost),backCost), openEggCost);
        }

        Debug.Log("right " + rightCost);
        Debug.Log("left  " + leftCost);
        Debug.Log("forwd " + forwardCost);
        Debug.Log("back  " + backCost);
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
            if (backCost < 100000)
            {
                AddOpenEgg(backRay);
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
            if (backCost < 100000)
            {
                AddOpenEgg(backRay);
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
            if (backCost < 100000)
            {
                AddOpenEgg(backRay);
            }
            return Path.Left;
        }

        if (min == backCost)
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
            return Path.Back;
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
            if (backCost < 100000)
            {
                AddOpenEgg(backRay);
            }
            return Path.Return;
        }

        return Path.Back;

    }

    Path ShortestPath()
    {
        RaycastHit forwardRay;
        RaycastHit leftRay;
        RaycastHit rightRay;
        RaycastHit backRay;

        Physics.Raycast(transform.position, transform.forward, out forwardRay, 1f);
        Physics.Raycast(transform.position, -transform.right, out leftRay, 1f);
        Physics.Raycast(transform.position, transform.right, out rightRay, 1f);
        Physics.Raycast(transform.position, -transform.forward, out backRay, 1f);

        if (forwardRay.collider.CompareTag("Exit"))
        {
            return Path.Forward;
        }

        if (leftRay.collider.CompareTag("Exit"))
        {
            return Path.Left;
        }
        if (rightRay.collider.CompareTag("Exit"))
        {
            return Path.Right;
        }
        if (backRay.collider.CompareTag("Exit"))
        {
            return Path.Back;
        }


        if (forwardRay.collider.CompareTag("DataEgg"))
        {
            if(forwardRay.collider.gameObject.GetComponent<PathData>().GetIsShortestPath())
            {
                return Path.Forward;
            }
        }

        if (leftRay.collider.CompareTag("DataEgg"))
        {
            if (leftRay.collider.gameObject.GetComponent<PathData>().GetIsShortestPath())
            {
                return Path.Left;
            }
        }

        if (rightRay.collider.CompareTag("DataEgg"))
        {
            if (rightRay.collider.gameObject.GetComponent<PathData>().GetIsShortestPath())
            {
                return Path.Right;
            }
        }

        if (backRay.collider.CompareTag("DataEgg"))
        {
            if (backRay.collider.gameObject.GetComponent<PathData>().GetIsShortestPath())
            {
                return Path.Back;
            }
        }

        return Path.Return ;
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
            else if (ray.collider.tag == "Exit") { return -1; }
            else if (ray.collider.tag == "DataEgg")
            {
                if (touchedEggs.Contains(ray.transform.gameObject)) { return 0; }
                //if (ray.transform.gameObject.GetComponent<PathData>().GetIsDeadEnd()) { return 1; }
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
            if (currentState == States.Calculate)
            {
                other.GetComponent<Renderer>().material.color = new Color(0, 255, 0);
            }
            if (currentState == States.Collect)
            {
                other.GetComponent<Renderer>().material.color = new Color(0, 0, 255);
            }

            if (openEggs.Contains(other.gameObject))
            {
                openEggs.Remove(other.gameObject);
            }
            if (!touchedEggs.Contains(other.gameObject))
            {
                touchedEggs.Add(other.gameObject);
            }

            PathData eggData = other.GetComponent<PathData>();
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

    private void SortOpenEgg()
    {
        if (openEggs != null)
        {
            HashSet<GameObject> sortedEggs = new HashSet<GameObject>(openEggs.OrderBy(o => o.GetComponent<PathData>().GetHCost()));
            openEggs = sortedEggs;
        }
    }


    public void ResetEggs()
    {
        touchedEggs.Clear();
        openEggs.Clear();
        isTurning = true;
        isMoving = true;
        turnFinish = false;
    }

}
