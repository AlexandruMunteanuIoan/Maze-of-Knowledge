using System;
using System.Collections.Generic;

using UnityEngine;

public class TestPointSpawn: MonoBehaviour
{
    public GameObject testPointPrefab;

    public void SpawnPoint(ref List<CellCenter> mazeCellsList, int wallSize)
    {

        CellCenter questionCellCenter = MazeGenerator.GetRandomAvailableCellCenter(mazeCellsList);
        if (questionCellCenter != null && !questionCellCenter.IsOccupied)
        {
            questionCellCenter.IsOccupied = true;
            Vector3 spawnPosition = new Vector3(questionCellCenter.Position.x - wallSize / 2f, 0.1f, questionCellCenter.Position.z - wallSize / 2f);
            Instantiate(testPointPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
