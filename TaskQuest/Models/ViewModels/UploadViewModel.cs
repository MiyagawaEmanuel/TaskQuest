using System.Web;

namespace TaskQuest.ViewModels
{
    public class UploadViewModel
    {
        public HttpPostedFileBase File { get; set; }
    }
}