using System;
using System.Collections.Generic;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using AutoMapper;
using Caliburn.Micro;
using PanoramioExplorer.ViewModels;
using PanoramioSDK;

namespace PanoramioExplorer
{
    public sealed partial class App
    {
        private WinRTContainer container;
        private IEventAggregator eventAggregator;

        public App()
        {
            InitializeComponent();
        }

        protected override void Configure()
        {
            ConfigureMapping();
            RegisterDependencies();
        }

        private void ConfigureMapping()
        {
         
        }

        private void RegisterDependencies()
        {
            container = new WinRTContainer();
            container.RegisterWinRTServices();

            container.Singleton<MapViewModel>();
            container.PerRequest<PanoramioClient>();

            eventAggregator = container.GetInstance<IEventAggregator>();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            // Note we're using DisplayRootViewFor (which is view model first)
            // this means we're not creating a root frame and just directly
            // inserting ShellView as the Window.Content

            DisplayRootViewFor<MapViewModel>();
        }

        protected override void OnSuspending(object sender, SuspendingEventArgs e)
        {
        }

        protected override object GetInstance(Type service, string key)
        {
            return container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            container.BuildUp(instance);
        }
    }
}
