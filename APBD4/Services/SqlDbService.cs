namespace APBD4.Services

{
    public class SqlDbService
    {   
        private readonly IConfiguration Configuration;
        string connectionString;
        public SqlDbService() { 
            connectionString = Configuration.GetConnectionString("AnimalDB");
            Console.WriteLine("SAAAAAAAAAAAAAAAAAAAAAAAAAAAAIL" + connectionString);
        }
        
        public string getConString()
        {
            return connectionString;
        }
    }
}
