namespace ServicesDomicile.DTOs
{
    public class ProfileDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public List<string> Categories { get; set; }
        public string IdImage { get; set; }
    }
}
