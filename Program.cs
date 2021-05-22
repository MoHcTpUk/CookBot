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
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly()?.Location) ?? string.Empty, ConfigFile))
                .Build();

            var timeZoneId = configuration.GetValue<string>("TimeZone");

            var schedulerConfiguration = configuration.GetSection("Scheduler");

            var hours = schedulerConfiguration.GetValue<int>("Hours");
            var minutes = schedulerConfiguration.GetValue<int>("Minutes");

            LogProvider.SetCurrentLogProvider(new ConsoleLogProvider());

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
                .WithSchedule(CronScheduleBuilder
                    .DailyAtHourAndMinute(hours, minutes)
                    .InTimeZone(TimeZoneInfo.FindSystemTimeZoneById(timeZoneId)))
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