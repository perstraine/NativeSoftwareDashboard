namespace ConsoleUser.DTO
{
    //Details required for login
    public class LoginRequest
    {
        public string UserEmail { get; set; }
        public string Password { get; set; }
    }
}