using System;

namespace Docunet
{
    public class DocunetSettings
    {
        internal static DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        public DateTimeFormat DateTimeFormat { get; set; }
        
        public DocunetSettings()
        {
            DateTimeFormat = DateTimeFormat.DateTime;
        }
    }
}
