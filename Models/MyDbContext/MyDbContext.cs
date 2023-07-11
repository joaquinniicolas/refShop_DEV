using Microsoft.EntityFrameworkCore;
using refShop_DEV.Models.Login;
using refShop_DEV.Models.Permission;
using refShop_DEV.Models.Restaurant;
using System.Collections.Generic;
using System.Reflection.Emit;
using User = refShop_DEV.Models.Login.User;

namespace refShop_DEV.Models.MyDbContext
{


    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Promocion> Promociones { get; set; }
        public DbSet<Plato> Platos { get; set; }
        public DbSet<Mesa> Mesa { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<RolePermissions> RolePermissions { get; set; }
        public DbSet<Permissions> Permissions { get; set; }

        public DbSet<Turno> Turnos { get; set; }
        public DbSet<RegistroActividad> RegistrosActividad { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.UserRole)
                .WithMany(ur => ur.Users)
                .HasForeignKey(u => u.UserRoleId)
                .OnDelete(DeleteBehavior.Cascade);

            

            modelBuilder.Entity<User>()
                .HasOne(u => u.Turno)
                .WithOne() // No se especifica la propiedad de navegación en Turno
                .HasForeignKey<Turno>(t => t.IDEmpleado) // Clave foránea en Turno que referencia a User
                .IsRequired(false) // Permite que la relación sea opcional si IDEmpleado en Turno admite valores nulos
                .OnDelete(DeleteBehavior.Restrict); // Opcional, según el comportamiento deseado

            modelBuilder.Entity<User>()
              .HasOne(u => u.Turno)
              .WithOne() // No se especifica la propiedad de navegación en Turno
              .HasForeignKey<Turno>(t => t.CreadoPor) // Clave foránea en Turno que referencia a User
              .IsRequired(false) // Permite que la relación sea opcional si IDEmpleado en Turno admite valores nulos
              .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Mesa>()
                .HasOne(m => m.CreadoPorNavigation)
                .WithMany(u => u.MesasCreadas)
                .HasForeignKey(m => m.CreadoPor)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Mesa>()
                .HasOne(m => m.ModificadoPorNavigation)
                .WithMany(u => u.MesasModificadas)
                .HasForeignKey(m => m.ModificadoPor)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Mesa>()
                .HasOne(m => m.IdUsuarioNavigation)
                .WithMany(u => u.MesasAsociadas)
                .HasForeignKey(m => m.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Mesa>()
                .HasOne(m => m.IdMozoNavigation)
                .WithMany(u => u.MesasMozo)
                .HasForeignKey(m => m.IdMozo)
                .OnDelete(DeleteBehavior.Restrict);


            // PARA LOS PERMISOS
            modelBuilder.Entity<RolePermissions>()
                 .HasKey(rp => new { rp.RoleId, rp.PermissionId });

            modelBuilder.Entity<RolePermissions>()
                .HasOne(rp => rp.UserRole)
                .WithMany(ur => ur.RolePermissions)
                .HasForeignKey(rp => rp.RoleId);

            modelBuilder.Entity<RolePermissions>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId);

            // PARA EL TURNO

            modelBuilder.Entity<Turno>()
                .HasKey(t => t.IDTurno);

            // PARA EL REGISTRO DE ACTIVIDAD

            modelBuilder.Entity<RegistroActividad>()
            .HasKey(r => r.ID_Registro);

            modelBuilder.Entity<RegistroActividad>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.ID_Empleado)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RegistroActividad>()
                .HasOne(r => r.Turno)
                .WithMany()
                .HasForeignKey(r => r.IDTurno)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RegistroActividad>()
                .Property(r => r.Horas_Extras)
                .HasColumnType("decimal(10, 2)");

            modelBuilder.Entity<RegistroActividad>()
                .Property(r => r.Horas_Faltantes)
                .HasColumnType("decimal(10, 2)");




            //// Configuración explícita para la relación inversa de MesasAsociadas
            //modelBuilder.Entity<User>()
            //    .HasMany(u => u.MesasAsociadas)
            //    .WithOne(m => m.IdUsuarioNavigation)
            //    .HasForeignKey(m => m.IdUsuario)
            //    .OnDelete(DeleteBehavior.Restrict);

            //base.OnModelCreating(modelBuilder);
        }

    }

}
