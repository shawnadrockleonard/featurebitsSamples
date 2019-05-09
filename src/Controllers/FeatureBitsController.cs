using FeatureBits.Core;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace FeatureBitWebDev.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeatureBitsController : Controller
    {

        private readonly IFeatureBitEvaluator _evaluator;

        public FeatureBitsController(IFeatureBitEvaluator evaluator)
        {
            _evaluator = evaluator;
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] Features id)
        {
            var definition = _evaluator.Definitions.SingleOrDefault(d => d.Id == (int)id);

            if (definition != null)
            {
                bool isEnabled = _evaluator.IsEnabled(id, 0);
                return new JsonResult(isEnabled);
            }

            return NotFound();
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            if (_evaluator.IsEnabled(Features.DummyOn))
            {
                return new string[] { "value1", "value2" };
            }
            else
            {
                return new string[] { };
            }
        }

    }
}
