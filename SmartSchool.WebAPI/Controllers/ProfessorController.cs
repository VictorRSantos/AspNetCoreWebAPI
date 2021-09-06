using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchool.WebAPI.Data;
using SmartSchool.WebAPI.Models;

namespace SmartSchool.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]//http://localhost:5000/api/Professor
    public class ProfessorController : ControllerBase
    {


        private readonly IRepository _repo;

        public ProfessorController(IRepository repo)
        {
            _repo = repo;

        }


        [HttpGet]
        public IActionResult Get()
        {
            var result = _repo.GetAllProfessores(true);

            return Ok(result);
        }


        [HttpGet("{id}")]
        public IActionResult GetPorId(int id)
        {

            var professor = _repo.GetProfessorById(id, false);

            if (professor == null) return BadRequest("Professor não encontrado");

            return Ok(professor);

        }


        [HttpGet("ByName")]
        public IActionResult GetByNameQueryString(string nome)
        {
            var professor = _repo.GetAllProfessores().FirstOrDefault(x => x.Nome.Contains(nome));

            if (professor == null) return BadRequest("Professo não encontrado");

            return Ok(professor);

        }


        [HttpPost]
        public IActionResult Post(Professor professor)
        {

            _repo.Add(professor);



            if (_repo.SaveChanges())
            {

                return Ok(professor);
            }

            return BadRequest("Professor não cadastrado");
        }


        [HttpPut("{id}")]
        public IActionResult Put(int id, Professor professor)
        {
            var prof = _repo.GetProfessorById(id, false);

            if (prof == null) return BadRequest("Professo não encontrado");

            _repo.Update(professor);

            if (_repo.SaveChanges())
            {
                return Ok(professor);

            }

            return BadRequest("Professor não atualizado");




        }

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, Professor professor)
        {
            var prof = _repo.GetProfessorById(id, false);

            if (prof is null) return BadRequest("Professor não encontrado");

            _repo.Update(professor);

            if (_repo.SaveChanges())
            {
                return Ok(professor);

            }


            return BadRequest("Professor não atualizado");

        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var professor = _repo.GetProfessorById(id);

            if (professor == null) return BadRequest("Professor não encontrado");

            _repo.Delete(professor);

            if (_repo.SaveChanges())
            {
                return Ok("Professor deletado");

            }

            return BadRequest("professor não deletado");


        }

    }
}