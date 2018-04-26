using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WorldCapBet.ApplicationException;
using WorldCapBet.BLL;
using WorldCapBet.Data;
using WorldCapBet.Helpers;
using WorldCapBet.Model;
using WorldCapBet.ModelDTO;

namespace WorldCapBet.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : Controller
    {
        private IUserService userService;
        private IMatchService matchService;
        private IMapper mapper;
        private readonly AppSettings appSettings;

        public UsersController(
            IUserService _userService,
            IMatchService _matchService,
            IMapper _mapper,
            IOptions<AppSettings> _appSettings)
        {
            this.userService = _userService;
            this.matchService = _matchService;
            this.mapper = _mapper;
            this.appSettings = _appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]UserDTO userDto)
        {
            var user = userService.Authenticate(userDto.Username, userDto.Password);

            if (user == null)
                return Unauthorized();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info (without password) and token to store client side
            return Ok(new
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Token = tokenString
            });
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register([FromBody]UserDTO userDto)
        {
            // map dto to entity
            var user = mapper.Map<User>(userDto);

            try
            {
                // save 
                userService.Create(user, userDto.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = userService.GetAll();
            var userDtos = mapper.Map<IList<UserDTO>>(users);
            return Ok(userDtos);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = userService.GetById(id);
            var userDto = mapper.Map<UserDTO>(user);
            return Ok(userDto);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProfile(int id, [FromBody]UserDTO userDto)
        {
            // map dto to entity and set id
            var user = mapper.Map<User>(userDto);
            user.Id = id;

            try
            {
                // save 
                userService.UpdateProfile(user, userDto.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            userService.Delete(id);
            return Ok();
        }

        [HttpGet("{id}/pronostics")]
        public IActionResult GetUserPronostic(int id)
        {
            var pronostic = userService.GetUserPronostics(id);
            var pronosticDtos = mapper.Map<IList<PronosticDTO>>(pronostic);
            return Ok(pronosticDtos);
        }

        [HttpGet("ranking")]
        public IActionResult GetRanking()
        {
            var users = userService.GetAll();
            var userDtos = mapper.Map <IList<UserDTO>>(users);

            foreach(UserDTO userDto in userDtos)  
                userDto.Score = userService.GetUserScore(userDto.Id);


            var rankedDto = userDtos.OrderBy(userdto => userdto.Score);
            int inc = 0;

            foreach(UserDTO userDto in rankedDto)
            {
                userDto.Rank = rankedDto.Count() - 0;
                inc++;
            }

            return Ok(rankedDto);
        }

        [HttpGet("{id}/AllMatchAndPronostic")]
        public IActionResult GetAllMatchAndPronostic([FromRoute]int id)
        {
            var pronostics = userService.GetUserPronostics(id);
            var matchs = matchService.GetAll();

            var matchDtos = mapper.Map<IList<MatchDTO>>(matchs);
            var pronosticDtos = mapper.Map<IList<PronosticDTO>>(pronostics);

            foreach (MatchDTO matchDto in matchDtos)
            {

                var pronosticdto = pronosticDtos.Where(x => x.IdMatch == matchDto.Id).Single();
                if (pronosticdto != null)
                    matchDto.Pronostic = pronosticdto;
            }

            return Ok(matchDtos);
        }
    }
}