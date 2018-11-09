﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StudentExerciseMVC.Models.ViewModels;
using StudentExercisesAPI.Data;

namespace StudentExerciseMVC.Controllers
{
    public class InstructorsController : Controller
    {
        private readonly IConfiguration _config;

        public InstructorsController(IConfiguration config)
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
        // GET: instructor
        
            public async Task<ActionResult> Index()
            {
                using (IDbConnection conn = Connection)
                {

                    IEnumerable<Instructor> instructors = await conn.QueryAsync<Instructor>(@"
                    SELECT 
                        i.Id,
                        i.FirstName,
                        i.LastName,
                        i.SlackHandle,
                        i.CohortId
                    FROM Instructor i
                ");
                    return View(instructors);
                }
            }
           
        

    // GET: instructor/Details/5
    public async Task<ActionResult> Details(int id)
    {
        string sql = $@"
            SELECT
                i.Id,
                i.FirstName,
                i.LastName,
                i.SlackHandle,
                i.CohortId
            FROM Instructor i
            WHERE i.Id = {id}
            ";

        using (IDbConnection conn = Connection)
        {
            Instructor instructor = await conn.QueryFirstAsync<Instructor>(sql);
            return View(instructor);
        }
    }

    //// GET: instructor/Create
    
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
    //// POST: instructor/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(InstructorCreateViewModel model)
    {
        string sql = $@"INSERT INTO Instructor
            (FirstName, LastName, SlackHandle, CohortId)
            VALUES
            (
                '{model.instructor.FirstName}'
                ,'{model.instructor.LastName}'
                ,'{model.instructor.SlackHandle}'
                ,{model.instructor.Cohort}
            );";

        using (IDbConnection conn = Connection)
        {
            var newId = await conn.ExecuteAsync(sql);
            return RedirectToAction(nameof(Index));
        }

    }

    //// GET: instructor/Edit/5
    [HttpGet]
    public async Task<ActionResult> Edit(int id)
    {
        string sql = $@"
            SELECT
                i.Id,
                i.FirstName,
                i.LastName,
                i.SlackHandle,
                i.CohortId
            FROM Instructor i
            WHERE s.Id = {id}
            ";

        using (IDbConnection conn = Connection)
        {
            Instructor instructor = await conn.QueryFirstAsync<Instructor>(sql);
            return View(instructor);
        }
    }
        //Post:instructor/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Instructor instructor)
        {
            try
            {
                // TODO: Add update logic here
                string sql = $@"
                    UPDATE INstructor
                    SET FirstName = '{instructor.FirstName}',
                        LastName = '{instructor.LastName}',
                        SlackHandle = '{instructor.SlackHandle}'
                    WHERE Id = {id}";

                using (IDbConnection conn = Connection)
                {
                    int rowsAffected = await conn.ExecuteAsync(sql);
                    if (rowsAffected > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    return BadRequest();

                }
            }
            catch
            {
                return View();
            }
        }

    //// GET: instructor/Delete/5
    public async Task<ActionResult> DeleteConfirm(int id)
    {
        string sql = $@"
            SELECT
                i.Id,
                i.FirstName,
                i.LastName,
                i.SlackHandle,
                i.CohortId
            FROM Instructor i
            WHERE i.Id = {id}
            ";

        using (IDbConnection conn = Connection)
        {
            Instructor instructor = await conn.QueryFirstAsync<Instructor>(sql);
            return View(instructor);
        }
    }

    // POST: Students/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Delete(int id)
    {
        string sql = $@"DELETE FROM Instructor WHERE Id = {id}";

        using (IDbConnection conn = Connection)
        {
            int rowsAffected = await conn.ExecuteAsync(sql);
            if (rowsAffected > 0)
            {
                return RedirectToAction(nameof(Index));
            }
            throw new Exception("No rows affected");
        }
    }
}
}

