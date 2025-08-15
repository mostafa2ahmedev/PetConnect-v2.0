using System.IO;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.Interfaces
{
    public interface IFaceComparisonService
    {
        
        Task<bool> AreFacesMatchingAsync(Stream image1, Stream image2);
    }
}
