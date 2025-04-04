using CommunityToolkit.Mvvm.DependencyInjection;
using Syncfusion.Maui.Charts;
using Syncfusion.Maui.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace MarmotAp.Pages;


public partial class EditPage : ContentPage
{
    private int iStep = 1;
    private bool bToggleUnlock = true;
    private bool bToggleLockup = true;
    private bool bToggleODoff = true;
    private bool bToggleODon = true;
    private bool bToggleShift12 = true;
    private bool bToggleShift23 = true;
    private bool bToggleShift21 = true;
    private bool bToggleShift32 = true;
    string currentTune = "1";
    string currentFile = "";
    

    Tune tune1 = new Tune();
    Tune tune2 = new Tune();
    Tune tune3 = new Tune();
    public EditPage(EditPageViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;

        currentTune = "1";
        tune1.SetDefaults();
        tune2.SetDefaults();
        tune3.SetDefaults();

        //bool bFoundDefault = false;
        //List<string> list = ListFiles();
        //for (int i = 0; i < list.Count; i++)
        //{
        //    //if (list[i].Contains(".xml"))
        //    //    System.IO.File.Delete(list[i]);

        //    if (!list[i].Contains("default"))
        //    {
        //        continue;
        //    }
        //    else
        //    {
        //        bFoundDefault = true;
        //        //string sFile = list[i];
        //        //System.IO.File.Delete("*.xml");
        //        System.Diagnostics.Debug.WriteLine("Found - default.xml");

        //        break;
        //    }
        //}
        //if (!bFoundDefault)
        //{
        //    System.Diagnostics.Debug.WriteLine("Creating default.xml");

        //    SaveDefaultTunes();
        //}
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
        AddVal.Items.Clear();
        for (int i = -10; i <= 10; i++)
        {
            if (i != 0)
            {
                AddVal.Items.Add(i.ToString());
            }
        }
        AddVal.SelectedIndex = 14;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
    }

    private void Unlock_Clicked(object sender, EventArgs e)
    {
        SetState(Unlock, btnUnlock);

        bToggleUnlock = !bToggleUnlock;

        if (bToggleUnlock == true)
        {
            SetState(null, btnUnlock);
        }
    }

    private void Lockup_Clicked(object sender, EventArgs e)
    {
        SetState(Lockup, btnLockup);

        bToggleLockup = !bToggleLockup;

        if (bToggleLockup == true)
        {
            SetState(null, btnLockup);
        }
    }

    private void ODoff_Clicked(object sender, EventArgs e)
    {
        SetState(ODoff, btnODoff);

        bToggleODoff = !bToggleODoff;

        if (bToggleODoff == true)
        {
            SetState(null, btnODoff);
        }

    }

    private void ODon_Clicked(object sender, EventArgs e)
    {
        SetState(ODon, btnODon);

        bToggleODon = !bToggleODon;

        if (bToggleODon == true)
        {
            SetState(null, btnODon);
        }

    }

    private void Shift_12_Clicked(object sender, EventArgs e)
    {
        SetState(Shift_12, btnShift_12);

        bToggleShift12 = !bToggleShift12;

        if (bToggleShift12 == true)
        {
            SetState(null, btnShift_12);
        }

    }

    private void Shift_23_Clicked(object sender, EventArgs e)
    {
        SetState(Shift_23, btnShift_23);

        bToggleShift23 = !bToggleShift23;

        if (bToggleShift23 == true)
        {
            SetState(null, btnShift_23);
        }

    }

    private void Shift_21_Clicked(object sender, EventArgs e)
    {
        SetState(Shift_21, btnShift_21);

        bToggleShift21 = !bToggleShift21;

        if (bToggleShift21 == true)
        {
            SetState(null, btnShift_21);
        }

    }

    private void Shift_32_Clicked(object sender, EventArgs e)
    {
        SetState(Shift_32, btnShift_32);

        bToggleShift32 = !bToggleShift32;

        if (bToggleShift32 == true)
        {
            SetState(null, btnShift_32);
        }

    }

    private void AddVal_SelectedIndexChanged(object sender, EventArgs e)
    {
        Picker p = (Picker)sender;
        var sel = p.SelectedItem;
        iStep = Int16.Parse(sel.ToString());
        if (iStep < -10)
        {
            iStep = 0;
        }
        if (iStep > 10)
        {
            iStep = 10;
        }
    }

