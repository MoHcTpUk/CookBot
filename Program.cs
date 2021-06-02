using CookBot.Class;
using Microsoft.Extensions.Configuration;
using Quartz;
using Quartz.Impl;
using Quartz.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CookBot
{
    class Program
    {
        public static string ConfigFile { get; } = "config.json";

        private static string ReadTimeZone(IConfigurationRoot configuration)
        {
            try
            {
                return configuration.GetValue<string>("TimeZone");
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error while reading 'TimeZone' section in " + ConfigFile + ": " + exception.Message);
            }

            return null;
        }

        private static (int?, int?) ReadScheduler(IConfigurationRoot configuration)
        {
            try
            {
                var schedulerConfiguration = configuration.GetSection("Scheduler");

                return (schedulerConfiguration.GetValue<int>("Hours"), schedulerConfiguration.GetValue<int>("Minutes"));
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error while reading 'Scheduler' section in " + ConfigFile + ": " + exception.Message);
            }

            return (null, null);
        }

        public static async Task Main()
        {
            BotClient.Start();

            var configuration = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly()?.Location) ?? string.Empty, ConfigFile))
                .Build();

            int? hours;
            int? minutes;

            (hours, minutes) = ReadScheduler(configuration);
            string timeZoneId = ReadTimeZone(configuration);

            if (minutes != null && hours != null && !string.IsNullOrEmpty(timeZoneId))
            {
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
                        .AtHourAndMinuteOnGivenDaysOfWeek(hours.Value, minutes.Value, DayOfWeek.Monday, DayOfWeek.Tuesday,
                            DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday)
                        .InTimeZone(TimeZoneInfo.FindSystemTimeZoneById(timeZoneId)))
                    .Build();


                // Tell quartz to schedule the job using our trigger
                await scheduler.ScheduleJob(job, trigger);

                // some sleep to show what's happening
                await Task.Delay(-1);

                // and last shut down the scheduler when you are ready to close your program
                await scheduler.Shutdown();
            }
            else
            {
                Console.WriteLine($@"Error reading configuration file {ConfigFile}: Hours, Minutes or TimeZone" );
            }
        }
    }
}