using System.Collections.Generic;
using AutoMapper;
using Guilded.Security.Claims;
using Guilded.Areas.Admin.ViewModels.Roles;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Guilded.AutoMapper
{
    public class IdentityMappingProfile : Profile
    {
        public IdentityMappingProfile()
        {
            CreateMap<IdentityRoleClaim<string>, RoleClaim>()
                .ForSourceMember(src => src.Id, opt => opt.Ignore())
                .ForSourceMember(src => src.RoleId, opt => opt.Ignore())
                .ConvertUsing(claim => RoleClaimTypes.LookUpGuildedRoleClaim(claim));
            CreateMap<RoleClaim, IdentityRoleClaim<string>>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.RoleId, opt => opt.Ignore())
                .ForMember(dest => dest.ClaimValue, opt => opt.UseValue("1"));

            CreateMap<RoleClaim, Permission>()
                .ForMember(dest => dest.PermissionType, opt => opt.MapFrom(src => src.ClaimType));
            CreateMap<Permission, RoleClaim>()
                .ForMember(dest => dest.ClaimType, opt => opt.Ignore())
                .ConstructUsing(src => new RoleClaim(src.PermissionType, src.Description));

            CreateMap<IdentityRoleClaim<string>, Permission>()
                .ConvertUsing(claim => Mapper.Map<RoleClaim, Permission>(
                    Mapper.Map<IdentityRoleClaim<string>, RoleClaim>(claim)
                ));
            CreateMap<Permission, IdentityRoleClaim<string>>()
                .ConvertUsing(src => Mapper.Map<RoleClaim, IdentityRoleClaim<string>>(
                    Mapper.Map<Permission, RoleClaim>(src)
                ));

            CreateMap<Identity.ApplicationRole, Areas.Admin.ViewModels.Roles.ApplicationRole>()
                .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.Claims));
            CreateMap<Areas.Admin.ViewModels.Roles.ApplicationRole, Identity.ApplicationRole>()
                .ForMember(dest => dest.Claims, opt => opt.ResolveUsing(src =>
                {
                    var claims = Mapper.Map<IEnumerable<IdentityRoleClaim<string>>>(src.Permissions);
                    foreach (var claim in claims)
                    {
                        claim.RoleId = src.Id;
                    }
                    return claims;
                }))
                .ForMember(dest => dest.NormalizedName, opt => opt.Ignore());
        }
    }
}