using System;
using System.Collections.Generic;
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

        private Cell[,] _spriteGrid;
        private IDisposable _disposable;
        private CellPool _cellPool;
        private ISubscriber<GeneralEvents, object> _generalEventssubscriber;
        private readonly Vector2Int[] _directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };


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
            _generalEventssubscriber.Subscribe(GeneralEvents.OnCellMarked, OnCellMarked).AddTo(bag);
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

        private void OnCellMarked(object obj)
        {
            Cell markedCell = (Cell)obj;
            CheckForMatches(markedCell);
        }

        #endregion

        #region Grid Generation

        private void CreateGrid(Vector2Int gridSize)
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

            if (_spriteGrid != null)
            {
                for (int y = 0; y < _spriteGrid.GetLength(1); y++)
                {
                    for (int x = 0; x < _spriteGrid.GetLength(0); x++)
                    {
                        if (_spriteGrid[x, y] != null)
                        {
                            _cellPool.Despawn(_spriteGrid[x, y]); 
                        }
                    }
                }
            }

            _spriteGrid = new Cell[gridSize.x, gridSize.y];

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

                    _spriteGrid[x, y] = newCell;
                }
            }
        }

        #endregion
        
        #region Match Checking
        private void CheckForMatches(Cell cell)
        {
            Vector2Int cellPosition = FindCellPosition(cell);

            if (cellPosition == new Vector2Int(-1, -1))
                return; 

            
            var matchedCells = new HashSet<Cell>();
            FindConnectedMarkedCells(cellPosition, matchedCells);

            // Eğer 3 veya daha fazla işaretli hücre varsa, hepsini temizle
            if (matchedCells.Count >= 3)
            {
                foreach (var matchedCell in matchedCells)
                {
                    matchedCell.ChangeMarked(false);
                }
            }
        }

        private void FindConnectedMarkedCells(Vector2Int startPos, HashSet<Cell> matchedCells)
        {
            Stack<Vector2Int> stack = new Stack<Vector2Int>();
            stack.Push(startPos);
            matchedCells.Add(_spriteGrid[startPos.x, startPos.y]);

            while (stack.Count > 0)
            {
                Vector2Int currentPos = stack.Pop();

                foreach (Vector2Int direction in _directions)
                {
                    Vector2Int newPos = currentPos + direction;

                    if (IsValidPosition(newPos) &&
                        _spriteGrid[newPos.x, newPos.y].IsMarked &&
                        !matchedCells.Contains(_spriteGrid[newPos.x, newPos.y]))
                    {
                        matchedCells.Add(_spriteGrid[newPos.x, newPos.y]);
                        stack.Push(newPos);
                    }
                }
            }
        }

        private Vector2Int FindCellPosition(Cell cell)
        {
            for (int y = 0; y < _spriteGrid.GetLength(1); y++)
            {
                for (int x = 0; x < _spriteGrid.GetLength(0); x++)
                {
                    if (_spriteGrid[x, y] == cell)
                        return new Vector2Int(x, y);
                }
            }
            return new Vector2Int(-1, -1); // Bulunamazsa -1 döndür
        }

        private bool IsValidPosition(Vector2Int pos)
        {
            return pos.x >= 0 && pos.x < _spriteGrid.GetLength(0) && pos.y >= 0 && pos.y < _spriteGrid.GetLength(1);
        }

        #endregion
    }
}
