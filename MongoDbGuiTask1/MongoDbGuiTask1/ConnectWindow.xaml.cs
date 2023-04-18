using MongoDB.Driver;
using MongoDbGuiTask1.Model;
using MongoDbGuiTask1.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MongoDbGuiTask1
{
    /// <summary>
    /// Логика взаимодействия для ConnectWindow.xaml
    /// </summary>
    public partial class ConnectWindow : Window, IClosable
    {
        public ConnectWindow()
        {
            InitializeComponent();
            DataContext = new ConnectViewModel();
        }
    }
}
