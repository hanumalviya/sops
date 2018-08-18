using System;
using System.Linq;
using System.Web.Mvc;
using SOPS.WebUI.ViewModels.Download;
using SOPS.Services.Documents;

namespace SOPS.WebUI.Controllers
{
    public class DownloadController : Controller
    {
        private readonly IDocumentsProvider _documentsProvider;

        public DownloadController(IDocumentsProvider documentsProvider)
        {
            _documentsProvider = documentsProvider;
        }

        public ActionResult Index()
        {
            var list = _documentsProvider.GetAllDocuments().Select(n => new DownloadViewModel()
            {
                Id = n.Id,
                Name = n.Name,
                Uri = n.Path
            });

            return View(list);
        }

        public ActionResult Document(int id)
        {
            var d = _documentsProvider.GetDocument(id);
            string fullPath = string.Format("{0}/{1}", Server.MapPath("~/App_Data/documents"), d.Path);

            return File(fullPath, d.ContentType, d.Name);
        }
    }
}
