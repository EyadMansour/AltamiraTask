using System.Collections.Generic;
using System.IO;

namespace Shared
{
    public class MailModel
    {
        public string TargetMail { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public List<FileInfo> Attachments { get; set; }
    }
}
