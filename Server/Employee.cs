namespace EmployeeClass
{
    public class Employee(string name, string lastName, int age, int salary, bool isWorkOnWeekends)
    {
        private string FirstName { get; set; } = name;
        private string LastName { get; set; } = lastName;
        private int Age { get; set; } = age;
        private int Salary { get; set; } = salary;
        private bool IsWorkOnWeekends { get; set; } = isWorkOnWeekends;

        public override string ToString()
        {
            return
                $"{FirstName} {LastName} | Age: {Age} | Salary {Salary} | {(IsWorkOnWeekends ? "Works on weekends" : "Not works on weekends")}";
        }

        public string ToCsvString()
        {
            return $"{FirstName},{LastName},{Age},{Salary},{IsWorkOnWeekends}";
        }
    }
}