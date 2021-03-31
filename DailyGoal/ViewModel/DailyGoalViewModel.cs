using System;
using System.Windows.Input;
using View;

class DailyGoalViewModel : CoreModule
{

    private string _dailyGoalText;
    public string DailyGoalText
    {
        get
        {

            return string.IsNullOrEmpty(_dailyGoalText) ? "Daily goal" : _dailyGoalText;
        }
        set
        {

            _dailyGoalText = value;
            OnPropertyChanged(nameof(DailyGoalText));
        }
    }


    IModuleController _host;
    DailyGoal.DailyGoal dailyGoal;

    public DailyGoalViewModel(IModuleController _host, DailyGoal.DailyGoal dailyGoal)
    {
        this._host = _host;
        this.dailyGoal = dailyGoal;
    }

    float topPercentFilled = 0;
    public int timeSecToFillTopBar = 0;

    public void SetDailyGoal(TimeSpan time)
    {


        DailyGoalText = "Daily goal: " + time.ToString();


        timeSecToFillTopBar = (int)time.TotalSeconds;


        //MainBarModule.timeSecToFillTopBar = (int)time.TotalSeconds;
    }


    private ICommand _dailyGoalClicked;
    public ICommand DailyGoalClicked
    {
        get
        {
            if (_dailyGoalClicked == null)
                _dailyGoalClicked = new RelayCommand(
                   (object o) =>
                   {

                       DailyGoalSetterView dialog = new DailyGoalSetterView();
                       dialog.DataContext = new DailyGoalSetterViewModel(dailyGoal);
                       dialog.ShowDialog();
                       DailyGoalSetterViewModel.GetDailyGoalTimespan(out TimeSpan result);
                       SetDailyGoal(result);

                   },
                   (object o) =>
                   {
                       return true;
                   });

            return _dailyGoalClicked;

        }
    }

    public void LoadDailyGoal()
    {

        if (DailyGoalSetterViewModel.GetDailyGoalTimespan(out TimeSpan result))
            SetDailyGoal(result);
        else
            DailyGoalText = "Set daily goal! ";

    }

    internal void UpdateMainBar(TimeSpan activeTime)
    {
        if (timeSecToFillTopBar == 0)
            return;


        var barValue = ToProcentage(activeTime.TotalSeconds, 0, timeSecToFillTopBar);
        if (barValue > 100)
        {
            barValue -= ((Math.Floor(barValue / 100)) * 100);
        }


        _host.SendMessage("MainBar", "value|||" + barValue);

    }


    public static float ToProcentage(float value, float min, float max)
    {
        var range = max - min;
        var correctedStartVal = value - min;

        return (correctedStartVal * 100) / range;
    }

    internal static double ToProcentage(double value, double minimum, double maximum)
    {
        var range = maximum - minimum;
        var correctedStartVal = value - minimum;

        return (correctedStartVal * 100) / range;
    }

    internal static double ProcentToValue(double procent, double max)
    {
        return (max * procent / 100);
    }


}
