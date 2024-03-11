using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MusicStore.Dto.Request;
using MusicStore.Dto.Response;
using MusicStore.Entities;
using MusicStore.Persistence;

namespace MusicStore.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly ApplicationDbContext context;

        //private readonly List<Genre> genreList = new List<Genre>();
        //MetodoContructor
        public GenreRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        //Metodos
        public async Task<List<GenreResponseDto>> GetAsync()
        {
           var items = await context.Genres
                .AsNoTracking()
                .ToListAsync();

            //Mapping
            var genresReponseDto = items.Select(x=> new GenreResponseDto { 
                Id = x.Id,
                Name = x.Name,
                Status = x.Status,
            }).ToList();
            return genresReponseDto;
        }

        public async Task<GenreResponseDto?> GetAsync(int id)
        {
            var item = await context.Genres
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            var genreResponseDto = new GenreResponseDto();
            if (item is not null)
            {
                //Mapping
                genreResponseDto.Id = item.Id;
                genreResponseDto.Name = item.Name;
                genreResponseDto.Status = item.Status;
            }
            else
            {
                throw new InvalidOperationException($"No se encontro el registro con id {id}.");
            }
            return genreResponseDto;
        }

        public async Task<int> AddAsync(GenreRequestDto genreRecuestDto)
        {
            //Mapping
            var genre = new Genre
            {
                Name = genreRecuestDto.Name,
                Status = genreRecuestDto.Status,
            };

            context.Genres.Add(genre);
            await context.SaveChangesAsync();
            return genre.Id;
        }

        public async Task UpdateAsync(int id, GenreRequestDto genreRecuestDto)
        {
            var item = await context.Genres
             .AsNoTracking()
             .FirstOrDefaultAsync(x => x.Id == id);


            if (item is not null)
            {
                //Mapping
                item.Name = genreRecuestDto.Name;
                item.Status = genreRecuestDto.Status;
                context.Update(item);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException($"No se encontro el registro con id {id}.");
            }
        }

        public async Task DeleteAsync(int id)
        {
            var item = await context.Genres
                  .AsNoTracking()
                  .FirstOrDefaultAsync(x => x.Id == id);

            if (item is not null)
            {
                context.Genres.Remove(item);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException($"No se encontro el registro con id {id}.");
            }
        }

    }
}
