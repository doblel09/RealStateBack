namespace RealStateApp.Core.Application.ViewModels.Home
{
    public class StatisticsViewModel
    {
        public int AvailableProperty { get; set; }
        public int NotAvailableProperty { get; set; }
        public int ActiveAgents { get; set; }
        public int InactiveAgents { get; set; }
        public int ActiveClients { get; set; }
        public int InactiveClients { get; set; }
        public int ActiveDevelopers { get; set; }
        public int InactiveDevelopers { get; set; }
    }

}
