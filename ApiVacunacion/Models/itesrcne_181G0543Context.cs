using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ApiVacunacion.Models
{
    public partial class itesrcne_181G0543Context : DbContext
    {
        public itesrcne_181G0543Context()
        {
        }

        public itesrcne_181G0543Context(DbContextOptions<itesrcne_181G0543Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Agenda> Agenda { get; set; }
        public virtual DbSet<Alumno> Alumno { get; set; }
        public virtual DbSet<Persona> Persona { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=204.93.216.11;user=itesrcne_julio;password=181G0543;database=itesrcne_181G0543", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.3.29-mariadb"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8");

            modelBuilder.Entity<Agenda>(entity =>
            {
                entity.ToTable("agenda");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Actividad)
                    .IsRequired()
                    .HasMaxLength(45);

                entity.Property(e => e.Eliminado).HasColumnType("bit(1)");

                entity.Property(e => e.FechaDeRealizar)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.Importancia)
                    .IsRequired()
                    .HasMaxLength(45);

                entity.Property(e => e.Timestamp).HasColumnType("datetime");
            });

            modelBuilder.Entity<Alumno>(entity =>
            {
                entity.ToTable("alumno");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Area)
                    .IsRequired()
                    .HasMaxLength(45);

                entity.Property(e => e.Edad)
                    .IsRequired()
                    .HasMaxLength(45);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(45);

                entity.Property(e => e.Peso).HasColumnType("int(11)");
            });

            modelBuilder.Entity<Persona>(entity =>
            {
                entity.ToTable("persona");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Edad)
                    .IsRequired()
                    .HasMaxLength(45);

                entity.Property(e => e.Eliminado).HasColumnType("bit(1)");

                entity.Property(e => e.Lote)
                    .IsRequired()
                    .HasMaxLength(45);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(45);

                entity.Property(e => e.Sexo)
                    .IsRequired()
                    .HasMaxLength(45);

                entity.Property(e => e.Timestamp).HasColumnType("datetime");

                entity.Property(e => e.Vacuna)
                    .IsRequired()
                    .HasMaxLength(45);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("usuario");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Contraseña)
                    .IsRequired()
                    .HasMaxLength(45);

                entity.Property(e => e.NombreUsuario)
                    .IsRequired()
                    .HasMaxLength(45);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
