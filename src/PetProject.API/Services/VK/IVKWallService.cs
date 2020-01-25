namespace PetProject.Services.VK
{
    public interface IVKWallService
    {
        void AddNewGroup(string domain);

        void ParseGroup(string domain);
    }
}
