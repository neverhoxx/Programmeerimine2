using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace KooliProjekt.WpfApplication
{
    public class DialogProvider : IDialogProvider
    {
        public bool Confirm(string message)
        {
            return Confirm(message, "Confirm");
        }

        public bool Confirm(string message, string v)
        {
            var result = MessageBox.Show(
                message,
                v,
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );
            return (result == MessageBoxResult.Yes);
        }

        public void ShowError(string error)
        {
            MessageBox.Show(
                error,
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );
        }
    }
}
