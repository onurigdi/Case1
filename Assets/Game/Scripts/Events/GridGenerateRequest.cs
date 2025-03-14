using UnityEngine;

namespace Game.Scripts.Events
{
    public class GridGenerateRequest
    {
        public Vector2Int GridSize;

        public GridGenerateRequest(Vector2Int gridSize)
        {
            GridSize = gridSize;
        }
    }
}
