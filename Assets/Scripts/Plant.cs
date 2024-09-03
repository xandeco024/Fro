using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Plant : MonoBehaviour
{
    //L SYSTEM ALGORITHM TO GENERATE PROCEDURAL PLANTS
    //https://en.wikipedia.org/wiki/L-system

    //that script needs to do:
    //generate a base plant
    //grow that plant (iterations but also randomly)
    //generate leaves
    //generate flowers (but that's for later)
    //generate fruits (but that's for later)
    //all above, but we can go step by step

    [SerializeField] GameObject branchPrefab;
    [SerializeField] GameObject leafyBranchPrefab;
    [SerializeField] GameObject leafPrefab;
    public List<GameObject> branches = new List<GameObject>();

    [Header("L sys Generation")]
    [SerializeField] private int age = 0; //represents the age of the plant (iterations)
    [SerializeField] private string axiom = "F";
    [SerializeField] private string[] rules;
    [SerializeField] private string currentString;
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

        GeneratePlant(axiom);
        currentString = axiom;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            age++;
            ReGeneratePlant(currentString);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            currentString = ApplyRules(rules, currentString);
        }
    }

    void GeneratePlant(string banana)
    {
        foreach (char c in banana)
        {
            if (c == 'B')
            {
                //generate branch

                Vector3 dir = TurtleRot * Vector3.up;
                Vector3 dest = TurtlePos + dir;
                GenerateBranch(branchPrefab, TurtlePos, dest, TurtleRot);
                TurtlePos = dest;
            }

            else if (c == 'L')
            {
                //generate leafy branch

                Vector3 dir = TurtleRot * Vector3.up;
                Vector3 dest = TurtlePos + dir;
                GenerateBranch(leafyBranchPrefab, TurtlePos, dest, TurtleRot);
                TurtlePos = dest;

                //generate leaves on the branch (that's for later)
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

    void ReGeneratePlant(string banana)
    {
        foreach (GameObject branch in branches)
        {
            Destroy(branch);
        }

        branches.Clear();

        TurtlePos = transform.position;
        TurtleRot = transform.rotation;

        GeneratePlant(banana);
    }

    void GenerateBranch(GameObject prefab, Vector3 start, Vector3 end, Quaternion rot)
    {
        GameObject branch = Instantiate(prefab, start, rot);
        branches.Add(branch);
    }

    string ApplyRules(string[] rules, string curString)
    {
        //rules are in the format "A -> B"

        string newString = "";

        foreach (char c in curString)
        {
            foreach (string rule in rules)
            {
                if (rule[0] == c)
                {
                    newString += rule.Substring(5);
                }
            }
        }

        return newString;
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