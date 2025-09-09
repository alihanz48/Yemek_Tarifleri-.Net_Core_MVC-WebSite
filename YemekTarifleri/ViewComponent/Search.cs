using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YemekTarifleri.Data.Abstract;
using YemekTarifleri.Models;

namespace YemekTarifleri.ViewComponents
{
    public class Search : ViewComponent
    {
        private IFtypeRepository _ftypeRepository;
        public Search(IFtypeRepository ftypeRepository)
        {
            _ftypeRepository = ftypeRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(new SearchDataViewModel
            {
                ftypes = await _ftypeRepository.Ftypes.ToListAsync(),
            });
        }
    }
}