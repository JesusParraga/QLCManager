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
            //var riskList = project?.Phases?.FirstOrDefault(x => x.IsCurrentPhase)?.Risks;

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
        public async Task<IActionResult> Edit(int? id)
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
            return View(risk);
        }

        // POST: Risks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RiskId,RiskName,RiskDescription,RiskType")] Risk risk)
        {
            if (id != risk.RiskId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(risk);
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
                return RedirectToAction(nameof(Index));
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

        public async Task<IActionResult> ValidatePhase(int id)
        {
            Phase? phase = await _context.Phase?.Include(x => x.Risks)?.Include(p => p.PhaseTypes)?.FirstOrDefaultAsync(x => x.PhaseId == id);
            int low, medium, high, blocker = 0;

            if (phase != null && phase.Risks != null)
            {
                low = phase.Risks.Count(r => r.RiskLevel < 25);
                medium = phase.Risks.Count(r => r.RiskLevel < 50 && r.RiskLevel >= 25);
                high = phase.Risks.Count(r => r.RiskLevel < 75 && r.RiskLevel >= 50);
                blocker = phase.Risks.Count(r => r.RiskLevel >= 75);

                var phaseViewModel = new PhaseViewModel {
                    PhaseId = phase.PhaseId,
                    IsCurrentPhase = phase.IsCurrentPhase,
                    ProjectId = phase.ProjectId,
                    NumberLowRisk = low,
                    NumberMediumRisk = medium,
                    NumberHighRisk = high,
                    NumberBlockerRisk = blocker,
                    PhaseTypes = phase?.PhaseTypes?.ToList()
                };

                CalculateNextPhasesAvailables(ref phaseViewModel);

                phaseViewModel.RiskTypeDecission = RiskTypeEnum.Addressable.ToString();
                phaseViewModel.IsDevelopmentPhaseChecked = true;

                return PartialView("_ValidatePhase", phaseViewModel);
            }

            return RedirectToAction("GetPhasesWithRisks", "Risks", new { id = phase.ProjectId });
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
        
        private bool RiskExists(int id)
        {
            return _context.Risks.Any(e => e.RiskId == id);
        }
        
        private void CalculateNextPhasesAvailables(ref PhaseViewModel phaseViewModel)
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
