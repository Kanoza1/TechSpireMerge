using AutoMapper;
using Core.Entities;
using PersonalLearning.Dtos;

namespace PersonalLearning.Helper
{
    public class ProfileMapping:Profile
    {
        public ProfileMapping()
        {
          
            CreateMap<SignupDto,AppUser > ()
                .ForMember(destination => destination.FirstName, options => options.MapFrom(
                    src => src.FirstName))
                .ForMember(destination => destination.LastName, options => options.MapFrom(
                    src => src.LastName))
                .ForMember(destination => destination.PhoneNumber, options => options.MapFrom(
                    src => src.PhoneNumber))
                 .ForMember(destination => destination.Email, options => options.MapFrom(
                     src => src.Email));

            CreateMap<AppUser,UserDto >()
                .ForMember(destination => destination.Email, options => options.MapFrom(
                    src => src.Email))
                .ForMember(destination => destination.Name, options => options.MapFrom(
                    src => src.UserName));
            CreateMap<LoginDto, AppUser>(

           ).ForMember(des => des.Email, options => options.MapFrom(
               u => u.Email));
            CreateMap<Question, QuestionDto>()
                .ForMember(destination => destination.Id, options => options.MapFrom(
                      src => src.Id))
                  .ForMember(destination => destination.QuestionText, options => options.MapFrom(
                      src => src.QuestionText))
                  .ForMember(destination => destination.Answer1, options => options.MapFrom(
                      src => src.Answer1))
                  .ForMember(destination => destination.Answer2, options => options.MapFrom(
                      src => src.Answer2))
                  .ForMember(destination => destination.Answer3, options => options.MapFrom(
                      src => src.Answer3));
                  
               

        }

    }
}
