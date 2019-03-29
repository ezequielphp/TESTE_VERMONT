using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VERMONT.Web.Infra;
using VERMONT.Web.Models;

namespace VERMONT.Web.Controllers
{
    public class PessoaController : Controller
    {
        private PessoaRepository _passoaRespository = new PessoaRepository();
        private TipoContatoRepository _TipoContatorespository = new TipoContatoRepository();

        
        public ActionResult Index()
        {
            return View(_passoaRespository.GetAll());
        }


        public ActionResult Create()
        {
            PreencherViewBag();
            return View();
        }
         

        [HttpPost]
        public ActionResult Create(Pessoa pessoa)
        {
            if (!pessoa.IdTipoContato.HasValue)
            {
                ModelState.AddModelError("IdTipoContato", "Informe o Tipo de Contato");
            }

            if (string.IsNullOrEmpty(pessoa.NomeContato))
            {
                ModelState.AddModelError("NomeContato", "Informe o Nome Contato");
            }

            if (string.IsNullOrEmpty(pessoa.InfoContato))
            {
                ModelState.AddModelError("InfoContato", "Informe o Info Contato");
            }


            if (ModelState.IsValid)
            {
                _passoaRespository.Save(pessoa);
                return RedirectToAction("Index", "Contato");
            }
            else
            {
                PreencherViewBag();
                return View(pessoa);
            }
        }
         
        public ActionResult Edit(int id)
        {
            var pessoa = _passoaRespository.GetById(id);

            if (pessoa == null)
            {
                return HttpNotFound();
            }

            return View(pessoa);
        }
         
        [HttpPost]
        public ActionResult Edit(Pessoa pessoa)
        {
            if (ModelState.IsValid)
            {
                _passoaRespository.Update(pessoa);
                return RedirectToAction("Index", "Contato");
            }
            else
            {
                return View(pessoa);
            }
        }
         

        private void PreencherViewBag(Contato model)
        {

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


            ViewBag.IdTipoContato = new SelectList(from R in _TipoContatorespository.GetAll().OrderBy(x => x.Descricao)
                                                   select new
                                                   {
                                                       IdTipoContato = R.IdTipoContato,
                                                       Descricao = R.Descricao
                                                   }, "IdTipoContato", "Descricao");

        }

    }
}
