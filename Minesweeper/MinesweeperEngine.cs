using System;

namespace Minesweeper
{
    public class MinesweeperEngine
    {
        private int _boardSize;
        private Cell[][] _board;

        public bool IsGameOver { get; private set; }

        public MinesweeperEngine(int boardSize)
        {
            _boardSize = boardSize;
            _board = new Cell[_boardSize][];
            for (int x = 0; x < boardSize; x++)
            {
                _board[x] = new Cell[_boardSize];
                for (int y = 0; y < _boardSize; y++)
                {
                    _board[x][y] = new Cell(x, y);
                }
            }
        }

        public Cell[][] GetBoard()
        {
            return _board;
        }

        public void PlaceMine(int x, int y)
        {
            _board[x][y].IsMine = true;

            IncrementMineCount(x - 1, y - 1);
            IncrementMineCount(x - 1, y - 0);
            IncrementMineCount(x - 1, y + 1);

            IncrementMineCount(x - 0, y - 1);
            IncrementMineCount(x + 0, y + 1);

            IncrementMineCount(x + 1, y - 1);
            IncrementMineCount(x + 1, y - 0);
            IncrementMineCount(x + 1, y + 1);
        }

        private void IncrementMineCount(int x, int y)
        {
            if (!IsWithinBoard(x, y)) return;

            _board[x][y].SurroundingMines++;
        }

        public bool IsWithinBoard(int x, int y)
        {
            if (x < 0) return false;
            if (y < 0) return false;
            if (x >= _boardSize) return false;
            if (y >= _boardSize) return false;

            return true;
        }

        public void OpenCell(int x, int y)
        {
            if (!CanOpenCell(x, y)) return;

            _board[x][y].IsOpen = true;

            if (_board[x][y].IsMine)
            {
                OpenAllMineCells();
                IsGameOver = true;
            }
            else if (_board[x][y].SurroundingMines == 0)
            {
                OpenCell(x - 1, y - 1);
                OpenCell(x - 1, y - 0);
                OpenCell(x - 1, y + 1);

                OpenCell(x - 0, y - 1);
                OpenCell(x + 0, y + 1);

                OpenCell(x + 1, y - 1);
                OpenCell(x + 1, y - 0);
                OpenCell(x + 1, y + 1);
            }
        }

        public bool CanOpenCell(int x, int y)
        {
            if (!IsWithinBoard(x, y)) return false;
            if (_board[x][y].IsOpen) return false;
            if (_board[x][y].IsFlagged) return false;

            return true;
        }

        private void OpenAllMineCells()
        {
            for (int x = 0; x < _boardSize; x++)
            {
                for (int y = 0; y < _boardSize; y++)
                {
                    if (_board[x][y].IsMine)
                    {
                        _board[x][y].IsOpen = true;
                    }
                }
            }
        }

        public int GetNumberOfOpenedCells()
        {
            var counter = 0;
            for (int x = 0; x < _boardSize; x++)
            {
                for (int y = 0; y < _boardSize; y++)
                {
                    if (_board[x][y].IsOpen)
                    {
                        counter++;
                    }
                }
            }
            return counter;
        }

        public void ToggleFlag(int x, int y)
        {
            if (!IsWithinBoard(x, y)) return;

            _board[x][y].IsFlagged = !_board[x][y].IsFlagged;
        }
    }

    public class Cell
    {
        public int SurroundingMines { get; set; }
        public bool IsMine { get; set; }
        public bool IsOpen { get; set; }
        public bool IsFlagged { get; set; }

        public int X { get; internal set; }
        public int Y { get; internal set; }

        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}