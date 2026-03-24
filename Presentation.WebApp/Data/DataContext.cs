using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Presentation.WebApp.Identity;

namespace Presentation.WebApp.Data;

public class DataContext(DbContextOptions<DataContext> options) 
    : IdentityDbContext<ApplicationUser>(options)
{

}


