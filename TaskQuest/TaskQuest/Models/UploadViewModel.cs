using System.Web;

namespace TaskQuest.Models
{
    public class UploadViewModel
    {
        public HttpPostedFileBase File { get; set; }
    }
}