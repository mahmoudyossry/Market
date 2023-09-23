using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Core.Global
{
    public class GlobalInfo
    {
        public string UserName { get; set; }
        public string UserId { get; set; }
        public long? ClubId { get; set; }
        public int Type { get; set; } //manager , staff , player
        public static string lang { get; set; }

        public void SetValues( string UserName, string UserId,string lang,long ?clubId,int type)
        {
            this.UserName = UserName;
            this.UserId = UserId;
            GlobalInfo.lang = lang;
            this.ClubId = clubId;
            this.Type = type;
        }
    }
}
