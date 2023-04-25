using Database.Entities;
using Microsoft.VisualBasic;
using MongoDbGuiTask1.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Sockets;
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

        public delegate void UpdateDatabasesEventHandler(List<string> databases);
        public event UpdateDatabasesEventHandler? UpdateDatabases;

        private DbEntity? _selectedEntity;
        private IEntityViewModel? _chosenEntity;
        private ObservableCollection<DbEntity> _entities;
        private int _pageNumber;
        private string _collectionName;
        private bool listTabSelected;
        private string singleHeader = SINGLE_HEADER;
        private bool nextButtonActive = true;
        private bool prevButtonActive = false;
        private bool addCategoryButtonEnabled = false;
        private string _newDatabaseName = string.Empty;

        private TreeViewItem TreeViewItem { get; set; }

        public RelayCommand ItemClick { get; private set; }
        public RelayCommand DeleteClick { get; private set; }
        public RelayCommand SaveClick { get; private set; }
        public RelayCommand AddClick { get; private set; }
        public RelayCommand NextClick { get; set; }
        public RelayCommand PrevClick { get; set; }
        public RelayCommand AddCollectionClick { get; set; }
        public RelayCommand CreateDatabaseClick { get; set; }
        public RelayCommand CommitNewDatabaseNameClick { get; set; }

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
            _collectionName = ((Run)((Hyperlink)sender).Inlines.FirstInline).Text;
            UpdateList();
            ListTabSelected = true;
            AddCategoryButtonEnabled = true;
        }

        private void OnItemClick(object? ignorableParameter)
        {
            try
            {
                ChosenEntity = GetEntityViewModel(SelectedEntity);
            }
            catch (InvalidCastException)
            {
                MessageBox.Show(SelectedEntity == null 
                    ? "Не выбран документ." 
                    : "Неправильный формат документа.", "Ошибка");
                return;
            }
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
            try
            {
                ChosenEntity = GetEntityViewModel(SelectedEntity);
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
            TreeViewItem.Items.Add(collectionButton);
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
