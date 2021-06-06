using CookBot.App.Options;
using MediatR;
using Microsoft.Extensions.Configuration;
using Quartz;
using Quartz.Impl;
using Quartz.Logging;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CookBot.App.Quartz;
using CookBot.App.Quartz.Jobs;

namespace CookBot.App.Commands.Scheduler
{
    public class SchedulerAutoPostPollSetupRequest : IRequest
    {
    }

    public class SchedulerAutoPostPollSetupRequestHandler : IRequestHandler<SchedulerAutoPostPollSetupRequest>
    {
        private readonly IConfiguration _configuration;
        private SchedulerOptions _schedulerOptions;
        private TimeZoneOptions _timeZoneOptions;

        public SchedulerAutoPostPollSetupRequestHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<Unit> Handle(SchedulerAutoPostPollSetupRequest request, CancellationToken cancellationToken)
        {
            _schedulerOptions = _configuration.GetSection(SchedulerOptions.Scheduler).Get<SchedulerOptions>();
            _timeZoneOptions = _configuration.GetSection(TimeZoneOptions.TimeZone).Get<TimeZoneOptions>();

            var timeZoneId = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? _timeZoneOptions.Windows : _timeZoneOptions.Linux;

            LogProvider.SetCurrentLogProvider(new ConsoleLogProvider());

            // Grab the Scheduler instance from the Factory
            StdSchedulerFactory factory = new StdSchedulerFactory();
            IScheduler scheduler = await factory.GetScheduler(cancellationToken);

            // and start it off
            await scheduler.Start(cancellationToken);

            // define the job and tie it to our HelloJob class
            IJobDetail job = JobBuilder.Create<SendCookingPoolJob>()
                .Build();

            // Trigger the job to run everyday
            ITrigger trigger = TriggerBuilder.Create()
                //.WithSchedule(CronScheduleBuilder
                    //.AtHourAndMinuteOnGivenDaysOfWeek(_schedulerOptions.Hours, _schedulerOptions.Minutes, DayOfWeek.Monday, DayOfWeek.Tuesday,
                    //    DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday)
                    //.InTimeZone(TimeZoneInfo.FindSystemTimeZoneById(timeZoneId)))
                .Build();

            // Tell quartz to schedule the job using our trigger
            await scheduler.ScheduleJob(job, trigger, cancellationToken);

            return Unit.Value;
        }
    }
}
