using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Scripts.Controllers
{
    public class GridController : MonoBehaviour
    {
        public GameObject cellPrefab; 
        private GameObject[,] spriteGrid; 

        void Start()
        {
            // create 5x5 grid for now
            CreateGrid(5, 5);
        }

        void CreateGrid(int columns, int rows)
        {
            if (cellPrefab == null) 
            {
                Debug.LogError("Sprite Prefab not assigned!");
                return;
            }

            Camera cam = Camera.main;// calculate the cell size from screen size
            float screenHeight = cam.orthographicSize * 2;
            float screenWidth = screenHeight * cam.aspect;

            float cellSize = screenWidth / columns; 

            spriteGrid = new GameObject[columns, rows]; //create an 2d array for store all spawned cells

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    GameObject newSprite = Instantiate(cellPrefab);//instantiate normally for now i will change it to pool later
                    newSprite.transform.position = new Vector3(
                        -screenWidth / 2 + cellSize / 2 + x * cellSize, 
                        screenHeight / 2 - cellSize / 2 - y * cellSize, 
                        0
                    );

                    SpriteRenderer sr = newSprite.GetComponent<SpriteRenderer>();
                    if (sr != null)
                    {
                        sr.drawMode = SpriteDrawMode.Sliced;//9-slice method
                        sr.size = new Vector2(cellSize, cellSize); 
                    }

                    spriteGrid[x, y] = newSprite; 
                }
            }
        }
    }
}