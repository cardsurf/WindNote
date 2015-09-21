using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using WindNote.Events;
using System.Reflection;
using System.Globalization;
using System.Data.Entity;
using WindNote.MvvmBase;
using WindNote.DataAccessLayer.Repositories;
using Microsoft.Practices.ServiceLocation;
using WindNote.DataAccessLayer.Queries;
using WindNote.DataAccessLayer.Context;
using WindNote.Model;
using WindNote.Threads.TimeIntervalExecutor;
using System.IO;
using WindNote.Assemblies;

namespace WindNote
{
    public partial class App : Application
    {
        private const int UPDATE_DATABASE_EVERY_SECONDS = 60 * 5;

        private IUnityContainer container;
        private ITimeIntervalExecutor updateDatabaseNotifier;

        void App_Startup(object sender, StartupEventArgs e)
        {
            this.InitContainer();
            this.InitUpdateDatabaseNotifier();
            this.BindViewModelsToViews();
        }

        private void InitContainer()
        {
            this.container = new UnityContainer();
            this.RegisterTypesUnityContainer();
        }

        private void RegisterTypesUnityContainer()
        {
            container.RegisterType<IEventAggregator, EventAggregator>(new ContainerControlledLifetimeManager());
            container.RegisterType<IApplicationDispatcher, ApplicationDispatcher>();
            container.RegisterType<IRepositoryFactory, RepositoryFactory>();
            container.RegisterType<IEntityFactory, EntityFactory>();
        }

        private void InitUpdateDatabaseNotifier()
        {
            this.updateDatabaseNotifier = new TimeIntervalExecutor(this.NotifyUpdateDatabase, UPDATE_DATABASE_EVERY_SECONDS);
            this.updateDatabaseNotifier.Start();
        }

        private void NotifyUpdateDatabase(object sender, EventArgs e)
        {
            IEventAggregator eventAggregator = container.Resolve<EventAggregator>();
            eventAggregator.GetEvent<UpdateDatabaseEvent>().Publish(null);
        }

        private void BindViewModelsToViews()
        {
            this.SetProjectViewModelsFolderLocation();
            ViewModelLocationProvider.SetDefaultViewModelFactory((type) => container.Resolve(type));
        }

        private void SetProjectViewModelsFolderLocation()
        {
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver((view) =>
            {
                String viewFileName = view.FullName;
                String viewAssemblyName = view.GetTypeInfo().Assembly.FullName;
                String viewModelFileName = this.GetViewModelFileName(viewFileName, viewAssemblyName);
                return Type.GetType(viewModelFileName);
             });
        }

        private String GetViewModelFileName(String viewFileName, String assemblyName)
        {
            String viewFileNameInViewModelFolder = viewFileName.Replace(".View", ".ViewModel");
            return String.Format(CultureInfo.InvariantCulture, "{0}ViewModel, {1}", viewFileNameInViewModelFolder, assemblyName);
        }
       
        void App_Exit(object sender, ExitEventArgs e)
        {
            IEventAggregator eventAggregator = container.Resolve<EventAggregator>();
            eventAggregator.GetEvent<ApplicationExitEvent>().Publish(null);
        }

    }
}
