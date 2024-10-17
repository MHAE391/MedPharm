namespace API.DataAccess.DTOs.Authentication
{
    public class RegisterCustomerDTO : CustomerDTO
    {
        public required string Password { get; set; }

    }
}
