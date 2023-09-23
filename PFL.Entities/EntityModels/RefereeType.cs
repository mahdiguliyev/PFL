using System.Collections.Generic;

namespace PFL.Entities.EntityModels
{
    public class RefereeType : BaseModel
    {
        public RefereeType()
        {
            Referees = new HashSet<Referee>();
        }

        public int Id { get; set; }
        public string Name { get; set; }


        public ICollection<Referee> Referees { get; set; }
        //public User UserCreatedBy { get; set; }
        //public User LastUpdateBy { get; set; }
    }
}