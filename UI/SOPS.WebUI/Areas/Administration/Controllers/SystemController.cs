using SOPS.Services.Students;
using SOPS.Services.Templates;
using SOPS.WebUI.Areas.Administration.ViewModels;
using SOPS.WebUI.Areas.Administration.ViewModels.System;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SOPS.Services.Documents;
using SOPS.Services.System;
using System.IO;
using Model.Employees;

namespace SOPS.WebUI.Areas.Administration.Controllers
{
    [Authorize(Roles = EmployeesRoles.Administrator)]
    public class SystemController : Controller
    {
        private const string DocXContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
        
        private readonly ITemplatesProvider _templatesProvider;
        private readonly IUniversityDetailsProvider _universityDetailsProvider;
        private readonly IDocumentDestructor _documentDestructor;
        private readonly IDocumentCreator _documentCreator;
        private readonly IDocumentsProvider _documentsProvider;
        private readonly ITemplateDestructor _templateDestructor;
        private readonly ITemplateCreator _templateCreator;
        private readonly IUniversityDetailsUpdater _universityDetailsUpdater;
        private readonly IStudentDestructor _studentDestructor;

        public SystemController(
            ITemplatesProvider templatesProvider,
            ITemplateCreator templateCreator,
            ITemplateDestructor templateDestructor,
            IDocumentsProvider documentsProvider,
            IDocumentCreator documentCreator,
            IDocumentDestructor documentDestructor,
            IUniversityDetailsProvider universityDetailsProvider,
            IUniversityDetailsUpdater universityDetailsUpdater,
            IStudentDestructor studentDestructor)
        {
            _templatesProvider = templatesProvider;
            _universityDetailsProvider = universityDetailsProvider;
            _documentDestructor = documentDestructor;
            _documentCreator = documentCreator;
            _documentsProvider = documentsProvider;
            _templateDestructor = templateDestructor;
            _templateCreator = templateCreator;
            _universityDetailsUpdater = universityDetailsUpdater;
            _studentDestructor = studentDestructor; 
        }

        public ActionResult Index()
        {
            ViewBag.IsRoot = User.IsInRole(EmployeesRoles.Root);
            return View();
        }

        [ChildActionOnly]
        public ActionResult UniversityPartial()
        {
            var details = _universityDetailsProvider.GetUniversity();
            var university = new UniversityViewModel
                {
                    Address = details.Address,
                    Name = details.Name
                };

            return PartialView("Partials/_university", university);
        }

        [HttpPost]
        public ActionResult University(UniversityViewModel university)
        {
            if (ModelState.IsValid)
            {
                var details = _universityDetailsProvider.GetUniversity();
                details.Address = university.Address;
                details.Name = university.Name;

                _universityDetailsUpdater.Update(details);                
            }

            return RedirectToAction("Index");
        }

        [ChildActionOnly]
        public ActionResult Templates()
        {
            var resources = new ResourcesListViewModel()
            {
                Title = "Szablony umów",
                Id = 1,
                Resources = _templatesProvider.GetAllTemplates().Select(n => new ResourceViewModel() { Id = n.Id, Name = n.Name }).ToList()
            };

            return PartialView("Partials/_templates", resources);
        }

        [ChildActionOnly]
        public ActionResult Documents()
        {
            var resources = new ResourcesListViewModel()
            {
                Title = "Dokumenty do pobrania",
                Id = 1,
                Resources = _documentsProvider.GetAllDocuments().Select(n => new ResourceViewModel() { Id = n.Id, Name = n.Name }).ToList()
            };

            return PartialView("Partials/_documents", resources);
        }

        [ChildActionOnly]
        public ActionResult DeleteTemplate()
        {
            return PartialView("Dialogs/remove-template-dialog");
        }

        public ActionResult Template(int id)
        {
            var t = _templatesProvider.GetTemplate(id);
            string fullPath = string.Format("{0}/{1}", Server.MapPath("~/App_Data/templates"), t.FilePath);

            return File(fullPath, t.ContentType, t.Name);
        }

        public ActionResult Document(int id)
        {
            var d = _documentsProvider.GetDocument(id);
            string fullPath = string.Format("{0}/{1}", Server.MapPath("~/App_Data/documents"), d.Path);

            return File(fullPath, d.ContentType, d.Name);
        }

        [HttpPost]
        public ActionResult DeleteTemplate(int id)
        {
            if (_templateDestructor.CanBeDestroyed(id))
            {
                var fileName = _templatesProvider.GetTemplate(id).FilePath;
                string fullPath = string.Format("{0}/{1}", Server.MapPath("~/App_Data/templates"), fileName);
                System.IO.File.Delete(fullPath);
                _templateDestructor.Destroy(id);                
            }

            return RedirectToAction("Index");
        }

        [ChildActionOnly]
        public ActionResult DeleteDocument()
        {
            return PartialView("Dialogs/remove-document-dialog");
        }

        [HttpPost]
        public ActionResult DeleteDocument(int id)
        {
            var fileName = _documentsProvider.GetDocument(id).Path;
            string fullPath = string.Format("{0}/{1}", Server.MapPath("~/App_Data/documents"), fileName);
            System.IO.File.Delete(fullPath);
            _documentDestructor.Destroy(id);            

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UploadDocument(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var storeFileName = string.Format("{0}.{1}", Guid.NewGuid().ToString(), fileName);
                var path = Path.Combine(Server.MapPath("~/App_Data/documents"), storeFileName);
                
                file.SaveAs(path);

                _documentCreator.Create(fileName, storeFileName, file.ContentType);                
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UploadTemplate(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0 && file.ContentType == DocXContentType)
            {
                var fileName = Path.GetFileName(file.FileName);
                var storeFileName = string.Format("{0}.{1}", Guid.NewGuid().ToString(), fileName);
                var path = Path.Combine(Server.MapPath("~/App_Data/templates"), storeFileName);
                file.SaveAs(path);

                _templateCreator.Create(fileName, storeFileName, DocXContentType);        
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = EmployeesRoles.Root)]
        public ActionResult RemoveAllStudents()
        {
            _studentDestructor.DestroyAllStudents();            

            return RedirectToAction("Index");
        }
    }
}
