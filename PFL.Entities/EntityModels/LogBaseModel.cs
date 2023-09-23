using System.ComponentModel.DataAnnotations.Schema;

namespace PFL.Entities.EntityModels
{
    public class LogBaseModel : BaseModel
    {
        [Column("main_id")]
        public int MainId { get; set; }
    }
}