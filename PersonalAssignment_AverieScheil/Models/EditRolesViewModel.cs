namespace PersonalAssignment_AverieScheil.Models
{
    public class EditRolesViewModel
    {
        public string UserId { get; set; } = "";
        public string UserName { get; set; } = "";
        public List<string> AvailableRoles { get; set; } = new List<string>();
        public List<string> SelectedRoles { get; set;  } = new List<string>();
    }
}
