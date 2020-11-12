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
    public class MigrationController : ControllerBase
    {
        private readonly ILogger<SchoolsController> _logger;

        public MigrationController(ILogger<SchoolsController> logger)
        {
            _logger = logger;
        }

        // POST: api/Migration
        [HttpPost("AdmissionStudent")]
        public string AdmissionStudent([FromBody] Migration.StudentMigration NewMigration)
        {
            Migration APIMigration = new Migration();
            return APIMigration.AdmissionStudent(NewMigration);
        }

        // POST: api/Migration
        [HttpPost("DismissialStudent")]
        public string DismissialStudent([FromBody] Migration.StudentMigration NewMigration)
        {
            Migration APIMigration = new Migration();
            return APIMigration.DismissialStudent(NewMigration);
        }

        // POST: api/Migration
        [HttpPost("GetStudentsByPeriod")]
        public string GetStudentsByPeriod([FromBody] Migration.StudentMigration HistoryParams)
        {
            Migration APIMigration = new Migration();
            return APIMigration.GetStudentsByPeriod(HistoryParams);
        }
    }
}