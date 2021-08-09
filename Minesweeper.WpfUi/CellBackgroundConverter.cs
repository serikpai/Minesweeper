using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace Minesweeper.WpfUi
{
    public class CellBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var cell = (Cell)value;

            return System.Windows.Media.Color.FromRgb(225, 225, 255);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class CellContentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var cell = (Cell)value;

            if (cell.IsMine && cell.IsOpen)
            {
                return "*";
            }

            if (cell.IsOpen && cell.SurroundingMines > 0)
            {
                return cell.SurroundingMines;
            }

            if (cell.IsFlagged)
                return "F";

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
