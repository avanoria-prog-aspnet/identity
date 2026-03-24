using Microsoft.EntityFrameworkCore;

namespace Presentation.WebApp.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{

}


