using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VERMONT.Web.Infra;
using VERMONT.Web.Models;

namespace VERMONT.Web.Controllers
{
    public class ContatoController : Controller
    {
        private ContatoRepository _contatoRespository = new ContatoRepository();
        private PessoaRepository _PessoalRespository = new PessoaRepository();
        private TipoContatoRepository _TipoContatorespository = new TipoContatoRepository();


        public ActionResult Index()
        {
            var filtro = new ContatoFiltro();
            ViewData["FiltroModel"] = filtro;
            return View(_contatoRespository.FindByFiltro(filtro));
        }


        [HttpPost]
        public ActionResult Index(ContatoFiltro filtro)
        {
            ViewData["FiltroModel"] = filtro;
            return View(_contatoRespository.FindByFiltro(filtro));
        }
         

        public ActionResult Create(int id)
        {
            var contato = new Contato
            {
                IdPessoa = id,
                Pessoa= _PessoalRespository.GetById(id),
            };

            PreencherViewBag();
            return View(contato);
        }



        [HttpPost]
        public ActionResult Create(Contato contato)
        {
            if (ModelState.IsValid)
            {
                _contatoRespository.Save(contato);
                return RedirectToAction("Index");
            }
            else
            {
                PreencherViewBag(contato);
                return View(contato);
            }
        }


        public ActionResult Edit(int id)
        {
            var contato= _contatoRespository.GetById(id);

            if (contato == null)
            {
                return HttpNotFound();
            }

            PreencherViewBag(contato);
            return View(contato);
        }


        [HttpPost]
        public ActionResult Edit(Contato contato)
        {
            if (ModelState.IsValid)
            {
                _contatoRespository.Update(contato);
                return RedirectToAction("Index");
            }
            else
            {
                PreencherViewBag(contato);
                return View(contato);
            }
        }


 


        private void PreencherViewBag(Contato model)
        {
            ViewBag.IdPessoa = new SelectList(from R in _PessoalRespository.GetAll().OrderBy(x => x.Nome)
                                              select new
                                              {
                                                  IdPessoa = R.IdPessoa,
                                                  Nome = R.Nome
                                              }, "IdPessoa", "Nome", model.IdPessoa
                                              );
            ViewBag.IdTipoContato = new SelectList(from R in _TipoContatorespository.GetAll().OrderBy(x => x.Descricao)
                                                   select new
                                                   {
                                                       IdTipoContato = R.IdTipoContato,
                                                       Descricao = R.Descricao
                                                   }, "IdTipoContato", "Descricao", model.IdTipoContato
                                  );

        }


        private void PreencherViewBag()
        {
            ViewBag.IdPessoa = new SelectList(from R in _PessoalRespository.GetAll().OrderBy(x => x.Nome)
                                              select new
                                              {
                                                  IdPessoa = R.IdPessoa,
                                                  Nome = R.Nome
                                              }, "IdPessoa", "Nome");

            ViewBag.IdTipoContato = new SelectList(from R in _TipoContatorespository.GetAll().OrderBy(x => x.Descricao)
                                                   select new
                                                   {
                                                       IdTipoContato = R.IdTipoContato,
                                                       Descricao = R.Descricao
                                                   }, "IdTipoContato", "Descricao");

        }
    }
}
