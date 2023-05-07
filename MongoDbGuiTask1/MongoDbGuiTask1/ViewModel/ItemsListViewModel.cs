using Database.Entities;
using MongoDbGuiTask1.Model;
using System;
using System.Collections.ObjectModel;

namespace MongoDbGuiTask1.ViewModel
{
    internal class ItemsListViewModel : ViewModelBase
    {
        private readonly DatabaseInteractor _database;
        private const int PAGE_SIZE = 10;

        public RelayCommand ItemClick { get; set; }
        public RelayCommand NextButton { get; private set; }
        public RelayCommand PrevCommand { get; private set; }
        private DbEntity? _selectedEntity;
        private ObservableCollection<DbEntity> _entities = new();
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

        private int _page = 0;
        private bool nextButtonActive = true;
        public bool NextButtonActive
        {
            get => nextButtonActive;
            set
            {
                nextButtonActive = value;
                OnPropertyChanged(nameof(NextButtonActive));
            }
        }
        private bool prevButtonActive = false;
        public bool PrevButtonActive
        {
            get => prevButtonActive;
            set
            {
                prevButtonActive = value;
                OnPropertyChanged(nameof(PrevButtonActive));
            }
        }
        private readonly string _collectionName;
        public string CollectionName { get => _collectionName; }

        public ItemsListViewModel(string collectionName, Action<object?> itemClick)
        {
            _database = DatabaseInteractor.Instance();
            ItemClick = new(itemClick);
            NextButton = new(OnNextButtonClick);
            PrevCommand = new(OnPrevButtonClick);
            _collectionName = collectionName;
            UpdateList();
        }

        public void UpdateList() => Entities = new(_database.GetCollectionEntities(_collectionName, _page));

        private void OnNextButtonClick(object? ignorable)
        {
            _page++;
            UpdateList();
            if (_page >= _database.GetCollectionLength(_collectionName) / PAGE_SIZE)
                NextButtonActive = false;
            if (_page > 0)
                PrevButtonActive = true;
        }

        private void OnPrevButtonClick(object? ignorable)
        {
            _page--;
            UpdateList();
            if (_page <= 0)
                PrevButtonActive = false;
            if (_page < _database.GetCollectionLength(_collectionName) / PAGE_SIZE)
                NextButtonActive = true;
        }
    }
}
