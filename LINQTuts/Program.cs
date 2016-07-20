using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQTuts
{
    class Program
    {
        static void Main(string[] args)
        {
            QueryingCustomTypes();

            //fill devs in type arraylist
            ArrayList devs = new ArrayList();
            devs.Add(new Developer { Name = "User1", Language = "Java", Age = 21});
            devs.Add(new Developer { Name = "User2", Language = "C#", Age = 32 });
            devs.Add(new Developer { Name = "User3", Language = "PHP", Age = 28 });
            devs.Add(new Developer { Name = "User4", Language = "ROR", Age = 14 });

            var devsUsingJava = from Developer d in devs
                                group d by d.Language into x
                                select new
                                {
                                    Language = x.Key,
                                    DevCount = x.Count()
                                }; 

            foreach(var dev in devsUsingJava)
            {
                Console.WriteLine("Language: " + dev.Language);
            }


            QueryWithJoin();

            //inner join
            InnerJoinClause();

            //
            Console.ReadKey();
        }

        static void QueryingCustomTypes()
        {
            List<Employee> employees = SeedEmployees();

            //IEnumerable<Employee> projEmp = from emp in employees
            //                                select emp;

            //custom projections
            //IEnumerable<Employee> projEmp = from emp in employees
            //                                select new Employee
            //                                {
            //                                    Name = emp.Name,
            //                                    Age = emp.Age,
            //                                    Positions = emp.Positions
            //                                };

            //custom projections on different type
            IEnumerable<PlayerViewModel> projEmp = from emp in employees
                                                   select new PlayerViewModel
                                                   {
                                                       PlayerName = emp.Name,
                                                       PlayerAge = emp.Age,
                                                       PlayerPositions = emp.Positions
                                                   };

            Console.WriteLine("==== Querying Custom Types ====");

            foreach(PlayerViewModel e in projEmp)
            {
                Console.WriteLine();
                Console.WriteLine("Name: " + e.PlayerName);
                Console.WriteLine("Age: " + e.PlayerAge);

                foreach(var p in e.PlayerPositions)
                {
                    Console.WriteLine("Title: " + p.Title + " - Level: " + p.Level);
                }
            }
        }

        static List<Employee> SeedEmployees()
        {
            //collection initialization
            return new List<Employee>
            {
                //obj init
                new Employee
                {
                    Name = "Carmelo Anthony",
                    Age = 32,
                    Positions = new List<Position>
                    {
                        new Position {Title = "SF", Level = 3},
                        new Position {Title = "PF", Level = 2}
                    }
                },
                
                new Employee
                {
                    Name = "Lebron James",
                    Age = 32,
                    Positions = new List<Position>
                    {
                        new Position {Title = "SF", Level = 3},
                        new Position {Title = "PG", Level = 2},
                        new Position {Title = "PF", Level = 3},
                        new Position {Title = "SG", Level = 2}
                    }
                },

                new Employee
                {
                    Name = "Steph Curry",
                    Age = 27,
                    Positions = new List<Position>
                    {
                        new Position { Title = "PG", Level = 3},
                        new Position { Title = "SG", Level = 3}
                    }
                }
            };
        }

        static void QueryWithJoin()
        {
            Customer[] customers = new Customer[]
            {
                new Customer
                {
                    Name = "Jon",
                    City = "Angeles",
                    Country = Countries.Germany,
                    Orders = new Order[] 
                    {
                        new Order {OrderID = 1, EuroAmount = 100, Description = "Order 1"},
                        new Order {OrderID = 2, EuroAmount = 200, Description = "Order 2"},
                        new Order {OrderID = 3, EuroAmount = 300, Description = "Order 3"}
                    }
                },
                new Customer
                {
                    Name = "Marco",
                    City = "Baguio",
                    Country = Countries.PHL,
                    Orders = new Order[]
                    {
                        new Order { OrderID = 4, EuroAmount = 400, Description = "Order 4"},
                        new Order { OrderID = 5, EuroAmount = 500, Description = "Order 5"},
                        new Order { OrderID = 6, EuroAmount = 600, Description = "Order 6"}
                    }
                }
            };

            //iterate
            var ordersQuery = from c in customers
                              from o in c.Orders
                              orderby c.Name descending, o.EuroAmount descending
                              select new
                              {
                                  c.Name,
                                  o.OrderID,
                                  o.EuroAmount,
                                  o.Description
                              };

            Console.WriteLine("========= Orders Query ========");

            foreach(var item in ordersQuery)
            {
                Console.WriteLine(item);
            }
        }

        static void InnerJoinClause()
        {
            Category[] categories = new Category[]
            {
                new Category { CategoryID = 1, Name = "CLAB"},
                new Category { CategoryID = 2, Name = "RFAB"},
                new Category { CategoryID = 3, Name = "IT Services"}
            };

            Product[] products = new Product[]
            {
                new Product { CategoryID = 1, ProductID = "11", Description = "Desc1"},
                new Product { CategoryID = 1, ProductID = "1.1.1", Description = "Desc1.1.1"},
                new Product { CategoryID = 2, ProductID = "22", Description = "Desc2"},
                new Product { CategoryID = 2, ProductID = "2.2.2", Description = "Description 2.2.2"}
            };

            //left join
            var q = from c in categories
                    join p in products on c.CategoryID equals p.CategoryID into productsByCategory
                    from pc in productsByCategory.DefaultIfEmpty
                    (
                        new Product
                        {
                            CategoryID = 0,
                            ProductID = String.Empty,
                            Description = String.Empty
                        }
                    )
                    select new
                    {
                        c.CategoryID,
                        CategoryName = c.Name,
                        Product = pc.Description
                    };

            Console.WriteLine();
            Console.WriteLine("==== INNER JOIN =======");

            foreach(var item in q)
            {
                Console.WriteLine(item);
            }

            // let clause - store the result of subexpression in a variable
            // that can be used somewhere else in the query
            var categoriesByProductNumber =
                from c in categories
                join p in products on c.CategoryID equals p.CategoryID into productsByCategory
                let ProductsCount = productsByCategory.Count()
                select new
                {
                    c.CategoryID,
                    ProductsCount
                };

            Console.WriteLine();
            Console.WriteLine("==== LET CLAUSE =======");

            foreach(var i in categoriesByProductNumber)
            {
                Console.WriteLine(i);
            }
        }
    }

    class PlayerViewModel
    {
        public string PlayerName;
        public int PlayerAge;
        public List<Position> PlayerPositions;
    }

    class Developer
    {
        private string _name;
        private string _language;
        private int _age;

        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public String Language
        {
            get { return _language; }
            set { _language = value; }
        }

        public Int32 Age { get; set; }
    }

    class Customer
    {
        public String Name { get; set; }
        public String City { get; set; }
        public Countries Country { get; set; }
        public Order[] Orders { get; set; }
    }

    class Order
    {
        public Int32 OrderID { get; set; }
        public Int32 Quantity { get; set; }
        public Boolean Shipped { get; set; }
        public String Month { get; set; }
        public int ProductID { get; set; }
        public Decimal EuroAmount { get; set; }
        public String Description { get; set; }
    }

    class Category
    {
        public Int32 CategoryID { get; set; }
        public String Name { get; set; }
    }

    class Product
    {
        public String ProductID { get; set; }
        public Int32 CategoryID { get; set; }
        public Decimal Price { get; set; }
        public String Description { get; set; }
    }

    enum Countries
    {
        PHL,
        USA,
        Germany
    }
}
