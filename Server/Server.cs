using System.Net;
using System.Net.Sockets;
using System.Text;
using EmployeeClass;
using static FileHandlerClass.FileHandler;
using NLog;

public class Server
{
    private static ManualResetEvent allDone = new ManualResetEvent(false);
    private static UdpClient _client;

    private static string _filePath = @"/Users/wonderfau1t/Study/Архитектура ИС/IS_Lab2/Server/employees.csv";
    private List<Employee> listOfEmployees = ReadFromFile(_filePath);

    Logger logger = LogManager.GetCurrentClassLogger();

    public Server(int _port)
    {
        _client = new UdpClient(_port);
        Console.WriteLine("Server is running...");
    }

    public void StartListeningAsync()
    {
        while (true)
        {
            allDone.Reset();
            _client.BeginReceive(RequestCallback, _client);
            allDone.WaitOne();
        }
    }

    private void RequestCallback(IAsyncResult ar)
    {
        allDone.Set();
        var listener = (UdpClient)ar.AsyncState!;
        var ep = (IPEndPoint)_client.Client.LocalEndPoint!;
        var res = listener.EndReceive(ar, ref ep);
        string request = Encoding.Unicode.GetString(res);

        string responseMessage = "Invalid request";
        switch (request.Split(' ')[0])
        {
            case "get_list_of_employees":
                responseMessage = GetListOfEmployees();
                break;
            case "get_employee_by_index":
                responseMessage = GetEmployeeByIndex(int.Parse(request.Split(' ')[1]));
                break;
            case "add_new_employee":
                responseMessage = AddNewEmployee(request.Split(' ')[1]);
                break;
            case "delete_employee_by_index":
                responseMessage = DeleteEmployeeByIndex(int.Parse(request.Split(' ')[1]));
                break;
            case "save_changes":
                WriteToFile(_filePath, listOfEmployees);
                logger.Info("Changes were saved to file.");
                responseMessage = "Change saved successfully";
                break;
        }

        var response = Encoding.Unicode.GetBytes(responseMessage);
        _client.SendAsync(response, response.Length, ep);
    }

    private string GetListOfEmployees()
    {
        var employees = new StringBuilder();
        for (var i = 0; i < listOfEmployees.Count; i++)
        {
            employees.AppendLine($"{i} {listOfEmployees[i].ToString()}");
        }

        logger.Info("Get list of employees");
        return employees.ToString();
    }

    private string GetEmployeeByIndex(int index)
    {
        if (index < 0 || index >= listOfEmployees.Count)
        {
            logger.Warn("Get employee by index: Index out of range");
            return "Index out of range";
        }

        logger.Info("Get employee by index");
        return listOfEmployees[index].ToString();
    }

    private string AddNewEmployee(string info)
    {
        var data = info.Split('_');
        listOfEmployees.Add(new Employee(data[0], data[1], int.Parse(data[2]), int.Parse(data[3]),
            bool.Parse(data[4])));
        logger.Info("Added new employee");
        return "Added Successfully";
    }

    private string DeleteEmployeeByIndex(int index)
    {
        if (index < 0 || index >= listOfEmployees.Count)
        {
            logger.Warn("Delete employee by index: Index out of range");
            return "Index out of range";
        }

        listOfEmployees.RemoveAt(index);
        logger.Info("Deleted employee by index");
        return "Deleted Successfully";
    }
}

class Program
{
    private static void Main(string[] args)
    {
        var server = new Server(8001);
        server.StartListeningAsync();
    }
}