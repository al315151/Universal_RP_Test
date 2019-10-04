using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CubeManager;

public class Level : MonoBehaviour
{
    public Transform[] spawnPoints;
    private List<Cube>[] cubes;

    void Awake()
    {
        cubes = new List<Cube>[(int)CubeColor.Count];
        for (int i = 0; i < cubes.Length; i++)
        {
            cubes[i] = new List<Cube>();
        }
    }

    public void AddCube(Cube cube, int color)
    {
        cubes[color].Add(cube);
    }

    public void Discard(int color)
    {
        foreach ( Cube cube in cubes[color])
        {
            cube.Discard();
        }
    }

    public Transform[] SpawnPoints { get => spawnPoints;  }


}
