using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceCommission.DTOs;
using ServiceCommission.Repositories.Interfaces;
using ServiceCommission.Utils;

namespace ServiceCommission.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : BaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IMapper _mapper;
        private readonly IUserRepository _repository;

        public UserController(ILogger<UserController> logger, IMapper mapper, IUserRepository repository)
        {
            _logger = logger;
            _mapper = mapper;
            _repository = repository;
        }


        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public ActionResult<dynamic> Authenticate([FromBody] LoginDTO model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = _repository.GetByNameAndPassword(model.Login, model.Password);

            if (user == null)
                return Unauthorized(new { Error = "Invalid user!" });

            var token = TokenProvider.GenerateToken(_mapper.Map<UserDTO>(user));

            return new
            {
                User = user.Login,
                token
            };
        }


        [HttpGet]
        [Route("recovery")]
        [AllowAnonymous]
        public ActionResult<dynamic> GetByEmail(string email)
        {

            if(string.IsNullOrEmpty(email) || !email.IsValidEmail())  return BadRequest(new { Error = "Email invalido!" });


            var user = _repository.GetByEmail(email);
            
            if(user==null) return Ok();

            var token = TokenProvider.GenerateToken(_mapper.Map<UserDTO>(user));


            try
            {
                var absoluteUri = string.Concat(
                          Request.Scheme,
                          "://",
                          Request.Host.ToUriComponent(),
                          Request.PathBase.ToUriComponent(),
                          Request.Path.ToUriComponent()
                        );

                var endPointHome = absoluteUri.Replace("api/user/recovery", "");

                var endPointPassword = endPointHome + "#alterarSenha/" + Helpers.Base64Encode(token);

                var templates = Helpers.GetTemplete("EmailRecovery.html").Replace("END_POINT", endPointPassword).Replace("SITE_HOME", endPointHome);
                Helpers.SendEmail(email, "Sistema - Recuperação de Senha", templates, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
               }

            return Ok();
            
        }




        [HttpPost]
        [Route("change-password")]
        [AllowAnonymous]
        public ActionResult<dynamic> ChangePassword(ChangePasswordDTO dto)
        {
            try
            {

                var jwtToken = Helpers.Base64Decode(dto.Token);
                var jwt = new JwtSecurityToken(jwtToken);

                var idStr = jwt.Claims.FirstOrDefault(f => f.Type == ClaimTypes.Sid)?.Value;

                if(string.IsNullOrEmpty(idStr)) return BadRequest("Token invalido!");

                var idUser = new Guid(idStr);

                var user = _repository.GetById(idUser);

                if(user==null) return BadRequest("Token invalido!");

                user.Password = dto.Password.GetHash(user.Login);

                _repository.Update(user);
                _repository.Save();

                return Ok();

            }
            catch(Exception ex)
            {
                return BadRequest("Token invalido!");
            }

         
        }

        [HttpPut]
        [Route("change-password-profile")]
        public ActionResult<dynamic> ChangePasswordProfile(ChangePasswordDTO dto)
        {
            try
            {

                var user = _repository.GetById(this.UserId);

                if (string.IsNullOrEmpty(dto.Password)) return BadRequest("Password invalido!");

                if (user == null) return BadRequest("User invalido!");

                user.Password = dto.Password.GetHash(user.Login);

                _repository.Update(user);
                _repository.Save();

                return Ok();

            }
            catch (Exception)
            {
                return BadRequest("Token invalido!");
            }


        }

    }
}
