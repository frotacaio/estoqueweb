using EstoqueWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SQLitePCL;

namespace EstoqueWeb.Controllers
{
    public class CategoriaController : Controller 
    {
        private readonly EstoqueWebContext _context;

        public CategoriaController(EstoqueWebContext context)
        {
            this._context = context;
        }
        
        public async Task<IActionResult> Index()//Retorna listagem de Categorias ordenando pelo nome
        {
            return View(await _context.Categorias.OrderBy(x => x.Nome).
            AsNoTracking().ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Cadastrar(int? id)
        {
            if (id.HasValue)
            {
                var categoria = await _context.Categorias.FindAsync(id);
                if (categoria == null)
                {
                    return NotFound();
                }
                return View(categoria);
            }
            return View(new CategoriaModel());
        }
        private bool CategoriaExiste(int id) //Método para verificar se o id existe no banco
        {
            return _context.Categorias.Any(x => x.IdCategoria == id);
        }
        [HttpPost]
        public async Task<IActionResult> Cadastrar(int? id,[FromForm]CategoriaModel categoria)
        {
            if (ModelState.IsValid)
            {
                if(id.HasValue) //Se for localizado ID no banco, ou seja, Update
                {
                    if (CategoriaExiste(id.Value))
                    {
                        _context.Categorias.Update(categoria);
                        if (await _context.SaveChangesAsync() > 0)
                        {
                            TempData["mensagem"] = MensagemModel.Serializar("Categoria Alterada com Sucesso!");
                        }
                        else
                        {
                            TempData["mensagem"] = MensagemModel.Serializar("Erro ao alterar categoria",TipoMensagem.Erro);
                        }
                    }
                    else
                    {
                        TempData["mensagem"] = MensagemModel.Serializar("Categoria não encontrada!", TipoMensagem.Erro);
                    }
                }
                else //Aqui é feito o Create
                {
                    _context.Categorias.Add(categoria);
                    if (await _context.SaveChangesAsync() > 0)
                    {
                        TempData["mensagem"] = MensagemModel.Serializar("Categoria Cadastrada com Sucesso!");
                    }
                    else
                    {
                        TempData["mensagem"] = MensagemModel.Serializar("Erro cadastrar categoria",TipoMensagem.Erro);
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(categoria);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Excluir(int? id)
        {
            if(!id.HasValue)
            {
                TempData["mensagem"] = MensagemModel.Serializar("Categoria não informada",TipoMensagem.Erro);
                return RedirectToAction(nameof(Index));                
            }

            var categoria = await _context.Categorias.FindAsync(id);
            if(categoria == null)
            {
                TempData["mensagem"] = MensagemModel.Serializar("Categoria não encontrada",TipoMensagem.Erro);
                return RedirectToAction(nameof(Index)); 
            }
            return View(categoria);
        }
        [HttpPost]
        public async Task<IActionResult> Excluir(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if(categoria != null)
            {
                _context.Categorias.Remove(categoria);
                if(await _context.SaveChangesAsync() > 0)
                {
                    TempData["mensagem"] = MensagemModel.Serializar("Categoria Excluída com sucesso");
                    
                }
                else
                {
                    TempData["mensagem"] = MensagemModel.Serializar("Não foi possível excluir categoria",TipoMensagem.Erro);
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["mensagem"] = MensagemModel.Serializar("Categoria não encontrada",TipoMensagem.Erro);
                return RedirectToAction(nameof(Index));
            }
        }
    }

}