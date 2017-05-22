﻿using AutoMapper;
using DBFirstDAL;
using DBFirstDAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pyramid.Controllers
{
    public class PageController : Controller
    {
        
        GenericRepository<PyramidFinalContext, DBFirstDAL.Pages> _pageRepo;

        public PageController()
        {
            _pageRepo = new PageRepository();
        }
        // GET: Page
        public ActionResult AdminIndex()
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Pages, Pyramid.Entity.Page>());
            // var model = _pageRepo.GetAll().ToList();
            var model =
                Mapper.Map<IEnumerable<Pages>, List<Entity.Page>>(_pageRepo.GetAll().ToList());
           
            return View(model);
        }

        public ActionResult AddOrUpdate(int id=0)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Pages, Pyramid.Entity.Page>());
            var ef = _pageRepo.FindBy(p => p.Id == id).SingleOrDefault();

            
            var model = Mapper.Map<Pages, Entity.Page>(_pageRepo.FindBy(p=>p.Id==id).SingleOrDefault());
            if (model==null)
            {
                model = new Entity.Page();
            }
            return View(model);
        }

        // POST: Page/Create
        [HttpPost]
        public ActionResult AddOrUpdate(Entity.Page model)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Pages, Entity.Page>());
            var efTest = Mapper.Map<Pages, Entity.Page>(_pageRepo.FindBy(p => p.Id == model.Id).SingleOrDefault());
            Mapper.Initialize(cfg => cfg.CreateMap< Entity.Page, Pages>());
            var efmodel = Mapper.Map<Entity.Page, Pages>(model);
            if (efTest == null)
            {
                _pageRepo.Add(efmodel);
            }
            else
            {
                _pageRepo.Edit(efmodel);
            }


            _pageRepo.Save();

            return RedirectToAction("AdminIndex");
            try
            {
               
            }
            catch
            {
                return RedirectToAction("AdminIndex");
            }
        }

        // GET: Page/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Page/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Page/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Page/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
