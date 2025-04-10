using System.Text;

namespace MarmotAp.Pages;

public partial class AboutPage : ContentPage
{
    public AboutPage(AboutPageViewModel viewModel)
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
        AboutPageViewModel viewModel = (AboutPageViewModel)BindingContext;
        viewModel.ReadESP32Ver();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
    }
}