using System;
using System.Linq;
using AutoMapper;
using cobric_backend.Api.Enums;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using usuarios_backend.Api.Entities;
using usuarios_backend.Api.Models;
using usuarios_backend.Api.Repositories;

namespace usuarios_backend.Api.Controllers
{
    [ApiController]
    [Route("usuarios")]

    public class UsuarioController : ControllerBase
    {
        private const string _mensagemErroExcecao = "Ocorreu um erro inesperado";
        private readonly ILogger<UsuarioController> _logger;
        private readonly IMapper _mapper;
        private readonly IUsuarioRepository _repository;

        public UsuarioController(ILogger<UsuarioController> logger
           , IUsuarioRepository repository
           , IMapper mapper)
        {
            _logger = logger;

            _repository = repository ??
                throw new ArgumentNullException(nameof(repository));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

        }

        /// <summary>
        /// Cria um novo usuario
        /// </summary>
        /// <response code="201">Retorna quando um recurso foi criado com sucesso</response>
        /// <response code="400">Retorna quando houve uma requisição mal formada</response>
        /// <response code="409">Retorna quando o recurso ja existe</response>
        /// <response code="500">Retorna quando houve um erro interno do serviço</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErroRetorno))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErroRetorno))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroRetorno))]
        [Produces("application/json")]
        public IActionResult Create([FromBody]UsuariosInclusao usuario, [FromHeader ]int tipoUsuario)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return new BadRequestObjectResult(new ErroRetorno(string.Join(", ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage))));
                }

                var usuarioEntity = _mapper.Map<Usuarios>(usuario);

                if (tipoUsuario != 1 && tipoUsuario != 2)
                    return new BadRequestObjectResult(new ErroRetorno("Tipo de usuário não existente "));

                if (String.IsNullOrEmpty(usuarioEntity.email))
                    return new BadRequestObjectResult(new ErroRetorno("Email não pode estar vazio "));

                if (String.IsNullOrEmpty(usuarioEntity.senha))
                    return new BadRequestObjectResult(new ErroRetorno("Senha não pode estar vazio "));

                if ((EnumTipoUsuario)tipoUsuario == EnumTipoUsuario.Aluno){
                        if(String.IsNullOrEmpty(usuarioEntity.ra))
                             return new BadRequestObjectResult(new ErroRetorno("Ra não pode estar vazio "));
                }

                if ((EnumTipoUsuario)tipoUsuario == EnumTipoUsuario.NAluno){
                        if(String.IsNullOrEmpty(usuarioEntity.telefone))
                             return new BadRequestObjectResult(new ErroRetorno("Telefone não pode estar vazio "));
                }

                if(_repository.ExisteCadastro(usuarioEntity.email, usuarioEntity.telefone, usuarioEntity.ra))
                     return new BadRequestObjectResult(new ErroRetorno("ja existe uma conta associada a este e-mail, ra ou telefone "));

                usuarioEntity.tipoUsuario = (EnumTipoUsuario)tipoUsuario;

                _repository.Cadastrar(usuarioEntity);


                if (_repository.Salvar())
                {
                    return CreatedAtRoute(null, null);
                }

                return new JsonResult(500, new ErroRetorno(_mensagemErroExcecao));
            }
            catch (Exception ex)
            {
                _logger.LogError(_mensagemErroExcecao, ex);
                return new JsonResult(500, new ErroRetorno(_mensagemErroExcecao));
            }
        }
    }
}