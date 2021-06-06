using CookBot.App.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using System;
using System.Runtime.InteropServices;

namespace CookBot.App.Quartz.Jobs
{
    public static class SendCookingPoolJobScheduler
    {

        public static async void Start(IServiceProvider serviceProvider)
        {
            var configuration = serviceProvider.GetService<IConfiguration>();

            if (configuration != null)
            {
                var schedulerOptions = configuration.GetSection(SchedulerOptions.Scheduler).Get<SchedulerOptions>();
                var timeZoneOptions = configuration.GetSection(TimeZoneOptions.TimeZone).Get<TimeZoneOptions>();

                var timeZoneId = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                    ? timeZoneOptions.Windows
                    : timeZoneOptions.Linux;

                var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
                scheduler.JobFactory =
                    serviceProvider.GetService<JobFactory>() ?? throw new InvalidOperationException();
                await scheduler.Start();

                var jobDetail = JobBuilder.Create<SendCookingPoolJob>().Build();
                var trigger = TriggerBuilder.Create()
                    .WithSchedule(CronScheduleBuilder
                        .AtHourAndMinuteOnGivenDaysOfWeek(schedulerOptions.Hours, schedulerOptions.Minutes, DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Sunday)
                        .InTimeZone(TimeZoneInfo.FindSystemTimeZoneById(timeZoneId)))
                    .Build();

                await scheduler.ScheduleJob(jobDetail, trigger);
            }
            else
            {
                throw new InvalidOperationException("Service not found!");
            }
        }
    }
}