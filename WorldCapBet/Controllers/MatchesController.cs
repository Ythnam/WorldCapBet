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
    [Route("api/Matches")]
    public class MatchesController : Controller
    {
        private IMatchService matchService;
        private IMapper mapper;

        public MatchesController(IMatchService _matchService, IMapper _mapper)
        {
            this.matchService = _matchService;
            this.mapper = _mapper;
        }

        // GET: api/Matches
        [HttpGet]
        public IActionResult GetMatch()
        {
            var match = matchService.GetAll();
            var matchDtos = mapper.Map<IList<MatchDTO>>(match);
            return Ok(matchDtos);
        }

        // GET: api/Matches/5
        [HttpGet("{id}")]
        public IActionResult GetMatch([FromRoute]int id)
        {
            var match = matchService.GetById(id);
            var matchDtos = mapper.Map<MatchDTO>(match);
            return Ok(matchDtos);
        }

        // PUT: api/Matches/5
        [HttpPut("{id}")]
        public IActionResult UpdateMatch([FromRoute]int id, [FromBody] MatchDTO matchDto)
        {
            // map dto to entity and set id
            var match = mapper.Map<Match>(matchDto);
            match.Id = id;

            try
            {
                // save 
                matchService.UpdateMatch(match);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Matches/5
        [HttpPut("{id}/score")]
        public IActionResult UpdateScore([FromRoute]int id, [FromBody] MatchDTO matchDto)
        {
            // map dto to entity and set id
            var match = mapper.Map<Match>(matchDto);
            match.Id = id;

            try
            {
                // save 
                matchService.UpdateScore(match);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(ex.Message);
            }
        }

        // POST: api/Matches
        [HttpPost]
        public IActionResult PostMatch([FromBody] MatchDTO matchDto)
        {
            // map dto to entity
            var match = mapper.Map<Match>(matchDto);

            try
            {
                // save 
                matchService.Create(match);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Matches/5
        [HttpDelete("{id}")]
        public IActionResult DeleteMatch([FromRoute] int id)
        {
            matchService.Delete(id);
            return Ok();
        }
    }
}