using Database.Entities;
using Microsoft.VisualBasic;
using MongoDB.Driver;
using MongoDbGuiTask1.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace MongoDbGuiTask1.ViewModel
{
    internal class MainViewModel : ViewModelBase
    {
        private readonly DatabaseInteractor _database;

        private const string SINGLE_HEADER = "Выбранный документ";
        private const string SINGLE_HEADER_CHANGED = "Выбранный документ (не сохранено)";

        private DbEntity? _selectedEntity;
        private IEntityViewModel? _chosenEntity;
        private ObservableCollection<DbEntity> _entities;
        private int _pageNumber;
        private string _collectionName;
        private bool listTabSelected;
        private string singleHeader = SINGLE_HEADER;
        private bool nextButtonActive = true;
        private bool prevButtonActive = false;

        public RelayCommand ItemClick { get; private set; }
        public RelayCommand DeleteClick { get; private set; }
        public RelayCommand SaveClick { get; private set; }
        public RelayCommand AddClick { get; private set; }
        public RelayCommand NextClick { get; set; }
        public RelayCommand PrevClick { get; set; }

        public string SingleHeader
        {
            get => singleHeader;
            set
            {
                singleHeader = value;
                OnPropertyChanged(nameof(SingleHeader));
            }
        }

        public MainViewModel()
        {
            _database = DatabaseInteractor.Instance();
            _entities = new();
            _pageNumber = 0;
            _collectionName = string.Empty;
            ItemClick = new(OnItemClick);
            DeleteClick = new(OnDeleteClick);
            SaveClick = new(OnSaveClick);
            AddClick = new(OnAddClick);
            NextClick = new(OnNextClick);
            PrevClick = new(OnPrevClick);
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

        public IEnumerable<string> Databases
        {
            get => _database.Databases;
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

        public bool NextButtonActive
        {
            get => nextButtonActive;
            set
            {
                nextButtonActive = value;
                OnPropertyChanged(nameof(NextButtonActive));
            }
        }

        public bool PrevButtonActive
        {
            get => prevButtonActive;
            set
            {
                prevButtonActive = value;
                OnPropertyChanged(nameof(PrevButtonActive));
            }
        }

        private void OnNextClick(object? ignorableParameter)
        {
            _pageNumber++;
            UpdateList();
            if (_pageNumber >= _database.GetCollectionLength(_collectionName) / 10)
                NextButtonActive = false;
            if (_pageNumber > 0)
                PrevButtonActive = true;
        }

        private void OnPrevClick(object? ignorableParameter)
        {
            _pageNumber--;
            UpdateList();
            if (_pageNumber <= 0)
                PrevButtonActive = false;
            if (_pageNumber < _database.GetCollectionLength(_collectionName) / 10)
                NextButtonActive = true;

        }

        public void DatabaseExpanded(object sender, RoutedEventArgs e)
        {
            var treeViewItem = (TreeViewItem)sender;
            if (treeViewItem.Items.Count > 0)
                return;
            var collections = _database.Collections(treeViewItem.Header.ToString() ?? throw new NullReferenceException("database name"));
            foreach (var collection in collections)
            {
                var run1 = new Run(collection);
                var collectionButton = new Hyperlink(run1);
                collectionButton.Click += CollectionExpanded;
                treeViewItem.Items.Add(collectionButton);
            }
        }

        private void CollectionExpanded(object sender, RoutedEventArgs e)
        {
            _collectionName = ((Run)((Hyperlink)sender).Inlines.FirstInline).Text;
            UpdateList();
            ListTabSelected = true;
        }

        private void OnItemClick(object? ignorableParameter)
        {
#warning перехватить тут исключение
            ChosenEntity = GetEntityViewModel(SelectedEntity);
            ChosenEntity.DocumentUpdated += OnDocumentUpdated;
            SingleTabSelected = true;
        }

        private IEntityViewModel GetEntityViewModel(DbEntity? dbEntity) => dbEntity switch
        {
            Category => new CategoryViewModel((Category)dbEntity),
            Employee => new EmployeeViewModel((Employee)dbEntity),
            Item => new ItemViewModel((Item)dbEntity),
            Order => new OrderViewModel((Order)dbEntity),
            _ => throw new InvalidCastException(),
        };

        private void OnDocumentUpdated()
        {
            SingleHeader = SINGLE_HEADER_CHANGED;
        }

        private void UpdateList()
        {
            Entities = new(_database.GetCollectionEntities(_collectionName, _pageNumber));
        }

        private void OnDeleteClick(object? ignorableParameter)
        {
            if (ChosenEntity == null)
                return;
            var entity = ChosenEntity.GetEntity();
            _database.DeleteEntity(entity);
            UpdateList();
        }

        public void OnSaveClick(object? ignorableParameter)
        {
            if (ChosenEntity == null)
                return;
            var entity = ChosenEntity.GetEntity();
            if (entity.IsDefault())
                return;
            if (entity.Id == default)
            {
                _database.InsertEntity(entity);
                return;
            }
            if (_database.UpdateEntity(entity) > 0)
                SingleHeader = SINGLE_HEADER;
            UpdateList();
            ListTabSelected = true;
        }

        private void OnAddClick(object? ignorableParameter)
        {
            SelectedEntity = _collectionName switch
            {
                DatabaseInteractor.ORDER_COLLECTION_NAME => Order.Default,
                DatabaseInteractor.ITEMS_COLLECTION_NAME => Item.Default,
                DatabaseInteractor.CATEGORIES_COLLECTION_NAME => Category.Default,
                DatabaseInteractor.EMPLOYEE_COLLECTION_NAME => Employee.Default,
                _ => null
            };
#warning тут перехватить исключение
            ChosenEntity = GetEntityViewModel(SelectedEntity);
            SingleTabSelected = true;
        }
    }
}
