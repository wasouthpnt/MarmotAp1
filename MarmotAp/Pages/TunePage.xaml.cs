using Syncfusion.Maui.Charts;
using System.Xml;
using System.Xml.Linq;

namespace MarmotAp.Pages;

public partial class TunePage : ContentPage
{
    private double dDiffRatio = 3.31F;
    private double dTireSize = 32.5F;
    private readonly double[] tireSizes = { 24, 25, 26.5, 27, 27.5, 28, 28.5, 29, 29.5, 30, 30.5, 31, 31.5, 32, 32.5, 33, 33.5, 34, 34.5, 35, 36, 37, 38, 39, 40 };
    private readonly double[] diffRatios = { 2.90, 3.11, 3.20, 3.31, 3.42, 3.50, 3.55, 3.60, 3.73, 3.90, 4.10, 4.33, 4.54, 4.86, 5.00 };

    private int iStep = 1;
    private bool bToggleUnlock = true;
    private bool bToggleLockup = true;
    private bool bToggleODoff = true;
    private bool bToggleODon = true;
    private bool bToggleShift12 = true;
    private bool bToggleShift23 = true;
    private bool bToggleShift21 = true;
    private bool bToggleShift32 = true;
    Tune tune1 = new Tune();
    Tune tune2 = new Tune();
    Tune tune3 = new Tune();
    TuneData tuneData = new TuneData();
    int icurrentTune = 0;
    string sCurrentFile = "";

