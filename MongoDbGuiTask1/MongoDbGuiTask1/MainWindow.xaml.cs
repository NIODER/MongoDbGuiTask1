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
        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            foreach (var db in DatabaseInteractor.Instance().Databases)
            {
                var item = new TreeViewItem() { Header = db };
                item.Expanded += ((MainViewModel)DataContext).DatabaseExpanded;
                DatabasesTree.Items.Add(item);
            }
            base.OnInitialized(e);
        }
    }
}
