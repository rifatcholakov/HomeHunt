namespace HomeHunt.Service.Data.Users.Response
{
    public class UserOperationResponseData
    {
        public bool Succeeded { get; set; }

        public List<string> Errors { get; set; } = new List<string>();
    }
}
