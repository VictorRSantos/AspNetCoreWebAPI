using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchool.WebAPI.Data;
using SmartSchool.WebAPI.Dtos;
using SmartSchool.WebAPI.Models;

namespace SmartSchool.WebAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]//http://localhost:5000/api/Aluno
    public class AlunoController : ControllerBase
    {
      
        private readonly IRepository _repo;

        private readonly IMapper _mapper;

        public AlunoController(IRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
           
        }


        [HttpGet]
        public IActionResult Get()
        {

            var alunos = _repo.GetAllAlunos(true);          


            return Ok(_mapper.Map<IEnumerable<AlunoDto>>(alunos));
        }


        #region Diferenciar rota do Get passando somente um parametro

        //api/aluno/1
        //http://localhost:5000/api/Aluno/1
        [HttpGet("{Id:int}")]
        public IActionResult GetById(int Id)
        {
            var aluno = _repo.GetAlunoById(Id, false);

            if (aluno is null) return BadRequest("O Aluno não foi encontrado.");


            var alunoDto = _mapper.Map<AlunoDto>(aluno);

            return Ok(alunoDto);
        }


        // //api/aluno/1
        // //http://localhost:5000/api/Aluno/Marta
        // [HttpGet("{Nome}")]
        // public IActionResult GetByName(string nome)
        // {
        //     var aluno = _repo.GetAllAlunos().FirstOrDefault(a => a.Nome.Contains(nome));

        //     if (aluno is null) return BadRequest("O Aluno não foi encontrado.");

        //     return Ok(aluno);
        // }

        //api/Aluno/ById/1
        //http://localhost:5000/api/Aluno/ById/1
        [HttpGet("ById/{Id}")]
        public IActionResult GetByIdVersao2(int id)
        {
            var aluno = _repo.GetAlunoById(id, false);

            if (aluno is null) return BadRequest("O Aluno não foi encontrado.");

            var alunoDto = _mapper.Map<AlunoDto>(aluno);

            return Ok(alunoDto);
        }

        #endregion

        //Passar Parametro via QueryString
        #region Passar Parametro via QueryString

        //http://localhost:5000/api/Aluno/ById?Id=1
        [HttpGet("ById")]
        public IActionResult GetByIdQueryString(int id)
        {

            var aluno = _repo.GetAllAlunos().FirstOrDefault(a => a.Id == id);

            if (aluno is null) return BadRequest("O Aluno não foi encontrado.");

            return Ok(aluno);
        }

        //http://localhost:5000/api/Aluno/ByName?nome=Marta&sobrenome=Kent
        [HttpGet("ByName")]
        public IActionResult GetByNameQueryStrin(string nome, string sobrenome)
        {
            var aluno = _repo.GetAllAlunos().FirstOrDefault(a =>
             a.Nome.Contains(nome) && a.Sobrenome.Contains(sobrenome));

            if (aluno is null) return BadRequest("O Aluno não foi encontrado");

            return Ok(aluno);
        }

        #endregion


        #region POST

        [HttpPost]
        public IActionResult Post(AlunoRegistrarDto model)
        {
            var aluno = _mapper.Map<Aluno>(model);

            _repo.Add(aluno);


            if (_repo.SaveChanges())
            {

                return Created($"/api/Aluno/{model.Id}", _mapper.Map<AlunoDto>(aluno));
            }

            return BadRequest("Aluno não cadastrado");

        }

        #endregion


        #region PUT

        [HttpPut("{id}")]
        public IActionResult Put(int id, AlunoRegistrarDto model)
        {

            var aluno = _repo.GetAlunoById(id);

            if (aluno is null) return BadRequest("Aluno não encontrado.");

            _mapper.Map(model, aluno);


            _repo.Update(aluno);
            
            if (_repo.SaveChanges())
            {
               return Created($"/api/Aluno/{model.Id}", _mapper.Map<AlunoDto>(aluno));
            }

            return BadRequest("Aluno não atualizado");
        }

        #endregion


        #region PATCH

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, AlunoRegistrarDto model)
        {

            var aluno = _repo.GetAlunoById(id);

            if (aluno is null) return BadRequest("Aluno não encontrado.");

            _mapper.Map(model, aluno);

            _repo.Update(aluno);
            if (_repo.SaveChanges())
            {
               return Created($"/api/Aluno/{model.Id}", _mapper.Map<AlunoDto>(aluno));
            }

            return BadRequest("Aluno não atualizado");
        }

        #endregion



        #region DELETE

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {

            var aluno = _repo.GetAlunoById(id);

            if (aluno is null) return BadRequest("Aluno não encontrado.");

            _repo.Delete(aluno);

            if (_repo.SaveChanges())
            {
                return Ok("Aluno Deletado");
            }

            return BadRequest("Aluno não encontrado");
        }


        #endregion



    }
}