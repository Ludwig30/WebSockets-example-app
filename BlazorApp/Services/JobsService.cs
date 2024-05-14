using ClassLibraryJobs;
using System.Text;

namespace BlazorApp.Services
{
    public class JobsService
    {
        private readonly List<Job> _jobs = [];

        public void AddJob(Job job) { _jobs.Add(job); }
        public List<Job> GetJobs() { return _jobs; }

        public Job? GetJob(string id)
        {
            return _jobs.FirstOrDefault(job => String.Equals(job.GetId(), id));
        }
        public void ModifyJob(string id, JobStatus status)
        {
            int i = _jobs.FindIndex(j => String.Equals(j.GetId(), id));
            if (i != -1)
            {
                _jobs[i].SetStatus(status);                
            }
        }
    }
}
