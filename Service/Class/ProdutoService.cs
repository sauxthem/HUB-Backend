﻿using System.Collections.Generic;
using HubUfpr.Data.DapperORM.Interface;
using HubUfpr.Model;
using HubUfpr.Service.Interface;

namespace HubUfpr.Service.Class
{
    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoService(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public List<User> GetUserList()
        {
            throw new System.NotImplementedException();
        }

        public void InsertProduto(
            string nome,
            bool status,
            float preco,
            string descricao,
            int quantidadeDisponivel,
            int idVendedor,
            string imagem
        )
        {
            _produtoRepository.InsertProduct (
                nome,
                status,
                preco,
                descricao,
                quantidadeDisponivel,
                idVendedor,
                imagem
            );
        }

        public List<Produto> SearchProduto(string nome, int idProduto, int idVendedor)
        {
            return _produtoRepository.SearchProduct(nome, idProduto, idVendedor);
        }

        public int DeleteProduto(int idProduto)
        {
            return _produtoRepository.DeleteProduto (idProduto);
        }

        public int UpdateProduto(
            int idProduto,
            string nome,
            bool status,
            float preco,
            string descricao,
            int quantidadeDisponivel,
            string image
        )
        {
            return _produtoRepository.UpdateProduto(
                idProduto,
                nome,
                status,
                preco,
                descricao,
                quantidadeDisponivel,
                image
            );
        }

        public int UpdateScore(int productId, float score)
        {
            return _produtoRepository.UpdateScore(productId, score);
        }
    }
}
