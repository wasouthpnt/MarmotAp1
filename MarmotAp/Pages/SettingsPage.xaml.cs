
namespace MarmotAp.Pages;

public partial class SettingsPage : ContentPage
{
	public SettingsPage(SettingsPageViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        int imph = Convert.ToInt32(Preferences.Get("MPH", "0"));
        if (imph > 20)
        {
            DisplayAlert("OPERATION ERROR", "Please do not operate while driving.", "OK");
            //return;
        }

        //name.Text = Preferences.Get("@string/DevKey", "");
        //currentname.Text = name.Text;
        //loaded = true;
    }


    protected override void OnDisappearing()
    {
        base.OnDisappearing();
    }
}