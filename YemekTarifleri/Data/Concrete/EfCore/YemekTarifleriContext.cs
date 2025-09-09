using System;
using YemekTarifleri.Entity;
using Microsoft.EntityFrameworkCore;

namespace YemekTarifleri.Data.Concrete.EfCore;

public class YemekTarifleriContext : DbContext
{
    public YemekTarifleriContext(DbContextOptions<YemekTarifleriContext> options) : base(options)
    {

    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Food> Foods => Set<Food>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<Step> Steps => Set<Step>();
    public DbSet<Ingredient> Ingredients => Set<Ingredient>();
    public DbSet<Ftype> Ftypes => Set<Ftype>();
    public DbSet<Image> Images => Set<Image>();
    public DbSet<Like> Likes => Set<Like>();
    public DbSet<View> Views => Set<View>();
    public DbSet<Role> Roles => Set<Role>();
}

