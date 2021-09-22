using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchool.WebAPI.Data;
using SmartSchool.WebAPI.Models;
using SmartSchool.WebAPI.V1.Dtos;

namespace SmartSchool.WebAPI.V1.Controllers
{
     /// <summary>
    /// Versão 1 do meu controlador de professor
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProfessorController : ControllerBase
    {


        private readonly IRepository _repo;

        private readonly IMapper _mapper;

        public ProfessorController(IRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;

        }


        [HttpGet]
        public IActionResult Get()
        {
            var professores = _repo.GetAllProfessores(true);

            return Ok(_mapper.Map<IEnumerable<ProfessorDto>>(professores));
        }


        [HttpGet("{id}")]
        public IActionResult GetPorId(int id)
        {

            var professor = _repo.GetProfessorById(id, false);

            if (professor == null) return BadRequest("Professor não encontrado");

            var professorDto = _mapper.Map<ProfessorDto>(professor);

            return Ok(professorDto);

        }


        [HttpGet("ByName")]
        public IActionResult GetByNameQueryString(string nome)
        {
            var professor = _repo.GetAllProfessores().FirstOrDefault(x => x.Nome.Contains(nome));

            if (professor == null) return BadRequest("Professo não encontrado");

            var professorDto = _mapper.Map<ProfessorDto>(professor);

            return Ok(professorDto);

        }


        [HttpPost]
        public IActionResult Post(ProfessorRegistradoDto model)
        {

            var professor = _mapper.Map<Professor>(model);

            _repo.Add(professor);

            if (_repo.SaveChanges())
            {

                return Created($"/api/Aluno/{model.Id}", _mapper.Map<ProfessorDto>(professor));
            }

            return BadRequest("Professor não cadastrado");
        }


        [HttpPut("{id}")]
        public IActionResult Put(int id, ProfessorRegistradoDto model)
        {
            var professor = _repo.GetProfessorById(id, false);

            if (professor == null) return BadRequest("Professo não encontrado");

            _mapper.Map(model, professor);

            _repo.Update(professor);

            if (_repo.SaveChanges())
            {
                  return Created($"/api/Aluno/{model.Id}", _mapper.Map<ProfessorDto>(professor));

            }

            return BadRequest("Professor não atualizado");




        }

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, ProfessorRegistradoDto model)
        {
            var professor = _repo.GetProfessorById(id, false);

            if (professor is null) return BadRequest("Professor não encontrado");

            _mapper.Map(model, professor);

            _repo.Update(professor);

            if (_repo.SaveChanges())
            {
                 return Created($"/api/Aluno/{model.Id}", _mapper.Map<ProfessorDto>(professor));

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