using System.Threading.Tasks;

namespace PetProject.Services.VK
{
    public interface IVKWallService
    {
        Task AddNewGroup(string domain);

        Task ParseGroup(string domain);
    }
}
