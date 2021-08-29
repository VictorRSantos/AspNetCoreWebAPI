using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.WebAPI.Models;

namespace SmartSchool.WebAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]//http://localhost:5000/api/Aluno
    public class AlunoController : ControllerBase
    {
        public List<Aluno> Alunos = new List<Aluno>()
        {
            new Aluno(){Id=1,Nome="Marcos",Sobrenome="Almeida",Telefone="123456"},
            new Aluno(){Id=2,Nome="Marta",Sobrenome="Kent",Telefone="9999877451"},
            new Aluno(){Id=3,Nome="Laura",Sobrenome="Maria",Telefone="441515154"},
        };

        public AlunoController() { }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(Alunos);
        }


        #region Diferenciar rota do Get passando somente um parametro

        //api/aluno/1
        //http://localhost:5000/api/Aluno/1
        [HttpGet("{Id:int}")]
        public IActionResult GetById(int Id)
        {
            var aluno = Alunos.FirstOrDefault(a => a.Id == Id);

            if (aluno is null) return BadRequest("O Aluno não foi encontrado.");

            return Ok(aluno);
        }


        //api/aluno/1
        //http://localhost:5000/api/Aluno/Marta
        [HttpGet("{Nome}")]
        public IActionResult GetByName(string nome)
        {
            var aluno = Alunos.FirstOrDefault(a => a.Nome.Contains(nome));

            if (aluno is null) return BadRequest("O Aluno não foi encontrado.");

            return Ok(aluno);
        }

        //api/Aluno/ById/1
        //http://localhost:5000/api/Aluno/ById/1
        [HttpGet("ById/{Id}")]
        public IActionResult GetByIdVersao2(int id)
        {
            var aluno = Alunos.FirstOrDefault(a => a.Id == id);

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

            var aluno = Alunos.FirstOrDefault(a => a.Id == id);

            if (aluno is null) return BadRequest("O Aluno não foi encontrado.");

            return Ok(aluno);
        }

        //http://localhost:5000/api/Aluno/ByName?nome=Marta&sobrenome=Kent
        [HttpGet("ByName")]
        public IActionResult GetByNameQueryStrin(string nome, string sobrenome)
        {
            var aluno = Alunos.FirstOrDefault(a =>
             a.Nome.Contains(nome) && a.Sobrenome.Contains(sobrenome));

            if (aluno is null) return BadRequest("O Aluno não foi encontrado");

            return Ok(aluno);
        }

        #endregion


        #region POST

        [HttpPost]
        public IActionResult Post(Aluno aluno)
        {
            return Ok(aluno);
        }

        #endregion


        #region PUT

        [HttpPut("{id}")]
        public IActionResult Put(int id, Aluno aluno)
        {
            return Ok(aluno);
        }

        #endregion


        #region PATCH

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, Aluno aluno)
        {
            return Ok(aluno);
        }

        #endregion



        #region DELETE

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok();
        }


        #endregion



    }
}