    public TunePage()
    {
        InitializeComponent();
        InitTunes();
        InitTuneData();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (AddVal.Items.Count > 0)
        {
            AddVal.Items.Clear();
        }
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
    private void InitTunes()
    {

        //string last_file = SecureStorage.Default.GetAsync("last_File");
        string last_file = Preferences.Get("Last_File", "default.xml");

        if (last_file != null)
        {
            if (last_file == "default.xml")
                SaveDefaultTunes();
            sCurrentFile = last_file;
            ParseSelectedFileToTunes(last_file);
        }
    }
    private void InitTuneData()
    {
        for (int i = 0; i < tireSizes.Length; i++)
            TireSize.Items.Add(tireSizes[i].ToString());
        TireSize.SelectedIndex = 0;

        AntiHunt.Text = tuneData.antiHunt.ToString();
        Pressure12.Text = tuneData.Pressure12.ToString();
        TpsMin.Text = tuneData.tpsMin.ToString();
        TpsMax.Text = tuneData.tpsMax.ToString();
        AuxMin.Text = tuneData.auxMin.ToString();
        AuxMax.Text = tuneData.auxMax.ToString();

        for (int i = 0; i < diffRatios.Length; i++)
            DiffRatio.Items.Add(String.Format("{0:F2}", diffRatios[i]));
        DiffRatio.SelectedIndex = 0;
    }
    private void Tune1_Clicked(object sender, EventArgs e)
    {
        if (icurrentTune != 1)
        {
            PageTitle.Text = "Tune 1 " + sCurrentFile;

            SaveCurrentTune();
            icurrentTune = 1;

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
        if (icurrentTune != 2)
        {
            PageTitle.Text = "Tune 2 " + sCurrentFile;

            SaveCurrentTune();
            icurrentTune = 2;

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
        if (icurrentTune != 3)
        {
            PageTitle.Text = "Tune 3 " + sCurrentFile;

            SaveCurrentTune();
            icurrentTune = 3;

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

    private void TuneData_Clicked(object sender, EventArgs e)
    {
        PageTitle.Text = "Tune Data " + sCurrentFile;
        if (TuneDataStack.IsVisible == true)
        { return; }

        MainStack.IsVisible = false;
        TuneDataStack.IsVisible = true;
        icurrentTune = 0;

    }

    private void SaveDefaultTunes()
    {
        VersionAndBuild v = new VersionAndBuild();

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
                            new XElement("antiHunt", "0"),
                            new XElement("gearRatio", "3.31"),
                            new XElement("tireSize", "32.5"),
                            new XElement("tpsMin", "0"),
                            new XElement("tpsMax", "2.8"),
                            new XElement("auxMin", "0.5"),
                            new XElement("auxMax", "4.5"),
                            new XElement("dsrevpermile", "2000"),
                            new XElement("buildNum", Int16.Parse(v.GetBuildNumber())),
                            new XElement("tempRatio", "0"),
                            new XElement("pressure12", "45"),
                            new XElement("pressure23", "100")
                        )
                    )
        );

        d.Declaration = new XDeclaration("1.0", "utf-8", "true");
        //Console.WriteLine(d);
        //System.Diagnostics.Debug.WriteLine(d);

        var path = FileSystem.Current.AppDataDirectory;
        var fullpath = Path.Combine(path, "default.xml");

        d.Save(fullpath);
    }
    private void SaveCurrentTune()
    {
        System.Diagnostics.Debug.WriteLine("Current Tune is " + icurrentTune.ToString());

        switch (icurrentTune)
        {
            case 1:
                tune1.Unlock = Unlock.ItemsSource as ObservableCollection<Models.Point>;
                tune1.Lockup = Lockup.ItemsSource as ObservableCollection<Models.Point>;
                tune1.ODoff = ODoff.ItemsSource as ObservableCollection<Models.Point>;
                tune1.ODon = ODon.ItemsSource as ObservableCollection<Models.Point>;
                tune1.Shift_12 = Shift_12.ItemsSource as ObservableCollection<Models.Point>;
                tune1.Shift_23 = Shift_23.ItemsSource as ObservableCollection<Models.Point>;
                tune1.Shift_21 = Shift_21.ItemsSource as ObservableCollection<Models.Point>;
                tune1.Shift_32 = Shift_32.ItemsSource as ObservableCollection<Models.Point>;
                break;
            case 2:
                tune2.Unlock = Unlock.ItemsSource as ObservableCollection<Models.Point>;
                tune2.Lockup = Lockup.ItemsSource as ObservableCollection<Models.Point>;
                tune1.ODoff = ODoff.ItemsSource as ObservableCollection<Models.Point>;
                tune2.ODon = ODon.ItemsSource as ObservableCollection<Models.Point>;
                tune2.Shift_12 = Shift_12.ItemsSource as ObservableCollection<Models.Point>;
                tune2.Shift_23 = Shift_23.ItemsSource as ObservableCollection<Models.Point>;
                tune2.Shift_21 = Shift_21.ItemsSource as ObservableCollection<Models.Point>;
                tune2.Shift_32 = Shift_32.ItemsSource as ObservableCollection<Models.Point>;
                break;
            case 3:
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
    private void Chart_SelectionChanged(object sender, Syncfusion.Maui.Charts.ChartSelectionChangedEventArgs e)
    {
        LineSeries ls = sender as LineSeries;
        List<int> nix = e.NewIndexes;
        ObservableCollection<Models.Point> lp = ls.ItemsSource as ObservableCollection<Models.Point>;
        lp[nix[0]].y += iStep;
        if (lp[nix[0]].y > 120)
            lp[nix[0]].y = 120;
        if (lp[nix[0]].y < 0)
            lp[nix[0]].y = 0;
        ls.ItemsSource = null;
        ls.ItemsSource = lp;
        switch (lp[nix[0]].Id)
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
                    break;
                case "Lockup":
                    mp = tune.Lockup.ToArray();
                    foreach (var p in mp)
                    {
                        series += p.y.ToString();
                        series += ",";
                    }
                    break;
                case "ODon":
                    mp = tune.ODon.ToArray();
                    foreach (var p in mp)
                    {
                        series += p.y.ToString();
                        series += ",";
                    }
                    break;
                case "ODoff":
                    mp = tune.ODoff.ToArray();
                    foreach (var p in mp)
                    {
                        series += p.y.ToString();
                        series += ",";
                    }
                    break;
                case "Shift_12":
                    mp = tune.Shift_12.ToArray();
                    foreach (var p in mp)
                    {
                        series += p.y.ToString();
                        series += ",";
                    }
                    break;
                case "Shift_23":
                    mp = tune.Shift_23.ToArray();
                    foreach (var p in mp)
                    {
                        series += p.y.ToString();
                        series += ",";
                    }
                    break;
                case "Shift_21":
                    mp = tune.Shift_21.ToArray();
                    foreach (var p in mp)
                    {
                        series += p.y.ToString();
                        series += ",";
                    }
                    break;
                case "Shift_32":
                    mp = tune.Shift_32.ToArray();
                    foreach (var p in mp)
                    {
                        series += p.y.ToString();
                        series += ",";
                    }
                    break;
            }
            return series.TrimEnd(',');
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("TuneSeriesToString Error", ex.Message, "Ok");
        }
        return String.Empty;
    }
    private XDocument CreateXmlDocFromTunes()
    {
        try
        {
            VersionAndBuild v = new VersionAndBuild();
            double diff = double.Parse(DiffRatio.SelectedItem.ToString());
            double tire = double.Parse((string)TireSize.SelectedItem.ToString());
            int rpm = Convert.ToUInt16((5280 / (tire * Math.PI / 12)) * diff);
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
                                new XElement("antiHunt", AntiHunt.Text),
                                new XElement("gearRatio", diff.ToString()),
                                new XElement("tireSize", tire.ToString()),
                                new XElement("tpsMin", TpsMin.Text),
                                new XElement("tpsMax", TpsMax.Text),
                                new XElement("auxMin", AuxMin.Text),
                                new XElement("auxMax", AuxMax.Text),
                                new XElement("dsrevpermile", rpm.ToString()), //Convert.ToUInt16((5280 / (tireSize * Math.PI / 12)) * gearRatio);
                                new XElement("buildNum", Int16.Parse(v.GetBuildNumber())),
                                new XElement("tempRatio", "0"),
                                new XElement("pressure12", Pressure12.Text),
                                new XElement("pressure23", "100")
                            )
                        )
            );

            d.Declaration = new XDeclaration("1.0", "utf-8", "true");
            //Console.WriteLine(d);
            //System.Diagnostics.Debug.WriteLine(d);

            return d;
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("TuneSeriesToString Error", ex.Message, "Ok");

        }
        return null;
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
            await Shell.Current.DisplayAlert("ReadFile Error", msg, "Ok");

        }

        return String.Empty;
    }

    private void UpdateChartFromList(Tune tune, List<TuneSeries> lst)
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
                    Models.Point mp = new Models.Point(t.Name, i += 10, Convert.ToInt32(y));
                    oc.Add(mp);
                    //System.Diagnostics.Debug.WriteLine(String.Format("{0} - x={1} y={2}", mp.Id, mp.x, mp.y));

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
        MyActivity.IsVisible = true;

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
                //System.Diagnostics.Debug.WriteLine(String.Format("{0} - Series= {1}", name, s));
            }
            XmlNode t2 = doc.DocumentElement.SelectSingleNode("/Tunes/Tune2");
            foreach (XmlNode n in t2.ChildNodes)
            {
                string name = n.Name;
                string s = n.InnerText.ToString();
                lstT2.Add(new TuneSeries(name, s));
                //System.Diagnostics.Debug.WriteLine(String.Format("{0} - Series= {1}", name, s));

            }
            XmlNode t3 = doc.DocumentElement.SelectSingleNode("/Tunes/Tune3");
            foreach (XmlNode n in t3.ChildNodes)
            {
                string name = n.Name;
                string s = n.InnerText.ToString();
                lstT3.Add(new TuneSeries(name, s));
                //System.Diagnostics.Debug.WriteLine(String.Format("{0} - Series= {1}", name, s));

            }

            XmlNode td = doc.DocumentElement.SelectSingleNode("/Tunes/TuneData");
            
            XmlNode antiHunt = td.SelectSingleNode("antiHunt");
            AntiHunt.Text = antiHunt.InnerText;
            
            XmlNode tpsMin = td.SelectSingleNode("tpsMin");
            TpsMin.Text = tpsMin.InnerText;
            
            XmlNode tpsMax = td.SelectSingleNode("tpsMax");
            TpsMax.Text = tpsMax.InnerText;
            
            XmlNode auxMin = td.SelectSingleNode("auxMin");
            AuxMin.Text = auxMin.InnerText;
            
            XmlNode auxMax = td.SelectSingleNode("auxMax");
            AuxMax.Text = auxMax.InnerText;

            XmlNode pressure12 = td.SelectSingleNode("pressure12");
            Pressure12.Text = pressure12.InnerText;

            XmlNode gear = td.SelectSingleNode("gearRatio");
            DiffRatio.SelectedItem = gear.InnerText;

            XmlNode tire = td.SelectSingleNode("tireSize");
            TireSize.SelectedItem = tire.InnerText;

            XmlNode buildNum = td.SelectSingleNode("buildNum");
            BuildNum.Text = buildNum.InnerText;

            UpdateChartFromList(tune1, lstT1);
            UpdateChartFromList(tune2, lstT2);
            UpdateChartFromList(tune3, lstT3);

            // Set to tune 1
            icurrentTune = 1;
            PageTitle.Text = "Tune 1 " + sFile;

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
        finally
        {
            MyActivity.IsVisible = false;
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
            sCurrentFile = sel.ToString();

            Preferences.Set("Last_File", sel.ToString());

        }
        FileList.IsVisible = false;
    }

    private void FileList_Unfocused(object sender, FocusEventArgs e)
    {
        FileList.IsVisible = false;
    }

    private void Exit_Clicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//StatusPage");
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
        try
        {
            Picker p = (Picker)sender;
            var sel = p.SelectedItem;
            if (sel == null)
                return;
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
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("AddVal_SelectedIndexChanged Error", ex.Message, "Ok");
        }
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

        }
    }

    private void OnSwipeLeft(object sender, EventArgs e)
    {
        // Actually we are moving Left
        if (TuneDataStack.IsVisible == true)
        {
            TuneDataStack.IsVisible = false;
            icurrentTune = 0;
            Tune3_Clicked(sender, e);
            MainStack.IsVisible = true;
            return;
        }
        if (icurrentTune == 3)
        {
            Tune2_Clicked(sender, e);
            return;
        }
        if (icurrentTune == 2)
        {
            Tune1_Clicked(sender, e);
            return;
        }
    }

    private void OnSwipeRight(object sender, EventArgs e)
    {

        if (icurrentTune == 1)
        {
            Tune2_Clicked(sender, e);
            return;
        }
        if (icurrentTune == 2)
        {
            Tune3_Clicked(sender, e);
            return;
        }
        if (icurrentTune == 3)
        {
            MainStack.IsVisible = false;
            TuneDataStack.IsVisible = true;
            PageTitle.Text = "Tune Data " + sCurrentFile;
        }
    }

    private void TireSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Picker p = (Picker)sender;
            if (p.SelectedItem != null)
            {
                var sel = p.SelectedItem;
                dTireSize = double.Parse(sel.ToString());
                //var i = p.Items[p.SelectedIndex];
            }
        }
        catch (Exception ex)
        {
            DisplayAlert("Error *", "TireSize_SelectedIndex failed " + ex.Message, "Ok");
        }
    }

    private void DiffRatio_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Picker p = (Picker)sender;
            if (p.SelectedItem != null)
            {
                var sel = p.SelectedItem;
                dDiffRatio = double.Parse(sel.ToString());
                //var i = p.Items[p.SelectedIndex];
            }
        }
        catch (Exception ex)
        {
            DisplayAlert("Error *", "DiffRatio_SelectedIndexChanged failed " + ex.Message, "Ok");
        }
    }
}