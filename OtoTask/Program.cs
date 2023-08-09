namespace OtoTask
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using static System.Text.Json.JsonSerializer;

    public class Item
    {
        public int Id { get; set; }
        public int? Parent { get; set; }
    }

    public static class ItemSorter
    {
        public static List<Item> SortItems(List<Item> items)
        {
            Dictionary<int, List<Item>> itemDictionary = new Dictionary<int, List<Item>>();
            List<Item> result = new List<Item>();

            // Group items by their parent value
            foreach (var item in items)
            {
                if (!itemDictionary.ContainsKey(item.Parent ?? -1))
                {
                    itemDictionary[item.Parent ?? -1] = new List<Item>();
                }

                itemDictionary[item.Parent ?? -1].Add(item);
            }

/*            Console.WriteLine("itemDictionary: " + itemDictionary);
*/
            /*            Console.WriteLine("itemDict: " + Serialize(itemDictionary.ToList()));
            */
            Console.WriteLine();
            // Recursive function to add items to the result list

            void AddItems(int? parentId)
            {
                if (itemDictionary.ContainsKey(parentId ?? -1))
                {
                    foreach (var item in itemDictionary[parentId ?? -1])
                    {
                        result.Add(item);
                        AddItems(item.Id);
                    }
                }
            }

            // Start with items that have no parent (parentId == null)
            AddItems(null);

/*            Console.WriteLine("sortedItems: " + result);
*/
/*            Console.WriteLine("sortedItems: " + Serialize(result));
*/

            return result;
        }
    }

    class Program
    {
        static void Main()
        {
            /*
            List<Item> items = new List<Item>
        {
            new Item { Id = 1, Parent = null },
            new Item { Id = 2, Parent = null },
            new Item { Id = 3, Parent = 1 },
            new Item { Id = 4, Parent = 1 },
            new Item { Id = 5, Parent = 1 },
            new Item { Id = 6, Parent = null },
            new Item { Id = 7, Parent = 6 },
        };
            */

            /*
           List<Item> items = new List<Item>
       {
           new Item { Id = 1, Parent = 2 },
           new Item { Id = 2, Parent = null },
           new Item { Id = 3, Parent = 2 },
           new Item { Id = 4, Parent = 2 },
           new Item { Id = 5, Parent = 2 },
           new Item { Id = 6, Parent = null },
           new Item { Id = 7, Parent = 6 },
       };
           */

            // /*
        //    List<Item> items = new List<Item>
        //{
        //    new Item { Id = 6, Parent = null },
        //    new Item { Id = 8, Parent = 5 },
        //    new Item { Id = 1, Parent = 2 },
        //    new Item { Id = 3, Parent = 2 },
        //    new Item { Id = 4, Parent = 2 },
        //    new Item { Id = 5, Parent = 2 },
        //    new Item { Id = 2, Parent = null },
        //    new Item { Id = 7, Parent = 6 },
        //    new Item { Id = 9, Parent = 8 },
        //};


            List<Item> items = new List<Item>
        {
            new Item { Id = 10, Parent = 2 },
            new Item { Id = 11, Parent = 10 },
            new Item { Id = 6, Parent = null },
            new Item { Id = 8, Parent = 5 },
            new Item { Id = 1, Parent = 2 },
            new Item { Id = 3, Parent = 2 },
            new Item { Id = 4, Parent = 2 },
            new Item { Id = 5, Parent = 2 },
            new Item { Id = 2, Parent = null },
            new Item { Id = 7, Parent = 6 },
            new Item { Id = 9, Parent = 8 },
        };
            //  */

            List<Item> sortedItems = ItemSorter.SortItems(items);

            foreach (var item in sortedItems)
            {

                Console.WriteLine($"Id: {item.Id}, Parent: {item.Parent}");
            }
        }
    }

}