    private void Exit_Clicked(object sender, EventArgs e)
    {
        //Shell.Current.GoToAsync("//HomePage", false);
        
    }

    private string TuneSeriesToString(Tune tune, string seriesName)
    {
        string series = "";
        Models.Point[] mp;
        try
        {
            switch (seriesName)
            {
                case "Unlock":
                    mp = tune.Unlock.ToArray();
                    foreach (var p in mp)
                    {
                        series += p.y.ToString();
                        series += ",";
                    }
                    series.TrimEnd(',');
                    break;
                case "Lockup":
                    mp = tune.Lockup.ToArray();
                    foreach (var p in mp)
                    {
                        series += p.y.ToString();
                        series += ",";
                    }
                    series.TrimEnd(',');
                    break;
                case "ODon":
                    mp = tune.ODon.ToArray();
                    foreach (var p in mp)
                    {
                        series += p.y.ToString();
                        series += ",";
                    }
                    series.TrimEnd(',');
                    break;
                case "ODoff":
                    mp = tune.ODoff.ToArray();
                    foreach (var p in mp)
                    {
                        series += p.y.ToString();
                        series += ",";
                    }
                    series.TrimEnd(',');
                    break;
                case "Shift_12":
                    mp = tune.Shift_12.ToArray();
                    foreach (var p in mp)
                    {
                        series += p.y.ToString();
                        series += ",";
                    }
                    series.TrimEnd(',');
                    break;
                case "Shift_23":
                    mp = tune.Shift_23.ToArray();
                    foreach (var p in mp)
                    {
                        series += p.y.ToString();
                        series += ",";
                    }
                    series.TrimEnd(',');
                    break;
                case "Shift_21":
                    mp = tune.Shift_21.ToArray();
                    foreach (var p in mp)
                    {
                        series += p.y.ToString();
                        series += ",";
                    }
                    series.TrimEnd(',');
                    break;
                case "Shift_32":
                    mp = tune.Shift_32.ToArray();
                    foreach (var p in mp)
                    {
                        series += p.y.ToString();
                        series += ",";
                    }
                    series.TrimEnd(',');
                    break;
            }
            return series.TrimEnd(',');
        }
        catch(Exception ex)
        {
            Shell.Current.DisplayAlert("TuneSeriesToString Error", ex.Message, "Ok");
        }
        return String.Empty;
    }
    private XDocument CreateXmlDocFromTunes()
    {
        try
        {
            XDocument d = new XDocument(
                        new XComment("The Three Tunes."),
                        new XProcessingInstruction("xml-stylesheet", "href='mystyle.css' title='Compact' type='text/css'"),
                        new XElement("Tunes",
                            new XElement("Tune1",
                                new XElement("Unlock", TuneSeriesToString(tune1, "Unlock")),
                                new XElement("Lockup", TuneSeriesToString(tune1, "Lockup")),
                                new XElement("ODon", TuneSeriesToString(tune1, "ODon")),
                                new XElement("ODoff", TuneSeriesToString(tune1, "ODoff")),
                                new XElement("Shift_12", TuneSeriesToString(tune1, "Shift_12")),
                                new XElement("Shift_23", TuneSeriesToString(tune1, "Shift_23")),
                                new XElement("Shift_21", TuneSeriesToString(tune1, "Shift_21")),
                                new XElement("Shift_32", TuneSeriesToString(tune1, "Shift_32"))
                            ),
                            new XElement("Tune2",
                               new XElement("Unlock", TuneSeriesToString(tune2, "Unlock")),
                                new XElement("Lockup", TuneSeriesToString(tune2, "Lockup")),
                                new XElement("ODon", TuneSeriesToString(tune2, "ODon")),
                                new XElement("ODoff", TuneSeriesToString(tune2, "ODoff")),
                                new XElement("Shift_12", TuneSeriesToString(tune2, "Shift_12")),
                                new XElement("Shift_23", TuneSeriesToString(tune2, "Shift_23")),
                                new XElement("Shift_21", TuneSeriesToString(tune2, "Shift_21")),
                                new XElement("Shift_32", TuneSeriesToString(tune2, "Shift_32"))
                            ),
                            new XElement("Tune3",
                                new XElement("Unlock", TuneSeriesToString(tune3, "Unlock")),
                                new XElement("Lockup", TuneSeriesToString(tune3, "Lockup")),
                                new XElement("ODon", TuneSeriesToString(tune3, "ODon")),
                                new XElement("ODoff", TuneSeriesToString(tune3, "ODoff")),
                                new XElement("Shift_12", TuneSeriesToString(tune3, "Shift_12")),
                                new XElement("Shift_23", TuneSeriesToString(tune3, "Shift_23")),
                                new XElement("Shift_21", TuneSeriesToString(tune3, "Shift_21")),
                                new XElement("Shift_32", TuneSeriesToString(tune3, "Shift_32"))
                            ),
                            new XComment("Tune Data."),
                            new XElement("TuneData",
                                new XElement("antiHunt", "0"),
                                new XElement("gearRatio", "3.31"),
                                new XElement("tireSize", "32.5"),
                                new XElement("tpsMin", "0"),
                                new XElement("tpsMax", "2.8"),
                                new XElement("auxMin", "0.5"),
                                new XElement("auxMax", "4.5"),
                                new XElement("dsrevpermile", "2000"),
                                new XElement("buildNum", "1"),
                                new XElement("tempRatio", "0"),
                                new XElement("pressure12", "45"),
                                new XElement("pressure23", "100")
                            )
                        )
            );

            d.Declaration = new XDeclaration("1.0", "utf-8", "true");
            Console.WriteLine(d);
            System.Diagnostics.Debug.WriteLine(d);

            return d;
        }
        catch(Exception ex)
        {
            Shell.Current.DisplayAlert("TuneSeriesToString Error", ex.Message, "Ok");

        }
        return null;
    }

