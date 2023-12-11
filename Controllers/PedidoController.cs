using EstoqueWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SQLitePCL;

namespace EstoqueWeb.Controllers
{
    public class PedidoController : Controller 
    {
        private readonly EstoqueWebContext _context;

        public PedidoController(EstoqueWebContext context)
        {
            this._context = context;
        }
        
        public async Task<IActionResult> Index(int? cid)//Retorna listagem de Pedidos ordenando pelo nome
        {
            if(cid.HasValue)
            {
                var cliente = await _context.Clientes.FindAsync(cid);
                if(cliente != null)
                {
                    var pedidos = await _context.Pedidos
                        .Where(p=> p.IdCliente == cid)
                        .OrderByDescending(x => x.IdPedido)
                        .AsNoTracking().ToListAsync();

                    ViewBag.Cliente = cliente;
                    return View(pedidos);
                        
                }
                else
                {
                    TempData["mensagem"] = MensagemModel.Serializar("Cliente não escontrado",TipoMensagem.Erro);
                    return RedirectToAction("Index","Cliente");
                }

            }
            else
            {
                TempData["mensagem"] = MensagemModel.Serializar("Cliente não informado",TipoMensagem.Erro);
                return RedirectToAction("Index","Cliente");
            }

        }

        [HttpGet]
        public async Task<IActionResult> Cadastrar(int? cid)
        {
            if (cid.HasValue)
            {
                var cliente = await _context.Clientes.FindAsync(cid);
                if(cliente != null)
                {
                    _context.Entry(cliente).Collection(c => c.Pedidos).Load();
                    PedidoModel pedido = null;
                    if(_context.Pedidos.Any(p => p.IdCliente == cid && !p.DataPedido.HasValue))
                    {
                        pedido = await _context.Pedidos
                            .FirstOrDefaultAsync(p => p.IdCliente == cid && !p.DataPedido.HasValue);
                    }
                    else
                    {
                        pedido = new PedidoModel {IdCliente = cid.Value, ValorTotal = 0 };
                        cliente.Pedidos.Add(pedido);
                        await _context.SaveChangesAsync();
                    }
                    return RedirectToAction("Index","ItemPedido", new {ped = pedido.IdPedido});
                }
                TempData["mensagem"] = MensagemModel.Serializar("Cliente não encontrado",       TipoMensagem.Erro);
                return RedirectToAction("Index","Cliente");
            }
            TempData["mensagem"] = MensagemModel.Serializar("Cliente não informado",       TipoMensagem.Erro);
            return RedirectToAction("Index","Cliente");

        }
        private bool PedidoExiste(int id) //Método para verificar se o id existe no banco
        {
            return _context.Pedidos.Any(x => x.IdPedido == id);
        }
        [HttpPost]
        public async Task<IActionResult> Cadastrar(int? id,[FromForm]PedidoModel pedido)
        {
            if (ModelState.IsValid)
            {
                if(id.HasValue) //Se for localizado ID no banco, ou seja, Update
                {
                    if (PedidoExiste(id.Value))
                    {
                        _context.Pedidos.Update(pedido);
                        if (await _context.SaveChangesAsync() > 0)
                        {
                            TempData["mensagem"] = MensagemModel.Serializar("Pedido Alterada com Sucesso!");
                        }
                        else
                        {
                            TempData["mensagem"] = MensagemModel.Serializar("Erro ao alterar pedido",TipoMensagem.Erro);
                        }
                    }
                    else
                    {
                        TempData["mensagem"] = MensagemModel.Serializar("Pedido não encontrada!", TipoMensagem.Erro);
                    }
                }
                else //Aqui é feito o Create
                {
                    _context.Pedidos.Add(pedido);
                    if (await _context.SaveChangesAsync() > 0)
                    {
                        TempData["mensagem"] = MensagemModel.Serializar("Pedido Cadastrada com Sucesso!");
                    }
                    else
                    {
                        TempData["mensagem"] = MensagemModel.Serializar("Erro cadastrar pedido",TipoMensagem.Erro);
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(pedido);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Excluir(int? id)
        {
            if(!id.HasValue)
            {
                TempData["mensagem"] = MensagemModel.Serializar("Pedido não informada",TipoMensagem.Erro);
                return RedirectToAction(nameof(Index));                
            }

            var pedido = await _context.Pedidos.FindAsync(id);
            if(pedido == null)
            {
                TempData["mensagem"] = MensagemModel.Serializar("Pedido não encontrada",TipoMensagem.Erro);
                return RedirectToAction(nameof(Index)); 
            }
            return View(pedido);
        }
        [HttpPost]
        public async Task<IActionResult> Excluir(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if(pedido != null)
            {
                _context.Pedidos.Remove(pedido);
                if(await _context.SaveChangesAsync() > 0)
                {
                    TempData["mensagem"] = MensagemModel.Serializar("Pedido Excluída com sucesso");
                    
                }
                else
                {
                    TempData["mensagem"] = MensagemModel.Serializar("Não foi possível excluir pedido",TipoMensagem.Erro);
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["mensagem"] = MensagemModel.Serializar("Pedido não encontrada",TipoMensagem.Erro);
                return RedirectToAction(nameof(Index));
            }
        }
    }

}