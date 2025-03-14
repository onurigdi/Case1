using System;
using Game.Scripts.Enums;
using Game.Scripts.Events;
using Game.Scripts.Mono;
using Game.Scripts.Pools;
using MessagePipe;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Controllers
{
    public class GridController : MonoBehaviour
    {
        #region Fields

        private Cell[,] spriteGrid;
        private IDisposable _disposable;
        private CellPool _cellPool;
        private ISubscriber<GeneralEvents, object> _generalEventssubscriber;

        #endregion

        #region Dependency Injection

        [Inject]
        private void Setup(ISubscriber<GeneralEvents, object> gameEventSubscriber, CellPool cellPool)
        {
            _cellPool = cellPool;
            _generalEventssubscriber = gameEventSubscriber;
        }

        #endregion

        #region Unity Callbacks

        private void OnEnable()
        {
            var bag = DisposableBag.CreateBuilder();
            _generalEventssubscriber.Subscribe(GeneralEvents.OnGridGenerateRequested, OnGridGenerateRequested).AddTo(bag);
            _disposable = bag.Build();
        }

        private void OnDisable()
        {
            _disposable?.Dispose();
        }

        #endregion

        #region Event Handlers

        private void OnGridGenerateRequested(object obj)
        {
            var gridSize = (GridGenerateRequest)obj;
            CreateGrid(gridSize.GridSize);
        }

        #endregion

        #region Grid Generation

        void CreateGrid(Vector2Int gridSize)
        {
            if (_cellPool == null) 
            {
                Debug.LogError("CellPool is not injected!");
                return;
            }

            Camera cam = Camera.main;
            if (cam == null) 
            {
                Debug.LogError("Main camera not found in scene!");
                return;
            }

            float screenHeight = cam.orthographicSize * 2;
            float screenWidth = screenHeight * cam.aspect;
            float cellSize = screenWidth / gridSize.x;

            if (spriteGrid != null)
            {
                for (int y = 0; y < spriteGrid.GetLength(1); y++)
                {
                    for (int x = 0; x < spriteGrid.GetLength(0); x++)
                    {
                        if (spriteGrid[x, y] != null)
                        {
                            _cellPool.Despawn(spriteGrid[x, y]); 
                        }
                    }
                }
            }

            spriteGrid = new Cell[gridSize.x, gridSize.y];

            for (int y = 0; y < gridSize.y; y++)
            {
                for (int x = 0; x < gridSize.x; x++)
                {
                    Cell newCell = _cellPool.Spawn();
                
                    newCell.SetPosition(new Vector3(
                        -screenWidth / 2 + cellSize / 2 + x * cellSize, 
                        screenHeight / 2 - cellSize / 2 - y * cellSize, 
                        0
                    ));

                    newCell.SetSize(new Vector2(cellSize, cellSize));

                    spriteGrid[x, y] = newCell;
                }
            }
        }

        #endregion
    }
}
