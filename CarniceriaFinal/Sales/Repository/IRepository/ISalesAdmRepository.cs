﻿using CarniceriaFinal.ModelsEF;
using CarniceriaFinal.Sales.DTOs;

namespace CarniceriaFinal.Sales.Repository.IRepository
{
    public interface ISalesAdmRepository
    {
        Task<List<SalesAdmEntity>> GetAllSales(int idStatus);
        Task<List<DetailSaleAdmEntity>> GetDetailByIdSale(int idSale);
        Task<Producto> GetInfoProductByIdProduct(int idProduct);
        Task<Ventum> GetSalesById(int idSale);
        Task<int> GetUserIdByIdSale(int idSale);
        Task<List<VentaStatus>> GetSalesStatusAdm();
        Task<Ventum> attendSaleByIdSale(int idSale, int idUser);
        Task<Boolean> declineSaleByIdSale(int idSale);
        Task<Boolean> pendingSaleByIdSale(int idSale);
        Task<Ventum> GetSaleDetailById(int idSale);
        Task<Boolean> IsValidExpandTimeSaleDetailById(int idSale);
        Task<Ventum> ExpandTimeSaleDetailById(int idSale);
    }
}
