using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiskDashBoard.Context;
using RiskDashBoard.Models;
using RiskDashBoard.Models.ViewModels;
using RiskDashBoard.Resources;
using static RiskDashBoard.Resources.StaticInfo;

namespace RiskDashBoard.Controllers
{
    public class RisksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RisksController(ApplicationDbContext context)
        {
            _context = context;
        }

        //GET: Risks
        public async Task<IActionResult> Index()
        {
            return View(await _context.Risks.ToListAsync());
        }

        // GET: Risks
        public async Task<IActionResult> GetPhasesWithRisks(int id)
        {
            Project? project = await _context.Projects.FirstOrDefaultAsync(p => p.ProjectId == id).ConfigureAwait(false);

            return View(nameof(Index), project);
        }

        public async Task<IActionResult> GetPhasesAndRiskByProject(int id, bool orderByRiskLevel = false)
        {
            Project? project = await GetProjectPhasesInfo(id, orderByRiskLevel).ConfigureAwait(false);

            return PartialView("_ProjectPhases",project?.Phases);
        }

        public async Task<IActionResult> GetProjectHistoric(int id)
        {
            var historicProject = await _context.HistoricPhases.Where(h => h.ProjectId == id).OrderByDescending(h => h.Date).ToListAsync().ConfigureAwait(false);

            return PartialView("_ProjectPhasesTimeLine", historicProject);
        }

        // GET: Risks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var risk = await _context.Risks
                .FirstOrDefaultAsync(m => m.RiskId == id);
            if (risk == null)
            {
                return NotFound();
            }

