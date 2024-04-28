using APISignalR.Services;
using ClassLibraryJobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace SignalRApp
{
    public class JobHub : Hub
    {
        public async Task AddJob([FromServices] JobService jobService, int duration)
        {
            string id = Guid.NewGuid().ToString();
            Console.WriteLine(duration);
            Job job = new(id, duration, JobStatus.Pending);
            jobService.QueueJob(job);
            Console.WriteLine(jobService.GetJobs().Count());
            Console.WriteLine(job.ToString());
            await Clients.Caller.SendAsync("JobAdded", id);
            Console.WriteLine("client invoked");
        }

        public async Task StartJob([FromServices] JobService jobService, string id)
        {
            Job? job = jobService.GetJob(id);
            if (job != null)
            {
                await Clients.Caller.SendAsync("JobStarted", job.GetId());
                await job.StartAsync();
                await Clients.Caller.SendAsync("JobComplete", job.GetId());
            }
        }        

        public async Task CancelJob([FromServices] JobService jobService, string id)
        {

            Job? job = jobService.GetJob(id);
            if (job != null)
            {
                int i = jobService.GetJobs().IndexOf(job);
                job.CancelJob();
                jobService.UpdateJob(job, i);
                await Clients.Caller.SendAsync($"job with id {id} cancelled");
            }
            else
            {
                throw new HubException($"Failed to update job with id {id}");
            }
        }        
    }
}
