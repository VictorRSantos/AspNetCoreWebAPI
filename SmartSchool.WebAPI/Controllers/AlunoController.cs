using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchool.WebAPI.Data;
using SmartSchool.WebAPI.Models;

namespace SmartSchool.WebAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]//http://localhost:5000/api/Aluno
    public class AlunoController : ControllerBase
    {
        private readonly SmartContext _context;
        public AlunoController(SmartContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.Alunos);
        }


        #region Diferenciar rota do Get passando somente um parametro

        //api/aluno/1
        //http://localhost:5000/api/Aluno/1
        [HttpGet("{Id:int}")]
        public IActionResult GetById(int Id)
        {
            var aluno = _context.Alunos.FirstOrDefault(a => a.Id == Id);

            if (aluno is null) return BadRequest("O Aluno não foi encontrado.");

            return Ok(aluno);
        }


        //api/aluno/1
        //http://localhost:5000/api/Aluno/Marta
        [HttpGet("{Nome}")]
        public IActionResult GetByName(string nome)
        {
            var aluno = _context.Alunos.FirstOrDefault(a => a.Nome.Contains(nome));

            if (aluno is null) return BadRequest("O Aluno não foi encontrado.");

            return Ok(aluno);
        }

        //api/Aluno/ById/1
        //http://localhost:5000/api/Aluno/ById/1
        [HttpGet("ById/{Id}")]
        public IActionResult GetByIdVersao2(int id)
        {
            var aluno = _context.Alunos.FirstOrDefault(a => a.Id == id);

            if (aluno is null) return BadRequest("O Aluno não foi encontrado.");

            return Ok(aluno);
        }

        #endregion

        //Passar Parametro via QueryString
        #region Passar Parametro via QueryString

        //http://localhost:5000/api/Aluno/ById?Id=1
        [HttpGet("ById")]
        public IActionResult GetByIdQueryString(int id)
        {

            var aluno = _context.Alunos.FirstOrDefault(a => a.Id == id);

            if (aluno is null) return BadRequest("O Aluno não foi encontrado.");

            return Ok(aluno);
        }

        //http://localhost:5000/api/Aluno/ByName?nome=Marta&sobrenome=Kent
        [HttpGet("ByName")]
        public IActionResult GetByNameQueryStrin(string nome, string sobrenome)
        {
            var aluno = _context.Alunos.FirstOrDefault(a =>
             a.Nome.Contains(nome) && a.Sobrenome.Contains(sobrenome));

            if (aluno is null) return BadRequest("O Aluno não foi encontrado");

            return Ok(aluno);
        }

        #endregion


        #region POST

        [HttpPost]
        public IActionResult Post(Aluno aluno)
        {
            _context.Add(aluno);

            _context.SaveChanges();

            return Ok(aluno);
        }

        #endregion


        #region PUT

        [HttpPut("{id}")]
        public IActionResult Put(int id, Aluno aluno)
        {

            var alu = _context.Alunos.AsNoTracking().FirstOrDefault(x => x.Id == id);

            if (alu is null) return BadRequest("Aluno não encontrado.");

            _context.Update(aluno);

            _context.SaveChanges();

            return Ok(aluno);
        }

        #endregion


        #region PATCH

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, Aluno aluno)
        {

            var alu = _context.Alunos.AsNoTracking().FirstOrDefault(x => x.Id == id);

            if (alu is null) return BadRequest("Aluno não encontrado.");

            _context.Update(aluno);
            _context.SaveChanges();
            return Ok(aluno);
        }

        #endregion



        #region DELETE

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {

            var aluno = _context.Alunos.FirstOrDefault(x => x.Id == id);

            if (aluno is null) return BadRequest("Aluno não encontrado.");

            _context.Remove(aluno);

            _context.SaveChanges();

            return Ok();
        }


        #endregion



    }
}