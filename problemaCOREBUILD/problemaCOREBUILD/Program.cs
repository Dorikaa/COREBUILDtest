using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using Newtonsoft.Json;

namespace problemaCOREBUILD
{
    class Characters // The following declaration creates the Characters class
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public double attack { get; set; }
        public double health { get; set; }
        public bool isVillain { get; set; }

        public static implicit operator List<object>(Characters v)
        {
            throw new NotImplementedException();
        }
    }

    class Planets // The following declaration creates the Planets class
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public Modifier modifiers { get; set; }
    }

    class Modifier // The following declaration creates the Modifier class which we need for Planets
    {
        public int heroAttackModifier { get; set; }
        public int heroHealthModifier { get; set; }
        public int villainAttackModifier { get; set; }
        public int villainHealthModifier { get; set; }
    }

    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Please choose a hero: ");
            StreamReader c = new StreamReader("characters.json"); // Initializing a new instance of the StreamReader class for the "characters.json" file
            StreamReader p = new StreamReader("planets.json"); // Initializing a new instance of the StreaReader class for the "planets.json" file


            var json = c.ReadToEnd(); // We read all the characters from the current position to the end of the stream (characters.json)
            var items = JsonConvert.DeserializeObject<List<Characters>>(json); // Creating a .NET object by using the deserialization method of the JSON file

            var json2 = p.ReadToEnd(); // We read all the characters from the current position to the end of the stream (planets.json)
            var planets = JsonConvert.DeserializeObject<List<Planets>>(json2); // Creating a .NET object by using the deserialization method of the JSON file

            var battle = 0; // Creating a variable "battle" for later in the combat to keep the battle going until someone's health reaches 0
            Random rnd = new Random(); // Creating a random for later in the combat to determine the percentage of the attack

            List<Characters> heroes = new List<Characters>(); // Creating a list called "heroes" in which we later add each hero
            List<Characters> villains = new List<Characters>(); // Creating a list called "villains" in which we later add each villain
            List<Characters> chosenHero = new List<Characters>(); // Creating a list called "chosenHero" in which we later add the Hero we chose to fight with
            List<Characters> chosenVillain = new List<Characters>(); // Creating a list called "chosenVillain" in which we later add the Villain we chose to fight against
            List<Characters> combat = new List<Characters>();

            var heroes_cnt = 1; // Creating a variable to keep count of how many heroes we add to the "heroes" list
            var villains_cnt = 1; // Creating a variable to keep count of how many villains we add to the "villains" list

            foreach (var item in items) // In the following statement, we check if the character is a villain or not, and if it's not one, we add it to the "heroes" list
            {
                if (item.isVillain == false)
                {
                    heroes.Add(new Characters { name = item.name, description = item.description, attack = item.attack, health = item.health, id = heroes_cnt });
                    heroes_cnt++;
                }
            }
            foreach (var item in heroes) // We print all the heroes so the player can choose one by interting it's ID
                Console.WriteLine("{0} - {1}: {2}", item.id, item.name, item.description);

            var newhero = Convert.ToInt16(Console.ReadLine());

            foreach (var item in heroes) // In the following statement, we add the hero we chose to fight with in the "chosenHero" list
            {
                if (item.id == newhero)
                {
                    Console.WriteLine("You chose: - {0}: {1}\n", item.name, item.description);
                    chosenHero.Add(item);
                }
            }

            Console.WriteLine("Please choose a villain: "); // In the following statement, we check if the character is a villain or not, and if it's a villain, we add it to the "villains" list
            foreach (var item in items)
            {
                if (item.isVillain == true)
                {
                    villains.Add(new Characters { name = item.name, description = item.description, attack = item.attack, health = item.health, id = villains_cnt });
                    villains_cnt++;
                }
            }
            foreach (var item in villains) // We print all the villains so the player can shoose one by it's ID
                Console.WriteLine("{0} - {1}: {2}", item.id, item.name, item.description);


            var newvillain = Convert.ToInt16(Console.ReadLine()); // In the following statement, we add the villain we chose to fight against in the "chosenVillain" list
            foreach (var item in villains)
            {
                if (item.id == newvillain)
                {
                    Console.WriteLine("You chose: {0}: {1}\n", item.name, item.description);
                    chosenVillain.Add(item);
                }
            }

            Console.WriteLine("Please choose a planet: ");

            // In the following we declare 4 variables which we will use for the modifiers of heroes's stats and villains's stats
            var newattack_v = 0; // Villain attack modifier => new villain attack
            var newhealth_v = 0; // Villain health modifier => new villain health
            var newattack_h = 0; // Hero attack modifier => new hero attack
            var newhealth_h = 0; // Hero health modifier => new hero health

            foreach (var item in planets) // Printing all the planets so the player can choose one
            {
                Console.WriteLine("{0}. {1}: {2}", item.id, item.name, item.description);
            }

            var newplanet = Convert.ToInt16(Console.ReadLine());

            // The following statement is pretty much heads-on, after we get the planet the fights begins immediately
            foreach (var item in planets)
            {
                if (item.id == newplanet)
                {
                    newattack_h = item.modifiers.heroAttackModifier; // Getting the attack modifier for the hero
                    newattack_v = item.modifiers.villainAttackModifier; // Getting the attack modifier for the villain
                    newhealth_h = item.modifiers.heroHealthModifier; // Getting the health modifier for the hero
                    newhealth_v = item.modifiers.villainHealthModifier; // Getting the health modifier for the villain

                    // In the following statements, we calculate the new Attack, Health for the Hero and Villain using the modifiers provided earlier
                    foreach (var heroz in chosenHero)
                    {
                        heroz.attack = heroz.attack + newattack_h;
                        heroz.health = heroz.health + newhealth_h;
                    }
                    foreach (var villainz in chosenVillain)
                    {
                        villainz.attack = villainz.attack + newattack_v;
                        villainz.health = villainz.health + newhealth_v;
                    }

                    var hero = chosenHero.ElementAt(0); // Getting the hero we chose from the list
                    var villain = chosenVillain.ElementAt(0); // Getting the villain we chose from the list

                    Console.WriteLine("You chose: {0}. {1}: {2}\n", item.id, item.name, item.description);

                    // In the following statement the battle takes place
                    while (battle == 0)
                    {
                        var hit = hero.attack * (rnd.Next(60, 100) / 100.0); // Declaring a variable for the heroe's attack each round, in order to get a random stat between 60% and 100%
                        villain.health = villain.health - hit;
                        Console.WriteLine("{0} attacked {1} for {2}", hero.name, villain.name, hit);

                        if (villain.health <= 0)
                        {
                            Console.WriteLine("The Avengers prevailed!");
                            break;
                        }
                        Thread.Sleep(750); // Using thread sleep so we can have a chance to watch the battle as it goes, not printing only the result in an instant, so we can actually wait for eachother's turn

                        var hit2 = villain.attack * (rnd.Next(60, 100) / 100.0); // Declaring a variable for the villain attack each round, in order to get a random stat between 60% and 100%
                        hero.health = hero.health - hit2;
                        Console.WriteLine("{0} attacked {1} for {2}", villain.name, hero.name, hit2);

                        if (hero.health <= 0)
                        {
                            Console.WriteLine("The villains won! (this time)");
                            break;
                        }
                        Thread.Sleep(750); // Using thread sleep so we can have a chance to watch the battle as it goes, not printing only the result in an instant, so we can actually wait for eachother's turn
                    }
                    Console.WriteLine("Press any key to exit...");
                    Console.ReadKey();
                }
                
            }
        }
    }
}
