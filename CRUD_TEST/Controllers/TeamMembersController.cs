using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRUD_TEST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamMembersController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<TeamMember>> Get()
        {
            var teamMembers = new List<TeamMember>
            {
                new TeamMember 
                {
                    Id = Guid.NewGuid(),
                    Name = "Sikandar Ramzan",
                    FirstName = "Sikandar",
                    LastName = "Ramzan",
                    Role = "Full Stack Engineer",
                    TechStack = "MERN, Dotnet (.NET)"
                },

                new TeamMember 
                {
                    Id = Guid.NewGuid(),
                    Name = "Hashir Ali Shuja",
                    FirstName = "Hashir",
                    LastName = "Ali",
                    Role = "Backend Engineer",
                    TechStack = "Dotnet (.NET), Azure Cloud"
                },

                new TeamMember
                {
                    Id = Guid.NewGuid(),
                    Name = "Abdul Rehman",
                    FirstName = "Abdul",
                    LastName = "Rehman",
                    Role = "Backend Engineer",
                    TechStack = "Dotnet (.NET), Azure Cloud"

                }
            };

            return Ok(teamMembers);
        }
    }
}
