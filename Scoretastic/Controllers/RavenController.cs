using System;
using System.Web.Mvc;
using Raven.Client;

namespace Scoretastic.Web.Controllers
{
    //https://github.com/ayende/RaccoonBlog/blob/master/HibernatingRhinos.Loci.Common/Controllers/RavenController.cs
    public abstract class RavenController : Controller
    {
        public static IDocumentStore DocumentStore
        {
            get { return _documentStore; }
            set
            {
                if (_documentStore == null)
                {
                    _documentStore = value;
                }
            }
        }
        private static IDocumentStore _documentStore;

        public IDocumentSession RavenSession { get; protected set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            RavenSession = _documentStore.OpenSession();
        }

        // TODO: Consider re-applying https://github.com/ayende/RaccoonBlog/commit/ff954e563e6996d44eb59a28f0abb2d3d9305ffe
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.IsChildAction)
                return;

            CompleteSessionHandler(filterContext);
        }

        protected ActionResult ReturnRehydratedView<T>(Func<ActionResult> action, T input) where T : class
        {
            var result = action();
            var view = result as ViewResult;
            if (view != null)
            {
                var model = view.Model as IViewModel<T>;
                if (model != null)
                    model.Input = input;
            }

            return result;
        }

        protected void CompleteSessionHandler(ActionExecutedContext filterContext)
        {
            using (RavenSession)
            {
                if (filterContext.Exception != null)
                    return;

                if (RavenSession != null)
                    RavenSession.SaveChanges();
            }

//            TaskExecutor.StartExecuting();
        }

        protected HttpStatusCodeResult HttpNotModified()
        {
            return new HttpStatusCodeResult(304);
        }

//        protected ActionResult Xml(XDocument xml, string etag)
//        {
//            return new XmlResult(xml, etag);
//        }
    }
}