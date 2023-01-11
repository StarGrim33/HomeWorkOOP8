using System;

namespace HomeWorkOOP8
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Arena arena = new();
            arena.StartFight();
        }
    }

    class Arena
    {
        Fighter? leftFighter;
        Fighter? rightFighter;

        Fighter[] fighters =
        {
                new Monk("Монах", 100, 20, 10, "Каждый третий удар наносит противнику 40 ед. урона"),
                new Warrior("Воин", 110, 25, 15, "При падении здоровья ниже 50% наносит увеличенный урон"),
                new Hunter("Охотник", 100, 30,5,"Имеет шанс 30% восстановить 20 ед. здоровья"),
                new Paladin("Паладин", 120, 20, 35, "С вероятностью 25% блокирует входящий урон, \nпри снижении здоровья меньше 30 увеличивается наносимый урон"),
                new Wizard("Маг", 90, 30, 5, "Имеет шанс 20% нанести тройной урон и восстановить 20 ед. здоровья")
        };

        public void StartFight()
        {
            ShowFighters();
            leftFighter = ChooseFighter();
            rightFighter = ChooseFighter();
            Console.WriteLine("Для начала боя нажмите любую клавишу: ");
            Console.ReadKey();
            Console.Clear();

            while (leftFighter?.Health > 0 && rightFighter?.Health > 0)
            {
                leftFighter.Attack(rightFighter, leftFighter.Damage);

                rightFighter.Attack(leftFighter, rightFighter.Damage);

                leftFighter.ShowCurrentHealth();
                rightFighter.ShowCurrentHealth();
            }

            if (leftFighter?.Health > rightFighter?.Health)
            {
                Console.WriteLine($"Победил: {leftFighter.Name}");
            }
            else
            {
                Console.WriteLine($"Победил: {rightFighter?.Name}");
            }
        }

        private void ShowFighters()
        {
            Console.WriteLine("Список бойцов: ");

            for (int i = 0; i < fighters.Length; i++)
            {
                Console.Write((i + 1) + " ");
                fighters[i].ShowStats();
            }
        }

        private Fighter ChooseFighter()
        {
            Console.WriteLine($"Выберите бойца: ");
            int userChoice0 = Convert.ToInt32(Console.ReadLine());

            Fighter leftFighter = fighters[userChoice0 - 1];
            Console.WriteLine($"Выбран боец в левом углу {leftFighter.Name}");
            Console.WriteLine("------------");
            return leftFighter;
        }
    }

    abstract class Fighter
    {
        public Fighter(string name, int health, int damage, int armor, string spell)
        {
            Name = name;
            Health = health;
            Damage = damage;
            Armor = armor;
            Spell = spell;
        }

        public string Name { get; private set; }
        public int Health { get; protected set; }
        public int Damage { get; protected set; }
        public int Armor { get; protected set; }
        public string Spell { get; protected set; }

        public void ShowStats()
        {
            Console.WriteLine($"{Name}, здоровье: {Health}, урон: {Damage}, способность: {Spell}");
            Console.WriteLine($"{new string('-', 25)}");
        }

        public void ShowCurrentHealth()
        {
            Console.WriteLine($"{Name}, здоровье: {Health}, урон: {Damage}");
        }

        public abstract void Attack(Fighter fighter, int damage);
        public virtual void TakeDamage(int damage)
        {
            Health -= damage - Armor;
        }
    }

    class Monk : Fighter
    {
        private int _attackCount;

        public Monk(string name, int health, int damage, int armor, string spell) : base(name, health, damage, armor, spell)
        {

        }

        public override void Attack(Fighter fighter, int damage)
        {
            int criticalDamage = 40;
            _attackCount++;

            if (_attackCount == 3)
            {
                fighter.TakeDamage(criticalDamage);
                _attackCount = 0;
            }
            else
            {
                fighter.TakeDamage(Damage);
            }
        }
    }

    class Warrior : Fighter
    {
        public Warrior(string name, int health, int damage, int armor, string spell) : base(name, health, damage, armor, spell)
        {

        }

        public override void Attack(Fighter fighter, int damage)
        {
            int increasedDamage;

            if (Health < 50)
            {
                increasedDamage = 35;
                fighter.TakeDamage(increasedDamage);
            }
            else
            {
                fighter.TakeDamage(Damage);
            }
        }
    }

    class Hunter : Fighter
    {
        public Hunter(string name, int health, int damage, int armor, string spell) : base(name, health, damage, armor, spell)
        {
            Health = health;
        }

        public new int Health { get; private set; }

        public override void Attack(Fighter fighter, int damage)
        {
            Random random = new();

            if (SelfHealing(random))
            {
                Health += 20;
                Console.WriteLine($"{Name} восстановил 20 ед. здоровья");
                fighter.TakeDamage(Damage);
            }
            else
            {
                fighter.TakeDamage(Damage);
            }
        }

        private bool SelfHealing(Random random)
        {
            int number = random.Next(1, 4);

            if (number == 3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    class Paladin : Fighter
    {
        public Paladin(string name, int health, int damage, int armor, string spell) : base(name, health, damage, armor, spell)
        {
            
        }

        public override void Attack(Fighter fighter, int damage)
        {
            int increasedDamage;

            if (Health < 40)
            {
                increasedDamage = 30;
                fighter.TakeDamage(increasedDamage);
            }
            else
            {
                fighter.TakeDamage(Damage);
            }
        }

        public override void TakeDamage(int damage)
        {
            Random random = new();

            if(LightProtection(random))
            {
                Health -= damage * 0;
            }
            else
            {
                Health -= damage - Armor;
            }
        }

        private bool LightProtection(Random random)
        {
            int number = random.Next(1, 5);

            if (number == 5)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    class Wizard : Fighter
    {
        public Wizard(string name, int health, int damage, int armor, string spell) : base(name, health, damage, armor, spell)
        {
            Health = health;
        }

        public new int Health { get; private set; }

        public override void Attack(Fighter fighter, int damage)
        {
            Random random = new();
            int increasedDamage;
            int multiplierDamage = 3;

            if (MoonSword(random))
            {
                increasedDamage = damage * multiplierDamage;
                Health += 20;
                Console.WriteLine($"{Name} восстановил 20 ед. здоровья и нанес критический урон");
                fighter.TakeDamage(increasedDamage);
            }
            else
            {
                fighter.TakeDamage(damage);
            }
        }

        private bool MoonSword(Random random)
        {
            int number = random.Next(1, 6);

            if (number == 5)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}










