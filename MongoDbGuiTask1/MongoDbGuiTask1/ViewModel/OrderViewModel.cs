using Database.Entities;

namespace MongoDbGuiTask1.ViewModel
{
    internal class OrderViewModel : ViewModelBase, IEntityViewModel
    {
        public event DocumentUpdatedEventHandler? DocumentUpdated;

        private readonly Order _order;

        public OrderViewModel(Order order)
        {
            _order = order;
        }

        public string Customer
        {
            get => _order.Customer;
            set
            {
                _order.Customer = value;
                OnPropertyChanged(nameof(Customer));
                DocumentUpdated?.Invoke();
            }
        }

        public int Status
        {
            get => _order.Status;
            set
            {
                _order.Status = value;
                OnPropertyChanged(nameof(Status));
                DocumentUpdated?.Invoke();
            }
        }

        public int CategoryId
        {
            get => _order.CategoryId;
            set
            {
                _order.CategoryId = value;
                OnPropertyChanged(nameof(CategoryId));
                DocumentUpdated?.Invoke();
            }
        }

        public int ItemId
        {
            get => _order.ItemId;
            set
            {
                _order.ItemId = value;
                OnPropertyChanged(nameof(ItemId));
                DocumentUpdated?.Invoke();
            }
        }

        public int Quantity
        {
            get => _order.Quantity;
            set
            {
                _order.Quantity = value;
                OnPropertyChanged(nameof(Quantity));
                DocumentUpdated?.Invoke();
            }
        }

        public string Date
        {
            get => _order.Date;
            set
            {
                _order.Date = value;
                OnPropertyChanged(nameof(Date));
                DocumentUpdated?.Invoke();
            }
        }

        public DbEntity GetEntity() => _order;
    }
}
