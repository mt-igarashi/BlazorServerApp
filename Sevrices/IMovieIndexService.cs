using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorApp.Forms;
using BlazorApp.Models;

namespace BlazorApp.Services {
    public interface IMovieIndexService
    {
        Task<List<string>> GetGenreList(); 
        Task<List<Movie>> GetMovieList(MovieIndexForm form);
    }
}