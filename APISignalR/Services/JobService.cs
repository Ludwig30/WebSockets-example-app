using ClassLibraryJobs;

namespace APISignalR.Services
{
    public class JobService
    {
        private readonly List<Job> _jobs = [];

        public void QueueJob(Job job) { _jobs.Add(job); }
        public List<Job> GetJobs() { return _jobs; }
        public void ClearQueue() { _jobs.Clear(); }
        public void UpdateJob(Job job, int i) { _jobs[i] = job; }
        public Job? GetJob(string id) { return _jobs.Find(job => job.GetId() == id); }
    }
}
