using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CFM.Data.Models;
using CFM.Infrastructure;
using CFM.Infrastructure.Base;
using CFM.Infrastructure.Events;
using CFM.Infrastructure.Repositories;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace CFM.UnitModule.ViewModels
{
    public class UnitsViewModel: SearchableBindableBase<Unit>
    {
        private readonly IUnitRepository _repository;
        private readonly IEventAggregator _eventAggregator;
        private readonly IApplicationCommands _applicationCommands;

        public DelegateCommand<int?> ProfessorDetailsCommand { get; private set; }

        public UnitsViewModel(IUnitRepository repository, IEventAggregator eventAggregator,
                                    IApplicationCommands applicationCommands)
        {
            _repository = repository;
            _eventAggregator = eventAggregator;
            _applicationCommands = applicationCommands;
            eventAggregator.GetEvent<UnitAddedEvent>().Subscribe(HandleNewUnitEvent);

            ProfessorDetailsCommand = new DelegateCommand<int?>(ShowDetails);
        }

        private void ShowDetails(int? id)
        {
            var uriQuery = new NavigationParameters();
            uriQuery.Add("profId", id);
            Filter = "";//Clear the search filter
            //_applicationCommands.NavigateCommand.Execute(typeof(UnityDetailsView).FullName + uriQuery);
        }

        private async void HandleNewUnitEvent(Unit obj)
        {
            await Task.Run(async () =>
            {
                Units = await _repository.GetAllAsync();
            });
        }

        private ICollection<Unit> _units;
        public ICollection<Unit> Units
        {
            get { return _units; }
            set
            {
                SetProperty(ref _units, value);
                FilteredCollection = value;
            }
        }

        public async void LoadData()
        {
            await Task.Run(async () =>
            {
                Units = await _repository.GetAllAsync();
            });
        }


        protected override void ApplyFilter(string filter)
        {
            FilteredCollection = string.IsNullOrWhiteSpace(filter) ? Units : Units.Where((u) =>
                u.Title.ToLower().Contains(filter.ToLower()) || u.Code.ToLower().Contains(filter.ToLower())
            ).ToList();
        }
    }
}
