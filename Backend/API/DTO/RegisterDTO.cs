namespace API.DTO
{
    public class RegisterDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int CodMember { get; set; }
        public int CodInstitution { get; set; }
    }
}