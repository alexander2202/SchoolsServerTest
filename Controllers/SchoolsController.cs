using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SchoolsServer.Controllers
{
    [ApiController]
    //[Route("[controller]")]
    [Route("api/[controller]")]
    public class SchoolsController : ControllerBase
    {
        private readonly ILogger<SchoolsController> _logger;

        public SchoolsController(ILogger<SchoolsController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetAllSchools")]
        public string GetAllSchools()
        {
            Schools Forecast = new Schools();
            return Forecast.GetSchools();
        }

        [HttpGet("GetSchoolByID/{ID}")]
        public string GetSchoolByID(string ID)
        {
            int VerifyValue = 0;    
            Schools Forecast = new Schools();
            if (!Int32.TryParse(ID, out VerifyValue))
                return Forecast.SendError(400);
            else
                return Forecast.GetSchoolByID(ID);
        }

        // POST: api/Schools
        [HttpPost]
        public string Post([FromBody] Schools.School NewSchool)
        {
            Schools APIClasses = new Schools();
            return APIClasses.AddNewSchool(NewSchool);
        }
    }
}
