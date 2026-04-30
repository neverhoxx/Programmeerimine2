using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace KooliProjekt.WpfApplication
{
    public class MainWindowViewModel : NotifyPropertyChangedBase
    {
        private readonly ObservableCollection<Project> _data;
        private readonly IApiClient _apiClient;
        private readonly IDialogProvider _dialogProvider;

        public ICommand AddNewCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }

        private Project _selectedItem;

        public MainWindowViewModel() : this(new ApiClient(), new DialogProvider())
        {
        }

        public MainWindowViewModel(IApiClient apiClient, IDialogProvider dialogProvider)
        {
            _apiClient = apiClient;
            _dialogProvider = dialogProvider;
            _data = new ObservableCollection<Project>();

            AddNewCommand = new RelayCommand<Project>(
                project =>
                {
                    SelectedItem = new Project();
                });
            SaveCommand = new RelayCommand<Project>(
                async project =>
                {
                    var result = await _apiClient.Save(project);
                    if (result.HasErrors)
                    {
                        ShowError("Cannot save data", result);
                        return;
                    }

                    SelectedItem = null;
                    await LoadData();
                },
                todoList =>
                {
                    return SelectedItem != null;
                });
            DeleteCommand = new RelayCommand<Project>(
                async project =>
                {
                    // Use the two-argument Confirm overload so unit tests that setup this overload will match
                    var canDelete = _dialogProvider.Confirm("Are you sure you want to delete this item?", "Confirm");
                    if (!canDelete)
                    {
                        return;
                    }

                    var result = await _apiClient.Delete(project.Id);
                    if (result.HasErrors)
                    {
                        ShowError("Cannot delete data", result);
                        return;
                    }
                    SelectedItem = null;
                    await LoadData();
                },
                project =>
                {
                    return SelectedItem != null && SelectedItem.Id != 0;
                });
        }

        public async Task LoadData()
        {
            var data = await _apiClient.List(1, 100);
            _data.Clear();

            if (data.HasErrors)
            {
                ShowError("Cannot load data", data);
                return;
            }

            foreach (var item in data.Value.Results)
            {
                _data.Add(item);
            }
        }

        public ObservableCollection<Project> Data
        {
            get
            {
                return _data;
            }
        }

        public Project SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;
                NotifyPropertyChanged();
            }
        }

        public void ShowError(string message, OperationResult result)
        {
            var error = message + "\r\n";
            var apiErrors = "";
            var propertyErrors = "";

            if (result.Errors != null)
            {
                foreach (var apiError in result.Errors)
                {
                    apiErrors += apiError + "\r\n";
                }
            }

            if (result.PropertyErrors != null)
            {
                foreach (var propertyError in result.PropertyErrors)
                {
                    propertyErrors += propertyError.Key + ": " + propertyError.Value;
                }
            }

            if (!string.IsNullOrEmpty(apiErrors))
            {
                error += "\r\n" + apiErrors + "\r\n";
            }

            if (!string.IsNullOrEmpty(propertyErrors))
            {
                error += "\r\n" + propertyErrors;
            }

            error = error.Trim();

            _dialogProvider.ShowError(error);
        }
    }

    //public class AddNewCommand : ICommand
    //{
    //    public event EventHandler CanExecuteChanged;

    //    public bool CanExecute(object parameter)
    //    {
    //        return true;
    //    }

    //    public void Execute(object parameter)
    //    {
    //        MessageBox.Show("Execute");
    //    }
    //}
}
