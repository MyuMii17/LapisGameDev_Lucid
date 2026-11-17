using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GridGen : MonoBehaviour
{
    public int columns = 6;
    public int rows = 10;

    public GameObject chunks;
    private List<GameObject> currentChunks;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        for (int x = 0; x < columns * 10; x+= 10)
        {
            for(int z = 0; z < rows * 9; z+= 9)
            {
                GameObject chunk = Instantiate(chunks, new Vector3(x, 0, z), Quaternion.identity);
                chunk.name = $"{x},{z}";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
