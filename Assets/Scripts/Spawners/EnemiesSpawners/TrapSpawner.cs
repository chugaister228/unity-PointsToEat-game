using Controllers.InGameControllers;
using MazeGeneration;
using Models.Enemies;
using Pooling;
using UnityEngine;
using Zenject;

namespace Spawners.EnemiesSpawners
{
    public class TrapSpawner : MonoBehaviour
    {
        [SerializeField] private Trap trapPrefab;

        private ObjectPool<Trap> _pool;
        private PositionsBlocker _positionsBlocker;
        
        [Inject]
        private void Construct(PositionsBlocker positionsBlocker)
        {
            _positionsBlocker = positionsBlocker;
        }

        private void Awake()
        {
            _pool = new ObjectPool<Trap>(trapPrefab);
        }

        public void Spawn(Cell[,] maze, int mazeWidth, int mazeHeight)
        {
            var iterations = Random.Range(mazeWidth - 15, mazeHeight - 5);

            for (var i = 0; i < iterations; i++)
            {
                int xPosition = Random.Range(1, mazeWidth - 1);
                int yPosition = Random.Range(1, mazeHeight - 1);

                if (xPosition != MazeGenerator.ExitCell.X &&
                    yPosition != MazeGenerator.ExitCell.Y &&
                    _positionsBlocker.CheckPositionAvailability(xPosition, yPosition))
                {
                    var cell = maze[xPosition, yPosition];
                    var trap = GetTrapObject();
                    trap.transform.localPosition = MazeSpawner.GetCellWorldCoordinates(cell, mazeWidth, mazeHeight);
                    trap.PlayAppearingAnimation();

                    _positionsBlocker.BlockPosition(xPosition, yPosition, true);
                }
            }
        }

        private Trap GetTrapObject()
        {
            var poolAble = _pool.GetFreeObject();
            return poolAble as Trap;
        }
    }
}
