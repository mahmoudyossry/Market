using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Core
{
    public class AppSettingsConfiguration
    {
        public ConnectionStrings ConnectionStrings { get; set; }
        public JWT JWT { get; set; }
        public AttachmentSettings AttachmentSettings { get; set; }
        public EnvironmentSettings EnvironmentSettings { get; set; }
    }
    public class ConnectionStrings
    {
        public string MarketDB { get; set; }

    }
    public class AttachmentSettings
    {
        public string TempAttachment { get; set; }
        public string ActualAttachment { get; set; }
    }
    public class EnvironmentSettings
    {
        public bool EnableSwagger { get; set; }
    }
    public class JWT
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public string SecretKey { get; set; }
    }
}
