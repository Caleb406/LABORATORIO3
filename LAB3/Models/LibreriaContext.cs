using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LAB3.Models;

public partial class LibreriaContext : DbContext
{
    public LibreriaContext()
    {
    }

    public LibreriaContext(DbContextOptions<LibreriaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Autor> Autors { get; set; }

    public virtual DbSet<Libro> Libros { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Autor>(entity =>
        {
            entity.HasKey(e => e.IdAutor).HasName("PK__Autor__DD33B031D14E0865");

            entity.ToTable("Autor");

            entity.Property(e => e.IdAutor).ValueGeneratedNever();
            entity.Property(e => e.Autor1)
                .HasMaxLength(100)
                .HasColumnName("Autor");
            entity.Property(e => e.NacionalidadAutor).HasMaxLength(50);

            entity.HasOne(d => d.oLibro).WithMany(p => p.Autors)
                .HasForeignKey(d => d.CodigoLibro)
                .HasConstraintName("FK__Autor__CodigoLib__46E78A0C");
        });

        modelBuilder.Entity<Libro>(entity =>
        {
            entity.HasKey(e => e.CodigoLibro).HasName("PK__Libro__854E34E0F93775D1");

            entity.ToTable("Libro");

            entity.Property(e => e.CodigoLibro).ValueGeneratedNever();
            entity.Property(e => e.Editorial).HasMaxLength(50);
            entity.Property(e => e.Isbn)
                .HasMaxLength(20)
                .HasColumnName("ISBN");
            entity.Property(e => e.Titulo).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
