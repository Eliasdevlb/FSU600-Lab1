namespace Benchmark
{
    class Employee
    {
        private string name { get; set; }

        public string GetName()
        {
            return name;
        }

        public void SetName(string value)
        {
            name = value;
        }

        private string department { get; set; }

        public string GetDepartment()
        {
            return department;
        }

        public void SetDepartment(string value)
        {
            department = value;
        }

        public int YearsOfExperience { get; set; }
    }
}
