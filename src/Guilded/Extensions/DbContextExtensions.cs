using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Linq;

namespace Guilded.Extensions
{
    public static class DbContextExtensions
    {
        public static bool AllMigrationsAreApplied(this DbContext context)
        {
            var appliedMigrations = context.GetService<IHistoryRepository>()
                .GetAppliedMigrations()
                .Select(m => m.MigrationId);

            var totalMigrations = context.GetService<IMigrationsAssembly>()
                .Migrations
                .Select(m => m.Key);

            return !totalMigrations.Except(appliedMigrations).Any();
        }
    }
}
