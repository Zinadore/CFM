﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CFM.Data;
using CFM.Infrastructure;
using CFM.Infrastructure.Constants;
using CFM.Infrastructure.Interfaces;
using CFM.Infrastructure.RegionAdapters;
using CFM.Infrastructure.Repositories;
using CFM.Infrastructure.Services;
using CFM.Shell.Views;
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

            var regionManager = this.Container.Resolve<IRegionManager>();
            if (regionManager != null)
            {
                // Add flyouts
                regionManager.RegisterViewWithRegion(RegionNames.FlyoutRegion, typeof(ShellSettingsFlyout));
            }

            // Register services
            this.RegisterServices();

            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            //Repositories
            Container.RegisterType<CfmDbContext>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IProfessorRepository, ProfessorRepository>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IUnitRepository, UnitRepository>(new ContainerControlledLifetimeManager());
            // Application commands
            Container.RegisterType<IApplicationCommands, ApplicationCommandsProxy>(new ContainerControlledLifetimeManager());
            // Flyout service
            Container.RegisterInstance<IFlyoutService>(Container.Resolve<FlyoutService>());
        }

        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();
            ModuleCatalog moduleCatalog = (ModuleCatalog)this.ModuleCatalog;
            moduleCatalog.AddModule(typeof(UnitModule.UnitModule));
            moduleCatalog.AddModule(typeof(ProfessorModule.ProfessorModule));
        }

        protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            var mappings = base.ConfigureRegionAdapterMappings();
            mappings.RegisterMapping(typeof(StackPanel), ServiceLocator.Current.GetInstance<StackPanelRegionAdapter>());
            return mappings;
        }

        private void RegisterServices()
        {
            Container.RegisterType<IDialogService, DialogService>(new ContainerControlledLifetimeManager());
        }
    }
}