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
    public class StudentsController : ControllerBase
    {
        private readonly ILogger<SchoolsController> _logger;

        public StudentsController(ILogger<SchoolsController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetStudentByID/{ID}")]
        public string GetStudentByID(string ID)
        {
            int VerifyValue = 0;
            Students APIStudents = new Students();
            if (!Int32.TryParse(ID, out VerifyValue))
                return APIStudents.SendError(400);
            else
                return APIStudents.GetStudentByID(ID);
        }

        // POST: api/Students
        [HttpPost]
        public string Post([FromBody] Students.Student NewStudent)
        {
            DateTime VerifyDate = DateTime.MinValue;            
            Students APIStudents = new Students();
            if (!DateTime.TryParse(NewStudent.Birthdate, out VerifyDate))
                return APIStudents.SendError(400);
            else
                return APIStudents.AddNewStudent(NewStudent);
        }

        [HttpGet("GetStudentsBySchoolID/{SchoolID}")]
        public string GetStudentsBySchoolID(string SchoolID)
        {
            int VerifyValue = 0;
            Students APIStudents = new Students();
            if (!Int32.TryParse(SchoolID, out VerifyValue))
                return APIStudents.SendError(400);
            else
                return APIStudents.GetStudentsBySchoolID(SchoolID);
        }
    }
}