    private void SaveDefaultTunes()
    {
        XDocument d = new XDocument(
                    new XComment("The Three Tunes."),
                    new XProcessingInstruction("xml-stylesheet", "href='mystyle.css' title='Compact' type='text/css'"),
                    new XElement("Tunes",
                        new XElement("Tune1",
                            new XElement("Unlock", "20,20,20,20,20,20,20,20,20,20"),
                            new XElement("Lockup", "30,30,30,30,30,30,30,30,30,30"),
                            new XElement("ODon", "40,40,40,40,40,40,40,40,40,40"),
                            new XElement("ODoff", "50,50,50,50,50,50,50,50,50,50"),
                            new XElement("Shift_12", "60,60,60,60,60,60,60,60,60,60"),
                            new XElement("Shift_23", "70,70,70,70,70,70,70,70,70,70"),
                            new XElement("Shift_21", "80,80,80,80,80,80,80,80,80,80"),
                            new XElement("Shift_32", "90,90,90,90,90,90,90,90,90,90")
                        ),
                        new XElement("Tune2",
                            new XElement("Unlock", "25,25,25,25,25,25,25,25,25,25"),
                            new XElement("Lockup", "35,35,35,35,35,35,35,35,35,35"),
                            new XElement("ODon", "45,45,45,45,45,45,45,45,45,45"),
                            new XElement("ODoff", "55,55,55,55,55,55,55,55,55,55"),
                            new XElement("Shift_12", "65,65,65,65,65,65,65,65,65,65"),
                            new XElement("Shift_23", "75,75,75,75,75,75,75,75,75,75"),
                            new XElement("Shift_21", "85,85,85,85,85,85,85,85,85,85"),
                            new XElement("Shift_32", "95,95,95,95,95,95,95,95,95,95")
                        ),
                        new XElement("Tune3",
                            new XElement("Unlock", "30,30,30,30,30,30,30,30,30,30"),
                            new XElement("Lockup", "35,35,35,35,35,35,35,35,35,35"),
                            new XElement("ODon", "40,40,40,40,40,40,40,40,40,40"),
                            new XElement("ODoff", "45,45,45,45,45,45,45,45,45,45"),
                            new XElement("Shift_12", "50,50,50,50,50,50,50,50,50,50"),
                            new XElement("Shift_23", "55,55,55,55,55,55,55,55,55,55"),
                            new XElement("Shift_21", "60,60,60,60,60,60,60,60,60,60"),
                            new XElement("Shift_32", "65,65,65,65,65,65,65,65,65,65")
                        ),
                        new XComment("Tune Data."),
                        new XElement("TuneData",
                            new XElement("antiHunt","0"),
                            new XElement("gearRatio","3.31"),
                            new XElement("tireSize","32.5"),
                            new XElement("tpsMin","0"),
                            new XElement("tpsMax","2.8"),
                            new XElement("auxMin","0.5"),
                            new XElement("auxMax","4.5"),
                            new XElement("dsrevpermile", "2000"),
                            new XElement("buildNum", "1"),
                            new XElement("tempRatio", "0"),
                            new XElement("pressure12", "45"),
                            new XElement("pressure23", "100")
                        )
                    )
        );

        d.Declaration = new XDeclaration("1.0", "utf-8", "true");
        Console.WriteLine(d);
        System.Diagnostics.Debug.WriteLine(d);

        var path = FileSystem.Current.AppDataDirectory;
        var fullpath = Path.Combine(path, "default.xml");

        d.Save(fullpath);
    }
    private async void SaveFile_Clicked(object sender, EventArgs e)
    {
        string sans = "";

        try
        {
            var ans = await DisplayPromptAsync("Prompt", "Input file name:");
            if (ans == null)
                return;

            if (ans.ToString().ToLower().EndsWith(".xml"))
                ans.ToString().Replace(".xml", "");

            if (ans.ToString().ToLower().EndsWith(".xml") == false)
            {
                sans = ans.ToString();
                sans = sans.Replace(".", "");
                sans += ".xml";
            }
            if (sans.Length < 5)
                return;

            if (sans != null && sans.ToString().ToLower().EndsWith(".xml"))
            {
                //List<string> strings = new List<string>();

                XDocument d = CreateXmlDocFromTunes();
                var path = FileSystem.Current.AppDataDirectory;
                var fullpath = Path.Combine(path, sans);

                System.IO.File.Delete(fullpath);
                d.Save(fullpath);

                await DisplayAlert("Success", "File Saved!", "Ok");
            }
            else
            {
                await DisplayAlert("Error", "File name must end with '.tnz'", "Ok");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("SaveFile_Clicked Error", ex.Message, "Ok");

        }


    }

    private static List<string> ListFiles()
    {
        List<string> lst = new List<string>();
        var path = FileSystem.Current.AppDataDirectory;
        string[] sFiles = System.IO.Directory.GetFiles(path);
        foreach (string sFile in sFiles)
        {
            lst.Add(sFile);
        }
        return lst;
    }

    private void LoadFile_Clicked(object sender, EventArgs e)
    {
        List<string> lst = ListFiles();
        FileList.Items.Clear();
        if (lst.Count <= 1)
        {
            Shell.Current.DisplayAlert("Nothing", "No files", "Ok");
            return;
        }
        foreach (string file in lst)
        {
            
            if (file.Contains(".xml"))
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(file);
                FileList.Items.Add(fi.Name);
            }
        }
        FileList.IsVisible = true;
        FileList.Focus();
    }

