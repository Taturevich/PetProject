namespace PetProject.Services.VK
{
    public interface IVKService
    {
        void AddNewGroup(string domain);

        void ParseGroup(string domain);
    }
}
