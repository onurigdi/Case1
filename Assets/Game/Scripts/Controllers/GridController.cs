using Game.Scripts.Mono;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Scripts.Controllers
{
    public class GridController : MonoBehaviour
    {
        public Cell cellPrefab; 
        private Cell[,] spriteGrid; 

        void Start()
        {
            // create 5x5 grid for now
            CreateGrid(7, 7);
        }

        void CreateGrid(int columns, int rows)
        {
            if (cellPrefab == null) 
            {
                Debug.LogError("Sprite Prefab not assigned!");
                return;
            }

            Camera cam = Camera.main;// calculate the cell size from screen size
            if (cam == null) 
            {
                Debug.LogError("Main camera not found in scene!");
                return;
            }
            float screenHeight = cam.orthographicSize * 2;
            float screenWidth = screenHeight * cam.aspect;

            float cellSize = screenWidth / columns; 

            spriteGrid = new Cell[columns, rows]; //create an 2d array for store all spawned cells

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    Cell newSprite = Instantiate(cellPrefab);//instantiate normally for now i will change it to pool later
                    newSprite.SetPosition(new Vector3(
                        -screenWidth / 2 + cellSize / 2 + x * cellSize, 
                        screenHeight / 2 - cellSize / 2 - y * cellSize, 
                        0
                    ));
                    newSprite.SetSize(new Vector2(cellSize, cellSize));

                    spriteGrid[x, y] = newSprite; 
                }
            }
        }
    }
}