    private static async Task<string> ReadFile(string sFile)
    {
        var path = FileSystem.Current.AppDataDirectory;
        var fullpath = Path.Combine(path, sFile);

        try
        {
            using Stream fileStream = System.IO.File.OpenRead(fullpath);
            using StreamReader reader = new StreamReader(fileStream);

            var c = await reader.ReadToEndAsync();

            return c;
        }
        catch (Exception ex)
        {
            // Handle this as you will 
            string msg = ex.Message;
            await Shell.Current.DisplayAlert("Error", msg, "Ok");

        }

        return String.Empty;
    }

    private void UpdateChartFromList(Tune tune,List<TuneSeries> lst)
    {
        TuneSeries[] ts = lst.ToArray();
        
        try
        {
            foreach (TuneSeries t in ts)
            {
                ObservableCollection<Models.Point> oc = new ObservableCollection<Models.Point>();

                int i = 0;
                string[] s = t.Series.Split([',']);
                oc.Clear();
                foreach (string y in s)
                {
                    Models.Point mp = new Models.Point(t.Name, i+=10, Convert.ToInt32(y));
                    oc.Add(mp);
                    System.Diagnostics.Debug.WriteLine(String.Format("{0} - x={1} y={2}",mp.Id,mp.x,mp.y));

                }
                switch (oc[0].Id)
                {
                    case "Unlock":
                        tune.Unlock = oc;
                        break;
                    case "Lockup":
                        tune.Lockup = oc;
                        break;
                    case "ODon":
                        tune.ODon = oc;
                        break;
                    case "ODoff":
                        tune.ODoff = oc;
                        break;
                    case "Shift_12":
                        tune.Shift_12 = oc;
                        break;
                    case "Shift_23":
                        tune.Shift_23 = oc;
                        break;
                    case "Shift_21":
                        tune.Shift_21 = oc;
                        break;
                    case "Shift_32":
                        tune.Shift_32 = oc;
                        break;
                    default:
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("UpdateChartFromList Error", ex.Message, "Ok");
        }
    }
    private async void ParseSelectedFileToTunes(string sFile)
    {
        // TODO: Parse file to the graph
        List<TuneSeries> lstT1 = new List<TuneSeries>();
        List<TuneSeries> lstT2 = new List<TuneSeries>();
        List<TuneSeries> lstT3 = new List<TuneSeries>();
        string xml = await ReadFile(sFile);
        try
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            XmlNode t1 = doc.DocumentElement.SelectSingleNode("/Tunes/Tune1");
            foreach (XmlNode n in t1.ChildNodes)
            {
                string name = n.Name;
                string s = n.InnerText.ToString();
                lstT1.Add(new TuneSeries(name, s));
                System.Diagnostics.Debug.WriteLine(String.Format("{0} - Series= {1}", name, s));


            }
            XmlNode t2 = doc.DocumentElement.SelectSingleNode("/Tunes/Tune2");
            foreach (XmlNode n in t2.ChildNodes)
            {
                string name = n.Name;
                string s = n.InnerText.ToString();
                lstT2.Add(new TuneSeries(name, s));
                System.Diagnostics.Debug.WriteLine(String.Format("{0} - Series= {1}", name, s));

            }
            XmlNode t3 = doc.DocumentElement.SelectSingleNode("/Tunes/Tune3");
            foreach (XmlNode n in t3.ChildNodes)
            {
                string name = n.Name;
                string s = n.InnerText.ToString();
                lstT3.Add(new TuneSeries(name, s));
                System.Diagnostics.Debug.WriteLine(String.Format("{0} - Series= {1}", name, s));

            }
            UpdateChartFromList(tune1,lstT1);
            UpdateChartFromList(tune2,lstT2);
            UpdateChartFromList(tune3,lstT3);

            // Set to tune 1
            currentTune = "1";
            Title = "Tune 1";
            Unlock.ItemsSource = null;
            Unlock.ItemsSource = tune1.Unlock;
            Lockup.ItemsSource = null;
            Lockup.ItemsSource = tune1.Lockup;
            ODon.ItemsSource = null;
            ODon.ItemsSource = tune1.ODon;
            ODoff.ItemsSource = null;
            ODoff.ItemsSource = tune1.ODoff;
            Shift_12.ItemsSource = null;
            Shift_12.ItemsSource = tune1.Shift_12;
            Shift_23.ItemsSource = null;
            Shift_23.ItemsSource = tune1.Shift_23;
            Shift_21.ItemsSource = null;
            Shift_21.ItemsSource = tune1.Shift_21;
            Shift_32.ItemsSource = null;
            Shift_32.ItemsSource = tune1.Shift_32;

        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("ParseSelectedFileToTunes Error", ex.Message, "Ok");

        }
    }
    private async void Filelist_SelectedIndexChanged(object sender, EventArgs e)
    {
        Picker p = (Picker)sender;
        if (p.SelectedItem != null)
        {

            var sel = p.SelectedItem;
            if (!sel.ToString().ToLower().EndsWith(".xml"))
            {
                await DisplayAlert("Filelist_SelectedIndexChanged Error", "Wrong file type!", "Ok");
                return;
            }
            FileList.IsVisible = false;

            ParseSelectedFileToTunes(sel.ToString());

        }
    }

    private void FileList_Unfocused(object sender, FocusEventArgs e)
    {
        FileList.IsVisible = false;
    }

    private void Save_Clicked(object sender, EventArgs e)
    {

    }

    private void SetState(LineSeries ls, Button btn)
    {
        if (ls != null)
        {
            Unlock.IsVisible = false;
            Lockup.IsVisible = false;
            ODoff.IsVisible = false;
            ODon.IsVisible = false;
            Shift_12.IsVisible = false;
            Shift_23.IsVisible = false;
            Shift_21.IsVisible = false;
            Shift_32.IsVisible = false;

            //btnUnlock.BackgroundColor = Color.Teal;
            //btnLockup.BackgroundColor = Color.Teal;
            //btnODoff.BackgroundColor = Color.Teal;
            //btnODon.BackgroundColor = Color.Teal;
            //btnShift_12.BackgroundColor = Color.Teal;
            //btnShift_23.BackgroundColor = Color.Teal;
            //btnShift_21.BackgroundColor = Color.Teal;
            //btnShift_32.BackgroundColor = Color.Teal;
            ls.IsVisible = true;
            //btn.BackgroundColor = Color.RosyBrown;

        }
        else
        {
            // reset all
            Unlock.IsVisible = true;
            Lockup.IsVisible = true;
            ODoff.IsVisible = true;
            ODon.IsVisible = true;
            Shift_12.IsVisible = true;
            Shift_23.IsVisible = true;
            Shift_21.IsVisible = true;
            Shift_32.IsVisible = true;

            //btnUnlock.BackgroundColor = Color.Teal;
            //btnLockup.BackgroundColor = Color.Teal;
            //btnODoff.BackgroundColor = Color.Teal;
            //btnODon.BackgroundColor = Color.Teal;
            //btnShift_12.BackgroundColor = Color.Teal;
            //btnShift_23.BackgroundColor = Color.Teal;
            //btnShift_21.BackgroundColor = Color.Teal;
            //btnShift_32.BackgroundColor = Color.Teal;
        }
    }

    private void Chart_SelectionChanged(object sender, ChartSelectionChangedEventArgs e)
    {
        LineSeries ls = sender as LineSeries;
        List<int> nix= e.NewIndexes;
        ObservableCollection<Models.Point> lp = ls.ItemsSource as ObservableCollection<Models.Point>;
        lp[nix[0]].y += iStep;
        if (lp[nix[0]].y > 120)
            lp[nix[0]].y = 120;
        if (lp[nix[0]].y < 0)
            lp[nix[0]].y = 0;
        ls.ItemsSource = null;
        ls.ItemsSource = lp;
        switch(lp[nix[0]].Id)
        {
            case "Unlock":
                lblUnlock.Text = String.Format("TPS={0},MPH={1}", lp[nix[0]].x, lp[nix[0]].y);
                
                break;
            case "Lockup":
                lblLockup.Text = String.Format("TPS={0},MPH={1}", lp[nix[0]].x, lp[nix[0]].y);
                break;
            case "ODoff":
                lblODoff.Text = String.Format("TPS={0},MPH={1}", lp[nix[0]].x, lp[nix[0]].y);
                break;
            case "ODon":
                lblODon.Text = String.Format("TPS={0},MPH={1}", lp[nix[0]].x, lp[nix[0]].y);
                break;
            case "Shift_12":
                lblShift_12.Text = String.Format("TPS={0},MPH={1}", lp[nix[0]].x, lp[nix[0]].y);
                break;
            case "Shift_23":
                lblShift_23.Text = String.Format("TPS={0},MPH={1}", lp[nix[0]].x, lp[nix[0]].y);
                break;
            case "Shift_21":
                lblShift_21.Text = String.Format("TPS={0},MPH={1}", lp[nix[0]].x, lp[nix[0]].y);
                break;
            case "Shift_32":
                lblShift_32.Text = String.Format("TPS={0},MPH={1}", lp[nix[0]].x, lp[nix[0]].y);
                break;
        }
       
    }

    private void Tune1_Clicked(object sender, EventArgs e)
    {
        if (currentTune != "1")
        {
            Title = "Tune 1";

            SaveCurrentTune();
            currentTune = "1";

            Unlock.ItemsSource = null;
            Unlock.ItemsSource = tune1.Unlock;
            
            Lockup.ItemsSource = null;
            Lockup.ItemsSource = tune1.Lockup;

            ODon.ItemsSource = null;
            ODon.ItemsSource = tune1.ODon;

            ODoff.ItemsSource = null;
            ODoff.ItemsSource = tune1.ODoff;

            Shift_12.ItemsSource = null;
            Shift_12.ItemsSource = tune1.Shift_12;

            Shift_23.ItemsSource = null;
            Shift_23.ItemsSource = tune1.Shift_23;

            Shift_21.ItemsSource = null;
            Shift_21.ItemsSource = tune1.Shift_21;

            Shift_32.ItemsSource = null;
            Shift_32.ItemsSource = tune1.Shift_32;
        }
    }

    private void Tune2_Clicked(object sender, EventArgs e)
    {
        if (currentTune != "2")
        {
            Title = "Tune 2";

            SaveCurrentTune();
            currentTune = "2";

            Unlock.ItemsSource = null;
            Unlock.ItemsSource = tune2.Unlock;

            Lockup.ItemsSource = null;
            Lockup.ItemsSource = tune2.Lockup;

            ODon.ItemsSource = null;
            ODon.ItemsSource = tune2.ODon;

            ODoff.ItemsSource = null;
            ODoff.ItemsSource = tune2.ODoff;

            Shift_12.ItemsSource = null;
            Shift_12.ItemsSource = tune2.Shift_12;

            Shift_23.ItemsSource = null;
            Shift_23.ItemsSource = tune2.Shift_23;

            Shift_21.ItemsSource = null;
            Shift_21.ItemsSource = tune2.Shift_21;

            Shift_32.ItemsSource = null;
            Shift_32.ItemsSource = tune2.Shift_32;
        }
    }
    private void Tune3_Clicked(object sender, EventArgs e)
    {
        if (currentTune != "3")
        {
            Title = "Tune 3";

            SaveCurrentTune();
            currentTune = "3";

            Unlock.ItemsSource = null;
            Unlock.ItemsSource = tune3.Unlock;

            Lockup.ItemsSource = null;
            Lockup.ItemsSource = tune3.Lockup;

            ODon.ItemsSource = null;
            ODon.ItemsSource = tune3.ODon;

            ODoff.ItemsSource = null;
            ODoff.ItemsSource = tune3.ODoff;

            Shift_12.ItemsSource = null;
            Shift_12.ItemsSource = tune3.Shift_12;

            Shift_23.ItemsSource = null;
            Shift_23.ItemsSource = tune3.Shift_23;

            Shift_21.ItemsSource = null;
            Shift_21.ItemsSource = tune3.Shift_21;

            Shift_32.ItemsSource = null;
            Shift_32.ItemsSource = tune3.Shift_32;
        }
    }
    private void SaveCurrentTune()
    {
        System.Diagnostics.Debug.WriteLine("Current Tune is " + currentTune);

        switch (currentTune)
        {
            case "1":
                tune1.Unlock = Unlock.ItemsSource as ObservableCollection<Models.Point>;
                tune1.Lockup = Lockup.ItemsSource as ObservableCollection<Models.Point>;
                tune1.ODoff = ODoff.ItemsSource as ObservableCollection<Models.Point>;
                tune1.ODon = ODon.ItemsSource as ObservableCollection<Models.Point>;
                tune1.Shift_12 = Shift_12.ItemsSource as ObservableCollection<Models.Point>;
                tune1.Shift_23 = Shift_23.ItemsSource as ObservableCollection<Models.Point>;
                tune1.Shift_21 = Shift_21.ItemsSource as ObservableCollection<Models.Point>;
                tune1.Shift_32 = Shift_32.ItemsSource as ObservableCollection<Models.Point>;
                break;
            case "2":
                tune2.Unlock = Unlock.ItemsSource as ObservableCollection<Models.Point>;
                tune2.Lockup = Lockup.ItemsSource as ObservableCollection<Models.Point>;
                tune1.ODoff = ODoff.ItemsSource as ObservableCollection<Models.Point>;
                tune2.ODon = ODon.ItemsSource as ObservableCollection<Models.Point>;
                tune2.Shift_12 = Shift_12.ItemsSource as ObservableCollection<Models.Point>;
                tune2.Shift_23 = Shift_23.ItemsSource as ObservableCollection<Models.Point>;
                tune2.Shift_21 = Shift_21.ItemsSource as ObservableCollection<Models.Point>;
                tune2.Shift_32 = Shift_32.ItemsSource as ObservableCollection<Models.Point>;
                break;
            case "3":
                tune3.Unlock = Unlock.ItemsSource as ObservableCollection<Models.Point>;
                tune3.Lockup = Lockup.ItemsSource as ObservableCollection<Models.Point>;
                tune3.ODoff = ODoff.ItemsSource as ObservableCollection<Models.Point>;
                tune3.ODon = ODon.ItemsSource as ObservableCollection<Models.Point>;
                tune3.Shift_12 = Shift_12.ItemsSource as ObservableCollection<Models.Point>;
                tune3.Shift_23 = Shift_23.ItemsSource as ObservableCollection<Models.Point>;
                tune3.Shift_21 = Shift_21.ItemsSource as ObservableCollection<Models.Point>;
                tune3.Shift_32 = Shift_32.ItemsSource as ObservableCollection<Models.Point>;
                break;

        }
    }

    
}