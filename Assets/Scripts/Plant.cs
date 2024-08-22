using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Plant : MonoBehaviour
{
    //L SYSTEM ALGORITHM TO GENERATE PROCEDURAL PLANTS
    //https://en.wikipedia.org/wiki/L-system

    class Branch //represents a branch of the plant
    {
        public Vector3 start;
        public Vector3 end;
        public float angle;
        public float length;
        public float thickness;

        public Branch(Vector3 start, Vector3 end, float angle, float length, float thickness)
        {
            this.start = start;
            this.end = end;
            this.angle = angle;
            this.length = length;
            this.thickness = thickness;
        }
    }

    [SerializeField] private int age = 0; //represents the age of the plant (iterations)

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void DrawBranch(Vector3 start, Vector3 end)
    {
        //draw a line from start to end
        Gizmos.DrawLine(start, end);
    }

    void OnDrawGizmos()
    {
        //draw the plant
    }
}
