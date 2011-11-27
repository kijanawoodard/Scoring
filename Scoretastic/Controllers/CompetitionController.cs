using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Scoretastic.Web.Infrastructure.AutoMapper;

namespace Scoretastic.Web.Controllers
{
    public class CompetitionController : RavenController
    {
        public ActionResult Index()
        {
            var competitions = RavenSession.Query<Competition>()
                                .OrderBy(x => x.Name);

            var model = new CompetitionIndexViewModel(competitions);
            return View(model);
        }


        [HttpGet]
        public ActionResult Create()
        {
            var model = new CompetitionCreateViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CompetitionCreateViewModel.ViewInput input, FormCollection form)
        {
            return Execute(
                () =>
                    {
                        var competion = input.MapTo<Competition>();
                        RavenSession.Store(competion);
                    },
                () => RedirectToAction("Index"),
                () => ReturnRehydratedView(Create, input));
        }

        

    }
    //make general error show up on page
    public interface IViewModel<T>
    {
        T Input { get; set; }
    }

    public class Competition
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    //create a bit of ceremony to test out some concepts that could be quite useful later
    public class CompetitionCreateViewModel : IViewModel<CompetitionCreateViewModel.ViewInput>
    {
        public CompetitionCreateViewModel()
        {
            Input = new ViewInput();
        }

        public ViewInput Input { get; set; }

        public class ViewInput
        {
            [Required]
            public string Name { get; set; }

            [Required]
            [DataType(DataType.MultilineText)]
            public string Description { get; set; }
        }
    }

    public class CompetitionIndexViewModel
    {
        public CompetitionIndexViewModel(IEnumerable<Competition> competitions)
        {
            Competitions = competitions;
        }

        public IEnumerable<Competition> Competitions { get; set; }
    }
}
