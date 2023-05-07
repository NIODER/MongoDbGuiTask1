using Database.Entities;
using Microsoft.VisualBasic;
using MongoDbGuiTask1.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Documents;

namespace MongoDbGuiTask1.ViewModel
{
    internal class MainViewModel : ViewModelBase
    {
        private readonly DatabaseInteractor _database;

        private const string SINGLE_HEADER = "Выбранный документ";
        private const string SINGLE_HEADER_CHANGED = "Выбранный документ (не сохранено)";

        public delegate void UpdateDatabasesEventHandler(List<string> databases);
        public event UpdateDatabasesEventHandler? UpdateDatabases;

        private IEntityViewModel? _chosenEntity;
        private ObservableCollection<DbEntity> _entities;
        private bool listTabSelected;
        private string singleHeader = SINGLE_HEADER;
        private bool addCategoryButtonEnabled = false;
        private string _newDatabaseName = string.Empty;

        private TreeViewItem? TreeViewItem { get; set; }

        public RelayCommand ItemClick { get; private set; }
        public RelayCommand DeleteClick { get; private set; }
        public RelayCommand SaveClick { get; private set; }
        public RelayCommand AddClick { get; private set; }
        public RelayCommand AddCollectionClick { get; set; }
        public RelayCommand CreateDatabaseClick { get; set; }
        public RelayCommand CommitNewDatabaseNameClick { get; set; }

        private ItemsListViewModel? _itemsListViewModel;
        public ItemsListViewModel? ItemsListViewModel
        {
            get => _itemsListViewModel;
            set
            {
                _itemsListViewModel = value;
                OnPropertyChanged(nameof(ItemsListViewModel));
            }
        }

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
            ItemClick = new(OnItemClick);
            DeleteClick = new(OnDeleteClick);
            SaveClick = new(OnSaveClick);
            AddClick = new(OnAddClick);
            AddCollectionClick = new(OnAddCollectionClick);
            CreateDatabaseClick = new(OnCreateDatabaseClick);
            CommitNewDatabaseNameClick = new(OnCommitNewDatabaseNameClick);
        }

        public string NewDatabaseName
        {
            get => _newDatabaseName;
            set
            {
                _newDatabaseName = value;
                OnPropertyChanged(nameof(NewDatabaseName));
            }
        }

        public bool AddCategoryButtonEnabled
        {
            get => addCategoryButtonEnabled;
            set
            {
                addCategoryButtonEnabled = value;
                OnPropertyChanged(nameof(AddCategoryButtonEnabled));
            }
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

        public void DatabaseExpanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem = (TreeViewItem)sender;
            if (TreeViewItem.Items.Count > 0)
                return;
            try
            {
                _database.SelectedDatabaseName = TreeViewItem.Header.ToString() ?? throw new NullReferenceException("database name");
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Выбранная база данных не найдена.", "Ошибка");
            }
            catch(NullReferenceException)
            {
                MessageBox.Show("База данных не выбрана (внутренняя ошибка).", "Ошибка");
            }
            var collections = _database.Collections();
            foreach (var collection in collections)
            {
                var collectionButton = new Hyperlink(new Run(collection));
                collectionButton.Click += CollectionExpanded;
                TreeViewItem.Items.Add(collectionButton);
            }
        }

        private void CollectionExpanded(object sender, RoutedEventArgs e)
        {
            string collectionName = ((Run)((Hyperlink)sender).Inlines.FirstInline).Text;
            ItemsListViewModel = new(collectionName, OnItemClick);
            ListTabSelected = true;
            AddCategoryButtonEnabled = true;
        }

        private void OnItemClick(object? ignorableParameter)
        {
            try
            {
                ChosenEntity = GetEntityViewModel(ItemsListViewModel?.SelectedEntity);
            }
            catch (InvalidCastException)
            {
                MessageBox.Show(ItemsListViewModel?.SelectedEntity == null 
                    ? "Не выбран документ." 
                    : "Неправильный формат документа.", "Ошибка");
                return;
            }
            ChosenEntity.DocumentUpdated += OnDocumentUpdated;
            SingleTabSelected = true;
        }

        private static IEntityViewModel GetEntityViewModel(DbEntity? dbEntity) => dbEntity switch
        {
            Category => new CategoryViewModel((Category)dbEntity),
            Employee => new EmployeeViewModel((Employee)dbEntity),
            Item => new ItemViewModel((Item)dbEntity),
            Order => new OrderViewModel((Order)dbEntity),
            _ => throw new InvalidCastException()
        };

        private void OnDocumentUpdated()
        {
            SingleHeader = SINGLE_HEADER_CHANGED;
        }

        private void OnDeleteClick(object? ignorableParameter)
        {
            if (ChosenEntity == null)
                return;
            var entity = ChosenEntity.GetEntity();
            _database.DeleteEntity(entity);
            ItemsListViewModel?.UpdateList();
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
            ItemsListViewModel?.UpdateList();
            ListTabSelected = true;
        }

        private void OnAddClick(object? ignorableParameter)
        {
            if (ItemsListViewModel == null)
            {
                MessageBox.Show("Выберите коллекцию перед добавлением.", "Ошибка");
                return;
            }
            ItemsListViewModel.SelectedEntity = ItemsListViewModel.CollectionName switch
            {
                DatabaseInteractor.ORDER_COLLECTION_NAME => Order.Default,
                DatabaseInteractor.ITEMS_COLLECTION_NAME => Item.Default,
                DatabaseInteractor.CATEGORIES_COLLECTION_NAME => Category.Default,
                DatabaseInteractor.EMPLOYEE_COLLECTION_NAME => Employee.Default,
                _ => null
            };
            try
            {
                ChosenEntity = GetEntityViewModel(ItemsListViewModel.SelectedEntity);
            }
            catch (InvalidCastException)
            {
                MessageBox.Show("Произошла непредвиденная ошибка.", "Ошибка");
                return;
            }
            SingleTabSelected = true;
        }

        private void OnAddCollectionClick(object? collectionName)
        {
            if (collectionName is not string || collectionName == null)
                return;
            _database.AddCollection((string)collectionName);
            var collectionButton = new Hyperlink(new Run((string)collectionName));
            collectionButton.Click += CollectionExpanded;
            TreeViewItem?.Items.Add(collectionButton);
        }

        private void OnCreateDatabaseClick(object? ignorableParameter)
        {
            WindowPresenter.ShowWindow(WindowPresenter.WindowType.DbNameDialog);
        }

        private void OnCommitNewDatabaseNameClick(object? ignorableParameter)
        {
            if (string.IsNullOrEmpty(NewDatabaseName))
            {
                MessageBox.Show("Введите название базы данных.", "Ошибка");
                return;
            }
            if (_database.Databases.Contains(NewDatabaseName))
            {
                MessageBox.Show("База данных с таким названием уже существует.", "Ошибка");
                return;
            }
            _database.CreateDatabase(NewDatabaseName);
            WindowPresenter.CloseWindow(WindowPresenter.WindowType.DbNameDialog);
            MessageBox.Show($"Создана база данных {NewDatabaseName}", "Внимание");
            UpdateDatabases?.Invoke(_database.Databases);
        }
    }
}
