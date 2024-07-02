using UnityEngine;
using System.Collections.Generic;

public class BookSpawner : MonoBehaviour
{
    public GameObject bookPrefab;

    public void SpawnBooks(List<Question> questions, ref List<CellCenter> mazeCellsList, int wallSize)
    {
        for (int i = 0; i < questions.Count; i++)
        {
            CellCenter questionCellCenter = MazeGenerator.GetRandomAvailableCellCenter(mazeCellsList);
            if (questionCellCenter == null || questionCellCenter.IsOccupied)
            {
                break;
            }
            questionCellCenter.IsOccupied = true;
            Vector3 spawnPosition = new Vector3(questionCellCenter.Position.x - wallSize / 2f, 1.5f, questionCellCenter.Position.z - wallSize / 2f);
            GameObject bookObject = Instantiate(bookPrefab, spawnPosition, Quaternion.identity);
            QuestionBook questionBook = bookObject.GetComponent<QuestionBook>();
            questionBook.SetQuestion(questions[i]);
        }
    }
}
