using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scoretastic.Web.Controllers
{
    public class RegistrationController : Controller
    {
        //
        // GET: /Registration/

        public ActionResult Index()
        {
            return View();
        }
    }

    public class GymRegistration
    {
        public string Id { get; set; }
        public string CompetitionId { get; set; }

        public string GymName { get; set; }
        public string City { get; set; }
        public bool IsSmallGym { get; set; }   
    }

    public class TeamRegistration
    {
        public string Id { get; set; }
        public string GymRegistrationId { get; set; }

        public string TeamName { get; set; }
        public string Division { get; set; }
        public int NumberOfParticipants { get; set; }
        public bool IsShowTeam { get; set; }
    }
}
