using System;
using System.Collections.Generic;

namespace CookBot.DAL.Repository.Menu
{
    public interface IMenuRepository
    {
        List<string> GetMenu(DateTime date);
    }
}
