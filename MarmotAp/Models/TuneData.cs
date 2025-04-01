using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarmotAp.Models
{
    public class TuneData
    {
        public int antiHunt { get; set; }
        public double gearRatio { get; set; }
        public double tireSize { get; set; }
        public double tpsMin { get; set; }
        public double tpsMax { get; set; }
        public double auxMin { get; set; }
        public double auxMax { get; set; }
        public int dsrevpermile { get; set; }
        public int buildNum { get; set; }

        //public byte[] packedData { get; set; }
        public int TempRatio { get; set; }
        public int Pressure12 { get; set; }
        public int Pressure23 { get; set; }

        public TuneData()
        {
            VersionAndBuild v = new VersionAndBuild();

            antiHunt = 0;
            gearRatio = 3.31;
            tireSize = 32.5;
            tpsMin = 0.5;
            tpsMax = 2.8;
            auxMin = 0.5;
            auxMax = 4.5;
            dsrevpermile = 2000;
            buildNum = Int16.Parse(v.GetBuildNumber());
            TempRatio = 0;
            Pressure12 = 45;
            Pressure23 = 100;
        }

        public bool Parse(string svals)
        {
            bool bRet = false;
            try
            {
                string[] vals = svals.Split(new char[] { ',' });
                if (vals.Length != 12)
                {
                    return bRet;
                }
                bool bOK = false;
                int antihunt = antiHunt;
                bOK = int.TryParse(vals[0], out antihunt);
                if (bOK) { antiHunt = antihunt; }
                double gearratio = gearRatio;
                bOK = double.TryParse(vals[1], out gearratio);
                if (bOK) { gearRatio = gearratio; }
                double tiresize = tireSize;
                bOK = double.TryParse(vals[2], out tiresize);
                if (bOK) { tireSize = tiresize; }
                double tpsmin = tpsMin;
                bOK = double.TryParse(vals[3], out tpsmin);
                if (bOK) { tpsMin = tpsmin; }
                double tpsmax = tpsMax;
                bOK = double.TryParse(vals[4], out tpsmin);
                if (bOK) { tpsMax = tpsmax; }
                double auxmin = auxMin;
                bOK = double.TryParse(vals[5], out auxmin);
                if (bOK) { auxMin = auxmin; }
                double auxmax = auxMax;
                bOK = double.TryParse(vals[6], out auxmax);
                if (bOK) { auxMax = auxmax; }
                int srevpermile = dsrevpermile;
                bOK = int.TryParse(vals[7], out srevpermile);
                if (bOK) { dsrevpermile = srevpermile; }
                int buildnum = buildNum;
                bOK = int.TryParse(vals[8], out buildnum);
                if (bOK) { buildNum = buildnum; }
                int tempratio = TempRatio;
                bOK = int.TryParse(vals[9], out tempratio);
                if (bOK) { TempRatio = tempratio; }
                int pressure12 = Pressure12;
                bOK = int.TryParse(vals[10], out pressure12);
                if (bOK) { Pressure12 = pressure12; }

                // Find the index of the null terminator
                int nullIndex = vals[11].IndexOf('\0');
                // If a null terminator is found, trim the string
                if (nullIndex >= 0)
                {
                    vals[11] = vals[11].Substring(0, nullIndex);
                }
                int pressure23 = Pressure23;
                bOK = int.TryParse(vals[11], out pressure23);
                if (bOK) { Pressure23 = pressure23; }
                bRet = true;
            }
            catch
            {
                return false;
            }

            return bRet;
        }
    }
}
