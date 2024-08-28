using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Plant : MonoBehaviour
{
    //L SYSTEM ALGORITHM TO GENERATE PROCEDURAL PLANTS
    //https://en.wikipedia.org/wiki/L-system



    [SerializeField] GameObject branchPrefab;
    public List<GameObject> branches = new List<GameObject>();

    [Header("L sys Generation")]
    [SerializeField] private int age = 0; //represents the age of the plant (iterations)
    [SerializeField] private string axiom = "F";
    [SerializeField] private string rule = "F+F";
    [SerializeField] private string currentString = "F";
    [SerializeField] private string finalString;
    [SerializeField] private float rotationAngle;
    [SerializeField] private float randomsiness;
    private Vector3 TurtlePos;
    private Quaternion TurtleRot;
    private Vector3 savePos;
    private Quaternion saveRot;


    void Start()
    {
        TurtlePos = transform.position;
        TurtleRot = transform.rotation;

        foreach (char c in finalString)
        {
            if (c == 'F')
            {
                Vector3 dir = TurtleRot * Vector3.up;
                Vector3 dest = TurtlePos + dir;
                GenerateBranch(TurtlePos, dest, TurtleRot);
                TurtlePos = dest;
            }
            else if (c == '+')
            {
                //rotate turtlerot to the right
                if (randomsiness != 0)
                {
                    rotationAngle += Random.Range(-randomsiness, randomsiness);
                    //generate random POSITIVE vector3 to rotate
                    Vector3 randomRotation = new Vector3(Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1));
                    TurtleRot *= Quaternion.AngleAxis(rotationAngle, randomRotation);
                }

                else 
                {
                    TurtleRot *= Quaternion.AngleAxis(rotationAngle, Vector3.right);

                }

                Debug.Log("RODOU PRA DIREITA");
            }
            else if (c == '-')
            {
                //rotate turtlerot to the left
                if (randomsiness != 0)
                {
                    rotationAngle += Random.Range(-randomsiness, randomsiness);
                    //generate random NEGATIVE vector3 to rotate
                    Vector3 randomRotation = new Vector3(Random.Range(-1, 0), Random.Range(-1, 0), Random.Range(-1, 0));
                    TurtleRot *= Quaternion.AngleAxis(rotationAngle, randomRotation);
                }

                else
                {
                    TurtleRot *= Quaternion.AngleAxis(-rotationAngle, Vector3.right);
                }

                Debug.Log("RODOU PRA ESQUERDA");
            }
            else if (c == '[')
            {
                savePos = TurtlePos;
                saveRot = TurtleRot;
            }
            else if (c == ']')
            {
                TurtlePos = savePos;
                TurtleRot = saveRot;
            }
        }

        ChildfyBranches(branches);
    }

    void Update()
    {

    }

    void GenerateBranch(Vector3 start, Vector3 end, Quaternion rot)
    {
        GameObject branch = Instantiate(branchPrefab, start, rot);
        branches.Add(branch);
    }

    void ChildfyBranches(List<GameObject> branches)
    {
        foreach (GameObject branch in branches)
        {
            branch.transform.parent = transform;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (TurtlePos != null)
        {
            Gizmos.DrawWireSphere(TurtlePos, 0.1f);
            Gizmos.color = Color.red;
            Vector3 dir = TurtleRot * Vector3.up;
            Gizmos.DrawRay(TurtlePos, dir / 2);
            Gizmos.color = Color.blue;
            dir = TurtleRot * Vector3.down;
            Gizmos.DrawRay(TurtlePos, dir / 2);
        }
    }
}