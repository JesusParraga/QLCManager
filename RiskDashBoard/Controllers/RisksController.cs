using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiskDashBoard.Context;
using RiskDashBoard.Models;
using RiskDashBoard.Models.ViewModels;
using RiskDashBoard.Resources;

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
            Project? project = await _context.Projects.Include(x => x.Phases)?.ThenInclude(p=>p.PhaseTypes).Include(x => x.Phases).ThenInclude(p=>p.Risks)?.FirstOrDefaultAsync(x => x.ProjectId == id);

            var totalOperation = project.Phases.SelectMany(p => p.PhaseTypes).Count(pt => pt.PhaseTypeName == (int)StaticInfo.ProjectPhases.OPERATION);
            var totalDevelopment = project.Phases.SelectMany(p => p.PhaseTypes).Count(pt => pt.PhaseTypeName == (int)StaticInfo.ProjectPhases.DEVELOPMENT);
            var totalFoundation = project.Phases.SelectMany(p => p.PhaseTypes).Count(pt => pt.PhaseTypeName == (int)StaticInfo.ProjectPhases.FOUNDATIONS);

            return View(nameof(Index), project?.Phases);
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
        public IActionResult Create(int id)
        {
            var riskViewModel = new RiskViewModel
            {
                PhaseId = id
            };
            return PartialView(riskViewModel);
        }

        // POST: Risks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RiskId,RiskName,RiskDescription,RiskLevel,PhaseId")] RiskViewModel risk)
        {
            if (ModelState.IsValid)
            {
                var newRisk = new Risk { RiskDescription = risk.RiskDescription, RiskLevel = risk.RiskLevel, RiskName = risk.RiskName };
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
                RiskLevel = risk.RiskLevel,
                RiskName = risk.RiskName,
                RiskDescription = risk.RiskDescription,
                ProjectId = idProy
            };

            return View(riskViewModel);
        }

        // POST: Risks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RiskId,RiskName,RiskDescription,RiskType,RiskLevel, ProjectId")] RiskViewModel risk)
        {
            if (id != risk.RiskId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var riskToUpdate = await _context.Risks.FirstAsync(r => id == r.RiskId);
                    riskToUpdate.RiskDescription = risk.RiskDescription;
                    riskToUpdate.RiskLevel = risk.RiskLevel;
                    riskToUpdate.RiskName = risk.RiskName;
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
                low = phase.phaseSelected.Risks.Count(r => r.RiskLevel < 25);
                medium = phase.phaseSelected.Risks.Count(r => r.RiskLevel < 50 && r.RiskLevel >= 25);
                high = phase.phaseSelected.Risks.Count(r => r.RiskLevel < 75 && r.RiskLevel >= 50);
                blocker = phase.phaseSelected.Risks.Count(r => r.RiskLevel >= 75);

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

                phaseViewModel.RiskTypeDecission = blocker >= 1 ? RiskTypeEnum.Unaddressable.ToString() : 
                    high > 5 ? RiskTypeEnum.Negligible.ToString() : 
                    medium >= 5 ? RiskTypeEnum.Addressable.ToString() : 
                    RiskTypeEnum.Acceptable.ToString();

                return PartialView("_ValidatePhase", phaseViewModel);
            }

            return RedirectToAction("GetPhasesWithRisks", "Risks", new { id = idProject });
        }

        public async Task<IActionResult> GetRisksPartial()
        {
            List<Risk> riskList = await _context.Risks.Include(x => x.PhasesType).ToListAsync();

            return PartialView("_ViewRiskList", riskList);
        }
        
        public async Task AddRiskToTheCurrentPhase(int id, int idPhase)
        {
            var currentPhase = await _context.Phase.Include(p=>p.Risks).Include(p => p.PhaseTypes).FirstOrDefaultAsync(p => p.PhaseId == idPhase);

            if (currentPhase != null && currentPhase.Risks != null && !currentPhase.Risks.Any(r => r.RiskId == id)) {
                var risk = await _context.Risks.Include(r => r.PhasesType).FirstOrDefaultAsync(r => r.RiskId == id);
                if (risk != null)
                {
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
        
        public async Task<IActionResult> NextPhase(int id, int idProject,bool checkFoundations, bool checkDevelopment, bool checkOperation)
        {
            string coment = String.Empty;
            var project = await _context.Projects.Include(p => p.Phases).ThenInclude(p => p.HistoricPhases).Include(p => p.Phases).ThenInclude(p => p.PhaseTypes).FirstOrDefaultAsync(p => p.ProjectId == idProject).ConfigureAwait(false);
            var phase = project.Phases.OrderBy(ph => ph.PhaseId).Select((p, i) => new {phase = p, index = i }).FirstOrDefault(p => p.phase.PhaseId == id);

            if (phase != null && project.Phases.OrderBy(ph => ph.PhaseId).ElementAtOrDefault(phase.index + 1) != null)
            {
                project.Phases.First(p => p.IsCurrentPhase).IsCurrentPhase = false;
                project.Phases.ElementAt(phase.index + 1).IsCurrentPhase = true;
                _context.SaveChanges();
            }
            else if (phase != null)
            {
                project.Phases.First(p => p.IsCurrentPhase).IsCurrentPhase = false;
                project.Phases.Add(new Phase
                {
                    PhaseTypes = new List<PhaseType>(),
                    IsCurrentPhase = true,
                    //HistoricPhases = new List<HistoricPhase> {
                    //    new(){
                    //        CurrentPhaseId = (int)StaticInfo.ProjectPhases.VALUATION,
                    //        PreviousPhaseId = (int)StaticInfo.ProjectPhases.NONE,
                    //        Comments = string.Empty
                    //    }
                    //}
                });

                if (phase.index < 2)
                {
                    var nextPhase = await BasicCalculationNewPhase(phase.phase.PhaseTypes.First().PhaseTypeName).ConfigureAwait(false);
                    project.Phases.FirstOrDefault(p => p.IsCurrentPhase)?.PhaseTypes?.Add(nextPhase);
                }
                else
                {
                    var nextPhase = await AdvancedCalculationNewPhase(checkFoundations, checkDevelopment, checkOperation).ConfigureAwait(false);
                    project.Phases.FirstOrDefault(p => p.IsCurrentPhase)?.PhaseTypes?.ToList().AddRange(nextPhase);
                }


                _context.SaveChanges();
            }

            return RedirectToAction("GetPhasesWithRisks", "Risks", new { id = idProject });
        }

        public async Task<IActionResult> BackPhase(int id, int idProject)
        {
            string coment = String.Empty;
            var project = await _context.Projects.Include(p => p.Phases).ThenInclude(p => p.HistoricPhases).Include(p => p.Phases).ThenInclude(p => p.PhaseTypes).FirstOrDefaultAsync(p => p.ProjectId == idProject).ConfigureAwait(false);
            var phase = project.Phases.OrderBy(ph => ph.PhaseId).Select((p, i) => new { phase = p, index = i }).FirstOrDefault(p => p.phase.PhaseId == id);

            if (phase != null && phase.index > 0)
            {
                project.Phases.First(p => p.IsCurrentPhase).IsCurrentPhase = false;
                project.Phases.ElementAt(phase.index - 1).IsCurrentPhase = true;
                _context.SaveChanges();
            }

            return RedirectToAction("GetPhasesWithRisks", "Risks", new { id = idProject });
        }

        [HttpGet]
        public async Task<IActionResult> GetRiskComments(int id)
        {
            var risk = await _context.Risks.FirstOrDefaultAsync(r => r.RiskId == id);

            return PartialView("_CommentsRisk");
        }

        private async Task<List<PhaseType>> AdvancedCalculationNewPhase(bool checkFoundations, bool checkDevelopment, bool checkOperation)
        {
            var nextPhase = new List<PhaseType>();

            if (checkFoundations)
            {
                nextPhase.Add(await _context.PhaseTypes.FirstAsync(pt => pt.PhaseTypeName == (int)StaticInfo.ProjectPhases.FOUNDATIONS).ConfigureAwait(false));
            }
            if (checkDevelopment)
            {
                nextPhase.Add(await _context.PhaseTypes.FirstAsync(pt => pt.PhaseTypeName == (int)StaticInfo.ProjectPhases.DEVELOPMENT).ConfigureAwait(false));
            }
            if (checkOperation)
            {
                nextPhase.Add(await _context.PhaseTypes.FirstAsync(pt => pt.PhaseTypeName == (int)StaticInfo.ProjectPhases.OPERATION).ConfigureAwait(false));
            }

            return nextPhase;
        }

        private async Task<PhaseType> BasicCalculationNewPhase(int phaseTypeName)
        {
            var nextPhase = new PhaseType();

            if (phaseTypeName == (int)StaticInfo.ProjectPhases.EXPLORATION)
            {
                nextPhase = await _context.PhaseTypes.FirstAsync(pt => pt.PhaseTypeName == (int)StaticInfo.ProjectPhases.VALUATION).ConfigureAwait(false);
            }
            if (phaseTypeName == (int)StaticInfo.ProjectPhases.VALUATION)
            {
                nextPhase = await _context.PhaseTypes.FirstAsync(pt => pt.PhaseTypeName == (int)StaticInfo.ProjectPhases.FOUNDATIONS).ConfigureAwait(false);
            }
            if (phaseTypeName == (int)StaticInfo.ProjectPhases.FOUNDATIONS)
            {
                nextPhase = await _context.PhaseTypes.FirstAsync(pt => pt.PhaseTypeName == (int)StaticInfo.ProjectPhases.DEVELOPMENT).ConfigureAwait(false);
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
                    case (int)StaticInfo.ProjectPhases.EXPLORATION:
                        phaseViewModel.IsExplorationPhaseEnabled = false;
                        phaseViewModel.IsValuationPhaseEnabled = true;
                        phaseViewModel.IsFoundationsPhaseEnabled = false;
                        phaseViewModel.IsDevelopmentPhaseEnabled = false;
                        phaseViewModel.IsOperationPhaseEnabled = false;
                    break;
                    case (int)StaticInfo.ProjectPhases.VALUATION:
                        phaseViewModel.IsExplorationPhaseEnabled = false;
                        phaseViewModel.IsValuationPhaseEnabled = false;
                        phaseViewModel.IsFoundationsPhaseEnabled = true;
                        phaseViewModel.IsDevelopmentPhaseEnabled = false;
                        phaseViewModel.IsOperationPhaseEnabled = false;
                    break;
                    case (int)StaticInfo.ProjectPhases.FOUNDATIONS:
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
    }
}
