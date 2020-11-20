using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceCommission.DTOs;
using ServiceCommission.Models;
using ServiceCommission.Repositories.Interfaces;
using ServiceCommission.Utils;

namespace ServiceCommission.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommissionController : BaseController
    {

        private readonly ILogger<CommissionController> _logger;
        private readonly IMapper _mapper;
        private readonly ICommissionRepository _repository;
        private readonly IUserRepository _userRepository;
        public CommissionController(ILogger<CommissionController> logger, 
                                    IMapper mapper, 
                                    ICommissionRepository repository,
                                    IUserRepository userRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _repository = repository;
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult> GetList([FromQuery] QueryParameters parameters)
        {
            var list = _repository.List(parameters);
            return Ok(_mapper.Map<IList<CommissionDTO>>(list));
        }

        [HttpGet]
        [Route("find")]
        public async Task<ActionResult> Find(int situation, string description = "") {

            int.TryParse(Regex.Match(description, @"\d+").Value, out int order);

            IQueryable<Commission> list = _repository.FindByDescriptionOrOrderOrSituation(description.Replace(order.ToString(),""), order, situation);
            return Ok(_mapper.Map<IList<CommissionDTO>>(list));
        }

        [HttpGet("{id}")]
        public ActionResult<dynamic> GetByOrder(Guid id)
        {

            Commission commission = _repository.GetById(id);

            if (commission == null)
                return NotFound(new { Error = "Commission not found" });


            return _mapper.Map<CommissionDTO>(commission);
        }



        [HttpGet]
        [Route("order/{order}")]
        [AllowAnonymous]
        public ActionResult<dynamic> GetByOrder(int order)
        {

           Commission commission = _repository.GetByOrder(order);

            if (commission == null)
                return NotFound(new { Error = "Commission not found" });


            return _mapper.Map<CommissionDTO>(commission);
        }


        [HttpPost]
        public ActionResult Create([FromBody] CommissionDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user= _userRepository.GetById(this.UserId);

            var entity = _mapper.Map<Commission>(dto);

            entity.User = user;

            int orderRandom;
            do
            {
                orderRandom = Helpers.RandomNumber6digits('1');
            } while (_repository.IsOrder(orderRandom));

            entity.Order = orderRandom;

            _repository.Create(entity);
            _repository.Save();

            return Ok(_mapper.Map<CommissionDTO>(entity));
        }

        [HttpPut]
        public ActionResult Update([FromBody] CommissionDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var user = _userRepository.GetById(this.UserId);

            var entity = _repository.GetById(dto.Id);
          
            if(entity == null)
                return NotFound(new { Error = "Commission not found" });

            if(entity.User.Id != user.Id)
                return NotFound(new { Error = "Commission not found" });

            var order = entity.Order;
            _mapper.Map(dto, entity);
            entity.Order = order;
            entity.UpdateAt = DateTime.Now;
            _repository.Update(entity);
            _repository.Save();
            return Ok(_mapper.Map<CommissionDTO>(entity));
        }

        [HttpDelete("{id}")]
        public ActionResult<dynamic> Delete(Guid id)
        {

            Commission commission = _repository.GetById(id);

            if (commission == null)
                return NotFound(new { Error = "Commission not found" });

            _repository.Delete(commission);
            _repository.Save();
            return null;
        }
    }
}
