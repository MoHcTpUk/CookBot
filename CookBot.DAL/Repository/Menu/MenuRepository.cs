﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CookBot.DAL.Repository.Menu
{
    public class MenuRepository : IMenuRepository
    {
        private string ConfigFile { get; } = "config.json";

        public List<string> GetMenu(DateTime date)
        {
            var weekNumber = new GregorianCalendar().GetWeekOfYear(date, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
            var menu = new List<string>();
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(ConfigFile)
                .Build();

            const string section = "Menu";
            var weekType = weekNumber % 2 == 0 ? "Even" : "NotEven";
            var day = date.DayOfWeek.ToString();

            try
            {
                menu = configuration.GetSection(section).GetSection(weekType).GetSection(day).Get<string[]>().ToList();
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error while reading 'Menu' section in " + ConfigFile + ": " + exception.Message);
            }

            return menu;
        }

    }
}
