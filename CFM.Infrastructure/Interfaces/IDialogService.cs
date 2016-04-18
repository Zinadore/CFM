using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MahApps.Metro.Controls.Dialogs;

namespace CFM.Infrastructure.Interfaces
{
    public interface IDialogService
    {
        Task<MessageDialogResult> ShowMessageAsnyc(string title, string message, 
                                                    MessageDialogStyle style = MessageDialogStyle.Affirmative, 
                                                    MetroDialogSettings settings = null);
    }
}
