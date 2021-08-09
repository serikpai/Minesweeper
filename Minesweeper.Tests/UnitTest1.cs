using NUnit.Framework;

namespace Minesweeper.Tests
{
    public class Tests
    {
        private MinesweeperEngine _engine;

        [SetUp]
        public void Setup()
        {
            _engine = new MinesweeperEngine(4);
        }
        
        [Test]
        public void PlacingTheMineMustConvertTheCellIntoAMine()
        {
            _engine.PlaceMine(1, 1);

            var board = _engine.GetBoard();
            Assert.IsTrue(board[1][1].IsMine);
        }

        [Test]
        public void PlacingMineInTheMiddleMustIncrementSurroundingFields()
        {
            _engine.PlaceMine(1, 1);

            var board = _engine.GetBoard();

            Assert.AreEqual(1, board[0][0].SurroundingMines);
            Assert.AreEqual(1, board[0][1].SurroundingMines);
            Assert.AreEqual(1, board[0][2].SurroundingMines);

            Assert.AreEqual(1, board[1][0].SurroundingMines);
            Assert.AreEqual(0, board[1][1].SurroundingMines);
            Assert.AreEqual(1, board[1][2].SurroundingMines);

            Assert.AreEqual(1, board[2][0].SurroundingMines);
            Assert.AreEqual(1, board[2][1].SurroundingMines);
            Assert.AreEqual(1, board[2][2].SurroundingMines);
        }

        [Test]
        public void PlacingMineInTheTopLeftCornerMustNotCrash()
        {
            _engine.PlaceMine(0, 0);

            var board = _engine.GetBoard();

            Assert.AreEqual(0, board[0][0].SurroundingMines);
            Assert.AreEqual(1, board[0][1].SurroundingMines);
            Assert.AreEqual(1, board[1][0].SurroundingMines);
            Assert.AreEqual(1, board[1][1].SurroundingMines);
        }


        [Test]
        public void PlacingMineInTheTopRightCornerMustNotCrash()
        {
            _engine.PlaceMine(3, 0);

            var board = _engine.GetBoard();

            Assert.AreEqual(0, board[3][0].SurroundingMines);
            Assert.AreEqual(1, board[2][0].SurroundingMines);
            Assert.AreEqual(1, board[2][1].SurroundingMines);
            Assert.AreEqual(1, board[3][1].SurroundingMines);
        }


        [Test]
        public void PlacingMineInTheBottomLeftCornerMustNotCrash()
        {
            _engine.PlaceMine(0, 3);

            var board = _engine.GetBoard();

            Assert.AreEqual(0, board[0][3].SurroundingMines);
            Assert.AreEqual(1, board[0][2].SurroundingMines);
            Assert.AreEqual(1, board[1][2].SurroundingMines);
            Assert.AreEqual(1, board[1][3].SurroundingMines);
        }


        [Test]
        public void PlacingMineInTheBottomRightCornerMustNotCrash()
        {
            _engine.PlaceMine(3, 3);

            var board = _engine.GetBoard();

            Assert.AreEqual(0, board[3][3].SurroundingMines);
            Assert.AreEqual(1, board[2][2].SurroundingMines);
            Assert.AreEqual(1, board[3][2].SurroundingMines);
            Assert.AreEqual(1, board[2][3].SurroundingMines);
        }

        [Test]
        public void PlacingSeveralMinesInTheNearOfEachOtherMustInfluenceTheOthers()
        {
            _engine.PlaceMine(0, 0);
            _engine.PlaceMine(1, 0);
            _engine.PlaceMine(0, 1);

            var board = _engine.GetBoard();

            Assert.AreEqual(3, board[1][1].SurroundingMines);
        }



        [TestCase(-1, -1)]
        [TestCase(-1, 2)]
        [TestCase(-1, 4)]
        [TestCase(4, 2)]
        [TestCase(2, -2)]
        [TestCase(2, 20)]
        public void CheckingPlacingMineOutsideOfTheFieldShouldNotBePossible(int x, int y)
        {
            var result = _engine.IsWithinBoard(x, y);

            Assert.IsFalse(result);
        }

        [TestCase(0,0)]
        [TestCase(0, 3)]
        [TestCase(3, 0)]
        [TestCase(3, 3)]
        [TestCase(1, 1)]
        public void CheckingPlacingMineInsideOfTheFieldMustAlwaysBePossible(int x, int y)
        {
            var result = _engine.IsWithinBoard(x, y);

            Assert.IsTrue(result);
        }

