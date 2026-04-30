using System;
using System.Collections.Generic;
using System.Text;

namespace KooliProjekt.WindowsForms
{
    public interface IMainView
    {
        IList<Project> DataSource { get; set; }
        Project SelectedItem { get; set; }
        void SetPresenter(MainViewPresenter presenter);
        void ShowError(string message, OperationResult result);
        int CurrentId { get; set; }
        string CurrentName { get; set; }
        DateTime CurrentStartDate { get; set; }
        DateTime CurrentDueDate { get; set; }
        decimal CurrentBudget { get; set; }
        decimal CurrentPricePerHour { get; set; }
        bool ConfirmDelete();
    }
}