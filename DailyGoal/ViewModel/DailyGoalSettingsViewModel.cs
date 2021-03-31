using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyGoal.ViewModel
{
    class DailyGoalSettingsViewModel
    {
        private IModuleController host;
        private DailyGoal dailyGoal;

        public DailyGoalSettingsViewModel(IModuleController host, DailyGoal dailyGoal)
        {
            this.host = host;
            this.dailyGoal = dailyGoal;
        }
    }
}
