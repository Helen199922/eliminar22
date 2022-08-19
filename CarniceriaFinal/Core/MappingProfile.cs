using AutoMapper;
using CarniceriaFinal.Cliente.Models;
using CarniceriaFinal.Core.Email.DTOs;
using CarniceriaFinal.Marketing.DTOs;
using CarniceriaFinal.ModelsEF;
using CarniceriaFinal.Productos.DTOs;
using CarniceriaFinal.Productos.Models;
using CarniceriaFinal.Roles.DTOs;
using CarniceriaFinal.Sales.DTOs;
using CarniceriaFinal.Sales.Models;
using CarniceriaFinal.User.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Core
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Ventum, SaleEntity>()
                .ForMember(d => d.status, o => o.MapFrom(x => x.IdStatus));
            CreateMap<SaleEntity, Ventum>()
                .ForMember(d => d.IdStatus, o => o.MapFrom( x => x.status));
            CreateMap<VentaStatus, SalesStatus>();
            CreateMap<DetalleVentum, SaleDetailEntity>();

            CreateMap<Persona, PersonEntity>();
            CreateMap<PruebaPerson, Persona>();
            CreateMap<PersonEntity, Persona>();

            CreateMap<UserEntity, Usuario>();
            CreateMap<Usuario, UserEntity>();

            //Manejar las categorias
            CreateMap<CategoriaProducto, CategoriaProductoEntity>();
            CreateMap<CategoriaProducto, CategoryEntity>();
            CreateMap<CategoryEntity, CategoriaProducto>();
            CreateMap<CategoriaProductoEntity, CategoriaProducto>();
            CreateMap<CategoryAdminEntity, CategoriaProducto>();
            CreateMap<CategoriaProducto, CategoryAdminEntity>();

            //Manejar las sub-categorias
            CreateMap<SubCategorium, SubCategoriaProductoEntity>();
            CreateMap<SubCategoriaProductoEntity, SubCategorium>();
            CreateMap<SubCategorium, CreateSubCategory>();
            CreateMap<CreateSubCategory, SubCategorium>();
            CreateMap<SubCategorium, SubCategoriaAdminEntity>();
            CreateMap<SubCategoriaAdminEntity, SubCategorium>();
            
            

            //ProductEntity
            CreateMap<Producto, ProductEntity>();
            CreateMap<ProductEntity, Producto>();
            CreateMap<Producto, ProductTableAdminEntity>();
            

            //Producto Response
            CreateMap<Promocion, PromotionSimpleEntity>();
            CreateMap<PromotionSimpleEntity, Promocion>();
            CreateMap<PromotionEntity, Promocion>();
            CreateMap<Promocion, PromotionEntity>();
            CreateMap<UnidadMedidum, MeasureUnitEntity>();
            CreateMap<DetalleProducto, ProductDetailEntity>();
            CreateMap<ProductDetailEntity, DetalleProducto>();

            //Marketing
            CreateMap<Comunicacion, CommunicationEntity>();
            CreateMap<CommunicationEntity, Comunicacion>();
            CreateMap<TipoComunicacion, TypeCommunicationEntity>();
            CreateMap<TypeCommunicationEntity, TipoComunicacion>();

            //Roles
            CreateMap<Rol, RolEntity>();
            CreateMap<RolEntity, Rol>();

            //Login

            //Venta - Información de banco
            CreateMap<InfoBancarium, AccountsEntity>()
                .ForMember(d => d.bankName, o => o.MapFrom(x => x.NombreBanco))
                .ForMember(d => d.numAccount, o => o.MapFrom(x => x.NumBanco))
                .ForMember(d => d.typeAccount, o => o.MapFrom(x => x.TipoBanco));

            //Productos en categorias
            CreateMap<SimpleProductInSubCategory, Producto>();
            CreateMap<Producto, SimpleProductInSubCategory>();

            //EMAIL Promociones
            CreateMap<CorreoPromocion, EmailEntity>();
            CreateMap<EmailEntity, CorreoPromocion>();
            CreateMap<EmailCreateEntity, CorreoPromocion>();
            CreateMap<CorreoPromocion, EmailCreateEntity>();

            //Membership
            CreateMap<MembershipUserEntity, MembresiaInUsuario>();
            CreateMap<MembresiaInUsuario, MembershipUserEntity>();
        }
    }
}
