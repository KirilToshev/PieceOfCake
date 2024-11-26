using Microsoft.EntityFrameworkCore;

namespace PieceOfCake.DAL;

public class PocDbContext(DbContextOptions<PocDbContext> options) : DbContext(options)
{

}
