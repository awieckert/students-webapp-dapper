using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using Workforce.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Workforce.Controllers
{
    public class InstructorController : Controller
    {

        private readonly IConfiguration _config;

        public InstructorController(IConfiguration config)
        {
            _config = config;
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        // GET: Instructor
        public async Task<IActionResult> Index()
        {
            string sql = @"SELECT i.Id, 
                            i.FirstName,
                            i.LastName,
                            i.SlackHandle,
                            i.Specialty,
                            c.Id,
                            c.Name
                            FROM Instructor i
                            JOIN Cohort c on i.CohortId = c.Id";

            using (IDbConnection conn = Connection)
            {
                List<Instructor> instructors = new List<Instructor>();

                var sqlReturn = await conn.QueryAsync<Instructor, Cohort, Instructor>(sql, (instructor, cohort) => {
                    instructor.Cohort = cohort;
                    instructors.Add(instructor);

                    return instructor;
                });

                return View(instructors);
            }
        }

        // GET: Instructor/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Instructor/Create
        private async Task<SelectList> CohortList(int? selected)
        {
            using (IDbConnection conn = Connection)
            {
                // Get all cohort data
                List<Cohort> cohorts = (await conn.QueryAsync<Cohort>("SELECT Id, Name FROM Cohort")).ToList();

                // Add a prompting cohort for dropdown
                cohorts.Insert(0, new Cohort() { Id = 0, Name = "Select cohort..." });

                // Generate SelectList from cohorts
                var selectList = new SelectList(cohorts, "Id", "Name", selected);
                return selectList;
            }
        }

        public async Task<IActionResult> Create()
        {
            using (IDbConnection conn = Connection)
            {
                ViewData["CohortId"] = await CohortList(null);
                return View();
            }
        }

        // POST: Instructor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Instructor/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Instructor/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Instructor/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Instructor/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}