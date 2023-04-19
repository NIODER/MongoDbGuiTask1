using Database.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MongoDbGuiTask1.ViewModel
{
    internal class EmployeeViewModel : ViewModelBase, IEntityViewModel
    {
        public event DocumentUpdatedEventHandler? DocumentUpdated;

		private readonly Employee _employee;

        public EmployeeViewModel([NotNull] Employee employee)
        {
            _employee = employee;
        }

        public string Name
        {
            get => _employee.Name;
            set
            {
                _employee.Name = value;
                OnPropertyChanged(nameof(Name));
                DocumentUpdated?.Invoke();
            }
        }

        public string Position
        {
            get => _employee.Position;
            set
            {
                _employee.Position = value;
                OnPropertyChanged(nameof(Position));
                DocumentUpdated?.Invoke();
            }
        }

        public int Salary
        {
            get => _employee.Salary;
            set
            {
                _employee.Salary = value;
                OnPropertyChanged(nameof(Salary));
                DocumentUpdated?.Invoke();
            }
        }

        public long PasswordData
        {
            get => _employee.PasswordData;
            set
            {
                _employee.PasswordData = value;
                OnPropertyChanged(nameof(PasswordData));
                DocumentUpdated?.Invoke();
            }
        }

        public string Address
        {
            get => _employee.Address;
            set
            {
                _employee.Address = value;
                OnPropertyChanged(nameof(Address));
                DocumentUpdated?.Invoke();
            }
        }

        public string PhoneNumber
        {
            get => _employee.PhoneNumber;
            set
            {
                _employee.PhoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
                DocumentUpdated?.Invoke();
            }
        }

        public string Email
        {
            get => _employee.Email;
            set
            {
                _employee.Email = value;
                OnPropertyChanged(nameof(Email));
                DocumentUpdated?.Invoke();
            }
        }

        public DbEntity GetEntity() => _employee;
    }
}
