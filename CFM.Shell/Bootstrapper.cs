using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Bulldog.FlyoutManager;
using CFM.Data;
using CFM.Data.Models;
using CFM.Infrastructure;
using CFM.Infrastructure.Constants;
using CFM.Infrastructure.Interfaces;
using CFM.Infrastructure.RegionAdapters;
using CFM.Infrastructure.Repositories;
using CFM.Infrastructure.Services;
using CFM.Shell.Views;
using MahApps.Metro.Controls;
using Mehdime.Entity;
using Microsoft.Practices.ServiceLocation;
using Prism.Unity;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;

namespace CFM.Shell
{
    public class Bootstrapper: UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            Container.RegisterInstance(typeof(Window), WindowNames.MainWindowName, Container.Resolve<MainWindow>(), new ContainerControlledLifetimeManager());
            return Container.Resolve<Window>(WindowNames.MainWindowName);
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

            // Register services
            this.RegisterServices();

            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            //AmbientDbContext
            Container.RegisterType<IDbContextScopeFactory, DbContextScopeFactory>(new ContainerControlledLifetimeManager(), new InjectionConstructor());
            Container.RegisterType<IAmbientDbContextLocator, AmbientDbContextLocator>(new ContainerControlledLifetimeManager());
            //Repositories
            //Container.RegisterType<CfmDbContext>(new PerResolveLifetimeManager());
            Container.RegisterType<IProfessorRepository, ProfessorRepository>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IUnitRepository, UnitRepository>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IAssignmentRepository, AssignmentRepository>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IFeedbackRepository, FeedbackRepository>(new ContainerControlledLifetimeManager());
            // Application commands
            Container.RegisterType<IApplicationCommands, ApplicationCommandsProxy>(new ContainerControlledLifetimeManager());
           
        }

        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();
            ModuleCatalog moduleCatalog = (ModuleCatalog)this.ModuleCatalog;
            moduleCatalog.AddModule(typeof(AssignmentModule.AssignmentModule));
            moduleCatalog.AddModule(typeof(UnitModule.UnitModule));
            moduleCatalog.AddModule(typeof(ProfessorModule.ProfessorModule));
            moduleCatalog.AddModule(typeof(FeedbackModule.FeedbackModule));
        }

        protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            var mappings = base.ConfigureRegionAdapterMappings();
            mappings.RegisterMapping(typeof(StackPanel), ServiceLocator.Current.GetInstance<StackPanelRegionAdapter>());
            mappings.RegisterMapping(typeof(FlyoutsControl), Container.Resolve<FlyoutsControlRegionAdapter>());
            return mappings;
        }

        private void RegisterServices()
        {
            // Flyout services
            Container.RegisterType<IFlyoutManager, FlyoutManager>(new ContainerControlledLifetimeManager());
            Container.RegisterInstance<IFlyoutService>(Container.Resolve<FlyoutService>());
            // Dialog services
            Container.RegisterType<IDialogService, DialogService>(new ContainerControlledLifetimeManager());
        }
    }
}
