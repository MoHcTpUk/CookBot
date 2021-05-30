using System;
using System.IO;
using CookBot.Class;
using Quartz;
using Quartz.Impl;
using Quartz.Logging;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace CookBot
{
    class Program
    {
        public static string ConfigFile { get; } = "config.json";

        public static async Task Main()
        {
            int hours;
            int minutes;
            string timeZoneId;

            var configuration = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly()?.Location) ?? string.Empty, ConfigFile))
                .Build();

            try
            {
                timeZoneId = configuration.GetValue<string>("TimeZone");
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error while reading 'TimeZone' section in " + ConfigFile + ": " + exception.Message);
                return;
            }

            try
            {
                var schedulerConfiguration = configuration.GetSection("Scheduler");

                hours = schedulerConfiguration.GetValue<int>("Hours");
                minutes = schedulerConfiguration.GetValue<int>("Minutes");
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error while reading 'Scheduler' section in " + ConfigFile + ": " + exception.Message);
                return;
            }

            //LogProvider.SetCurrentLogProvider(new ConsoleLogProvider());

            // Grab the Scheduler instance from the Factory
            StdSchedulerFactory factory = new StdSchedulerFactory();
            IScheduler scheduler = await factory.GetScheduler();

            // and start it off
            await scheduler.Start();

            // define the job and tie it to our HelloJob class
            IJobDetail job = JobBuilder.Create<SendCookingPoolJob>()
                .Build();

            // Trigger the job to run everyday
            ITrigger trigger = TriggerBuilder.Create()
                //.WithSchedule(CronScheduleBuilder
                //    .AtHourAndMinuteOnGivenDaysOfWeek(hours, minutes, DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday)
                //    .InTimeZone(TimeZoneInfo.FindSystemTimeZoneById(timeZoneId)))
                .Build();

            // Tell quartz to schedule the job using our trigger
            await scheduler.ScheduleJob(job, trigger);

            // some sleep to show what's happening
            await Task.Delay(-1);

            // and last shut down the scheduler when you are ready to close your program
            await scheduler.Shutdown();
        }
    }
}