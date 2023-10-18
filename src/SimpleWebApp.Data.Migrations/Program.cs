using Microsoft.EntityFrameworkCore;
using SimpleWebApp.Data.Migrations;

await using var context = new SimpleDBContextFactory().CreateDbContext(args);
await context.Database.MigrateAsync();