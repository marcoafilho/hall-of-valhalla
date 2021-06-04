using System;
using Microsoft.Extensions.Configuration;

namespace HallOfValhalla
{
    public class Database
    {
        public IConfiguration Configuration { get; }

        public Database(IConfiguration configuration)
        {
            Configuration = configuration;
        }
    }
}
