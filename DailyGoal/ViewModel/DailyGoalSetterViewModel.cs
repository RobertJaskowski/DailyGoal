using DailyGoal;
using System;
using System.Windows;
using System.Windows.Input;


class DailyGoalSetterViewModel : BaseViewModel
{
    public TimeSpan returnTime;

    #region Properties
    private bool buttonAcceptVisibility;
    public bool ButtonAcceptVisibility
    {
        get
        {
            return buttonAcceptVisibility;
        }
        set
        {
            buttonAcceptVisibility = value;
            OnPropertyChanged(nameof(ButtonAcceptVisibility));
        }
    }

    private string textFeedback;
    public string TextFeedback
    {
        get
        {
            return textFeedback != null ? textFeedback : string.Format("Input daily goal time \n (HH:MM ex.8:00)");

        }
        set
        {
            textFeedback = value;
            OnPropertyChanged(nameof(TextFeedback));
        }
    }

    private string textBoxInput;
    public string TextBoxInput
    {
        get
        {
            return textBoxInput;
        }
        set
        {
            textBoxInput = value;
            ValidateInput();
            OnPropertyChanged(nameof(TextBoxInput));
        }
    }

    #endregion


    #region Command
    private ICommand _acceptTimeButton;
    public ICommand AcceptTimeButton
    {
        get
        {
            if (_acceptTimeButton == null)
                _acceptTimeButton = new RelayCommand(
                   (object o) =>
                   {
                       if (o != null)
                           if (o is Window)
                           {

                               var d = o as Window;

                               SaveDailyGoalTimespan(returnTime);
                               d.Close();
                           }
                   },
                   (object o) =>
                   {
                       return ValidateInput();

                   });

            return _acceptTimeButton;

        }
    }


    #endregion


    static DailyGoal.DailyGoal _core;
    public DailyGoalSetterViewModel(DailyGoal.DailyGoal core)
    {
        _core = core;
    }

    public static void SaveDailyGoalTimespan(TimeSpan timeSpan)
    {

        Data.Settings.hours = timeSpan.Hours;
        Data.Settings.minutes = timeSpan.Minutes;


        _core.SaveSettings();


    }

    public static bool GetDailyGoalTimespan(out TimeSpan result)
    {


        int hours = Data.Settings.hours;
        int minutes = Data.Settings.minutes;

        if (hours + minutes > 0)
        {

            result = TimeSpan.FromHours(hours) + TimeSpan.FromMinutes(minutes);

            return true;
        }


        result = TimeSpan.FromMilliseconds(0);
        return false;
    }



    private bool ValidateInput()
    {
        ButtonAcceptVisibility = false;

        var text = TextBoxInput;



        bool success = TimeSpan.TryParse(text, out TimeSpan t);

        if (!success)
        {
            DisplayErrorMessage();
            return false;
        }

        if (t.Days > 0)
        {
            TextFeedback = string.Format(t.Days + " " + t.TotalHours + " " + t.TotalMinutes + " \n You are setting your daily goal for more than a day!");
            return false;
        }

        if (t.TotalMinutes < 1)
        {
            TextFeedback = string.Format(t.ToString() + "\n Your daily goal should be at least a minute!");
            return false;
        }




        TextFeedback = string.Format("You are setting your daily goal to \n" + t.ToString() + "\nConfirm your input");
        returnTime = t;

        ButtonAcceptVisibility = true;

        return true;
    }



    private void DisplayErrorMessage()
    {
        TextFeedback = "Input time incorrect format ex. \"1:30\" as of 1hour and 30minutes";
    }


}
