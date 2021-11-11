using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using BlazorApp.Messages;
using BlazorApp.Models;

namespace BlazorApp.Services {
    public interface IMovieCreateService
    {
        MessageList MessageList { get; set; }
        ValidationMessageStore MessageStore { get; set; }

        Task Register(Movie movie); 
        Task Update(Movie movie);
        void HandleUpdateValidationRequested(object sender, ValidationRequestedEventArgs args);
        void SetMessages(MessageList message, ValidationMessageStore store);
    }
}