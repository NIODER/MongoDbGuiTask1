using System;
using System.Windows;
using MongoDbGuiTask1.View;
using MongoDbGuiTask1.ViewModel;

namespace MongoDbGuiTask1.Model
{
    internal class WindowPresenter
    {
        public enum WindowType
        {
            ConnectWindow,
            MainWindow,
            DbNameDialog,
            AddItemCategoryWindow
        }

        public static void ShowWindow(WindowType windowType, ViewModelBase? dataContext = null)
        {
            Window window = windowType switch
            {
                WindowType.ConnectWindow => new ConnectWindow(),
                WindowType.MainWindow => new MainWindow(),
                WindowType.DbNameDialog => new DbNameDialogWindow(),
                WindowType.AddItemCategoryWindow => new AddItemCategoryWindow(),
                _ => throw new ArgumentOutOfRangeException(nameof(windowType))
            };
            if (dataContext != null)
                window.DataContext = dataContext;
            window.Show();
        }

        public static void CloseWindow(WindowType windowType)
        {
            Type w = windowType switch
            {
                WindowType.ConnectWindow => typeof(ConnectWindow),
                WindowType.MainWindow => typeof(MainWindow),
                WindowType.DbNameDialog => typeof(DbNameDialogWindow),
                WindowType.AddItemCategoryWindow => typeof(AddItemCategoryWindow),
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
