using EstoqueWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SQLitePCL;

namespace EstoqueWeb.Controllers
{
    public class ClienteController : Controller 
    {
        private readonly EstoqueWebContext _context;

        public ClienteController(EstoqueWebContext context)
        {
            this._context = context;
        }
        
        public async Task<IActionResult> Index()//Retorna listagem de Clientes ordenando pelo nome
        {
            return View(await _context.Clientes.OrderBy(x => x.Nome).
            AsNoTracking().ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Cadastrar(int? id)
        {
            if (id.HasValue)
            {
                var cliente = await _context.Clientes.FindAsync(id);
                if (cliente == null)
                {
                    TempData["mensagem"] = MensagemModel.Serializar("Cliente não encontrado!",TipoMensagem.Erro);
                    return RedirectToAction("Index");
                }
                return View(cliente);
            }
            return View(new ClienteModel());
        }
        private bool ClienteExiste(int id) //Método para verificar se o id existe no banco
        {
            return _context.Clientes.Any(x => x.IdUsuario == id);
        }
        [HttpPost]
        public async Task<IActionResult> Cadastrar(int? id,[FromForm]ClienteModel cliente)
        {
            if (ModelState.IsValid)
            {
                if(id.HasValue) //Se for localizado ID no banco, ou seja, Update
                {
                    if (ClienteExiste(id.Value))
                    {
                        _context.Clientes.Update(cliente);
                        _context.Entry(cliente).Property(x => x.Senha).IsModified = false;
                        if (await _context.SaveChangesAsync() > 0)
                        {
                            TempData["mensagem"] = MensagemModel.Serializar("Cliente Alterado com Sucesso!");
                        }
                        else
                        {
                            TempData["mensagem"] = MensagemModel.Serializar("Erro ao alterar cliente",TipoMensagem.Erro);
                        }
                    }
                    else
                    {
                        TempData["mensagem"] = MensagemModel.Serializar("Cliente não encontrado!", TipoMensagem.Erro);
                    }
                }
                else //Aqui é feito o Create
                {
                    _context.Clientes.Add(cliente);
                    if (await _context.SaveChangesAsync() > 0)
                    {
                        TempData["mensagem"] = MensagemModel.Serializar("Cliente Cadastrado com Sucesso!");
                    }
                    else
                    {
                        TempData["mensagem"] = MensagemModel.Serializar("Erro cadastrar cliente",TipoMensagem.Erro);
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(cliente);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Excluir(int? id)
        {
            if(!id.HasValue)
            {
                TempData["mensagem"] = MensagemModel.Serializar("Cliente não informado",TipoMensagem.Erro);
                return RedirectToAction(nameof(Index));                
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if(cliente == null)
            {
                TempData["mensagem"] = MensagemModel.Serializar("Cliente não encontrado",TipoMensagem.Erro);
                return RedirectToAction(nameof(Index)); 
            }
            return View(cliente);
        }
        [HttpPost]
        public async Task<IActionResult> Excluir(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if(cliente != null)
            {
                _context.Clientes.Remove(cliente);
                if(await _context.SaveChangesAsync() > 0)
                {
                    TempData["mensagem"] = MensagemModel.Serializar("Cliente Excluído com sucesso");
                    
                }
                else
                {
                    TempData["mensagem"] = MensagemModel.Serializar("Não foi possível excluir cliente",TipoMensagem.Erro);
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["mensagem"] = MensagemModel.Serializar("Cliente não encontrado",TipoMensagem.Erro);
                return RedirectToAction(nameof(Index));
            }
        }
    }

}