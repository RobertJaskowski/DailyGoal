using DailyGoal.View;
using DailyGoal.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using View;

namespace DailyGoal
{
    public class DailyGoal : ICoreModule
    {
        public string ModuleName => "DailyGoal";

        private UserControl _view;
        public UserControl View
        {
            get
            {
                if (_view == null)
                {
                    _view = new DailyGoalView();
                    _view.DataContext = new DailyGoalViewModel(_host, this);
                }

                return _view;
            }
            set { _view = value; }
        }



        private UserControl _settingsView;
        public UserControl SettingsView
        {
            get
            {
                if (_settingsView == null)
                {
                    _settingsView = new DailyGoalSettingsView();
                    _settingsView.DataContext = new DailyGoalSettingsViewModel(_host, this);
                }

                return _settingsView;
            }
            set { _settingsView = value; }
        }



        public static Action OnMinViewEnteredEvent;
        public static Action OnFullViewEnteredEvent;



        private IModuleController _host;
        public void Init(IModuleController host)
        {
            _host = host;
            Data.Settings = _host.LoadModuleInformation<DailyGoalSettings>(GetModuleName(), "DailyGoalSettings");

        }

        public void SaveSettings()
        {
            _host.SaveModuleInformation(GetModuleName(), "DailyGoalSettings", Data.Settings);
        }
        public string GetModuleName()
        {
            return ModuleName;
        }

        public ModulePosition GetModulePosition()
        {
            return ModulePosition.MID;
        }

        public UserControl GetModuleUserControlView()
        {
            return View;
        }

        public UserControl GetSettingsUserControlView()
        {
            return SettingsView;
        }



        public void OnFullViewEntered()
        {

        }

        public void OnInteractableEntered()
        {

        }

        public void OnInteractableExited()
        {

        }

        public void OnMinViewEntered()
        {

        }

        public void ReceiveMessage(string message)
        {

        }

        public void Start()
        {
            ((DailyGoalViewModel)View.DataContext).LoadDailyGoal();
            _host.SubscribeToEvent("ActiveTimer", "TimeUpdate", OnMainBarTimeUpdated);
            _host.SendMessage("MainBar", "value|||" + 0);

        }

        private void OnMainBarTimeUpdated(object obj)
        {
            if (obj is TimeSpan)
            {
                TimeSpan o = (TimeSpan)obj;
                ((DailyGoalViewModel)View.DataContext).UpdateMainBar(o);

            }
        }

        public void Stop()
        {
            _host.UnsubscribeToEvent("ActiveTimer", "TimeUpdate", OnMainBarTimeUpdated);

        }
    }
}
