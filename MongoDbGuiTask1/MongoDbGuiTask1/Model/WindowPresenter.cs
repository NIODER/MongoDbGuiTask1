using System;
using System.Windows;
using MongoDbGuiTask1.View;

namespace MongoDbGuiTask1.Model
{
    internal class WindowPresenter
    {
        public enum WindowType
        {
            ConnectWindow,
            MainWindow,
            DbNameDialog
        }

        public static void ShowWindow(WindowType windowType)
        {
            Window window = windowType switch
            {
                WindowType.ConnectWindow => new ConnectWindow(),
                WindowType.MainWindow => new MainWindow(),
                WindowType.DbNameDialog => new DbNameDialogWindow(),
                _ => throw new ArgumentOutOfRangeException(nameof(windowType))
            };
            window.Show();
        }

        public static void CloseWindow(WindowType windowType)
        {
            Type w = windowType switch
            {
                WindowType.ConnectWindow => typeof(ConnectWindow),
                WindowType.MainWindow => typeof(MainWindow),
                WindowType.DbNameDialog => typeof(DbNameDialogWindow),
                _ => throw new ArgumentOutOfRangeException(nameof(windowType))
            };
            foreach (var window in Application.Current.Windows)
            {
                if (window.GetType() == w)
                {
                    ((Window)window).Close();
                    break;
                }
            }
        }
    }
}
