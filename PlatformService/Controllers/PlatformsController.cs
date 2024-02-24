using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataSerices.Http;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Profiles.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _repo;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;
        private object platDto;

        public PlatformsController(IPlatformRepo repo, 
            IMapper mapper, ICommandDataClient commandDataClient)
        {
            _repo = repo;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine($"Getting Platforms from db"); 

            var platformItems = _repo.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
        }

        [HttpGet("{id}", Name ="GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            var platformItem = _repo.GetPlatformById(id);

            if (platformItem is null) return NotFound("Item not found");

            return Ok(_mapper.Map<PlatformReadDto>(platformItem));
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platDto)
        {
            var platformModel = _mapper.Map<Platform>(platDto);

            _repo.CreatePlatform(platformModel);
            _repo.SaveChanges();


            var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);

            try 
            {
                await _commandDataClient.SendPlatformToCommand(platformReadDto);
                
                Console.WriteLine($"platform has been sent");

            }
            catch(Exception ex )
            {
                Console.WriteLine($"Cound not send synchronuos message", ex.Message);
            }

            return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformReadDto.Id }, platformReadDto);

        }

        
    }
}