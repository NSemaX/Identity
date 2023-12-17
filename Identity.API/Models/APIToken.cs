namespace Identity.API.Models
{
    public class APIToken
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Roles { get; set; }
        public string Refreshtoken { get; set; }
        public string JWT { get; set; }
        public DateTime JWT_ExpireDate { get; set; }
        public DateTime RT_ExpireDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string IP { get; set; }
        public int Revoked { get; set; }

    }
}
