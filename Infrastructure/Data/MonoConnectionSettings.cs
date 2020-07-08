using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data
{
    public class MonoConnectionSettings : IMonoConnectionSettings
    {
        public string Table { get; set; }
        public string ConnectionString { get; set; }
        public string Database { get; set; }
    }

    public interface IMonoConnectionSettings
    {
        string Table { get; set; }
        string ConnectionString { get; set; }
        string Database { get; set; }
    }
}
