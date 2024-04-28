namespace ClassLibraryJobs
{
    public class Job
    {
        private string _id;
        private int _duration;
        private JobStatus _jobStatus = JobStatus.Pending;

        public Job(string id, int duration, JobStatus jobStatus = JobStatus.Pending)
        {
            _id = id;
            _duration = duration;
            _jobStatus = jobStatus;
        }

        public string GetId() { return _id; }
        public int GetDuration() { return _duration; }
        public JobStatus GetStatus() { return _jobStatus; }
        public void SetStatus(JobStatus status) { _jobStatus = status; }

        public void CancelJob()
        {
            _jobStatus = JobStatus.Cancelled;
        }

        public override string ToString()
        {
            return $"Job Id: {_id}, Duration: {_duration} ms, Status: {_jobStatus}";
        }

        public async Task StartAsync()
        {
            _jobStatus = JobStatus.Running;
            await Run();
        }

        public async Task Run()
        {
            await Task.Delay(_duration);
            _jobStatus = JobStatus.Complete;
        }
    }
}
