namespace MarmotAp.Models
{
    class TuneSeries
    {
        public string Name { get; set; }
        public string Series { get; set; }
        public TuneSeries(string sName, string sSeries)
        {
            Name = sName;
            Series = sSeries;
        }
    }
}
