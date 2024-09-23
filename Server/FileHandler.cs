using EmployeeClass;

namespace FileHandlerClass
{
    public class FileHandler
    {
        public static List<Employee> ReadFromFile(string filePath)
        {
            var employees = new List<Employee>();
            var lines = File.ReadAllLines(filePath);

            using var sr = new StreamReader(filePath);
            string str;
            while ((str = sr.ReadLine()) != null)
            {
                var parts = str.Split(',');

                var firstName = parts[0];
                var lastName = parts[1];
                var age = int.Parse(parts[2]);
                var salary = int.Parse(parts[3]);
                var isWorkOnWeekends = bool.Parse(parts[4]);

                employees.Add(new Employee(firstName, lastName, age, salary, isWorkOnWeekends));
            }

            return employees;
        }

        public static void WriteToFile(string filePath, List<Employee> employees)
        {
            using var sw = new StreamWriter(filePath);
            foreach (var employee in employees)
            {
                var line = employee.ToCsvString();
                sw.WriteLine(line);
            }
        }
    }
}