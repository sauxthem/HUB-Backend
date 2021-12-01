﻿using System;
using HubUfpr.Model;
using System.Collections.Generic;

namespace HubUfpr.Data.DapperORM.Interface
{
    public interface IVendedorRepository
    {
        Vendedor getVendedorById(int id);

        List<Vendedor> getVendedoresByName(string name);

        List<Vendedor> getVendedoresByLocation(float lat, float lon);

        List<Vendedor> getAllSellers();

        int AddFavoriteSeller(int idVendedor, int idCliente);

        int RemoveFavoriteSeller(int idVendedor, int idCliente);

        List<Vendedor> GetFavorteSellersByCustomer(int idCliente);

        bool IsVendedorInCustomerFavorites(int idCliente, int idVendedor);
    }
}
