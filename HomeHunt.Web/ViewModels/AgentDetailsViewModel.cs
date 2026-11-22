using HomeHunt.Common;

namespace HomeHunt.ViewModels
{
    public class AgentDetailsViewModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string? AgencyName { get; set; }

        public string AgencyDisplayName => AgencyName ?? DisplayConstants.IndependentAgent;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public int PropertyCount { get; set; }

        public int ForSaleCount { get; set; }

        public int ForRentCount { get; set; }

        public List<PropertyListViewModel> Properties { get; set; } = new List<PropertyListViewModel>();
    }
}
