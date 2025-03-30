

using Syncfusion.Maui.Charts;
using System.Collections.Immutable;
using System.Collections.Specialized;


namespace MarmotAp.ViewModels;

public partial class EditPageViewModel : BaseViewModel
{
    public ObservableCollection<Models.Point> Unlock { get; set; }
    public ObservableCollection<Models.Point> Lockup { get; set; }
    public ObservableCollection<Models.Point> ODon { get; set; }
    public ObservableCollection<Models.Point> ODoff { get; set; }
    public ObservableCollection<Models.Point> Shift_12 { get; set; }
    public ObservableCollection<Models.Point> Shift_23 { get; set; }
    public ObservableCollection<Models.Point> Shift_21 { get; set; }
    public ObservableCollection<Models.Point> Shift_32 { get; set; }

    public IRelayCommand Tune1AsyncCommand { get; }
    public IRelayCommand Tune2AsyncCommand { get; }
    public IRelayCommand Tune3AsyncCommand { get; }

    public Tune tune1 { get; set; }
    public Tune tune2 { get; set; }
    public Tune tune3 { get; set; }

    public EditPageViewModel()
    {
        Unlock = new ObservableCollection<Models.Point>();
        Lockup = new ObservableCollection<Models.Point>();
        ODon = new ObservableCollection<Models.Point>();
        ODoff = new ObservableCollection<Models.Point>();
        Shift_12 = new ObservableCollection<Models.Point>();
        Shift_23 = new ObservableCollection<Models.Point>();
        Shift_21 = new ObservableCollection<Models.Point>();
        Shift_32 = new ObservableCollection<Models.Point>();


        // Load defaults
        AddValues(Unlock, "Unlock", new byte[10] { 20, 20, 20, 20, 20, 32, 32, 32, 32, 32 });
        AddValues(Lockup, "Lockup", new byte[10] { 40, 40, 40, 40, 40, 40, 40, 40, 40, 40 });
        AddValues(ODon, "ODon", new byte[10] { 55, 55, 55, 55, 55, 55, 55, 55, 55, 55 });
        AddValues(ODoff, "ODoff", new byte[10] { 45, 45, 45, 45, 45, 45, 45, 45, 45, 45 });
        AddValues(Shift_12, "Shift_12", new byte[10] { 15, 15, 15, 15, 15, 15, 15, 15, 15, 15 });
        AddValues(Shift_23, "Shift_23", new byte[10] { 33, 33, 33, 33, 33, 33, 33, 33, 33, 33 });
        AddValues(Shift_21, "Shift_21", new byte[10] { 66, 66, 66, 66, 66, 66, 66, 66, 66, 66 });
        AddValues(Shift_32, "Shift_32", new byte[10] { 88, 88, 88, 88, 88, 88, 88, 88, 88, 88 });

        //UnlockAsyncCommand = new AsyncRelayCommand(UnlockAsync);

        //Tune1AsyncCommand = new AsyncRelayCommand(Tune1Async);
        //Tune2AsyncCommand = new AsyncRelayCommand(Tune2Async);
        //Tune3AsyncCommand = new AsyncRelayCommand(Tune3Async);

        tune1 = new Tune(); tune1.SetDefaults();
        tune2 = new Tune(); tune2.SetDefaults();
        tune3 = new Tune(); tune3.SetDefaults();


        //Unlock = tune1.Unlock;
        //Lockup = tune1.Lockup;
        //ODon = tune1.ODon;
        //ODoff = tune1.ODoff;
        //Shift_12 = tune1.Shift_12;
        //Shift_23 = tune1.Shift_23;
        //Shift_21 = tune1.Shift_21;
        //Shift_32 = tune1.Shift_32;

        //ChartTitle = "Tune 1";

        //Tune1Async();
        
    }
    

    public async Task Tune1Async()
    {
        //ChartTitle = "Tune 1";
        //Unlock = tune1.Unlock;
        //Lockup = tune1.Lockup;
        //ODon = tune1.ODon;
        //ODoff = tune1.ODoff;
        //Shift_12 = tune1.Shift_12;
        //Shift_23 = tune1.Shift_23;
        //Shift_21 = tune1.Shift_21;
        //Shift_32 = tune1.Shift_32;

    }
    //private async Task Tune2Async()
    //{
    //    ChartTitle = "Tune 2";
    //    Unlock = tune2.Unlock;
    //    Lockup = tune2.Lockup;
    //    ODon = tune2.ODon;
    //    ODoff = tune2.ODoff;
    //    Shift_12 = tune2.Shift_12;
    //    Shift_23 = tune2.Shift_23;
    //    Shift_21 = tune2.Shift_21;
    //    Shift_32 = tune2.Shift_32;
    //}
    //private async Task Tune3Async()
    //{
    //    this.ChartTitle = "Tune 3";
    //    Unlock = tune3.Unlock;
    //    Lockup = tune3.Lockup;
    //    ODon = tune3.ODon;
    //    ODoff = tune3.ODoff;
    //    Shift_12 = tune3.Shift_12;
    //    Shift_23 = tune3.Shift_23;
    //    Shift_21 = tune3.Shift_21;
    //    Shift_32 = tune3.Shift_32;
    //}

    //private async Task UnlockAsync()
    //{
    //    //LineSeries ls = Unlock1;
    //    //ls.IsVisible = true;
    //    foreach (var item in Unlock)
    //    {
    //        item.y = 90;
    //    }

    //}
    private void AddValues(ObservableCollection<Models.Point> lp, String sName, byte[] b)
    {
        lp.Add(new Models.Point(sName, 10, b[0]));
        lp.Add(new Models.Point(sName, 20, b[1]));
        lp.Add(new Models.Point(sName, 30, b[2]));
        lp.Add(new Models.Point(sName, 40, b[3]));
        lp.Add(new Models.Point(sName, 50, b[4]));
        lp.Add(new Models.Point(sName, 60, b[5]));
        lp.Add(new Models.Point(sName, 70, b[6]));
        lp.Add(new Models.Point(sName, 80, b[7]));
        lp.Add(new Models.Point(sName, 90, b[8]));
        lp.Add(new Models.Point(sName, 100, b[9]));
    }

    [ObservableProperty]
    private string chartTitle;

}
