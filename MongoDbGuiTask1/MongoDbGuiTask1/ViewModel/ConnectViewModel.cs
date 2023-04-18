using MongoDB.Driver;
using MongoDbGuiTask1.Model;
using System;
using System.Windows;

namespace MongoDbGuiTask1.ViewModel
{
    public class ConnectViewModel : ViewModelBase
    {
		private string _connectionString = string.Empty;
        private RelayCommand? _relayCommand;

		private void OnConnectClick(object? window)
		{
            if (window == null)
                throw new NullReferenceException();
            if (_connectionString == string.Empty)
            {
                MessageBox.Show("Введите значение.", "Ошибка");
                return;
            }
            DatabaseInteractor.ConnectionString = _connectionString;
            try
            {
                _ = DatabaseInteractor.Instance();
            }
            catch (TimeoutException)
            {
                MessageBox.Show("Ошибка подключения:\n" + _connectionString, "Ошибка");
                return;
            }
            catch (MongoAuthenticationException ex)
            {
                MessageBox.Show("Ошибка аутентификации.\n" + ex.Message, "Ошибка");
                return;
            }
            WindowPresenter.ShowWindow(WindowPresenter.WindowType.MainWindow);
            WindowPresenter.CloseWindow(window.GetType());
        }

        public string ConnectionString
		{
			get { return _connectionString; }
			set 
			{
				_connectionString = value;
				OnPropertyChanged(nameof(ConnectionString));
			}
		}

		public RelayCommand Connect
		{
			get => _relayCommand ??= new RelayCommand(OnConnectClick);
		}
	}
}
