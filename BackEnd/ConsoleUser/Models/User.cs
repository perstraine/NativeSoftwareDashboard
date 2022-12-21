using System.Diagnostics.CodeAnalysis;

namespace ConsoleUser.Models
{
    //User data in database
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; }
        public string Password { get; set; }
        public string Active { get; set; }
        public string UserType { get; set; }
    }
}
