using System;
using System.Collections.Generic;
using System.Text;

namespace KooliProjekt.WpfApplication
{
    public interface IDialogProvider
    {
        bool Confirm(string message);
        bool Confirm(string message, string v);
        void ShowError(string error);
    }
}

