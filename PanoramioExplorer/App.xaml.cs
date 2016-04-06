using System;
using System.Collections.Generic;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Caliburn.Micro;
using PanoramioExplorer.ViewModels;

namespace PanoramioExplorer
{
    public sealed partial class App
    {
        private WinRTContainer _container;
        private IEventAggregator _eventAggregator;

        public App()
        {
            InitializeComponent();
        }

        protected override void Configure()
        {
            _container = new WinRTContainer();
            _container.RegisterWinRTServices();

            _container.Singleton<MapViewModel>();

            _eventAggregator = _container.GetInstance<IEventAggregator>();

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
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }
    }
}
