using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalks.API.Data
{
    public class NzWalksAuthDbContext : IdentityDbContext
    {
        public NzWalksAuthDbContext(DbContextOptions<NzWalksAuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var readerRoleId = "e162c4e6-17bb-4e17-965d-13aae5335d81";
            var writerRoleId = "236b9b95-01be-4bd7-92dd-e08f337fb6e2";
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id= readerRoleId,
                    ConcurrencyStamp=readerRoleId,
                    Name ="Reader",
                    NormalizedName = "READER",
                },
                new IdentityRole
                {
                    Id= writerRoleId,
                    ConcurrencyStamp=writerRoleId,
                    Name ="Writer",
                    NormalizedName = "WRITER",
                }
            };
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
