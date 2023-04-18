using Database.Entities;

namespace MongoDbGuiTask1.ViewModel
{
    internal class ItemViewModel : ViewModelBase, IEntityViewModel
    {
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
            }
        }

        public long Price
        {
            get => _item.Price;
            set
            {
                _item.Price = value;
                OnPropertyChanged(nameof(Price));
            }
        }

        public int Count
        {
            get => _item.Count;
            set
            {
                _item.Count = value;
                OnPropertyChanged(nameof(Count));
            }
        }

        public string Company
        {
            get => _item.Company;
            set
            {
                _item.Company = value;
                OnPropertyChanged(nameof(Company));
            }
        }

        public DbEntity GetEntity() => _item;
    }
}
