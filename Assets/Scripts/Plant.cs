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

    void Start()
    {
        GeneratePlant();
    }

    void Update()
    {
        
    }

    void GeneratePlant()
    {
        for (int i = 0; i < age; i++)
        {
            Vector3 branchEnd = Vector3.up * age / i;
            DrawBranch(transform.position, branchEnd);
        }
    }

    void ApplyRule()
    {
        string newString = "";
        for (int i = 0; i < currentString.Length; i++)
        {
            if (currentString[i] == 'F')
            {
                newString += rule;
            }
            else
            {
                newString += currentString[i];
            }
        }
        currentString = newString;
        age++;
    }

    void DrawBranch(Vector3 start, Vector3 end)
    {
        GameObject branch = Instantiate(branchPrefab, start, Quaternion.identity);
        branch.transform.LookAt(end);
        branch.transform.localScale = new Vector3(1, 1, Vector3.Distance(start, end));
        branches.Add(branch);
    }
}
