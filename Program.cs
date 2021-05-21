using CookBot.Class;
using Quartz;
using Quartz.Impl;
using Quartz.Logging;
using System.Threading.Tasks;

namespace CookBot
{
    class Program
    {
        public static async Task Main()
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

            // Trigger the job to run now, and then repeat every 10 seconds
            ITrigger trigger = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule(s => s
                    .OnEveryDay()
                    .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(17, 50)))
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