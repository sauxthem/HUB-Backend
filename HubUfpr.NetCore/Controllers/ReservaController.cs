using System;
using System.Collections.Generic;
using HubUfpr.API.Requests;
using HubUfpr.Model;
using HubUfpr.Service.Class;
using HubUfpr.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HubUfpr.API.Controllers
{
    [Produces("application/json")]
    [Route("api/reserva")]
    public class ReservaController : Controller
    {
        protected readonly IReservaService _reservaService;
        protected readonly IProdutoService _produtoService;

        public ReservaController(IReservaService reservaService, IProdutoService produtoService)
        {
            _reservaService = reservaService;
            _produtoService = produtoService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("create")]
        public JsonResult CreateReserve([FromBody] CreateReserve req)
        {
            try
            {
                if (req.IdCliente > 0 && req.IdProduto > 0 && (req.Latitude != 0 || req.Longitude != 0) &&
                    req.QuantidadeDesejada > 0)
                {
                    if (!_produtoService.IsStockAvailable(req.IdProduto, req.QuantidadeDesejada))
                    {
                        Response.StatusCode = 400;
                        return Json(new { msg = "O Produto informado não possui estoque suficiente para completar a reserva." });
                    }

                    if (Request.Headers["Authorization"].Count > 0 && Request.Headers["Authorization"].ToString().Trim().Length > 0)
                    {
                        if (!TokenService.IsTokenValidMatchCustomerId(Request.Headers["Authorization"], req.IdCliente))
                        {
                            Response.StatusCode = 401;
                            return Json(new { msg = "O token de acesso informado não é válido." });
                        }
                    }
                    else
                    {
                        Response.StatusCode = 401;
                        return Json(new { msg = "Você deve informar seu token de acesso para acessar este conteúdo." });
                    }

                    _reservaService.CreateReserve(
                        req.IdCliente, req.IdProduto, req.QuantidadeDesejada, req.Latitude, req.Longitude);
                    return Json(new { msg = "Reserva criada com sucesso." });
                }
                
                Response.StatusCode = 400;
                return Json(new { msg = "Você deve informar o ID do cliente, id do produto, quantidade, latitude e longitude." });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { msg = "Erro ao criar reserva: " + ex });
            }
        }

        [AllowAnonymous]
        [HttpPatch]
        [Route("cancel/{id}")]
        public JsonResult CancelReserve(int id)
        {
            try
            {
                if (id > 0)
                {
                    if (Request.Headers["Authorization"].Count > 0 && Request.Headers["Authorization"].ToString().Trim().Length > 0)
                    {
                        if (!TokenService.IsTokenValidMatchCustomerId(Request.Headers["Authorization"], _reservaService.GetCustomerIdFromReservation(id)))
                        {
                            Response.StatusCode = 401;
                            return Json(new { msg = "O token de acesso informado não é válido." });
                        }
                    }
                    else
                    {
                        Response.StatusCode = 401;
                        return Json(new { msg = "Você deve informar seu token de acesso para acessar este conteúdo." });
                    }

                    if (_reservaService.GetCurrentStatus(id) != 0)
                    {
                        Response.StatusCode = 409;
                        return Json(new { msg = "O Status da reserva informada não pode mais ser alterado." });
                    }

                    if (_reservaService.UpdateReserveStatus(id, 3) > 0)
                    {
                        return Json(new { msg = "Reserva cancelada com sucesso." });
                    }

                    return Json(new { msg = "Reserva não encontrada." });
                }

                Response.StatusCode = 400;
                return Json(new { msg = "Você deve informar o ID da reserva." });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { msg = "Erro ao cancelar reserva: " + ex });
            }
        }

        [AllowAnonymous]
        [HttpPatch]
        [Route("confirm/{id}")]
        public JsonResult ConfirmReserve(int id)
        {
            try
            {
                if (id > 0)
                {
                    if (Request.Headers["Authorization"].Count > 0 && Request.Headers["Authorization"].ToString().Trim().Length > 0)
                    {
                        if (!TokenService.IsTokenValidMatchSellerId(Request.Headers["Authorization"], _reservaService.GetSellerIdFromReservation(id)))
                        {
                            Response.StatusCode = 401;
                            return Json(new { msg = "O token de acesso informado não é válido." });
                        }
                    }
                    else
                    {
                        Response.StatusCode = 401;
                        return Json(new { msg = "Você deve informar seu token de acesso para acessar este conteúdo." });
                    }

                    if (_reservaService.GetCurrentStatus(id) != 0)
                    {
                        Response.StatusCode = 409;
                        return Json(new { msg = "O Status da reserva informada não pode mais ser alterado." });
                    }

                    if (_reservaService.UpdateReserveStatus(id, 1) > 0)
                    {
                        return Json(new { msg = "Reserva confirmada com sucesso." });
                    }

                    return Json(new { msg = "Reserva não encontrada." });
                }

                Response.StatusCode = 400;
                return Json(new { msg = "Você deve informar o ID da reserva." });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { msg = "Erro ao confirmar reserva: " + ex });
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("getBySeller/{id}")]
        public JsonResult GetReservaBySeller(int id)
        {
            try
            {
                if (id > 0)
                {
                    if (Request.Headers["Authorization"].Count > 0 && Request.Headers["Authorization"].ToString().Trim().Length > 0)
                    {
                        if (!TokenService.IsTokenValidMatchSellerId(Request.Headers["Authorization"], id))
                        {
                            Response.StatusCode = 401;
                            return Json(new { msg = "O token de acesso informado não é válido." });
                        }
                    }
                    else
                    {
                        Response.StatusCode = 401;
                        return Json(new { msg = "Você deve informar seu token de acesso para acessar este conteúdo." });
                    }

                    List<Reserva> reservas = _reservaService.GetReservasByVendedor(id);
                    if (reservas.Count > 0)
                    {
                        return Json(new { reservas });
                    }

                    Response.StatusCode = 404;
                    return Json(new { msg = "Nenhuma reserva encontrada." });
                }

                Response.StatusCode = 400;
                return Json(new { msg = "Você deve informar o ID do Vendedor." });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { msg = "Erro ao buscar reservas: " + ex });
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("getByCustomer/{id}")]
        public JsonResult GetReservaByCustomer(int id)
        {
            try
            {
                if (id > 0)
                {
                    if (Request.Headers["Authorization"].Count > 0 && Request.Headers["Authorization"].ToString().Trim().Length > 0)
                    {
                        if (!TokenService.IsTokenValidMatchCustomerId(Request.Headers["Authorization"], id))
                        {
                            Response.StatusCode = 401;
                            return Json(new { msg = "O token de acesso informado não é válido." });
                        }
                    }
                    else
                    {
                        Response.StatusCode = 401;
                        return Json(new { msg = "Você deve informar seu token de acesso para acessar este conteúdo." });
                    }

                    List<Reserva> reservas = _reservaService.GetReservasByCliente(id);
                    if (reservas.Count > 0)
                    {
                        return Json(new { reservas });
                    }

                    Response.StatusCode = 404;
                    return Json(new { msg = "Nenhuma reserva encontrada." });
                }

                Response.StatusCode = 400;
                return Json(new { msg = "Você deve informar o ID do Cliente." });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { msg = "Erro ao buscar reservas: " + ex });
            }
        }
    }
}
