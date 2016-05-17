﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CFM.Data.Models;
using CFM.Infrastructure;
using CFM.Infrastructure.Base;
using CFM.Infrastructure.Events;
using CFM.Infrastructure.Interfaces;
using CFM.Infrastructure.Repositories;
using CFM.ProfessorModule.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace CFM.ProfessorModule.ViewModels
{
    public class ProfessorsViewModel: SearchableBindableBase<Professor>
    {
        private readonly IProfessorRepository _repository;
        private readonly IApplicationCommands _applicationCommands;

        public DelegateCommand<int?> ProfessorDetailsCommand { get; private set; } 

        public ProfessorsViewModel(IProfessorRepository repository, IEventAggregator eventAggregator,
                                    IApplicationCommands applicationCommands)
        {
            _repository = repository;
            _applicationCommands = applicationCommands;
            eventAggregator.GetEvent<ProfessorAddedEvent>().Subscribe(HandleNewProffesorEvent);

            ProfessorDetailsCommand = new DelegateCommand<int?>(ShowDetails);
        }

        private void ShowDetails(int? id)
        {
            var uriQuery = new NavigationParameters();
            uriQuery.Add("profId",id);
            Filter = "";//Clear the search filter
            _applicationCommands.NavigateCommand.Execute(typeof(ProfessorDetailsView).FullName + uriQuery);
        }

        private async void HandleNewProffesorEvent(Professor obj)
        {
            await Task.Run(() =>
            {
                Professors = _repository.GetAll();
            });
        }


        private ICollection<Professor> _professors;
        public ICollection<Professor> Professors
        {
            get { return _professors; }
            set
            {
                SetProperty(ref _professors, value);
                FilteredCollection = value;
            }
        }

        public async void LoadData()
        {
            await Task.Run(() =>
            {
                Professors =  _repository.GetAll();
            });
        }


        protected override void ApplyFilter(string filter)
        {
            FilteredCollection = string.IsNullOrWhiteSpace(filter) ? Professors : Professors.Where(p => p.FullName.ToLower().Contains(filter.ToLower())).ToList();
        }
    }
}
