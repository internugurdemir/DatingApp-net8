using System;
using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers;

public class AutoMapperProfiles: Profile
{
       public AutoMapperProfiles()
        {
        CreateMap<AppUser, MemberDto>()
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()))
            .ForMember(dest => dest.PhotoUrl,
                            opt => opt.MapFrom(src =>src.Photos.FirstOrDefault(x => x.IsMain)!.Url));
            CreateMap<Photo, PhotoDto>();
            CreateMap<MemberUpdateDto, AppUser>();
            CreateMap<RegisterDto, AppUser>();
            CreateMap<string, DateOnly>().ConvertUsing(s=>DateOnly.Parse(s));
            CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.SenderPhotoUrl,
                           opt => opt.MapFrom(src =>src.Sender.Photos.FirstOrDefault(x => x.IsMain)!.Url))
                .ForMember(dest => dest.RecipientPhotoUrl, opt => opt.MapFrom(src => 
                    src.Recipient.Photos.FirstOrDefault(x => x.IsMain)!.Url));
            CreateMap<MessageDto, Message>();
            CreateMap<DateTime, DateTime>().ConvertUsing(a=>DateTime.SpecifyKind(a, DateTimeKind.Utc));
            CreateMap<DateTime?, DateTime?>().ConvertUsing(a=>a.HasValue
                                                            ? DateTime.SpecifyKind(a.Value, DateTimeKind.Utc) : null);
        }
}
