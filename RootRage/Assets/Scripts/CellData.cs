using UnityEngine;

public class CellData
{
    public int Index;
    public int PlayerIndex;
    public CellBehaviour Viz;
    public Vector3 Position;
    public bool IsOccupied;
    public Building Building;

    public void ResetForNewGame()
    {
        PlayerIndex = 0;
        IsOccupied = false;
        Building = null;
    }
}