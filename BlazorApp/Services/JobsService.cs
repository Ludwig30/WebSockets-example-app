using ClassLibraryJobs;
using System.Text;

namespace BlazorApp.Services
{
    public class JobsService
    {
        private readonly List<Job> _jobs = [];

        public void AddJob(Job job) { _jobs.Add(job); }
        public List<Job> GetJobs() { return _jobs; }
        public void ModifyJob(string id, JobStatus status)
        {
            int i = _jobs.FindIndex(j => String.Equals(j.GetId(), id));
            Console.WriteLine(i);
            if (i != -1)
            {
                _jobs[i].SetStatus(status);                
            }
        }
    }
}
