using System.Net;
using System.Net.Sockets;
using System.Text;

class Client
{
    private static UdpClient _client = new UdpClient(8002);

    public static void Main(string[] args)
    {
        Console.WriteLine("Client is running...");
        var key = ConsoleKey.Enter;
        while (key != ConsoleKey.Escape)
        {
            PrintMenu();
            key = Console.ReadKey().Key;
            Console.WriteLine();
            int index;
            switch (key)
            {
                case ConsoleKey.D1:
                    SendMessage("get_list_of_employees");
                    ReceiveMessage();

                    break;
                case ConsoleKey.D2:
                    Console.Write("Enter index: ");
                    if (int.TryParse(Console.ReadLine(), out index))
                    {
                        SendMessage($"get_employee_by_index {index}");
                    }
                    else
                    {
                        Console.WriteLine("Invalid index format");
                        break;
                    }

                    ReceiveMessage();
                    break;

                case ConsoleKey.D3:
                    Console.Write("First name: ");
                    var firstName = Console.ReadLine();

                    Console.Write("Last name: ");
                    var lastName = Console.ReadLine();

                    int age;
                    while (true)
                    {
                        Console.Write("Age: ");
                        if (int.TryParse(Console.ReadLine(), out age))
                        {
                            break;
                        }

                        Console.WriteLine("Invalid input. Please enter a valid age.");
                    }

                    int salary;
                    while (true)
                    {
                        Console.Write("Salary: ");
                        if (int.TryParse(Console.ReadLine(), out salary))
                        {
                            break;
                        }

                        Console.WriteLine("Invalid input. Please enter a valid salary.");
                    }

                    bool isWorkOnWeekends;
                    while (true)
                    {
                        Console.Write("Working on weekends? (true or false): ");
                        var input = Console.ReadLine();
                        if (bool.TryParse(input, out isWorkOnWeekends))
                        {
                            break;
                        }

                        Console.WriteLine("Invalid input. Please enter 'true' or 'false'.");
                    }

                    SendMessage($"add_new_employee {firstName}_{lastName}_{age}_{salary}_{isWorkOnWeekends}");
                    ReceiveMessage();
                    break;

                case ConsoleKey.D4:
                    Console.Write("Enter index: ");
                    if (int.TryParse(Console.ReadLine(), out index))
                    {
                        SendMessage($"delete_employee_by_index {index}");
                    }
                    else
                    {
                        Console.WriteLine("Invalid index format");
                        break;
                    }

                    ReceiveMessage();
                    break;

                case ConsoleKey.D5:
                    SendMessage("save_changes");
                    ReceiveMessage();
                    break;
            }
        }
    }

    private static void PrintMenu()
    {
        Console.WriteLine("--------------------");
        Console.WriteLine("1. Print all employees");
        Console.WriteLine("2. Print employee by index");
        Console.WriteLine("3. Add new employee");
        Console.WriteLine("4. Delete employee");
        Console.WriteLine("5. Save changes");
        Console.WriteLine("----- ESC for exit -----");
    }

    private static void SendMessage(string message)
    {
        try
        {
            var data = Encoding.Unicode.GetBytes(message);
            _client.Send(data, data.Length, "127.0.0.1", 8001);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    private static void ReceiveMessage()
    {
        var remoteEp = (IPEndPoint)_client.Client.LocalEndPoint!;
        try
        {
            var data = _client.Receive(ref remoteEp);
            var message = Encoding.Unicode.GetString(data);
            Console.WriteLine(message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
}