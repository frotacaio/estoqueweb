using EstoqueWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SQLitePCL;

namespace EstoqueWeb.Controllers
{
    public class ProdutoController : Controller 
    {
        private readonly EstoqueWebContext _context;

        public ProdutoController(EstoqueWebContext context)
        {
            this._context = context;
        }
        
        public async Task<IActionResult> Index()//Retorna listagem de Produtos ordenando pelo nome
        {
            return View(await _context.Produtos.OrderBy(x => x.Nome).Include( x=> x.Categoria).
            AsNoTracking().ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Cadastrar(int? id)
        {
            // Capturando categorias para realizar o relacionamento ao cadastrar um produto
            var categorias = _context.Categorias.OrderBy(x => x.Nome).AsNoTracking().ToList();
            //SelectList(Lista que vai povoar o select, valor que vai contar ao selecionar, o que vai aparecer para o usuário)
            var categoriasSelectList = new SelectList(categorias,
                nameof(CategoriaModel.IdCategoria),nameof(CategoriaModel.Nome));
            ViewBag.Categorias = categoriasSelectList;
            if (id.HasValue)
            {
                var produto = await _context.Produtos.FindAsync(id);
                if (produto == null)
                {
                    return NotFound();
                }
                return View(produto);
            }
            return View(new ProdutoModel());
        }
        private bool ProdutoExiste(int id) //Método para verificar se o id existe no banco
        {
            return _context.Produtos.Any(x => x.IdProduto == id);
        }
        [HttpPost]
        public async Task<IActionResult> Cadastrar(int? id,[FromForm]ProdutoModel produto)
        {
            var categorias = _context.Categorias.OrderBy(x => x.Nome).AsNoTracking().ToList();
            //SelectList(Lista que vai povoar o select, valor que vai contar ao selecionar, o que vai aparecer para o usuário)
            var categoriasSelectList = new SelectList(categorias,
                nameof(CategoriaModel.IdCategoria),nameof(CategoriaModel.Nome));
            ViewBag.Categorias = categoriasSelectList;
            if (ModelState.IsValid)
            {
                if(id.HasValue) //Se for localizado ID no banco, ou seja, Update
                {
                    if (ProdutoExiste(id.Value))
                    {
                        _context.Produtos.Update(produto);
                        if (await _context.SaveChangesAsync() > 0)
                        {
                            TempData["mensagem"] = MensagemModel.Serializar("Produto Alteradao com Sucesso!");
                        }
                        else
                        {
                            TempData["mensagem"] = MensagemModel.Serializar("Erro ao alterar produto",TipoMensagem.Erro);
                        }
                    }
                    else
                    {
                        TempData["mensagem"] = MensagemModel.Serializar("Produto não encontrado!", TipoMensagem.Erro);
                    }
                }
                else //Aqui é feito o Create
                {
                    _context.Produtos.Add(produto);
                    if (await _context.SaveChangesAsync() > 0)
                    {
                        TempData["mensagem"] = MensagemModel.Serializar("Produto Cadastrado com Sucesso!");
                    }
                    else
                    {
                        TempData["mensagem"] = MensagemModel.Serializar("Erro cadastrar produto",TipoMensagem.Erro);
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(produto);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Excluir(int? id)
        {
            if(!id.HasValue)
            {
                TempData["mensagem"] = MensagemModel.Serializar("Produto não informado",TipoMensagem.Erro);
                return RedirectToAction(nameof(Index));                
            }

            var produto = await _context.Produtos.FindAsync(id);
            if(produto == null)
            {
                TempData["mensagem"] = MensagemModel.Serializar("Produto não encontrado",TipoMensagem.Erro);
                return RedirectToAction(nameof(Index)); 
            }
            return View(produto);
        }
        [HttpPost]
        public async Task<IActionResult> Excluir(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if(produto != null)
            {
                _context.Produtos.Remove(produto);
                if(await _context.SaveChangesAsync() > 0)
                {
                    TempData["mensagem"] = MensagemModel.Serializar("Produto Excluído com sucesso");
                    
                }
                else
                {
                    TempData["mensagem"] = MensagemModel.Serializar("Não foi possível excluir produto",TipoMensagem.Erro);
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["mensagem"] = MensagemModel.Serializar("Produto não encontrado",TipoMensagem.Erro);
                return RedirectToAction(nameof(Index));
            }
        }
    }

}