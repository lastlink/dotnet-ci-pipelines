namespace MyProject.Repository.Data
{
    public class AppSettings
    {

        public ConnectionStrings ConnectionStrings { get; set; }
    }
    public class ConnectionStrings
    {
        public string Provider { get; set; }
        public string DefaultConnection { get; set; }
    }

}