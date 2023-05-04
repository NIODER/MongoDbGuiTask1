using Database.Entities;
using MongoDbGuiTask1.Model;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace MongoDbGuiTask1.ViewModel
{
    class CategoryViewModel : ViewModelBase, IEntityViewModel
    {
        public event DocumentUpdatedEventHandler? DocumentUpdated;

        private readonly Category _category;

        private ObservableCollection<Item> items;

        public RelayCommand DeleteItemCommand { get; private set; }

        public CategoryViewModel([NotNull] Category category)
        {
            _category = category;
            items = _category.Items == null ? (new()) : (new(_category.Items));
            DeleteItemCommand = new(OnDeleteItemClick);
        }

        public string Id
        {
            get => _category.Id.ToString();
        }

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

        public DbEntity GetEntity() => _category;

        private void OnDeleteItemClick(object? item)
        {
            if (item is Item item1)
                _category.Items?.Remove(item1);
        }
    }
}
