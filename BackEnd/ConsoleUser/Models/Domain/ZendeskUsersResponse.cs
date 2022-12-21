namespace ConsoleUser.Models.Domain
{
    //Details about Zendesk Users
    public class Photo
    {
        public string url { get; set; }
        public long id { get; set; }
        public string file_name { get; set; }
        public string content_url { get; set; }
        public string mapped_content_url { get; set; }
        public string content_type { get; set; }
        public int size { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public bool inline { get; set; }
        public bool deleted { get; set; }
        public List<Thumbnail> thumbnails { get; set; }
    }

    public class ZendeskUsers
    {
        public List<User> users { get; set; }
        public object next_page { get; set; }
        public object previous_page { get; set; }
        public int count { get; set; }
    }

    public class Thumbnail
    {
        public string url { get; set; }
        public long id { get; set; }
        public string file_name { get; set; }
        public string content_url { get; set; }
        public string mapped_content_url { get; set; }
        public string content_type { get; set; }
        public int size { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public bool inline { get; set; }
        public bool deleted { get; set; }
    }

    public class User
    {
        public object id { get; set; }
        public string url { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string time_zone { get; set; }
        public string iana_time_zone { get; set; }
        public object phone { get; set; }
        public object shared_phone_number { get; set; }
        public Photo photo { get; set; }
        public int locale_id { get; set; }
        public string locale { get; set; }
        public long? organization_id { get; set; }
        public string role { get; set; }
        public bool verified { get; set; }
        public object external_id { get; set; }
        public List<object> tags { get; set; }
        public object alias { get; set; }
        public bool active { get; set; }
        public bool shared { get; set; }
        public bool shared_agent { get; set; }
        public DateTime? last_login_at { get; set; }
        public bool? two_factor_auth_enabled { get; set; }
        public object signature { get; set; }
        public object details { get; set; }
        public object notes { get; set; }
        public int? role_type { get; set; }
        public long? custom_role_id { get; set; }
        public bool moderator { get; set; }
        public string ticket_restriction { get; set; }
        public bool only_private_comments { get; set; }
        public bool restricted_agent { get; set; }
        public bool suspended { get; set; }
        public long? default_group_id { get; set; }
        public bool report_csv { get; set; }
        public UserFields user_fields { get; set; }
    }

    public class UserFields
    {
    }
}
