using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorldCapBet.ApplicationException;
using WorldCapBet.BLL;
using WorldCapBet.Data;
using WorldCapBet.Model;
using WorldCapBet.ModelDTO;

namespace WorldCapBet.Controllers
{
    [Produces("application/json")]
    [Route("api/Pronostics")]
    public class PronosticsController : Controller
    {
        private IPronosticService pronosticService;
        private IMapper mapper;

        public PronosticsController(IPronosticService _pronosticService, IMapper _mapper)
        {
            pronosticService = _pronosticService;
            mapper = _mapper;
        }

        // GET: api/Pronostics
        [HttpGet]
        public IActionResult GetPronostic()
        {
            var pronostic = pronosticService.GetAll();
            var pronosticDtos = mapper.Map<IList<PronosticDTO>>(pronostic);
            return Ok(pronosticDtos);
        }

        // GET: api/Pronostics/5
        [HttpGet("{id}")]
        public IActionResult GetPronostic([FromRoute] int id)
        {
            var pronostic = pronosticService.GetById(id);
            var pronosticDtos = mapper.Map<PronosticDTO>(pronostic);
            return Ok(pronosticDtos);
        }

        // PUT: api/Pronostics/5
        [HttpPut("{id}")]
        public IActionResult UpdatePronostic([FromRoute] int id, [FromBody] PronosticDTO pronosticDto)
        {
            // map dto to entity and set id
            var pronostic = mapper.Map<Pronostic>(pronosticDto);
            pronostic.Id = id;

            try
            {
                // save 
                pronosticService.Update(pronostic);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(ex.Message);
            }
        }

        // POST: api/Pronostics
        [HttpPost]
        public IActionResult PostPronostic([FromBody] PronosticDTO pronosticDto)
        {
            // map dto to entity
            var pronostic = mapper.Map<Pronostic>(pronosticDto);

            try
            {
                // save 
                pronosticService.Create(pronostic);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Pronostics/5
        [HttpDelete("{id}")]
        public IActionResult DeletePronostic([FromRoute] int id)
        {
            pronosticService.Delete(id);
            return Ok();
        }
    }
}