        [Test]
        public void OpenCellWithMineInTheNeighberhoodShouldOnlyOpenTheCell()
        {
            _engine.PlaceMine(1, 1);
            _engine.OpenCell(2, 1);

            var board = _engine.GetBoard();
            var numberOfOpenedCells = _engine.GetNumberOfOpenedCells();
            Assert.IsTrue(board[2][1].IsOpen);
            Assert.AreEqual(1, numberOfOpenedCells);
        }

        [Test]
        public void OpenCellWithoutMineInTheNeighberhoodMustOpenEveryFieldUntilTheNextNumber()
        {
            // +-----+-----+-----+-----+
            // | 0,0 | 1,0 | 2,0 | 3,0 |
            // |  1  |  1  |  1  |  x  |
            // +-----+-----+-----+-----+
            // | 0,1 | 1,1 | 2,1 | 3,1 |
            // |  1  |  *  |  1  |     |
            // +-----+-----+-----+-----+
            // | 0,2 | 1,2 | 2,2 | 3,2 |
            // |  1  |  2  |  2  |  1  |
            // +-----+-----+-----+-----+
            // | 0,3 | 1,3 | 2,3 | 3,3 |
            // |     |  1  |  *  |  1  |
            // +-----+-----+-----+-----+
            
            _engine.PlaceMine(1, 1);
            _engine.PlaceMine(2, 3);

            _engine.OpenCell(3, 0);

            var board = _engine.GetBoard();
            var numberOfOpenedCells = _engine.GetNumberOfOpenedCells();
            Assert.IsTrue(board[3][0].IsOpen, "3,0");
            Assert.IsTrue(board[3][1].IsOpen, "3,1");
            Assert.IsTrue(board[2][0].IsOpen, "2,0");
            Assert.IsTrue(board[2][1].IsOpen, "3,1");
            Assert.IsTrue(board[2][2].IsOpen, "2,2");
            Assert.IsTrue(board[3][2].IsOpen, "3,2");
            Assert.AreEqual(6, numberOfOpenedCells);
        }


        [Test]
        public void OpenCellWithOnlyOneMineMustOpenNearlyTheEntireField()
        {
            // +-----+-----+-----+-----+
            // | 0,0 | 1,0 | 2,0 | 3,0 |
            // |  *  |  1  |     |  x  |
            // +-----+-----+-----+-----+
            // | 0,1 | 1,1 | 2,1 | 3,1 |
            // |  1  |  1  |     |     |
            // +-----+-----+-----+-----+
            // | 0,2 | 1,2 | 2,2 | 3,2 |
            // |     |     |     |     |
            // +-----+-----+-----+-----+
            // | 0,3 | 1,3 | 2,3 | 3,3 |
            // |     |     |     |     |
            // +-----+-----+-----+-----+
            _engine.PlaceMine(0, 0);

            _engine.OpenCell(3, 0);

            var board = _engine.GetBoard();
            var numberOfOpenedCells = _engine.GetNumberOfOpenedCells();
            Assert.IsTrue(board[3][0].IsOpen, "3,0");
            Assert.IsTrue(board[3][1].IsOpen, "3,1");
            Assert.IsTrue(board[2][0].IsOpen, "2,0");
            Assert.IsTrue(board[2][1].IsOpen, "3,1");
            Assert.IsTrue(board[2][2].IsOpen, "2,2");
            Assert.IsTrue(board[3][2].IsOpen, "3,2");
            Assert.AreEqual(15, numberOfOpenedCells);
        }

        [Test]
        public void StepOnAMineMustOpenAllMines()
        {
            _engine.PlaceMine(0, 0);
            _engine.PlaceMine(2, 0);
            _engine.PlaceMine(1, 2);

            _engine.OpenCell(1, 2);

            var board = _engine.GetBoard();
            var numberOfOpenedCells = _engine.GetNumberOfOpenedCells();
            Assert.IsTrue(board[0][0].IsOpen, "0,0");
            Assert.IsTrue(board[2][0].IsOpen, "2,0");
            Assert.IsTrue(board[1][2].IsOpen, "1,2");
            Assert.AreEqual(3, numberOfOpenedCells);
            Assert.IsTrue(_engine.IsGameOver);
        }

        [Test]
        public void OpenedCellCannotBeReopenedAgain()
        {
            _engine.PlaceMine(0, 0);
            _engine.OpenCell(1,0);

            var result = _engine.CanOpenCell(1, 0);
            
            Assert.IsFalse(result);
        }

        [Test]
        public void FlaggedCellCannotBeOpened()
        {
            _engine.ToggleFlag(1, 0);

            var result = _engine.CanOpenCell(1, 0);

            Assert.IsFalse(result);
        }
    }
}