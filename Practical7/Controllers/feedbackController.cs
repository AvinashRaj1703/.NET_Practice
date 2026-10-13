using System.Collections.Generic;
using System.Web.Mvc;
using WebApplication7.Models;

namespace WebApplication7.Controllers
{
    public class FeedbackController : Controller
    {
        // Store feedback temporarily
        private static List<Feedback> feedbackList = new List<Feedback>();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Submit(Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                feedback.Id = feedbackList.Count + 1;

                feedbackList.Add(feedback);

                return RedirectToAction("ViewFeedback");
            }

            return View("Index", feedback);
        }

        public ActionResult ViewFeedback()
        {
            return View(feedbackList);
        }
    }
}