using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PFL.Entities.EntityModels;

namespace PFL.Utils
{
    public class SeasonHelper
    {
        public static int GetCurrentSeasonId()
        {
            DateTime currentDate = DateTime.Now;

            using (PFLContext db = new PFLContext())
            {

                var seasons = db.Seasons
                    .Where(x => x.StartDate <= currentDate.Date && x.EndDate >= currentDate.Date).Select(x=>x.Id).ToList();

                if (seasons.Count > 1 || seasons.Count == 0)
                    return 0;

                var seasonId = seasons.FirstOrDefault();

                return seasonId;
            }
        }

        public static Season GetCurrentSeason()
        {
            DateTime currentDate = DateTime.Now;

            using (PFLContext db = new PFLContext())
            {

                var season = db.Seasons
                    .FirstOrDefault(x => x.StartDate <= currentDate.Date && x.EndDate >= currentDate.Date);

                return season;
            }
        }

        /// <summary>
        /// retrun season if id is null returun current season
        /// </summary>
        /// <param name="id">seasonId</param>
        /// <returns>Season model</returns>
        public static Season GetSeason(int? id)
        {
            DateTime currentDate = DateTime.Now;

            using (PFLContext db = new PFLContext())
            {

                var seasonQuery = db.Seasons.AsQueryable();

                if (!id.HasValue)
                {
                    seasonQuery= seasonQuery.Where(x => x.StartDate <= currentDate.Date && x.EndDate >= currentDate.Date);
                }
                else
                {
                    seasonQuery = seasonQuery.Where(x => x.Id == id);
                }

                var season = seasonQuery.FirstOrDefault();

                return season;
            }
        }
    }
}