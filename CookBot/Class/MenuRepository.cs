using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace CookBot.Class
{
    public static class MenuRepository
    {
        private static readonly List<DayOfWeek> ListDayOfWeek = new()
        {
            DayOfWeek.Monday,
            DayOfWeek.Tuesday,
            DayOfWeek.Wednesday,
            DayOfWeek.Thursday,
            DayOfWeek.Friday
        };

        public static Dictionary<DayOfWeek, List<string>> GetMenu(DateTime date)
        {
            Dictionary<DayOfWeek, List<string>> menuEven = new Dictionary<DayOfWeek, List<string>>();
            Dictionary<DayOfWeek, List<string>> menuNotEven = new Dictionary<DayOfWeek, List<string>>();

            var configuration = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(
                    Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly()?.Location) ?? string.Empty,
                    Program.ConfigFile))
                .Build();

            try
            {
                menuEven = ParseMenu(configuration.GetSection("Menu").GetSection("Even"));
                menuNotEven = ParseMenu(configuration.GetSection("Menu").GetSection("NotEven"));
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error while reading 'Menu' section in " + Program.ConfigFile + ": " + exception.Message);
            }

            var weekNumber = new GregorianCalendar().GetWeekOfYear(date, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);

            return weekNumber / 2 == 0 ? menuEven : menuNotEven;
        }

        private static Dictionary<DayOfWeek, List<string>> ParseMenu(IConfigurationSection menuConfigurationSection)
        {
            var menu = new Dictionary<DayOfWeek, List<string>>();

            foreach (var dayOfWeek in ListDayOfWeek)
            {
                var monday = menuConfigurationSection.GetSection(dayOfWeek.ToString());
                var listOfDish = monday.Get<string[]>().ToList();
                menu.Add(dayOfWeek, listOfDish);
            }

            return menu;
        }
    }
}
