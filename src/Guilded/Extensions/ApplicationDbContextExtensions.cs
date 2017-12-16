using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Guilded.Areas.Forums.DAL;
using Guilded.Data;
using Guilded.Data.Forums;
using Guilded.Data.Identity;
using Guilded.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Guilded.Extensions
{
    public static class ApplicationDbContextExtensions
    {
        private static readonly Dictionary<string, string[]> _requiredForums = new Dictionary<string, string[]>
        {
            {
                "General Section",
                new []
                {
                    "General Discussion",
                    "News!",
                    "Recruitment"
                }
            },
            {
                "Members Only",
                new []
                {
                    "Dossiers of Selama",
                    "Guides (RP and Gameplay)",
                    "Creativity Corner",
                    "Synopses!",
                    "The Guardians",
                    "The Keepers",
                    "The Artisans",
                    "Members Only Discussion"
                }
            },
            {
                "Eastern Kingdom Liberators",
                new []
                {
                    "Creativity Corner",
                    "Dossiers",
                    "Event Synopses",
                    "Random Discussion"
                }
            },
            {
                "Officers Section",
                new []
                {
                    "Officers Lounge"
                }
            }
        };

        public static void EnsureRequiredDataIsPresent(this ApplicationDbContext context)
        {
            context.EnsureRequiredRolesArePresent();
            context.EnsureRequiredForumsArePresent();

            context.SaveChanges();
        }

        private static void EnsureRequiredRolesArePresent(this ApplicationDbContext context)
        {
            var adminRole = context.GetOrInsertRole("Admin");
            var guestRole = context.GetOrInsertRole("Guest");

            foreach (var roleClaim in RoleClaimValues.RoleClaims)
            {
                if (RoleDoesNotHaveClaim(context, adminRole, roleClaim))
                {
                    context.RoleClaims.Add(new IdentityRoleClaim<string>
                    {
                        ClaimValue = roleClaim.ClaimValue,
                        ClaimType = RoleClaimTypes.Permission,
                        RoleId = adminRole.Id
                    });
                }
            }

            if (RoleDoesNotHaveClaim(context, guestRole, RoleClaimValues.ForumsReader))
            {
                context.RoleClaims.Add(new IdentityRoleClaim<string>
                {
                    ClaimValue = RoleClaimValues.ForumsReaderClaim,
                    ClaimType = RoleClaimTypes.Permission,
                    RoleId = guestRole.Id
                });
            }
            if (RoleDoesNotHaveClaim(context, guestRole, RoleClaimValues.ForumsWriter))
            {
                context.RoleClaims.Add(new IdentityRoleClaim<string>
                {
                    ClaimValue = RoleClaimValues.ForumsWriterClaim,
                    ClaimType = RoleClaimTypes.Permission,
                    RoleId = guestRole.Id
                });
            }
        }

        private static void EnsureRequiredForumsArePresent(this ApplicationDbContext context)
        {
            var displayOrder = 0;

            foreach (var forumValue in _requiredForums)
            {
                var forumSection = context.ForumSections
                    .Include(f => f.Forums)
                    .FirstOrDefault(s => s.Title == forumValue.Key);

                if (forumSection == null)
                {
                    forumSection = new ForumSection
                    {
                        Title = forumValue.Key,
                        DisplayOrder = displayOrder,
                        Forums = new List<Forum>(),
                        IsActive = true
                    };

                    context.ForumSections.Add(forumSection);
                    context.SaveChanges();
                    context.Entry(forumSection).Reload();
                }

                foreach (var forum in forumValue.Value)
                {
                    if (forumSection.Forums.All(f => f.Title != forum))
                    {
                        forumSection.Forums.Add(new Forum
                        {
                            Title = forum,
                            IsActive = true,
                            Slug = ForumsDataContext.GenerateSlug(forum),
                            ForumSectionId = forumSection.Id
                        });
                    }
                }

                context.Update(forumSection);
                context.SaveChanges();

                displayOrder++;
            }
        }

        private static ApplicationRole GetOrInsertRole(this ApplicationDbContext context, string roleName)
        {
            var role = context.Roles.FirstOrDefault(r => r.Name.Equals(roleName, StringComparison.InvariantCultureIgnoreCase));

            if (role == null)
            {
                role = new ApplicationRole
                {
                    Name = roleName,
                    NormalizedName = roleName.ToUpper()
                };

                context.Roles.Add(role);
                context.SaveChanges();
                context.Entry(role).Reload();
            }

            return role;
        }

        private static bool RoleDoesNotHaveClaim(ApplicationDbContext context, ApplicationRole adminRole, RoleClaim roleClaim)
        {
            return !context.RoleClaims.Any(rc =>
                rc.ClaimType == RoleClaimTypes.Permission && rc.ClaimValue == roleClaim.ClaimValue &&
                rc.RoleId == adminRole.Id
            );
        }
    }
}
