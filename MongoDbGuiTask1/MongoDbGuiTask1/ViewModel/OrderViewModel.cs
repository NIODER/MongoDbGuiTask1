﻿using Database.Entities;

namespace MongoDbGuiTask1.ViewModel
{
    internal class OrderViewModel : ViewModelBase, IEntityViewModel
    {
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
            }
        }

        public int Status
        {
            get => _order.Status;
            set
            {
                _order.Status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        public int CategoryId
        {
            get => _order.CategoryId;
            set
            {
                _order.CategoryId = value;
                OnPropertyChanged(nameof(CategoryId));
            }
        }

        public int ItemId
        {
            get => _order.ItemId;
            set
            {
                _order.ItemId = value;
                OnPropertyChanged(nameof(ItemId));
            }
        }

        public int Quantity
        {
            get => _order.Quantity;
            set
            {
                _order.Quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }

        public string Date
        {
            get => _order.Date;
            set
            {
                _order.Date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        public DbEntity GetEntity() => _order;
    }
}
