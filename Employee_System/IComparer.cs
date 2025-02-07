namespace ClassLibraryEmployee
{
    public class SortByName : IComparer<Employee>
    {
        public int Compare(Employee x, Employee y)
        {
            if (x == null || y == null)
                return x == null ? (y == null ? 0 : -1) : 1;

            return string.Compare(x.Name, y.Name, StringComparison.Ordinal);
        }
    }

    public class SortBySalary : IComparer<Employee>
    {
        public int Compare(Employee x, Employee y)
        {
            if (x == null || y == null)
                return x == null ? (y == null ? 0 : -1) : 1;

            return x.Salary.CompareTo(y.Salary);
        }
    }

    public class SortByAge : IComparer<Employee>
    {
        public int Compare(Employee x, Employee y)
        {
            if (x == null || y == null)
                return x == null ? (y == null ? 0 : -1) : 1;

            return x.Age.CompareTo(y.Age);
        }
    }
}
