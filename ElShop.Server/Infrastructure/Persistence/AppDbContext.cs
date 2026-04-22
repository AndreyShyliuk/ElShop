using ElShop.Server.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ElShop.Server.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IAppDbContext;
