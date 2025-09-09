using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YemekTarifleri.Data.Abstract;

namespace YemekTarifleri.ViewComponents
{
    public class FtypesMenu : ViewComponent
    {
        private IFtypeRepository _ftypeRepository;
        public FtypesMenu(IFtypeRepository ftypeRepository)
        {
            _ftypeRepository = ftypeRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var ftypes = _ftypeRepository.Ftypes;
            return View(await ftypes.ToListAsync());
        }
    }
}