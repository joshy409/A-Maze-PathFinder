using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum Path {
    Forward,
    Left,
    Right,
    Back,
    Return
}

public class PathFinding : MonoBehaviour {

    private bool isTurning = true;
    private bool isMoving = true;
    private bool turnFinish = false;
    private bool touchingEgg = false;

    [SerializeField] float speed = .1f;
    [SerializeField] float rotationSpeed = 4f;

    [SerializeField] GameObject dataEgg;
    private Path path;
	// Use this for initialization
	void Start () {
        Debug.Log("I'm going to find my way out!");
	}

    void Update()
    {
        if (!turnFinish)
        {

            Turn(BasicPath());
   
        } else
        {
            MoveForward();
        }
    }

    /*
     * Logic
     * RayCast in 3 direction with lengh of 1 units. 
     * If raycast hit something in all direction then turn 180 degrees.
     * If raycast didn't hit anyting. Turn towards that direction and go forward.
     * If multiple direction didn't hit anything. prioritize right turn over any other direction
     */

    Vector3 target;
    void MoveForward()
    {
        if (isMoving)
        {
            isMoving = false;
            target = transform.position + transform.forward;
            Debug.LogError("I have a target destination!");
            Debug.Log(target);
        }

        // Check if the position of the cube and sphere are approximately equal.
        if (Vector3.Distance(transform.position, target) > 0.001f)
        {
            // Move our position a step closer to the target.
            float step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, target, step);
            Debug.Log("I'm moving!");
        } else
        {
            isMoving = true;
            isTurning = true;
            turnFinish = false;
            Debug.LogError("I reached my destination!");
            if (!touchingEgg)
            {
                GameObject instantiatedEgg = (GameObject) Instantiate(dataEgg, transform.position, Quaternion.identity);
                instantiatedEgg.GetComponent<PathData>().UpdateHCost();
            }
        }
    }

    Vector3 targetDir;
    void Turn(Path path)
    {
        if (isTurning) {
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

        //TODO: need to check if rotate finished
        if (Vector3.Distance(transform.forward, targetDir) < 0.1f)
        {
            transform.rotation = targetRotation;
            turnFinish = true;
            Debug.LogError("I'm done rotating!");
        }
    }

    /*
    * Wall Follower Logic
    * RayCast in 3 direction with lengh of 1 units. 
    * If raycast hit something in all direction then turn 180 degrees.
    * If raycast didn't hit anyting. Turn towards that direction and go forward.
    * If multiple direction didn't hit anything. prioritize right turn over any other direction
    */
    Path BasicPath()
    {
        RaycastHit forwardRay;
        RaycastHit leftRay;
        RaycastHit rightRay;
        int layerMask = 1 << 9;
        Physics.Raycast(transform.position, transform.forward, out forwardRay, 1f, layerMask);
        Physics.Raycast(transform.position, -transform.right, out leftRay, 1f, layerMask);
        Physics.Raycast(transform.position, transform.right, out rightRay, 1f, layerMask);

        RaycastHit forwardEggRay;
        RaycastHit leftEggRay;
        RaycastHit rightEggRay;
        int layerMaskEgg = 1 << 10;
        Physics.Raycast(transform.position, transform.forward, out forwardEggRay, 1f, layerMaskEgg);
        Physics.Raycast(transform.position, -transform.right, out leftEggRay, 1f, layerMaskEgg);
        Physics.Raycast(transform.position, transform.right, out rightEggRay, 1f, layerMaskEgg);


        if (rightRay.collider == null)
        {
            if (rightEggRay.collider == null)
            {
                return Path.Right;
            } else if (forwardRay.collider == null)
            {
                if(forwardEggRay.collider == null)
                {
                    return Path.Forward;
                }
            } else if (leftRay.collider == null)
            {
                if (leftEggRay.collider == null)
                {
                    return Path.Left;
                }
            }
            return Path.Right;
        } else if (forwardRay.collider == null)
        {
            if(forwardEggRay.collider == null)
            {
                return Path.Forward;
            } else if (leftRay.collider == null)
            {
                if (leftEggRay.collider == null)
                {
                    return Path.Left;
                }
            }
            return Path.Forward;
        } else if (leftRay.collider == null)
        {
            if (leftEggRay.collider == null)
            {
                return Path.Left;
            }
            return Path.Left;
        } else
        {
            return Path.Back;
        }

    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DataEgg"))
        {
            touchingEgg = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("DataEgg"))
        {
            touchingEgg = false;
        }
    }

}
