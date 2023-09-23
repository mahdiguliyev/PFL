using AutoMapper;
using PFL.Entities.EntityModels;
using PFL.Models.DTO;

using PFL.Models.ViewModels;

namespace PFL.Models
{
    public static class AutoMapperConfigurations
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(cfg =>
            {

                cfg.CreateMap<Club, ClubViewModel>().ReverseMap();
                cfg.CreateMap<User, UserAdminDto>().ReverseMap();
                cfg.CreateMap<User, UserClubAdminDto>().ReverseMap();
                cfg.CreateMap<User, UserRefereeDto>().ReverseMap();
                cfg.CreateMap<Match, MatchCreateDto>().ReverseMap();
                cfg.CreateMap<Player, PlayerCreateViewModel>()
                    .ForMember(dest => dest.PhotoPath, opt => opt.MapFrom(x => x.PhotoUrl))
                    .ReverseMap();
                cfg.CreateMap<ClubPlayerOrderDto, ClubPlayerOrder>().ReverseMap();
                cfg.CreateMap<ClubOfficialOrderDto, ClubOfficialOrder>().ReverseMap();
                cfg.CreateMap<ClubPlayerRequest, PlayerRequestDto>().ReverseMap();
                cfg.CreateMap<ClubPlayerRequest, PlayerRequestEditDto>().ReverseMap();
                cfg.CreateMap<ClubOfficial, ClubOfficialDto>().ReverseMap();
                cfg.CreateMap<ClubOfficial, ClubOfficialEditDto>().ReverseMap();
                cfg.CreateMap<ClubOfficial, LogClubOfficial>().ReverseMap();
                //cfg.CreateMap<Club, DTO.ClubDocumentDto>()
                //    .ForMember(dest => dest.ClubName, opt => opt.MapFrom(x => x.Name))
                //    .ReverseMap();

                //cfg.CreateMap<ClubDocumentOld, AClubDocumentDto>()
                //    .ForMember(dest => dest.ClubId, opt => opt.MapFrom(x => x.Id))
                //    //.ForMember(dest => dest.ClubName, opt => opt.MapFrom(x => x.Name))
                //    .ReverseMap();

                //cfg.CreateMap<ClubDocumentOld, BClubDocumentDto>()
                //    .ForMember(dest => dest.ClubId, opt => opt.MapFrom(x => x.Id))
                //    //.ForMember(dest => dest.ClubName, opt => opt.MapFrom(x => x.Name))
                //    .ReverseMap();

                //cfg.CreateMap<ClubDocumentOld, DClubDocumentDto>()
                //    .ForMember(dest => dest.ClubId, opt => opt.MapFrom(x => x.Id))
                //    //.ForMember(dest => dest.ClubName, opt => opt.MapFrom(x => x.Name))
                //    .ReverseMap();


            });
        }
    }
}