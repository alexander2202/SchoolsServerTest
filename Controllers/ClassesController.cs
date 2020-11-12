using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SchoolsServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private readonly ILogger<SchoolsController> _logger;

        public ClassesController(ILogger<SchoolsController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetClassByID/{ID}")]
        public string GetClassByID(string ID)
        {
            int VerifyValue = 0;
            Classes APIClasses = new Classes();
            if (!Int32.TryParse(ID, out VerifyValue))
                return APIClasses.SendError(400);
            else
                return APIClasses.GetClassByID(ID);
        }

        // POST: api/Classes
        [HttpPost]
        public string Post([FromBody] Classes.SchoolClass NewClass)
        {
            Classes APIClasses = new Classes();
            return APIClasses.AddNewClass(NewClass);
        }

        [HttpGet("GetClassessBySchoolID/{SchoolID}")]
        public string GetClassessBySchoolID(string SchoolID)
        {
            int VerifyValue = 0;
            Classes APIClasses = new Classes();
            if (!Int32.TryParse(SchoolID, out VerifyValue))
                return APIClasses.SendError(400);
            else
                return APIClasses.GetClassessBySchoolID(SchoolID);
        }
    }
}
