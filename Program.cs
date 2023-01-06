using System.Diagnostics.CodeAnalysis;

namespace HomeWorkOOP8
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Fighter[] fighters = { new Monk("Монах", 100, 20), new Warrior("Воин", 120, 20) };

            foreach (Fighter fighter in fighters)
            {
                fighter.ShowInfo();
            }

            Console.WriteLine($"Выберите первого бойца: ");
            bool isNumber = int.TryParse(Console.ReadLine(), out int userChoice);

            
            Fighter leftFighter = fighters[userChoice];
            Console.WriteLine($"Выбран боец в левом углу: {leftFighter.Name}");
            
            Console.WriteLine($"Выберите второго бойца: ");
            bool isNumber1 = int.TryParse(Console.ReadLine(), out int userChoice1);

            Fighter rightFighter = fighters[userChoice1];
            Console.WriteLine($"Выбран боец в правом углу: {rightFighter.Name}");
            
            while (leftFighter.Health > 0 && rightFighter.Health > 0)
            {
                leftFighter.TakeDamage(rightFighter.Damage);
                rightFighter.TakeDamage(leftFighter.Damage);
                leftFighter.ShowCurrentHealth();
                rightFighter.ShowCurrentHealth();
            }
        }
    }

    class Fighter
    {
        public Fighter(string name, int health, int damage, string spell)
        {
            Name = name;
            Health = health;
            Damage = damage;
            Spell = spell;
        }

        public int Health { get; private set; }
        public int Damage { get; private set; }
        public string Name { get; private set; }
        public string Spell { get; private set; }

        public void ShowInfo()
        {
            Console.WriteLine($"{Name}, здоровье:{Health}, урон: {Damage}, способность: {Spell}");
        }

        public void ShowCurrentHealth()
        {
            Console.WriteLine($"{Name}, здоровье: {Health}");
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
        }
    }

    class Monk : Fighter
    {
        public Monk(string name, int health, int damage, string spell = "Каждый третий удар наносит двойной урон противнику") : base(name, health, damage, spell)
        {
            
        }

        public new int Damage 
        {
            get
            {
                int count = 1;
                count++;

                if(count == 3)
                    return 40;

                return 0;
            }
            private set { }
        }
    }

    class Warrior : Fighter
    {
        public Warrior(string name, int health, int damage, string spell = "Издает рев и наносит противнику 40 ед. урона") : base(name, health, damage, spell)
        {
            Spell = spell;
        }

        public string Spell { get; private set; }

        public int Rough(out int handOfJustice)
        {
            int damageMultiply = 2;
            handOfJustice = Damage * damageMultiply;
            return handOfJustice;
        }
    }
}

//    class Hunter : Fighter
//    {
//        public void PoisonArrow()
//        {

//        }
//    }

//    class Paladin : Fighter 
//    {
//        public void RetributionOfLight()
//        {

//        }
//    }

//    class Wizard : Fighter 
//    {
//        public void MoonSword()
//        {

//        }
//    }
//}