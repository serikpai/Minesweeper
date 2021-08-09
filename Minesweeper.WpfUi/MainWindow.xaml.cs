using System.Windows;
using System.Windows.Controls;

namespace Minesweeper.WpfUi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MinesweeperEngine _engine;

        public MainWindow()
        {
            _engine = new MinesweeperEngine(10);
            _engine.PlaceMine(1, 1);
            _engine.PlaceMine(2, 3);

            InitializeComponent();

            MyBoard.ItemsSource = _engine.GetBoard();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var cell = (Cell)button.DataContext;

            _engine.OpenCell(cell.X, cell.Y);

            var board = _engine.GetBoard();
            //for (int x = 0; x < 10; x++)
            {
                //for (int y = 0; y < 10; y++)
                {
                    button.Content = GetContentOfTheCell(cell);
                }
            }
        }

        public string GetContentOfTheCell(Cell cell)
        {
            if (cell.IsMine && cell.IsOpen)
            {
                return "*";
            }

            if (cell.IsOpen && cell.SurroundingMines > 0)
            {
                return cell.SurroundingMines.ToString();
            }

            if (cell.IsFlagged)
                return "F";

            return string.Empty;
        }
    }
}
