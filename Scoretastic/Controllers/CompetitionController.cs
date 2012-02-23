using System;
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

            if (ControllerContext.IsChildAction)
                return PartialView(model);
            else
                return View(model);
        }


        [HttpGet]
        public ActionResult Create()
        {
            //hmmmm
            var model = new CompetitionCreateViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CompetitionCreateViewModel.ViewInput input, FormCollection form)
        {
            return Execute(
                action: () =>
                    {
                        var competion = input.MapTo<Competition>();
                        RavenSession.Store(competion);
                    },
                onsuccess: () => RedirectToAction("Index"),
                onfailure: () => ReturnRehydratedView(Create, input));
        }


        public ActionResult Edit(string id)
        {
            var competition = RavenSession.Load<Competition>(id);
            var model = new CompetitionEditViewModel();
            model.Input = competition.MapTo<CompetitionEditViewModel.ViewInput>();

            return View(model);
        }

        [HttpPut]
        public ActionResult Edit(CompetitionEditViewModel.ViewInput input)
        {
            return Execute(
                action: () =>
                {
                    var competition = RavenSession.Load<Competition>(input.Id);
                    competition = input.MapTo(competition);
                    RavenSession.Store(competition);
                },
                onsuccess: () => RedirectToAction("Edit", new { id = input.Id }),
                onfailure: () => ReturnRehydratedView(() => Edit(input.Id), input));
        }

        public ActionResult Details(string id)
        {
            var competition = RavenSession.Load<Competition>(id);
            var model = new CompetitionDetailsViewModel(competition);

            return View();
        }
    }

    public class CompetitionDetailsViewModel
    {
        public CompetitionDetailsViewModel(Competition competition)
        {
            
        }
    }

    public class Competition
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime FirstDay { get; set; }
        public DateTime LastDay { get; set; }
    }

    public class CompetitionEditViewModel : IViewModel<CompetitionEditViewModel.ViewInput>
    {

        public class ViewInput : CompetitionInput
        {
            public string Id { get; set; }
        }

        public ViewInput Input { get; set; }
    }

    public interface IViewModel<T>
    {
        T Input { get; set; }
    }

    //create a bit of ceremony to test out some concepts that could be quite useful later
    public class CompetitionCreateViewModel : IViewModel<CompetitionCreateViewModel.ViewInput>
    {
        public CompetitionCreateViewModel()
        {
            Input = new ViewInput();
        }

        public ViewInput Input { get; set; }

        public class ViewInput : CompetitionInput { }
    }

    public class CompetitionInput
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        public DateTime FirstDay { get; set; }
        
        [Required]
        public DateTime LastDay { get; set; }
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
