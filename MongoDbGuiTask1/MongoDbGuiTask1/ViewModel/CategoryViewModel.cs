using Database.Entities;
using MongoDbGuiTask1.Model;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace MongoDbGuiTask1.ViewModel
{
    class CategoryViewModel : ViewModelBase, IEntityViewModel
    {
        public event DocumentUpdatedEventHandler? DocumentUpdated;

        private readonly Category _category;

        private ObservableCollection<Item> items;

        public RelayCommand DeleteItemCommand { get; private set; }
        public RelayCommand AddItemCommand { get; private set; }
        private ItemsListViewModel itemsList;

        public ItemsListViewModel ItemsListViewModel
        {
            get { return itemsList; }
            set
            {
                itemsList = value;
                OnPropertyChanged(nameof(ItemsListViewModel));
            }
        }


        public CategoryViewModel([NotNull] Category category)
        {
            _category = category;
            items = _category.Items == null ? (new()) : (new(_category.Items));
            DeleteItemCommand = new(OnDeleteItemClick);
            AddItemCommand = new(OnAddItemClick);
            itemsList = new(DatabaseInteractor.ITEMS_COLLECTION_NAME, OnItemSelected);
        }

        public string Id { get => _category.Id.ToString(); }

        public string CategoryName
        {
            get => _category.CategoryName;
            set
            {
                _category.CategoryName = value;
                OnPropertyChanged(nameof(CategoryName));
                DocumentUpdated?.Invoke();
            }
        }

        public ObservableCollection<Item> Items
        {
            get => items;
            set
            {
                items = value;
                OnPropertyChanged(nameof(Items));
                DocumentUpdated?.Invoke();
            }
        }

        public DbEntity GetEntity()
        {
            _category.Items = Items.ToList();
            return _category;
        }

        private void OnDeleteItemClick(object? item)
        {
            if (item is Item item1)
                Items.Remove(item1);
        }

        private void OnAddItemClick(object? ignorable)
        {
            WindowPresenter.ShowWindow(WindowPresenter.WindowType.AddItemCategoryWindow, this);
        }

        private void OnItemSelected(object? ignorable)
        {
            if (ItemsListViewModel.SelectedEntity is not null and Item)
                Items.Add((Item)ItemsListViewModel.SelectedEntity);
            WindowPresenter.CloseWindow(WindowPresenter.WindowType.AddItemCategoryWindow);
        }
    }
}
