using UnityEngine;

public class CellCenter
{
    public Vector3 Position { get; set; }
    public bool IsOccupied { get; set; }

    public CellCenter(Vector3 position)
    {
        Position = position;
        IsOccupied = false;
    }
}
