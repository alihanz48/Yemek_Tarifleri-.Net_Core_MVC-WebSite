using YemekTarifleri.Data.Abstract;
using YemekTarifleri.Entity;

namespace YemekTarifleri.Data.Concrete.EfCore;

public class EfStepRepository : IStepRepository
{
    private YemekTarifleriContext _context;
    public EfStepRepository(YemekTarifleriContext context)
    {
        _context = context;
    }

    public IQueryable<Step> Steps => _context.Steps;

    public void CreateStep(Step step)
    {
        _context.Steps.Add(step);
        _context.SaveChanges();
    }

    public void DeleteStep(Step step)
    {
        _context.Steps.Remove(step);
        _context.SaveChanges();
    }

    public void EditStep(Step step)
    {
        var StepEntity = _context.Steps.Where(s => s.StepID == step.StepID);

        if (StepEntity != null)
        {




            _context.SaveChanges();
        }
    }
}
