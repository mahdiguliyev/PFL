using System.Collections.Generic;
using System.Linq;
using PFL.Models.ViewModels;

namespace PFL.Utils
{
    public class SortHelper
    {

        public static List<MatchPlayerSelectionViewModel> SortProtokolPlayerList(List<MatchPlayerSelectionViewModel> list, int minAgeLimit)
        {
            var sortedList = new List<MatchPlayerSelectionViewModel>();


            //var tempList = list.OrderBy(x => x.PlayerBirthDate).ToList();


            sortedList.AddRange(list.Where(x => x.CitizenshipId == 16 && x.PlayerBirthDate?.Year < minAgeLimit));

            sortedList.AddRange(list.Where(x => x.CitizenshipId != 16 && x.PlayerBirthDate?.Year < minAgeLimit));

            sortedList.AddRange(list.Where(x=> x.PlayerBirthDate?.Year >= minAgeLimit).OrderBy(x=>x.PlayerBirthDate));




            return sortedList;
        }


    }
}