using AutoMapper;
using Guilded.Identity;
using Guilded.Security.Claims;
using Guilded.ViewModels.Core;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Guilded.AutoMapper
{
    public class IdentityMappingProfile : Profile
    {
        public IdentityMappingProfile()
        {
            CreateMap<IdentityRoleClaim<string>, RoleClaim>()
                .ConvertUsing(claim => RoleClaimTypes.LookUpGuildedRoleClaim(claim));
            CreateMap<RoleClaim, Permission>()
                .ForMember(dest => dest.PermissionType, opt => opt.MapFrom(src => src.ClaimType));
            CreateMap<IdentityRoleClaim<string>, Permission>()
                .ConvertUsing(claim => Mapper.Map<RoleClaim, Permission>(
                    Mapper.Map<IdentityRoleClaim<string>, RoleClaim>(claim)
                ));
            CreateMap<Identity.ApplicationRole, ViewModels.Core.ApplicationRole>()
                .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.Claims));
        }
    }
}