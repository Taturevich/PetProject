namespace PetProject.Domain
{
    public class Image
    {
        public int ImageId { get; set; }
        
        public int PetId { get; set; }

        public string ImagePath { get; set; }
    }
}
