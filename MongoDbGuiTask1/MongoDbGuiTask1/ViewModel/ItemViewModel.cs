using Database.Entities;

namespace MongoDbGuiTask1.ViewModel
{
    internal class ItemViewModel : ViewModelBase, IEntityViewModel
    {
        public event DocumentUpdatedEventHandler? DocumentUpdated;

		private readonly Item _item;

        public ItemViewModel(Item item)
        {
            _item = item;
        }

        public string Name
        {
            get => _item.Name;
            set
            {
                _item.Name = value;
                OnPropertyChanged(nameof(Name));
                DocumentUpdated?.Invoke();
            }
        }

        public long Price
        {
            get => _item.Price;
            set
            {
                _item.Price = value;
                OnPropertyChanged(nameof(Price));
                DocumentUpdated?.Invoke();
            }
        }

        public int Count
        {
            get => _item.Count;
            set
            {
                _item.Count = value;
                OnPropertyChanged(nameof(Count));
                DocumentUpdated?.Invoke();
            }
        }

        public string Company
        {
            get => _item.Company;
            set
            {
                _item.Company = value;
                OnPropertyChanged(nameof(Company));
                DocumentUpdated?.Invoke();
            }
        }

        public DbEntity GetEntity() => _item;
    }
}
