using FeatureBits.Core;
using FeatureBitWebDev.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FeatureBitWebDev.Controllers
{
    public class HomeController : Controller
    {
        internal readonly IFeatureBitEvaluator featureBitEvaluator;
        public HomeController(IFeatureBitEvaluator bits)
        {
            featureBitEvaluator = bits;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ListBits()
        {
            var transformedBits = featureBitEvaluator.Definitions.Select(bitt => new SqlViewModel
            {
                Id = bitt.Id,
                AllowedUsers = bitt.AllowedUsers,
                CreatedByUser = bitt.CreatedByUser,
                CreatedDateTime = bitt.CreatedDateTime,
                ExactAllowedPermissionLevel = bitt.ExactAllowedPermissionLevel,
                ExcludedEnvironments = bitt.ExcludedEnvironments,
                LastModifiedByuser = bitt.LastModifiedByUser,
                LastModifiedDateTime = bitt.LastModifiedDateTime,
                MinimumAllowedPermissionLevel = bitt.MinimumAllowedPermissionLevel,
                Name = bitt.Name,
                OnOff = bitt.OnOff,
                Dependencies = bitt.Dependencies
            });

            return View(transformedBits);
        }
    }
}
