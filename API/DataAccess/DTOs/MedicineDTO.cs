using API.DataAccess.DTOs.Attribute;
using System.ComponentModel.DataAnnotations;

namespace API.DataAccess.DTOs
{
    public class MedicineDTO
    {
        public  required string Name { get; set; }

        public required string Description { get; set; }
        [Url]
        public  required string ImageURL { get; set; }
    }
}
