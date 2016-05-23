using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bulldog.FlyoutManager;

namespace CFM.UnitModule.ViewModels
{
    class EditUnitFlyoutViewModel: FlyoutBase
    {
        public EditUnitFlyoutViewModel()
        {
            Theme = FlyoutTheme.Dark;
            Position = FlyoutPosition.Bottom;
        }

        private string _message;

        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        protected override void OnOpening(FlyoutParameters flyoutParameters)
        {
            base.OnOpening(flyoutParameters);
            int id = Convert.ToInt32(flyoutParameters["unitId"]);
            Message = "You are edditing unit # " + id;
        }
    }
}
