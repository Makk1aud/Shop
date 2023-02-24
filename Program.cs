using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Diagnostics;

namespace Shestyrochka
{
    internal class Program
    {
        

        class Shop
        {
            // создания словарей в которые будет помещена информация о товаре
            public static Dictionary<string, int> Amount = new Dictionary<string, int>();
            public static Dictionary<string, int> goods = new Dictionary<string, int>();
            private static string PathToFile = @"Path to file with products";
            private static StreamReader sr_read = new StreamReader(PathToFile);

            private static string PathToFile_Write = @"Path to file with bought products";
            public static StreamWriter sr_write = new StreamWriter(PathToFile_Write, false);

            private int count = 1;
            private float Total;
            public float wallet { get; set; }
            public string name { get; set; }

            // генерация словаря при помощи информации из файла и функции рандома для цен
            static Shop()
            {
                var rand = new Random();
                
                if(File.Exists(PathToFile))
                {
                    string line = sr_read.ReadLine();
                    
                    while(line != null)
                    {
                        goods.Add(line.Remove(line.Length - 2), rand.Next(10, 100));
                        Amount.Add(line.Remove(line.Length - 2), int.Parse(line[line.Length - 1].ToString()));
                        line = sr_read.ReadLine();
                        
                        
                    }
                }
                sr_read.Close();
                

            }


            // конструктор который запрашивает данные о балансе и имя
            public Shop(float wallet, string name)
            {
                Total = this.wallet = wallet;
                this.name = name;
                ListGoods();
                Console.WriteLine($"\nНа балансе {wallet}\n\n");
            }


            // вывод товаров в консоль
            public void ListGoods()
            {
                int count = 1;
                foreach(var item in goods)
                {
                    if(item.Value != 0)
                    {
                        Console.WriteLine($"{count}) {item.Key} {item.Value} р.");
                        
                    }
                    count++;
                }
                if (goods.Count == 0)
                {
                    EndOfCheck();

                }
            }

            // покупка товара по названию
            public void BuySmth(string product)
            {
                // проверка на наличия товара в словаре по ключу, также проверка на баланс
                if(goods.ContainsKey(product) && goods[product] < wallet)
                {
                    Console.WriteLine($"\nВы купили {product} за {goods[product]} р.\nНа балансе осталось {wallet - goods[product]}\n");
                    sr_write.WriteLine($"{count}) {product} {goods[product]}");
                    wallet -= goods[product];
                    count++;
                    
                    Amount[product] -= 1;
                    if (Amount[product] == 0) 
                        goods.Remove(product);
                    
                    
                }
                else if(goods.ContainsKey(product))
                    Console.WriteLine($"Не хватает {goods[product] - wallet}");
                else
                {
                    Console.WriteLine("Товар закончился");
                    EndOfCheck();
                }
                    

            }

            // Генерация ИТОГА в чеке
            public void EndOfCheck()
            {
                sr_write.WriteLine($"...............................\nИТОГ {Total - wallet} р.");
                sr_write.Close();
                System.Environment.Exit(0);
            }
        }
        static void Main(string[] args)
        {
            var person = new Shop(2000f, "Oleg");
            bool YesNo = true;
            string product;
            while(YesNo)
            {
                Console.WriteLine("\nХотите преобрести товар?");
                if(Console.ReadLine() == "да")
                {
                    Console.WriteLine("Какой?");
                    product = Console.ReadLine();
                    person.BuySmth(product);
                    person.ListGoods();
                }
                else YesNo = false;
                
            }
            person.EndOfCheck();
        }
    }
}
