using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CarniceriaFinal.ModelsEF
{
    public partial class DBContext : DbContext
    {
        public DBContext()
        {
        }

        public DBContext(DbContextOptions<DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CategoriaProducto> CategoriaProductos { get; set; } = null!;
        public virtual DbSet<Ciudad> Ciudads { get; set; } = null!;
        public virtual DbSet<Cliente> Clientes { get; set; } = null!;
        public virtual DbSet<Comunicacion> Comunicacions { get; set; } = null!;
        public virtual DbSet<CorreoPromocion> CorreoPromocions { get; set; } = null!;
        public virtual DbSet<CorreoPromocionInUser> CorreoPromocionInUsers { get; set; } = null!;
        public virtual DbSet<DetalleProducto> DetalleProductos { get; set; } = null!;
        public virtual DbSet<DetalleVentum> DetalleVenta { get; set; } = null!;
        public virtual DbSet<Endpoint> Endpoints { get; set; } = null!;
        public virtual DbSet<EventoEspecial> EventoEspecials { get; set; } = null!;
        public virtual DbSet<FormaPago> FormaPagos { get; set; } = null!;
        public virtual DbSet<Impuesto> Impuestos { get; set; } = null!;
        public virtual DbSet<InfoBancarium> InfoBancaria { get; set; } = null!;
        public virtual DbSet<Log> Logs { get; set; } = null!;
        public virtual DbSet<MembresiaInUsuario> MembresiaInUsuarios { get; set; } = null!;
        public virtual DbSet<Membresium> Membresia { get; set; } = null!;
        public virtual DbSet<MetodosHttp> MetodosHttps { get; set; } = null!;
        public virtual DbSet<Modulo> Modulos { get; set; } = null!;
        public virtual DbSet<ModuloInOpcion> ModuloInOpcions { get; set; } = null!;
        public virtual DbSet<MomentoDegustacion> MomentoDegustacions { get; set; } = null!;
        public virtual DbSet<MomentoDegustacionInProducto> MomentoDegustacionInProductos { get; set; } = null!;
        public virtual DbSet<Opcion> Opcions { get; set; } = null!;
        public virtual DbSet<OptionInEndpoint> OptionInEndpoints { get; set; } = null!;
        public virtual DbSet<Persona> Personas { get; set; } = null!;
        public virtual DbSet<PreparacionProducto> PreparacionProductos { get; set; } = null!;
        public virtual DbSet<PreparacionProductoInProducto> PreparacionProductoInProductos { get; set; } = null!;
        public virtual DbSet<Producto> Productos { get; set; } = null!;
        public virtual DbSet<Promocion> Promocions { get; set; } = null!;
        public virtual DbSet<PromocionInProducto> PromocionInProductos { get; set; } = null!;
        public virtual DbSet<Provincium> Provincia { get; set; } = null!;
        public virtual DbSet<Rol> Rols { get; set; } = null!;
        public virtual DbSet<RolInOpcion> RolInOpcions { get; set; } = null!;
        public virtual DbSet<Sexo> Sexos { get; set; } = null!;
        public virtual DbSet<StatusEmail> StatusEmails { get; set; } = null!;
        public virtual DbSet<SubCategorium> SubCategoria { get; set; } = null!;
        public virtual DbSet<SubInCategorium> SubInCategoria { get; set; } = null!;
        public virtual DbSet<TipoComunicacion> TipoComunicacions { get; set; } = null!;
        public virtual DbSet<TipoUsuario> TipoUsuarios { get; set; } = null!;
        public virtual DbSet<UnidadMedidum> UnidadMedida { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;
        public virtual DbSet<VentaStatus> VentaStatuses { get; set; } = null!;
        public virtual DbSet<Ventum> Venta { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=carniceria-zamorano-db.mysql.database.azure.com;userid=zamoranoservidor;password=12345678#!C0rn3;database=carniceria", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.28-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8_spanish2_ci")
                .HasCharSet("utf8");

            modelBuilder.Entity<CategoriaProducto>(entity =>
            {
                entity.HasKey(e => e.IdCategoria)
                    .HasName("PRIMARY");
            });

            modelBuilder.Entity<Ciudad>(entity =>
            {
                entity.HasKey(e => e.IdCiudad)
                    .HasName("PRIMARY");

                entity.HasOne(d => d.IdProvinciaNavigation)
                    .WithMany(p => p.Ciudads)
                    .HasForeignKey(d => d.IdProvincia)
                    .HasConstraintName("provincia_provincia");
            });

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.IdCliente)
                    .HasName("PRIMARY");

                entity.HasOne(d => d.IdCiudadNavigation)
                    .WithMany(p => p.Clientes)
                    .HasForeignKey(d => d.IdCiudad)
                    .HasConstraintName("ciudad_ciudad");

                entity.HasOne(d => d.IdPersonaNavigation)
                    .WithMany(p => p.Clientes)
                    .HasForeignKey(d => d.IdPersona)
                    .HasConstraintName("persona_persona");
            });

            modelBuilder.Entity<Comunicacion>(entity =>
            {
                entity.HasKey(e => e.IdComunicacion)
                    .HasName("PRIMARY");

                entity.HasOne(d => d.IdTipoComunicacionNavigation)
                    .WithMany(p => p.Comunicacions)
                    .HasForeignKey(d => d.IdTipoComunicacion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("comunicacion_ibfk_1");
            });

            modelBuilder.Entity<CorreoPromocion>(entity =>
            {
                entity.HasKey(e => e.IdCorreo)
                    .HasName("PRIMARY");

                entity.HasOne(d => d.IdPromocionNavigation)
                    .WithMany(p => p.CorreoPromocions)
                    .HasForeignKey(d => d.IdPromocion)
                    .HasConstraintName("correo_promocion_ibfk_1");
            });

            modelBuilder.Entity<CorreoPromocionInUser>(entity =>
            {
                entity.HasKey(e => e.IdCorreoPromocionInUser)
                    .HasName("PRIMARY");

                entity.HasOne(d => d.IdCorreoPromocionNavigation)
                    .WithMany(p => p.CorreoPromocionInUsers)
                    .HasForeignKey(d => d.IdCorreoPromocion)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("correo_promocion_in_user_ibfk_3");

                entity.HasOne(d => d.IdEstatusEmailNavigation)
                    .WithMany(p => p.CorreoPromocionInUsers)
                    .HasForeignKey(d => d.IdEstatusEmail)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("correo_promocion_in_user_ibfk_2");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.CorreoPromocionInUsers)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("correo_promocion_in_user_ibfk_1");
            });

            modelBuilder.Entity<DetalleProducto>(entity =>
            {
                entity.HasKey(e => e.IdDetalleProducto)
                    .HasName("PRIMARY");

                entity.HasOne(d => d.IdProductoNavigation)
                    .WithMany(p => p.DetalleProductos)
                    .HasForeignKey(d => d.IdProducto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idDetalleProducto");
            });

            modelBuilder.Entity<DetalleVentum>(entity =>
            {
                entity.HasKey(e => e.IdDetalleVenta)
                    .HasName("PRIMARY");

                entity.HasOne(d => d.IdMembresiaInUsuarioNavigation)
                    .WithMany(p => p.DetalleVenta)
                    .HasForeignKey(d => d.IdMembresiaInUsuario)
                    .HasConstraintName("detalle_venta_ibfk_2");

                entity.HasOne(d => d.IdProductoNavigation)
                    .WithMany(p => p.DetalleVenta)
                    .HasForeignKey(d => d.IdProducto)
                    .HasConstraintName("detalle_venta_ibfk_1");

                entity.HasOne(d => d.IdPromocionNavigation)
                    .WithMany(p => p.DetalleVenta)
                    .HasForeignKey(d => d.IdPromocion)
                    .HasConstraintName("promocion_promocion");

                entity.HasOne(d => d.IdVentaNavigation)
                    .WithMany(p => p.DetalleVenta)
                    .HasForeignKey(d => d.IdVenta)
                    .HasConstraintName("venta_venta");
            });

            modelBuilder.Entity<Endpoint>(entity =>
            {
                entity.HasKey(e => e.IdEndPoint)
                    .HasName("PRIMARY");

                entity.HasOne(d => d.IdMetodoNavigation)
                    .WithMany(p => p.Endpoints)
                    .HasForeignKey(d => d.IdMetodo)
                    .HasConstraintName("endpoints_ibfk_1");
            });

            modelBuilder.Entity<EventoEspecial>(entity =>
            {
                entity.HasKey(e => e.IdEventoEspecial)
                    .HasName("PRIMARY");
            });

            modelBuilder.Entity<FormaPago>(entity =>
            {
                entity.HasKey(e => e.IdFormaPago)
                    .HasName("PRIMARY");
            });

            modelBuilder.Entity<Impuesto>(entity =>
            {
                entity.HasKey(e => e.IdImpuesto)
                    .HasName("PRIMARY");
            });

            modelBuilder.Entity<InfoBancarium>(entity =>
            {
                entity.HasKey(e => e.IdBanco)
                    .HasName("PRIMARY");
            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.HasKey(e => e.IdLog)
                    .HasName("PRIMARY");
            });

            modelBuilder.Entity<MembresiaInUsuario>(entity =>
            {
                entity.HasKey(e => e.IdMembresiaInUsuario)
                    .HasName("PRIMARY");

                entity.HasOne(d => d.IdMembresiaNavigation)
                    .WithMany(p => p.MembresiaInUsuarios)
                    .HasForeignKey(d => d.IdMembresia)
                    .HasConstraintName("membresia_in_usuario_ibfk_1");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.MembresiaInUsuarios)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("membresia_in_usuario_ibfk_2");
            });

            modelBuilder.Entity<Membresium>(entity =>
            {
                entity.HasKey(e => e.IdMembresia)
                    .HasName("PRIMARY");
            });

            modelBuilder.Entity<MetodosHttp>(entity =>
            {
                entity.HasKey(e => e.IdMetodo)
                    .HasName("PRIMARY");
            });

            modelBuilder.Entity<Modulo>(entity =>
            {
                entity.HasKey(e => e.IdModulo)
                    .HasName("PRIMARY");
            });

            modelBuilder.Entity<ModuloInOpcion>(entity =>
            {
                entity.HasKey(e => new { e.IdModulo, e.IdOpcion })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.HasOne(d => d.IdModuloNavigation)
                    .WithMany(p => p.ModuloInOpcions)
                    .HasForeignKey(d => d.IdModulo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("modulo_in_opcion_ibfk_1");

                entity.HasOne(d => d.IdOpcionNavigation)
                    .WithMany(p => p.ModuloInOpcions)
                    .HasForeignKey(d => d.IdOpcion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("modulo_in_opcion_ibfk_2");
            });

            modelBuilder.Entity<MomentoDegustacion>(entity =>
            {
                entity.HasKey(e => e.IdMomentoDegustacion)
                    .HasName("PRIMARY");
            });

            modelBuilder.Entity<MomentoDegustacionInProducto>(entity =>
            {
                entity.HasKey(e => e.IdMomentoDegustacionInProducto)
                    .HasName("PRIMARY");

                entity.HasOne(d => d.IdMomentoDegustacionNavigation)
                    .WithMany(p => p.MomentoDegustacionInProductos)
                    .HasForeignKey(d => d.IdMomentoDegustacion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("momento_degustacion_in_producto_ibfk_1");

                entity.HasOne(d => d.IdProductoNavigation)
                    .WithMany(p => p.MomentoDegustacionInProductos)
                    .HasForeignKey(d => d.IdProducto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("momento_degustacion_in_producto_ibfk_2");
            });

            modelBuilder.Entity<Opcion>(entity =>
            {
                entity.HasKey(e => e.IdOpcion)
                    .HasName("PRIMARY");
            });

            modelBuilder.Entity<OptionInEndpoint>(entity =>
            {
                entity.HasKey(e => new { e.IdOption, e.IdEndPoint })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.HasOne(d => d.IdEndPointNavigation)
                    .WithMany(p => p.OptionInEndpoints)
                    .HasForeignKey(d => d.IdEndPoint)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("option_in_endpoint_ibfk_2");

                entity.HasOne(d => d.IdOptionNavigation)
                    .WithMany(p => p.OptionInEndpoints)
                    .HasForeignKey(d => d.IdOption)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("option_in_endpoint_ibfk_1");
            });

            modelBuilder.Entity<Persona>(entity =>
            {
                entity.HasKey(e => e.IdPersona)
                    .HasName("PRIMARY");

                entity.HasOne(d => d.IdCiudadNavigation)
                    .WithMany(p => p.Personas)
                    .HasForeignKey(d => d.IdCiudad)
                    .HasConstraintName("persona_ibfk_2");

                entity.HasOne(d => d.IdSexoNavigation)
                    .WithMany(p => p.Personas)
                    .HasForeignKey(d => d.IdSexo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("persona_ibfk_1");
            });

            modelBuilder.Entity<PreparacionProducto>(entity =>
            {
                entity.HasKey(e => e.IdPreparacionProducto)
                    .HasName("PRIMARY");
            });

            modelBuilder.Entity<PreparacionProductoInProducto>(entity =>
            {
                entity.HasKey(e => e.IdPreparacionInProducto)
                    .HasName("PRIMARY");

                entity.HasOne(d => d.IdPreparacionProductoNavigation)
                    .WithMany(p => p.PreparacionProductoInProductos)
                    .HasForeignKey(d => d.IdPreparacionProducto)
                    .HasConstraintName("preparacion_producto_in_producto_ibfk_1");

                entity.HasOne(d => d.IdProductoNavigation)
                    .WithMany(p => p.PreparacionProductoInProductos)
                    .HasForeignKey(d => d.IdProducto)
                    .HasConstraintName("preparacion_producto_in_producto_ibfk_2");
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(e => e.IdProducto)
                    .HasName("PRIMARY");

                entity.HasOne(d => d.IdUnidadNavigation)
                    .WithMany(p => p.Productos)
                    .HasForeignKey(d => d.IdUnidad)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("unidad_unidad");
            });

            modelBuilder.Entity<Promocion>(entity =>
            {
                entity.HasKey(e => e.IdPromocion)
                    .HasName("PRIMARY");
            });

            modelBuilder.Entity<PromocionInProducto>(entity =>
            {
                entity.HasKey(e => e.IdPromocionInProducto)
                    .HasName("PRIMARY");

                entity.HasOne(d => d.IdProductoNavigation)
                    .WithMany(p => p.PromocionInProductos)
                    .HasForeignKey(d => d.IdProducto)
                    .HasConstraintName("promocion_in_producto_ibfk_2");

                entity.HasOne(d => d.IdPromocionNavigation)
                    .WithMany(p => p.PromocionInProductos)
                    .HasForeignKey(d => d.IdPromocion)
                    .HasConstraintName("promocion_in_producto_ibfk_1");
            });

            modelBuilder.Entity<Provincium>(entity =>
            {
                entity.HasKey(e => e.IdProvincia)
                    .HasName("PRIMARY");
            });

            modelBuilder.Entity<Rol>(entity =>
            {
                entity.HasKey(e => e.IdRol)
                    .HasName("PRIMARY");
            });

            modelBuilder.Entity<RolInOpcion>(entity =>
            {
                entity.HasKey(e => new { e.IdRol, e.IdOpcion })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.HasOne(d => d.IdOpcionNavigation)
                    .WithMany(p => p.RolInOpcions)
                    .HasForeignKey(d => d.IdOpcion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("rol_in_opcion_ibfk_1");

                entity.HasOne(d => d.IdRolNavigation)
                    .WithMany(p => p.RolInOpcions)
                    .HasForeignKey(d => d.IdRol)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("rol_rol");
            });

            modelBuilder.Entity<Sexo>(entity =>
            {
                entity.HasKey(e => e.IdSexo)
                    .HasName("PRIMARY");
            });

            modelBuilder.Entity<StatusEmail>(entity =>
            {
                entity.HasKey(e => e.IdEstatusEmail)
                    .HasName("PRIMARY");

                entity.Property(e => e.IdEstatusEmail).ValueGeneratedNever();
            });

            modelBuilder.Entity<SubCategorium>(entity =>
            {
                entity.HasKey(e => e.IdSubCategoria)
                    .HasName("PRIMARY");
            });

            modelBuilder.Entity<SubInCategorium>(entity =>
            {
                entity.HasKey(e => e.IdSubInCategory)
                    .HasName("PRIMARY");

                entity.HasOne(d => d.IdCategoriaNavigation)
                    .WithMany(p => p.SubInCategoria)
                    .HasForeignKey(d => d.IdCategoria)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("sub_in_categoria_ibfk_3");

                entity.HasOne(d => d.IdProductoNavigation)
                    .WithMany(p => p.SubInCategoria)
                    .HasForeignKey(d => d.IdProducto)
                    .HasConstraintName("sub_in_categoria_ibfk_1");

                entity.HasOne(d => d.IdSubCategoriaNavigation)
                    .WithMany(p => p.SubInCategoria)
                    .HasForeignKey(d => d.IdSubCategoria)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("sub_in_categoria_ibfk_2");
            });

            modelBuilder.Entity<TipoComunicacion>(entity =>
            {
                entity.HasKey(e => e.IdTipoComunicacion)
                    .HasName("PRIMARY");
            });

            modelBuilder.Entity<TipoUsuario>(entity =>
            {
                entity.HasKey(e => e.IdTipoUsuario)
                    .HasName("PRIMARY");
            });

            modelBuilder.Entity<UnidadMedidum>(entity =>
            {
                entity.HasKey(e => e.IdUnidad)
                    .HasName("PRIMARY");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario)
                    .HasName("PRIMARY");

                entity.HasOne(d => d.IdPersonaNavigation)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.IdPersona)
                    .HasConstraintName("personaUSR_personaUSR");

                entity.HasOne(d => d.IdRolNavigation)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.IdRol)
                    .HasConstraintName("rolUSR_rolUSR");
            });

            modelBuilder.Entity<VentaStatus>(entity =>
            {
                entity.HasKey(e => e.IdVentaStatus)
                    .HasName("PRIMARY");
            });

            modelBuilder.Entity<Ventum>(entity =>
            {
                entity.HasKey(e => e.IdVenta)
                    .HasName("PRIMARY");

                entity.HasOne(d => d.IdCiudadNavigation)
                    .WithMany(p => p.Venta)
                    .HasForeignKey(d => d.IdCiudad)
                    .HasConstraintName("venta_ibfk_2");

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.Venta)
                    .HasForeignKey(d => d.IdCliente)
                    .HasConstraintName("cliente_cliente");

                entity.HasOne(d => d.IdFormaPagoNavigation)
                    .WithMany(p => p.Venta)
                    .HasForeignKey(d => d.IdFormaPago)
                    .HasConstraintName("venta_ibfk_1");

                entity.HasOne(d => d.IdImpuestoNavigation)
                    .WithMany(p => p.Venta)
                    .HasForeignKey(d => d.IdImpuesto)
                    .HasConstraintName("impuesto_impuesto");

                entity.HasOne(d => d.IdStatusNavigation)
                    .WithMany(p => p.Venta)
                    .HasForeignKey(d => d.IdStatus)
                    .HasConstraintName("venta_ibfk_3");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
