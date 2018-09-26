using Dapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Workforce.Models
{
    public class InstructorEditViewModel
    {
        public Instructor Instructor { get; set; }

        [Display(Name = "Current Cohort")]
        public List<SelectListItem> Cohorts { get; }

        private readonly IConfiguration _config;

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public InstructorEditViewModel() { }

        public InstructorEditViewModel(IConfiguration config)
        {
            _config = config;

            string sql = $@"SELECT Id, Name FROM Cohort";

            using (IDbConnection conn = Connection)
            {
                List<Cohort> cohorts = (conn.Query<Cohort>(sql)).ToList();

                this.Cohorts = cohorts
                    .Select(li => new SelectListItem
                    {
                        Text = li.Name,
                        Value = li.Id.ToString()
                    }).ToList();
            }


            // Add a prompt so that the <select> element isn't blank
            this.Cohorts.Insert(0, new SelectListItem
            {
                Text = "Choose cohort...",
                Value = "0"
            });
        }
    }
}
