using System.Collections.Generic;

namespace PFL.Models
{
    public class Select2Data
    {
        public int id { get; set; }
        public string text { get; set; }
    }
    public class Select2PlayerData
    {
        public long id { get; set; }
        public string text { get; set; }
    }

    public class Select2PlayerModel
    {
        public List<Select2PlayerData> results { get; set; }

        public Pagination pagination { get; set; }
    }

    public class Select2Model
    {
        public List<Select2Data> results { get; set; }

        public Pagination pagination { get; set; }
    }


    public class Pagination
    {
        public bool more { get; set; }
    }
}