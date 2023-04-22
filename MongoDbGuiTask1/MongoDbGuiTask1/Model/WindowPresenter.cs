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
            DialogWindowSelectCollectionType
        }

        public static void ShowWindow(WindowType windowType)
        {
            Window window = windowType switch
            {
                WindowType.ConnectWindow => new ConnectWindow(),
                WindowType.MainWindow => new MainWindow(),
                _ => throw new ArgumentOutOfRangeException(nameof(windowType))
            };
            window.Show();
        }

        public static void CloseWindow(Type windowType)
        {
            foreach (var window in Application.Current.Windows)
            {
                if (window.GetType() == windowType)
                {
                    ((Window)window).Close();
                    break;
                }
            }
        }
    }
}
