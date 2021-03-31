namespace DailyGoal
{
    public static class Data
    {


        private static DailyGoalSettings _settings;

        public static DailyGoalSettings Settings
        {
            get
            {
                if (_settings == null)
                {
                    _settings = new DailyGoalSettings()
                    {
                        hours = 0,
                        minutes = 0
                    };

                }
                return _settings;
            }
            internal set
            {
                _settings = value;


            }
        }
    }
}