            return View(risk);
        }

        // GET: Risks/Create
        public IActionResult Create(int id, int idProject)
        {
            var riskViewModel = new RiskViewModel
            {
                PhaseId = id,
                ProjectId = idProject
            };
            return PartialView(riskViewModel);
        }

        // POST: Risks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RiskId,RiskName,RiskDescription,RiskProbability,RiskImpact,PhaseId")] RiskViewModel risk)
        {
            if (ModelState.IsValid)
            {
                var newRisk = new Risk { RiskDescription = risk.RiskDescription, RiskProbability = risk.RiskProbability, RiskImpact = risk.RiskImpact, RiskName = risk.RiskName, Resolved = false};
                _context.Risks.Add(newRisk);
                await _context.SaveChangesAsync();

                var phase = await _context.Phase.Include(x => x.Risks).FirstOrDefaultAsync(x => x.PhaseId == risk.PhaseId).ConfigureAwait(false);

                if (phase != null && newRisk != null) {          
                    phase.Risks ??= new List<Risk>();
                    phase.Risks.Add(newRisk);
                    await _context.SaveChangesAsync();
                }
                
                return RedirectToAction("GetPhasesWithRisks", "Risks", new { id = phase.ProjectId });
            }
            return PartialView(risk);
        }

        // GET: Risks/Edit/5
        public async Task<IActionResult> Edit(int? id, int idProy)
        {
            if (id == null)
            {
                return NotFound();
            }

            var risk = await _context.Risks.FindAsync(id);
            if (risk == null)
            {
                return NotFound();
            }
            var riskViewModel = new RiskViewModel
            {
                RiskId = risk.RiskId,
                RiskImpact = risk.RiskImpact,
                RiskProbability = risk.RiskProbability,
                RiskName = risk.RiskName,
                RiskDescription = risk.RiskDescription,
                ProjectId = idProy,
                Resolved = risk.Resolved
            };

            return View(riskViewModel);
        }

        // POST: Risks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RiskId,RiskName,RiskDescription,RiskType,RiskProbability,RiskImpact,ProjectId,Resolved")] RiskViewModel risk)
        {
            if (id != risk.RiskId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var riskToUpdate = await _context.Risks.Include(r => r.Comments).FirstAsync(r => id == r.RiskId);
                    AddEditComment(riskToUpdate, risk);
                    riskToUpdate.RiskDescription = risk.RiskDescription;
                    riskToUpdate.RiskImpact = risk.RiskImpact;
                    riskToUpdate.RiskProbability = risk.RiskProbability;
                    riskToUpdate.RiskName = risk.RiskName;
                    riskToUpdate.Resolved = risk.Resolved;               
                    _context.Update(riskToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RiskExists(risk.RiskId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("GetPhasesWithRisks", "Risks", new { id = risk.ProjectId });
            }
            return View(risk);
        }

        // GET: Risks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var risk = await _context.Risks
                .FirstOrDefaultAsync(m => m.RiskId == id);
            if (risk == null)
            {
                return NotFound();
            }

            return View(risk);
        }

        // POST: Risks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var risk = await _context.Risks.FindAsync(id);
            if (risk != null)
            {
                _context.Risks.Remove(risk);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ValidatePhase(int id, int idProject)
        {
            var project = await _context.Projects.Include(p => p.Phases).ThenInclude(p => p.Risks).Include(p => p.Phases).ThenInclude( p => p.PhaseTypes).FirstOrDefaultAsync(p => p.ProjectId == idProject);
            var phase = project.Phases.OrderBy(ph => ph.PhaseId).Select((p, i) => new { phaseSelected = p, index = i }).FirstOrDefault(p => p.phaseSelected.PhaseId == id);
            int low, medium, high, blocker = 0;

            if (phase != null && phase.phaseSelected.Risks != null)
            {
                low = phase.phaseSelected.Risks.Count(r => r.RiskLevel  == (int)RiskLevelEnum.LOW);
                medium = phase.phaseSelected.Risks.Count(r => r.RiskLevel == (int)RiskLevelEnum.MEDIUM);
                high = phase.phaseSelected.Risks.Count(r => r.RiskLevel == (int)RiskLevelEnum.HIGH);
                blocker = phase.phaseSelected.Risks.Count(r => r.RiskLevel == (int)RiskLevelEnum.BLOCKER);

                var phaseViewModel = new PhaseViewModel
                {
                    PhaseId = phase.phaseSelected.PhaseId,
                    IsCurrentPhase = phase.phaseSelected.IsCurrentPhase,
                    ProjectId = phase.phaseSelected.ProjectId,
                    NumberLowRisk = low,
                    NumberMediumRisk = medium,
                    NumberHighRisk = high,
                    NumberBlockerRisk = blocker,
                    PhaseTypes = phase?.phaseSelected?.PhaseTypes?.ToList()
                };
               
                if (project.Phases.ElementAtOrDefault(phase.index + 1) == null)
                {
                    CalculateNextPhasesAvailables(phaseViewModel);
                }
                else
                {
                    phaseViewModel.IsExplorationPhaseEnabled = false;
                    phaseViewModel.IsValuationPhaseEnabled = false;
                    phaseViewModel.IsFoundationsPhaseEnabled = false;
                    phaseViewModel.IsDevelopmentPhaseEnabled = false;
                    phaseViewModel.IsOperationPhaseEnabled = false;
                }

                phaseViewModel.EvaluationPhase = blocker >= 1 ? RiskLevelEvaluationTypeEnum.Unaddressable.ToString() : 
                    high >= 5 ? RiskLevelEvaluationTypeEnum.Addressable.ToString() : 
                    medium >= 5 ? RiskLevelEvaluationTypeEnum.Acceptable.ToString() : 
                    RiskLevelEvaluationTypeEnum.Negligible.ToString();

                phaseViewModel.ProposalRiskDecission = phaseViewModel.EvaluationPhase ==
                    RiskLevelEvaluationTypeEnum.Unaddressable.ToString() ? (int)PhaseDecissionProposalTypeEnum.Discontinue :
                    phaseViewModel.EvaluationPhase ==
                    RiskLevelEvaluationTypeEnum.Addressable.ToString() ? (int)PhaseDecissionProposalTypeEnum.GoBack :
                     (int)PhaseDecissionProposalTypeEnum.GoForward;

                return PartialView("_ValidatePhase", phaseViewModel);
            }

            return RedirectToAction("GetPhasesWithRisks", "Risks", new { id = idProject });
        }

        public async Task<IActionResult> GetRisksPartial(int idProject)
        {
            
            List<Risk> riskList = new List<Risk>(); 

            if (idProject != 0)
            {
                var project = await _context.Projects.Include(p => p.Phases)
                    .ThenInclude(ph => ph.Risks)
                    .ThenInclude(r => r.PhasesType).FirstAsync(p => p.ProjectId == idProject);

                riskList = project.Phases.SelectMany(ph => ph.Risks).ToList();
            }
            else
            {
                var UserId = HttpContext?.Session?.GetString(SessionVariables.SessionEnum.SessionKeyUserId.ToString());
                int UserIntID = int.Parse(UserId);

                var projectList = await _context.Projects.Include(p => p.Users.Where(u => u.UserId == UserIntID)).Include(p => p.Phases)
                    .ThenInclude(ph => ph.Risks)
                    .ThenInclude(r => r.PhasesType).ToListAsync();

                riskList = projectList.SelectMany(p => p.Phases.SelectMany(ph => ph.Risks)).ToList();
            }

            return PartialView("_ViewRiskList", riskList);
        }
        
        public async Task AddRiskToTheCurrentPhase(int id, int idPhase)
        {
            var currentPhase = await _context.Phase.Include(p=>p.Risks).Include(p => p.PhaseTypes).FirstOrDefaultAsync(p => p.PhaseId == idPhase);

            if (currentPhase != null && currentPhase.Risks != null && !currentPhase.Risks.Any(r => r.RiskId == id)) {
                var riskToCopy = await _context.Risks.Include(r => r.PhasesType).FirstOrDefaultAsync(r => r.RiskId == id);
                if (riskToCopy != null)
                {
                    var risk = new Risk
                    {
                        RiskDescription = riskToCopy.RiskDescription,
                        RiskProbability = riskToCopy.RiskProbability,
                        RiskImpact = riskToCopy.RiskImpact,
                        RiskName = riskToCopy.RiskName,
                        PhasesType = new List<PhaseType>()
                    };
                    foreach(var ph in riskToCopy.PhasesType) { risk.PhasesType.Add(ph); }

                    currentPhase.Risks.Add(risk);
                    var phasesToAdd = currentPhase.PhaseTypes.Select(pt => pt.PhaseTypeName).Except(risk.PhasesType.Select(r => r.PhaseTypeName));
                    if (phasesToAdd != null && phasesToAdd.Any())
                    {
                        foreach (var phaseNameToAdd in phasesToAdd)
                        {
                            risk.PhasesType.Add(currentPhase.PhaseTypes.First(pt => pt.PhaseTypeName == phaseNameToAdd));
                        }
                    }

                    await _context.SaveChangesAsync();
                }
            }
        }
     
        public async Task<IActionResult> NextPhase(int id, int idProject,bool checkFoundations, bool checkDevelopment, bool checkOperation, string comments, int proposalRiskDecission, string riskEvaluation)
        {
            var UserId = HttpContext?.Session?.GetString(SessionVariables.SessionEnum.SessionKeyUserId.ToString());
            var UserName = HttpContext?.Session?.GetString(SessionVariables.SessionEnum.SessionKeyUserName.ToString());
            var project = await _context.Projects.Include(p=> p.HistoricPhases).Include(p => p.Phases).ThenInclude(p => p.PhaseTypes).Include(p => p.Phases).ThenInclude(ph => ph.Risks).FirstOrDefaultAsync(p => p.ProjectId == idProject).ConfigureAwait(false);
            var phase = project.Phases.OrderBy(ph => ph.PhaseId).Select((p, i) => new {phase = p, index = i }).FirstOrDefault(p => p.phase.PhaseId == id);
            var newHistoricProject = new HistoricPhase { 
                Date = DateTime.UtcNow, 
                IsBack = false, 
                ProjectId = idProject,
                UserId = int.Parse(UserId),
                UserName = UserName,
                Comments = comments,
                DecissionId = (int)PhaseDecissionProposalTypeEnum.GoForward,
                PhaseRiskEvaluation = (int)(RiskLevelEvaluationTypeEnum)Enum.Parse(typeof(RiskLevelEvaluationTypeEnum), riskEvaluation),
                ProposalRiskDecission = proposalRiskDecission
            };
            

            if (phase != null && project.Phases.OrderBy(ph => ph.PhaseId).ElementAtOrDefault(phase.index + 1) != null)
            {
                newHistoricProject.PreviousPhaseType = string.Join(",", project.Phases.First(p => p.IsCurrentPhase).PhaseTypes.Select(pt => pt.PhaseTypeNameDescription).ToList());
                project.Phases.First(p => p.IsCurrentPhase).IsCurrentPhase = false;
                project.Phases.ElementAt(phase.index + 1).IsCurrentPhase = true;
                newHistoricProject.CurrentPhaseType = string.Join(",", project.Phases.First(p => p.IsCurrentPhase).PhaseTypes.Select(pt => pt.PhaseTypeNameDescription).ToList());
                newHistoricProject.IterationPhaseNumber = project.Phases.First(p => p.IsCurrentPhase).IterationNumber;
                MoveRiskBetweenPhases(project);

                project.HistoricPhases?.Add(newHistoricProject);
                _context.SaveChanges();
            }
            else if (phase != null)
            {
                newHistoricProject.PreviousPhaseType = string.Join(",", project.Phases.First(p => p.IsCurrentPhase).PhaseTypes.Select(pt => pt.PhaseTypeNameDescription).ToList());
                project.Phases.First(p => p.IsCurrentPhase).IsCurrentPhase = false;

                if (phase.index < 2)
                {
                    var nextPhase = await BasicCalculationNewPhase(phase.phase.PhaseTypes.First().PhaseTypeName).ConfigureAwait(false);
                    newHistoricProject.CurrentPhaseType = nextPhase.PhaseTypeNameDescription;
                    newHistoricProject.IterationPhaseNumber = phase.index + 1;
                    project.Phases.Add(new Phase
                    {
                        PhaseTypes = new List<PhaseType> { nextPhase },
                        IsCurrentPhase = true,
                        ProjectId = idProject,
                        IterationNumber = phase.index + 1
                    });
                    _context.SaveChanges();
                }
                else
                {
                    var nextPhase = await AdvancedCalculationNewPhase(checkFoundations, checkDevelopment, checkOperation).ConfigureAwait(false);
                    newHistoricProject.CurrentPhaseType = string.Join(",", nextPhase.Select(pt => pt.PhaseTypeNameDescription).ToList());
                    newHistoricProject.IterationPhaseNumber = phase.index + 1;
                    project.Phases.Add(new Phase
                    {
                        PhaseTypes = nextPhase,
                        IsCurrentPhase = true,
                        ProjectId = idProject,
                        IterationNumber = phase.index + 1
                    });
                    _context.SaveChanges();
                }

                project.HistoricPhases.Add(newHistoricProject);
                MoveRiskBetweenPhases(project);
                _context.SaveChanges();
            }

            return RedirectToAction("GetPhasesAndRiskByProject", "Risks", new { id = idProject });
        }

        public async Task<IActionResult> BackPhase(int id, int idProject, string comments, int proposalRiskDecission, string riskEvaluation)
        {
            var project = await _context.Projects.Include(p => p.HistoricPhases).Include(p => p.Phases).ThenInclude(p => p.PhaseTypes).Include(p=> p.Phases).ThenInclude(ph => ph.Risks).FirstOrDefaultAsync(p => p.ProjectId == idProject).ConfigureAwait(false);
            var phase = project.Phases.OrderBy(ph => ph.PhaseId).Select((p, i) => new { phase = p, index = i }).FirstOrDefault(p => p.phase.PhaseId == id);

            if (phase != null && phase.index > 0)
            {
                var UserId = HttpContext?.Session?.GetString(SessionVariables.SessionEnum.SessionKeyUserId.ToString());
                var UserName = HttpContext?.Session?.GetString(SessionVariables.SessionEnum.SessionKeyUserName.ToString());

                var newHistoricProject = new HistoricPhase
                {
                    Date = DateTime.UtcNow,
                    IsBack = true,
                    ProjectId = idProject,
                    UserId = int.Parse(UserId),
                    UserName = UserName,
                    Comments = comments,
                    DecissionId = (int)PhaseDecissionProposalTypeEnum.GoBack,
                    PreviousPhaseType = string.Join(",", project.Phases.First(p => p.IsCurrentPhase).PhaseTypes.Select(pt => pt.PhaseTypeNameDescription).ToList()),
                    PhaseRiskEvaluation = (int)(RiskLevelEvaluationTypeEnum)Enum.Parse(typeof(RiskLevelEvaluationTypeEnum), riskEvaluation),
                    ProposalRiskDecission = proposalRiskDecission,
                };
                project.Phases.First(p => p.IsCurrentPhase).IsCurrentPhase = false;
                project.Phases.ElementAt(phase.index - 1).IsCurrentPhase = true;
                newHistoricProject.IterationPhaseNumber = project.Phases.ElementAt(phase.index - 1).IterationNumber;
                newHistoricProject.CurrentPhaseType = string.Join(",", project.Phases.First(p => p.IsCurrentPhase).PhaseTypes.Select(pt => pt.PhaseTypeNameDescription).ToList());
                project.HistoricPhases.Add(newHistoricProject);

                MoveRiskBetweenPhases(project);

                _context.SaveChanges();
            }

            return RedirectToAction("GetPhasesAndRiskByProject", "Risks", new { id = idProject });
        }

        [HttpGet]
        public async Task<IActionResult> GetRiskComments(int id)
        {
            var risk = await _context.Risks.Include(r => r.Comments).FirstAsync(r => r.RiskId == id).ConfigureAwait(false);

            return PartialView("_CommentsRisk", risk);
        }

        public async Task DeleteCommentToRisk(int idComment)
        {
            var comment = await _context.Comments.FindAsync(idComment);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
            }

            await _context.SaveChangesAsync();
        }

        public async Task AddCommentToRisk(int idRisk, string commentTxt)
        {
            var stringUserId = HttpContext?.Session?.GetString(SessionVariables.SessionEnum.SessionKeyUserId.ToString());
            _ = int.TryParse(stringUserId, out var userId);

            var commentToAdd = new Comment
            {
                RiskId = idRisk,
                CreatedAt = DateTime.Now,
                LastUpdatedAt = DateTime.Now,
                UserComment = commentTxt,
                UserId = userId,
                UserName = HttpContext?.Session?.GetString(SessionVariables.SessionEnum.SessionKeyUserName.ToString()),
            };

            var risk = await _context.Risks.Include(r => r.Comments).FirstAsync(r => r.RiskId == idRisk).ConfigureAwait(false);
            if (risk != null)
            {
                if (risk.Comments == null) { 
                    risk.Comments = new List<Comment>
                    {
                        commentToAdd
                    }; 
                }
                else { risk.Comments.Add(commentToAdd); }

                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        private async Task<Project?> GetProjectPhasesInfo(int id, bool orderByRiskLevel = false)
        {
            Project? project = await _context.Projects.Include(x => x.Phases)?.ThenInclude(p => p.PhaseTypes).Include(x => x.Phases).ThenInclude(p => p.Risks)?.FirstOrDefaultAsync(x => x.ProjectId == id);

            var totalOperation = project.Phases.SelectMany(p => p.PhaseTypes).Count(pt => pt.PhaseTypeName == (int)StaticInfo.ProjectPhasesEnum.OPERATION);
            var totalDevelopment = project.Phases.SelectMany(p => p.PhaseTypes).Count(pt => pt.PhaseTypeName == (int)StaticInfo.ProjectPhasesEnum.DEVELOPMENT);
            var totalFoundation = project.Phases.SelectMany(p => p.PhaseTypes).Count(pt => pt.PhaseTypeName == (int)StaticInfo.ProjectPhasesEnum.FOUNDATIONS);

            if (orderByRiskLevel)
            {
                project.Phases.First(p => p.IsCurrentPhase).Risks = project.Phases.First(p => p.IsCurrentPhase).Risks.OrderByDescending(r => r.RiskLevel).ToList();
            }

            return project;
        }

        private async Task<List<PhaseType>> AdvancedCalculationNewPhase(bool checkFoundations, bool checkDevelopment, bool checkOperation)
        {
            var nextPhase = new List<PhaseType>();

            if (checkFoundations)
            {
                nextPhase.Add(await _context.PhaseTypes.FirstAsync(pt => pt.PhaseTypeName == (int)StaticInfo.ProjectPhasesEnum.FOUNDATIONS).ConfigureAwait(false));
            }
            if (checkDevelopment)
            {
                nextPhase.Add(await _context.PhaseTypes.FirstAsync(pt => pt.PhaseTypeName == (int)StaticInfo.ProjectPhasesEnum.DEVELOPMENT).ConfigureAwait(false));
            }
            if (checkOperation)
            {
                nextPhase.Add(await _context.PhaseTypes.FirstAsync(pt => pt.PhaseTypeName == (int)StaticInfo.ProjectPhasesEnum.OPERATION).ConfigureAwait(false));
            }

            return nextPhase;
        }

        private async Task<PhaseType> BasicCalculationNewPhase(int phaseTypeName)
        {
            var nextPhase = new PhaseType();

            if (phaseTypeName == (int)StaticInfo.ProjectPhasesEnum.EXPLORATION)
            {
                nextPhase = await _context.PhaseTypes.FirstAsync(pt => pt.PhaseTypeName == (int)StaticInfo.ProjectPhasesEnum.VALUATION).ConfigureAwait(false);
            }
            if (phaseTypeName == (int)StaticInfo.ProjectPhasesEnum.VALUATION)
            {
                nextPhase = await _context.PhaseTypes.FirstAsync(pt => pt.PhaseTypeName == (int)StaticInfo.ProjectPhasesEnum.FOUNDATIONS).ConfigureAwait(false);
            }
            if (phaseTypeName == (int)StaticInfo.ProjectPhasesEnum.FOUNDATIONS)
            {
                nextPhase = await _context.PhaseTypes.FirstAsync(pt => pt.PhaseTypeName == (int)StaticInfo.ProjectPhasesEnum.DEVELOPMENT).ConfigureAwait(false);
            }

            return nextPhase;
        }

        private bool RiskExists(int id)
        {
            return _context.Risks.Any(e => e.RiskId == id);
        }
        
        private void CalculateNextPhasesAvailables(PhaseViewModel phaseViewModel)
        {
            if(phaseViewModel.PhaseTypes.Count == 1)
            {
                switch(phaseViewModel.PhaseTypes.First().PhaseTypeName)
                {
                    case (int)StaticInfo.ProjectPhasesEnum.EXPLORATION:
                        phaseViewModel.IsExplorationPhaseEnabled = false;
                        phaseViewModel.IsValuationPhaseEnabled = true;
                        phaseViewModel.IsFoundationsPhaseEnabled = false;
                        phaseViewModel.IsDevelopmentPhaseEnabled = false;
                        phaseViewModel.IsOperationPhaseEnabled = false;
                    break;
                    case (int)StaticInfo.ProjectPhasesEnum.VALUATION:
                        phaseViewModel.IsExplorationPhaseEnabled = false;
                        phaseViewModel.IsValuationPhaseEnabled = false;
                        phaseViewModel.IsFoundationsPhaseEnabled = true;
                        phaseViewModel.IsDevelopmentPhaseEnabled = false;
                        phaseViewModel.IsOperationPhaseEnabled = false;
                    break;
                    case (int)StaticInfo.ProjectPhasesEnum.FOUNDATIONS:
                        phaseViewModel.IsExplorationPhaseEnabled = false;
                        phaseViewModel.IsValuationPhaseEnabled = false;
                        phaseViewModel.IsFoundationsPhaseEnabled = true;
                        phaseViewModel.IsDevelopmentPhaseEnabled = true;
                        phaseViewModel.IsOperationPhaseEnabled = true;
                    break;
                    default:
                        phaseViewModel.IsExplorationPhaseEnabled = false;
                        phaseViewModel.IsValuationPhaseEnabled = false;
                        phaseViewModel.IsFoundationsPhaseEnabled = true;
                        phaseViewModel.IsDevelopmentPhaseEnabled = true;
                        phaseViewModel.IsOperationPhaseEnabled = true;
                    break;
                }
            }
            else
            {
                phaseViewModel.IsExplorationPhaseEnabled = false;
                phaseViewModel.IsValuationPhaseEnabled = false;
                phaseViewModel.IsFoundationsPhaseEnabled = true;
                phaseViewModel.IsDevelopmentPhaseEnabled = true;
                phaseViewModel.IsOperationPhaseEnabled = true;
            }
        }

        private static void MoveRiskBetweenPhases(Project? project)
        {
            var phaseId = project.Phases.First(p => p.IsCurrentPhase).PhaseId;
            project.Phases.Where(ph => ph.Risks != null).SelectMany(p => p.Risks?.Where(r => !r.Resolved)).ToList().ForEach(r => r.PhaseId = phaseId);
        }

        private void AddEditComment(Risk riskToUpdate, RiskViewModel risk)
        {
            string message = string.Empty; 

            if (riskToUpdate.RiskName != risk.RiskName)
            {
                message = message + "Risk name was changed from: " + risk.RiskName + " to " + riskToUpdate.RiskName + "\n";
            }

            if (riskToUpdate.RiskDescription != risk.RiskDescription)
            {
                message = message + "Risk description was changed from: " + risk.RiskDescription + " to " + riskToUpdate.RiskDescription + "\n";
            }

            if (riskToUpdate.RiskImpact != risk.RiskImpact)
            {
                message = message + "Risk impact was changed from: " + ((StaticInfo.RiskImpactEnum)risk.RiskImpact).ToString() + " to " + ((StaticInfo.RiskImpactEnum)riskToUpdate.RiskImpact).ToString() + "\n";
            }

            if (riskToUpdate.RiskProbability != risk.RiskProbability)
            {
                message = message + "Risk probability was changed from: " + ((StaticInfo.RiskProbabilityEnum)risk.RiskProbability).ToString() + " to " + ((StaticInfo.RiskProbabilityEnum)riskToUpdate.RiskProbability).ToString() + "\n";
            }

            if (riskToUpdate.Resolved != risk.Resolved)
            {
                var txt = risk.Resolved == true ? "Resolved" : "In Progress";
                var txtchanged = riskToUpdate.Resolved == true ? "Resolved" : "In Progress";
                message = message + "Risk status was changed from: " + txt + " to " + txtchanged + "\n";
            }

            if (!string.IsNullOrEmpty(message))
            {
                var UserId = HttpContext?.Session?.GetString(SessionVariables.SessionEnum.SessionKeyUserId.ToString());
                var UserName = HttpContext?.Session?.GetString(SessionVariables.SessionEnum.SessionKeyUserName.ToString());

                var comment = new Comment
                {
                    RiskId = riskToUpdate.RiskId,
                    UserComment = message,
                    UserId = int.Parse(UserId),
                    UserName = UserName,
                    LastUpdatedAt = DateTime.Now
                };

                riskToUpdate.Comments.Add(comment);
            }
        }
    }
}
