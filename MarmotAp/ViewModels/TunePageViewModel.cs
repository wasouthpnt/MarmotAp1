using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarmotAp.ViewModels
{

    public partial class TunePageViewModel
    {
        public ObservableCollection<Models.Point> Unlock { get; set; }
        public ObservableCollection<Models.Point> Lockup { get; set; }
        public ObservableCollection<Models.Point> ODon { get; set; }
        public ObservableCollection<Models.Point> ODoff { get; set; }
        public ObservableCollection<Models.Point> Shift_12 { get; set; }
        public ObservableCollection<Models.Point> Shift_23 { get; set; }
        public ObservableCollection<Models.Point> Shift_21 { get; set; }
        public ObservableCollection<Models.Point> Shift_32 { get; set; }

        public TunePageViewModel()
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

            //tune1 = new Tune(); tune1.SetDefaults();
            //tune2 = new Tune(); tune2.SetDefaults();
            //tune3 = new Tune(); tune3.SetDefaults();

        }
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
    }
}
