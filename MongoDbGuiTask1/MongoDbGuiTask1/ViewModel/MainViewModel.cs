using Database.Entities;
using MongoDbGuiTask1.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace MongoDbGuiTask1.ViewModel
{
    internal class MainViewModel : ViewModelBase
    {
        private readonly DatabaseInteractor _database;

        private DbEntity? _selectedEntity;
        private IEntityViewModel? _chosenEntity;
        private ObservableCollection<DbEntity> _entities;
        private int _pageNumber;
        private bool listTabSelected;
        private RelayCommand? itemClick;

        public MainViewModel()
        {
            _database = DatabaseInteractor.Instance();
            _entities = new();
            _pageNumber = 0;
        }

        public IEntityViewModel? ChosenEntity
        {
            get => _chosenEntity;
            set
            {
                _chosenEntity = value;
                OnPropertyChanged(nameof(ChosenEntity));
            }
        }

        public DbEntity? SelectedEntity
        {
            get { return _selectedEntity; }
            set
            {
                _selectedEntity = value;
                OnPropertyChanged(nameof(SelectedEntity));
            }
        }

        public ObservableCollection<DbEntity> Entities
        {
            get { return _entities; }
            set
            {
                _entities = value; 
                OnPropertyChanged(nameof(Entities));
            }
        }

        public RelayCommand ItemClick
        {
            get => itemClick ??= new RelayCommand(OnItemClick);
        }

        public IEnumerable<string> Databases
        {
            get => _database.Databases;
        }

        public void DatabaseExpanded(object sender, RoutedEventArgs e)
        {
            var parentItem = (TreeViewItem)sender;
            var collections = _database.Collections(parentItem.Header.ToString() ?? throw new NullReferenceException("database name"));
            foreach (var collection in collections)
            {
                var run1 = new Run(collection);
                var collectionButton = new Hyperlink(run1);
                collectionButton.Click += CollectionExpanded;
                parentItem.Items.Add(collectionButton);
            }
        }

        private void CollectionExpanded(object sender, RoutedEventArgs e)
        {
            string collection = ((Run)((Hyperlink)sender).Inlines.FirstInline).Text;
            Entities = new(_database.GetCollectionEntities(collection, _pageNumber)); // сделать чтобы номер менялся
            ListTabSelected = true;
        }

        private void OnItemClick(object? ignorableParameter)
        {
            ChosenEntity = SelectedEntity switch
            {
                Category => new CategoryViewModel((Category)SelectedEntity),
                Employee => new EmployeeViewModel((Employee)SelectedEntity),
                Item => new ItemViewModel((Item)SelectedEntity),
                Order => new OrderViewModel((Order)SelectedEntity),
                _ => throw new InvalidCastException(),
            };
            SingleTabSelected = true;
        }

        public bool ListTabSelected
        {
            get => listTabSelected;
            set
            {
                listTabSelected = value;
                OnPropertyChanged(nameof(ListTabSelected));
                OnPropertyChanged(nameof(SingleTabSelected));
            }
        }

        public bool SingleTabSelected
        {
            get => !listTabSelected;
            set
            {
                listTabSelected = !value;
                OnPropertyChanged(nameof(SingleTabSelected));
                OnPropertyChanged(nameof(ListTabSelected));
            }
        }
    }
}
