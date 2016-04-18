using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CFM.Data.Models;
using CFM.Infrastructure;
using CFM.Infrastructure.Repositories;
using Prism.Mvvm;

namespace CFM.UnitModule.ViewModels
{
    public class NewUnitFlyoutModel: BindableBase
    {
        private readonly IUnitRepository _unitRepository;
        private readonly IProfessorRepository _professorRepository;

        public NewUnitFlyoutModel(IUnitRepository unitRepository, IProfessorRepository professorRepository,
                                    IApplicationCommands applicationCommands)
        {
            _unitRepository = unitRepository;
            _professorRepository = professorRepository;
        }

        public async void LoadData()
        {
            await Task.Run(() =>
            {
                Professors = _professorRepository.GetAll();
            });
        }

        #region Properties

        private string _code;

        public string Code
        {
            get { return _code; }
            set { SetProperty(ref _code, value); }
        }

        private string _title;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private ICollection<Professor> _professors;

        public ICollection<Professor> Professors
        {
            get { return _professors; }
            set { SetProperty(ref _professors, value); }
        }

        private ICollection<Professor> _teachers;

        public ICollection<Professor> Teachers
        {
            get { return _teachers; }
            set { SetProperty(ref _teachers, value); }
        }
    #endregion
    }
}
