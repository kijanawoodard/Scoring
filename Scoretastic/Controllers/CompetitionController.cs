using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Scoretastic.Infrastructure.AutoMapper;

namespace Scoretastic.Controllers
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
            if (!ModelState.IsValid)
                return ReturnReHydratedView(Create, input);

            var competion = input.MapTo<Competition>();
            RavenSession.Store(competion);

            return RedirectToAction("Index");
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
