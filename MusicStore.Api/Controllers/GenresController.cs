using Microsoft.AspNetCore.Mvc;
using MusicStore.Dto;
using MusicStore.Entities;
using MusicStore.Repositories;
using System.Net;

namespace MusicStore.Api.Controllers
{
    [ApiController]
    [Route("api/genres")]
    public class GenresController : ControllerBase
    {
        private readonly IGenreRepository repository;
        private readonly ILogger<GenresController> logger;

        //Constructor
        public GenresController(IGenreRepository repository, ILogger<GenresController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = new BaseResponseGeneric<ICollection<Genre>>();

            try
            {
                response.Data = await repository.GetAsync();
                response.Success = true;
                logger.LogInformation($"Se obtuvieron todos los generos musicales");
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Ocurrio un error al obtener la infomracion";
                logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
                return BadRequest(response);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var response = new BaseResponseGeneric<Genre>();
            try
            {
                response.Data = await repository.GetAsync(id);
                response.Success = true;

                //return item is not null ? Ok(item) : NotFound();

                if (response.Data is null)
                {
                    logger.LogWarning($"Genero musical con id {id} no se encontro.");
                    return NotFound(response);

                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = "Ocurrio un error al obtener la informacion";
                logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
                return BadRequest(response);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(Genre genre)
        {
            var response = new BaseResponseGeneric<int>();
            try
            {
                await repository.AddAsync(genre);
                response.Data = genre.Id;
                response.Success = true;
                logger.LogInformation($"Genero musical con id {genre.Id} insertado");
                return StatusCode((int)HttpStatusCode.Created, response);
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Ocurrio un error al insertar la informacion";

                logger.LogError($"{ex.Message}");
                return BadRequest(response);
            }

        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, Genre genre)
        {
            var response = new BaseResponse();

            try
            {
                var item = await repository.GetAsync(id);
                if (item is null)
                {
                    logger.LogWarning($"Genero musical con id {id} no se encontro");
                    return NotFound(response);
                }


                await repository.UpdateAsync(id, genre);
                response.Success = true;
                logger.LogInformation($"Genero musical con id {id} actualizado");
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Ocurrio un error al actualizar la informacion";
                logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
                return BadRequest(response);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = new BaseResponse();
            try
            {
                var item = await repository.GetAsync(id);
                if (item is null)
                {
                    logger.LogWarning($"Genero musical con id {id} no se encontro");
                    return NotFound(response);
                }

                await repository.DeleteAsync(id);
                response.Success = true;
                logger.LogInformation($"Genero musical con id {id} eliminado");
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Ocurrio un error al eliminar la informacion";
                logger.LogError($"{ex.Message}");
                return BadRequest(response);
            }
        }
    }
}
