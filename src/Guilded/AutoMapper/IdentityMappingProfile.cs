using System.Collections.Generic;
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
            CreateMap<RoleClaim, IdentityRoleClaim<string>>()
                .ForMember(dest => dest.ClaimValue, opt => opt.UseValue("1"));

            CreateMap<RoleClaim, Permission>()
                .ForMember(dest => dest.PermissionType, opt => opt.MapFrom(src => src.ClaimType));
            CreateMap<Permission, RoleClaim>()
                .ConstructUsing(src => new RoleClaim(src.PermissionType, src.Description));

            CreateMap<IdentityRoleClaim<string>, Permission>()
                .ConvertUsing(claim => Mapper.Map<RoleClaim, Permission>(
                    Mapper.Map<IdentityRoleClaim<string>, RoleClaim>(claim)
                ));
            CreateMap<Permission, IdentityRoleClaim<string>>()
                .ConvertUsing(src => Mapper.Map<RoleClaim, IdentityRoleClaim<string>>(
                    Mapper.Map<Permission, RoleClaim>(src)
                ));

            CreateMap<Identity.ApplicationRole, ViewModels.Core.ApplicationRole>()
                .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.Claims));
            CreateMap<ViewModels.Core.ApplicationRole, Identity.ApplicationRole>()
                .ForMember(dest => dest.Claims, opt => opt.ResolveUsing(src =>
                {
                    var claims = Mapper.Map<IEnumerable<IdentityRoleClaim<string>>>(src.Permissions);
                    foreach (var claim in claims)
                    {
                        claim.RoleId = src.Id;
                    }
                    return claims;
                }));
        }
    }
}