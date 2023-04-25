using MongoDbGuiTask1.Model;
using MongoDbGuiTask1.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace MongoDbGuiTask1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel MainContext
        {
            get => (MainViewModel)DataContext;
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            MainContext.UpdateDatabases += UpdateDatabases;
            UpdateDatabases(DatabaseInteractor.Instance().Databases);
            base.OnInitialized(e);
        }

        private void UpdateDatabases(List<string> databases)
        {
            DatabasesTree.Items.Clear();
            foreach (var database in databases)
            {
                var item = new TreeViewItem() { Header = database };
                item.Expanded += MainContext.DatabaseExpanded;
                DatabasesTree.Items.Add(item);
            }
        }
    }
}
