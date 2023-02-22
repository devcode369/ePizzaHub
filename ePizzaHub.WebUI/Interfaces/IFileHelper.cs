using Microsoft.AspNetCore.Http;

namespace ePizzaHub.WebUI.Interfaces
{
    public interface IFileHelper
    {
        void DelteFile(string imageUrl);
        string UploadFile(IFormFile file);
    }
}
