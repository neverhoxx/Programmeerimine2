using KooliProjekt.WindowsForms.Api;

namespace KooliProjekt.WindowsForms
{
    public class MainViewPresenter
    {
        private readonly IApiClient _apiClient;
        private readonly IMainView _mainView;

        private Project _selectedList;

        public MainViewPresenter(IApiClient apiClient, IMainView mainView)
        {
            _apiClient = apiClient;
            _mainView = mainView;
            _mainView.SetPresenter(this);
        }

        public async Task LoadData()
        {
            var response = await _apiClient.List(1, 100);
            if (response.HasErrors)
            {
                _mainView.ShowError("Viga andmete laadimisel", response);
                _mainView.DataSource = null;
                return;
            }

            _mainView.DataSource = response.Value.Results;
        }

        public void SetSelection(Project selectedList)
        {
            _selectedList = selectedList;
            if (_selectedList == null)
            {
                _mainView.CurrentId = 0;
                _mainView.CurrentName = "";
                _mainView.CurrentBudget = 0;
                _mainView.CurrentPricePerHour = 0;
                _mainView.CurrentStartDate = DateTime.Now;
                _mainView.CurrentDueDate = DateTime.Now;
            }
            else
            {
                _mainView.CurrentId = _selectedList.Id;
                _mainView.CurrentName = _selectedList.Name;
                _mainView.CurrentBudget = _selectedList.Budget;
                _mainView.CurrentPricePerHour = _selectedList.PricePerHour;
                _mainView.CurrentStartDate = _selectedList.StartDate;
                _mainView.CurrentDueDate = _selectedList.DueDate;
            }


        }

        public async Task Save()
        {
            var project = new Project();
            project.Id = _mainView.CurrentId;
            project.Name = _mainView.CurrentName;
            project.Budget = _mainView.CurrentBudget;
            project.PricePerHour = _mainView.CurrentPricePerHour;
            project.StartDate = _mainView.CurrentStartDate;
            project.DueDate = _mainView.CurrentDueDate;

            var result = await _apiClient.Save(project);
            if (result.HasErrors)
            {
                _mainView.ShowError("Error salvestamisel", result);
                return;
            }

            await LoadData();
        }

        public async Task Delete()
        {
            if (!_mainView.ConfirmDelete())
            {
                return;
            }

            var result = await _apiClient.Delete(_mainView.CurrentId);
            if (result.HasErrors)
            {
                _mainView.ShowError("Error kustutamisel", result);
                return;
            }

            await LoadData();
        }
    }
}
