using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchool.WebAPI.Data;
using SmartSchool.WebAPI.V1.Dtos;
using SmartSchool.WebAPI.Models;


namespace SmartSchool.WebAPI.V1.Controllers
{

    /// <summary>
    /// Versão 1 do meu controlador de aluno
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]//http://localhost:5000/api/Aluno
    public class AlunoController : ControllerBase
    {
      
        private readonly IRepository _repo;

        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repo"></param>
        /// <param name="mapper"></param>
        public AlunoController(IRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
           
        }


        /// <summary>
        /// Método responsável para retornar todos os meus alunos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {

            var alunos = _repo.GetAllAlunos(true);          


            return Ok(_mapper.Map<IEnumerable<AlunoDto>>(alunos));
        }



        #region Diferenciar rota do Get passando somente um parametro

        /// <summary>
        /// Método responsável por retornar apenas um Aluno por meio do Código ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("ById/{Id}")]
        public IActionResult GetByIdVersao2(int id)
        {
            var aluno = _repo.GetAlunoById(id, false);

            if (aluno is null) return BadRequest("O Aluno não foi encontrado.");

            var alunoDto = _mapper.Map<AlunoDto>(aluno);

            return Ok(alunoDto);
        }

        #endregion

       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //http://localhost:5000/api/Aluno/ById?Id=1
        [HttpGet("ById")]
        public IActionResult GetByIdQueryString(int id)
        {

            var aluno = _repo.GetAllAlunos().FirstOrDefault(a => a.Id == id);

            if (aluno is null) return BadRequest("O Aluno não foi encontrado.");

            return Ok(aluno);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nome"></param>
        /// <param name="sobrenome"></param>
        /// <returns></returns>
        //http://localhost:5000/api/Aluno/ByName?nome=Marta&sobrenome=Kent
        [HttpGet("ByName")]
        public IActionResult GetByNameQueryStrin(string nome, string sobrenome)
        {
            var aluno = _repo.GetAllAlunos().FirstOrDefault(a =>
             a.Nome.Contains(nome) && a.Sobrenome.Contains(sobrenome));

            if (aluno is null) return BadRequest("O Aluno não foi encontrado");

            return Ok(aluno);
        }
  

        #region POST
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
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
      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
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

        #region DELETE
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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