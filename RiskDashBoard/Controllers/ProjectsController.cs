﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiskDashBoard.Context;
using RiskDashBoard.Models;
using RiskDashBoard.Models.ViewModels;
using RiskDashBoard.Resources;
using static RiskDashBoard.Resources.StaticInfo;

namespace RiskDashBoard.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
            string stringUserId = HttpContext?.Session?.GetString(SessionVariables.SessionEnum.SessionKeyUserId.ToString());
            int.TryParse(stringUserId, out var userId);
            var projects = await _context.Projects.Include(x => x.Phases).ThenInclude(p=> p.PhaseTypes).Where(x => x.Users.Any(x => x.UserId == userId)).ToListAsync();

            return View(projects);


            //return View(await _context.Projects.ToListAsync());
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: Projects/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProjectId,ProjectName,ProjectCreateDate,ProjectLastUpdateDate")] Project project)
        {
            if (ModelState.IsValid)
            {
                string stringUserId = HttpContext?.Session?.GetString(SessionVariables.SessionEnum.SessionKeyUserId.ToString());
                int.TryParse(stringUserId, out int userId);
                var phaseType = await _context.PhaseTypes.FirstAsync(pt => pt.PhaseTypeName == (int)StaticInfo.ProjectPhasesEnum.VALUATION).ConfigureAwait(false);
                var userOwner = await _context.Users.FirstAsync(x => x.UserId == userId).ConfigureAwait(false);

                project.Users = new List<User> { userOwner };        
                project.Phases = new List<Phase> { new() {
                    PhaseTypes = new List<PhaseType> { phaseType },
                    IsCurrentPhase = true,
                    IterationNumber = 1
                    },
                };
                project.HistoricPhases = new List<HistoricPhase> { new(){
                            Comments = "Start project",
                            CurrentPhaseType = StaticInfo.ProjectPhasesEnum.VALUATION.ToString(),
                            PreviousPhaseType = StaticInfo.ProjectPhasesEnum.NONE.ToString(),
                            Date = DateTime.UtcNow,
                            UserId = userId,
                            UserName = userOwner.UserName,
                            IsBack = false,
                            IterationPhaseNumber = 1
                        } 
                };

                _context.Projects.Add(project);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); 
            }
            return View(project);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.Include("Users").FirstOrDefaultAsync(x=> x.ProjectId == id);
            
           if(project != null)
            {
                return View(project);
            }
            
            return NotFound();        
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectId,ProjectName,ProjectCreateDate,ProjectLastUpdateDate, ProjectStatus")] Project project)
        {
            if (id != project.ProjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.ProjectId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> GetUserProjectsPartial(int id)
        {
            var project = await _context.Projects.Include("Users").FirstOrDefaultAsync(x => x.ProjectId == id);
            var userProjectViewModel = new List<UserProjectViewModel>();

            if (project != null)
            {
                userProjectViewModel = await UsersByProject(project);
            }

            return PartialView("_ViewUserProject", userProjectViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveUserToTheProject (int idUser, int idProy)
        {
            var project = await _context.Projects.Include("Users").FirstOrDefaultAsync(x => x.ProjectId == idProy);
            var userProjectViewModel = new List<UserProjectViewModel>();

            if (project != null && project.Users != null) {
                var userToRemove = project.Users.FirstOrDefault(x => x.UserId == idUser);              

                if (userToRemove != null)
                {
                    project.Users.Remove(userToRemove);
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }

                userProjectViewModel = await UsersByProject(project);
            }

            return PartialView("_ViewUserProject", userProjectViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserToTheProject(int idUser, int idProy)
        {
            var project = await _context.Projects.Include("Users").FirstOrDefaultAsync(x => x.ProjectId == idProy);
            var userProjectViewModel = new List<UserProjectViewModel>();

            if (project != null && project.Users != null)
            {
                var userToAdd = _context.Users.FirstOrDefault(x => x.UserId == idUser);

                if (userToAdd != null)
                {
                    project.Users.Add(userToAdd);
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }

                userProjectViewModel = await UsersByProject(project);
            }

            return PartialView("_ViewUserProject", userProjectViewModel);
        }

        [HttpPost]
        public async Task<JsonResult> CloseProject(int idProject)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(x => x.ProjectId == idProject);

            if (project != null)
            {
                project.ProjectStatus = (int)ProjectSatusEnum.Close;
                _context.Update(project);
                await _context.SaveChangesAsync();

                string stringUserId = HttpContext?.Session?.GetString(SessionVariables.SessionEnum.SessionKeyUserId.ToString());
                int.TryParse(stringUserId, out var userId);
                var projects = await _context.Projects.Include(x => x.Phases).ThenInclude(p => p.PhaseTypes).Where(x => x.Users.Any(x => x.UserId == userId)).ToListAsync();
            }

            return Json(new { redirectTo = Url.Action("Index", "Projects") });
        }

        private async Task<List<UserProjectViewModel>> UsersByProject(Project project)
        {
            List<UserProjectViewModel> userProjectViewModel = new List<UserProjectViewModel>();

            var userList = await _context.Users.ToListAsync();

            if (project != null && project.Users != null)
            {
                foreach (var user in userList)
                {
                    userProjectViewModel.Add(new UserProjectViewModel
                    {
                        projectId = project.ProjectId,
                        UserId = user.UserId, 
                        UserName = user.UserName,
                        Email = user.Email,
                        projectAsigned = project.Users.Any(x=> x.UserId == user.UserId)
                    });
                }
            }

            return userProjectViewModel;
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.ProjectId == id);
        }
    }
}
