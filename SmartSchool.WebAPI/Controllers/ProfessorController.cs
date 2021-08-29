using Microsoft.AspNetCore.Mvc;

namespace SmartSchool.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]//http://localhost:5000/api/Professor
    public class ProfessorController : ControllerBase
    {
        public ProfessorController()
        {
            
        }


        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Professores: Testa, Malandro, Capivara");
        }

    }
}