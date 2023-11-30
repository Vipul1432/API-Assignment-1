using System.ComponentModel.DataAnnotations;

namespace API_CRUD.Models
{
    public class KeyValueRequest
    {
        [Key]
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
