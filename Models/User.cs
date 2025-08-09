namespace FC_Application.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;  // Ideally, this should be a hashed password
    }

}
