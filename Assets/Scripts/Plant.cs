using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Plant : MonoBehaviour
{
    //L SYSTEM ALGORITHM TO GENERATE PROCEDURAL PLANTS
    //https://en.wikipedia.org/wiki/L-system



    [SerializeField] GameObject branchPrefab;
    private List<GameObject> branches = new List<GameObject>();

    [Header("L sys Generation")]
    [SerializeField] private int age = 0; //represents the age of the plant (iterations)
    [SerializeField] private string axiom = "F";
    [SerializeField] private string rule = "F+F";
    [SerializeField] private string currentString = "F";
    [SerializeField] private string finalString;
    private Vector3 TurtlePos;
    private Quaternion TurtleRot;

    void Start()
    {
        TurtlePos = transform.position;
        TurtleRot = transform.rotation;

        foreach (char c in finalString)
        {
            if (c == 'F')
            {
                Vector3 dest = TurtlePos + Vector3.up;
                GenerateBranch(TurtlePos, dest, TurtleRot);
                TurtlePos = dest;
            }
            else if (c == '+')
            {
                //rotate turtlerot to the right
                TurtleRot *= Quaternion.AngleAxis(45, Vector3.up);
                Debug.Log("RODOU PRA DIREITA");
            }
            else if (c == '-')
            {
                //rotate turtlerot to the left
                TurtleRot *= Quaternion.AngleAxis(-45, Vector3.up);
                Debug.Log("RODOU PRA ESQUERDA");
            }
        }
    }

    void Update()
    {

    }

    void GenerateBranch(Vector3 start, Vector3 end, Quaternion rot)
    {
        GameObject branch = Instantiate(branchPrefab, start, rot);
        branches.Add(branch);
        branch.transform.parent = transform;
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