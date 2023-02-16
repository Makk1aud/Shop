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
            // создания словаря в который будут помещены товары и цены на них
            
            public static Dictionary<string, int> goods = new Dictionary<string, int>();
            private static string PathToFile = @"E:\Proekts Visual com\Shestyrochka\Goods.txt";
            private static StreamReader sr_read = new StreamReader(PathToFile);

            private static string PathToFile_Write = @"E:\Proekts Visual com\Shestyrochka\Busket.txt";
            public static StreamWriter sr_write = new StreamWriter(PathToFile_Write, false);

            public float wallet { get; set; }
            public string name { get; set; }

            // генерация словаря при помощи файла и функции рандома для цен
            static Shop()
            {
                var rand = new Random();
                
                if(File.Exists(PathToFile))
                {
                    string line = sr_read.ReadLine();
                    while(line != null)
                    {
                        goods.Add(line, rand.Next(0, 100));
                        line = sr_read.ReadLine();
                    }
                }
                sr_read.Close();

            }


            // конструктор который запрашивает данные о балансе и имя, также выводит список товаров
            public Shop(float wallet, string name)
            {
                this.wallet = wallet;
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
                    Console.WriteLine($"{count}) {item.Key} {item.Value}р");
                    count++;
                    
                }
                if (goods.Count == 0)
                {
                    sr_write.Close();
                    System.Environment.Exit(0);

                }
            }

            // покупка товара по названию
            public void BuySmth(string product)
            {
                // проверка на наличия товара в словаре по ключу, также проверка на баланс
                if(goods.ContainsKey(product) && goods[product] < wallet)
                {
                    Console.WriteLine($"\nВы купили {product} за {goods[product]}\nНа балансе осталось {wallet - goods[product]}\n");
                    sr_write.WriteLine($"{product} {goods[product]}");
                    wallet -= goods[product];
                    goods.Remove(product);
                    
                }
                else if(goods.ContainsKey(product))
                    Console.WriteLine($"Не хватает {goods[product] - wallet}");
                else
                {
                    Console.WriteLine("Товар закончился");
                    sr_write.Close();
                    System.Environment.Exit(0);
                }
                    

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
            Shop.sr_write.Close();
        }
    }
}
