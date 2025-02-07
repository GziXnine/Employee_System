using ClassLibraryEmployee;

namespace Employee_System
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Project Employee System For Iti
            // Menu: New, Display, Sort, Search, Exit
            // By Using: C#, Microsoft SQL Server, ADO.NET

            int highlight = 0;
            bool loop = true;
            string[] menu = new string[] { "  New  ", " Display ", "  Sort  ", " Search ", "  Exit  " };
            List<Employee> employees = new List<Employee>();

            while (loop)
            {
                for (int i = 0; i < menu.Length; i++)
                {
                    Console.BackgroundColor = (highlight == i) ? ConsoleColor.Blue : ConsoleColor.Black;
                    Console.SetCursorPosition(60, (i + 1) * (30 / (menu.Length + 1)));
                    Console.WriteLine(menu[i]);
                }

                ConsoleKeyInfo x = Console.ReadKey();

                switch (x.Key)
                {
                    case ConsoleKey.DownArrow:
                        highlight = (highlight + 1) % menu.Length;
                        break;
                    case ConsoleKey.UpArrow:
                        highlight = (highlight - 1 + menu.Length) % menu.Length;
                        break;
                    case ConsoleKey.Home:
                        highlight = 0;
                        break;
                    case ConsoleKey.End:
                        highlight = menu.Length - 1;
                        break;
                    case ConsoleKey.Enter:
                        if (highlight == 0)
                        {
                            Console.Clear();
                            Console.WriteLine("\nNew Employee");
                            Console.Write("Enter Employee Name: ");
                            string name = Console.ReadLine().Trim();

                            Console.Write("Enter Employee Salary: ");
                            float salary = float.Parse(Console.ReadLine());

                            Console.Write("Enter Employee Age: ");
                            int age = int.Parse(Console.ReadLine());

                            Console.Write("Enter Employee Gender (M/F): ");
                            char genderInput = char.ToUpper(Console.ReadKey().KeyChar);
                            Console.WriteLine();
                            Gender gender = (genderInput == 'M') ? Gender.Male : Gender.Female;
                            employees.Add(new Employee(name, salary, gender, age));

                            Console.ReadLine();
                            Console.Clear();
                        }
                        else if (highlight == 1)
                        {
                            Console.Clear();
                            foreach (Employee emp in employees)
                                emp.Display();

                            Console.ReadLine();
                            Console.Clear();
                        }
                        else if (highlight == 2) // Sort 
                        {
                            Console.Clear();
                            Console.WriteLine("Choose Sorting Method:");
                            Console.WriteLine("1 - Sort by ID (Default)");
                            Console.WriteLine("2 - Sort by Name");
                            Console.WriteLine("3 - Sort by Salary");
                            Console.WriteLine("4 - Sort by Age");
                            Console.Write("Enter your choice: ");
                            int choice = int.Parse(Console.ReadLine());

                            switch (choice)
                            {
                                case 1:
                                    employees.Sort();
                                    break;
                                case 2:
                                    employees.Sort(new SortByName());
                                    break;
                                case 3:
                                    employees.Sort(new SortBySalary());
                                    break;
                                case 4:
                                    employees.Sort(new SortByAge());
                                    break;
                                default:
                                    Console.WriteLine("Invalid choice!");
                                    break;
                            }

                            Console.WriteLine("Employees Sorted Successfully!");
                            Console.ReadLine();
                            Console.Clear();
                        }
                        else if (highlight == 3) // Search
                        {
                            Console.Clear();
                            Console.WriteLine("Choose Search Method:");
                            Console.WriteLine("1 - Search By ID");
                            Console.WriteLine("2 - Search By Name");
                            Console.Write("Enter your choice: ");
                            int choice = int.Parse(Console.ReadLine());

                            if (choice == 1)
                            {
                                Console.Write("Enter ID to search: ");
                                int id = int.Parse(Console.ReadLine());

                                Employee found = null;
                                foreach (var emp in employees)
                                {
                                    if (emp.ID == id)
                                    {
                                        found = emp;
                                        break;
                                    }
                                }
                                if (found != null)
                                    found.Display();
                                else
                                    Console.WriteLine("Employee not found!");
                            }
                            else if (choice == 2)
                            {
                                Console.Write("Enter Name to search: ");
                                string name = Console.ReadLine();

                                Employee foundByName = null;
                                foreach (var emp in employees)
                                {
                                    if (emp.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                                    {
                                        foundByName = emp;
                                        break;
                                    }
                                }
                                if (foundByName != null)
                                    foundByName.Display();
                                else
                                    Console.WriteLine("Employee not found!");
                            }
                            Console.ReadLine();
                            Console.Clear();
                        }
                        else if (highlight == menu.Length - 1)
                            loop = false;
                        break;
                }
            }
        }
    }
}
