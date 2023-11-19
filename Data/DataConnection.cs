
namespace toDoApp_Server.Data
{
    public class DataConnection
    {
        private readonly string sqlString = string.Empty;

        public DataConnection()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();
            sqlString = builder.GetSection("ConnectionStrings:todoappDBCon").Value;
        }

        public string GetString()
        {
            return sqlString;
        }
    }
}
