namespace HomeWorkOOP8
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Menu();
        }

        public static void Menu()
        {
            Fighter[] fighters =
            {
                new Monk("Монах",100,20,10,"Каждый третий удар наносит противнику 40 ед. урона"),
                new Warrior("Воин",110, 25,15,"При падении здоровья ниже 50% наносит увеличенный урон"),
                //new Hunter("Охотник",100, 30,5,"Имеет шанс 30% восстановить 20 ед. здоровья"),
                //new Paladin("Паладин",120, 20, 35, "Снижение урона с помощью брони"),
                //new Wizard("Маг", 90, 30, 5, "Имеет шанс 20% нанести тройной урон и восстановить 20 ед. здоровья")
            };

            for (int i = 0; i < fighters.Length; i++)
            {
                Console.Write((i + 1) + " ");
                fighters[i].ShowStats();
            }

            Console.WriteLine($"Выберите первого бойца: ");
            bool isNumber = int.TryParse(Console.ReadLine(), out int userChoice);

            if (isNumber)
            {
                Fighter leftFighter = fighters[userChoice - 1];
                Console.WriteLine($"Выбран боец в левом углу: {leftFighter.Name}");

                Console.WriteLine($"Выберите второго бойца: ");
                bool isNumber1 = int.TryParse(Console.ReadLine(), out int userChoice1);

                if (isNumber1)
                {
                    Fighter rightFighter = fighters[userChoice1 - 1];
                    Console.WriteLine($"Выбран боец в правом углу: {rightFighter.Name}");
                    Console.Clear();

                    leftFighter.ShowCurrentHealth();
                    rightFighter.ShowCurrentHealth();
                    Console.WriteLine("------------");

                    while (leftFighter.Health > 0 && rightFighter.Health > 0)
                    {
                        leftFighter.Attack(rightFighter, leftFighter.Damage);
                        rightFighter.Attack(leftFighter, rightFighter.Damage);
                        leftFighter.ShowCurrentHealth();
                        rightFighter.ShowCurrentHealth();
                    }

                    if (leftFighter.Health > rightFighter.Health)
                    {
                        Console.WriteLine($"Победил: {leftFighter.Name}");
                    }
                    else
                    {
                        Console.WriteLine($"Победил: {rightFighter.Name}");
                    }
                }
            }
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
        public abstract void TakeDamage(int damage);
    }

    class Monk : Fighter
    {
        public Monk(string name, int health, int damage, int armor, string spell) : base(name, health, damage, armor, spell)
        {

        }

        private int _attackCount;

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
                fighter.TakeDamage(damage);
            }
        }

        public override void TakeDamage(int damage)
        {
            Health -= damage - Armor;
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
                fighter.TakeDamage(damage);
            }
        }

        public override void TakeDamage(int damage)
        {
            Health -= damage - Armor;
        }
    }

    //class Hunter : Fighter
    //{
    //    public Hunter(string name, int health, int damage, int armor, string spell) : base(name, health, damage, armor, spell)
    //    {
    //        Health = health;
    //    }

    //    public new int Health { get; private set; }

    //    public override void Attack(Fighter fighter, int damage)
    //    {
    //        Random random = new();

    //        if (Vanish(random))
    //        {
    //            Health += 20;
    //            base.Attack(fighter, damage);
    //            Console.WriteLine($"{Name} восстановил 20 ед. здоровья");
    //        }
    //        else
    //        {
    //            base.Attack(fighter, damage);
    //        }
    //    }

    //    private bool Vanish(Random random)
    //    {
    //        int number = random.Next(1, 4);

    //        if (number == 3)
    //        {
    //            return true;
    //        }
    //        else
    //        {
    //            return false;
    //        }
    //    }
    //}

    //class Paladin : Fighter
    //{
    //    public Paladin(string name, int health, int damage, int armor, string spell) : base(name, health += armor, damage, armor, spell)
    //    {
    //        Health = health;
    //        Armor = armor;
    //    }

    //    public int Armor { get; private set; }
    //    public new int Health { get; private set; }
    //}

    //class Wizard : Fighter
    //{
    //    public Wizard(string name, int health, int damage, int armor, string spell) : base(name, health, damage, armor, spell)
    //    {
    //        Health = health;
    //    }

    //    public new int Health { get; private set; }

    //    public override void Attack(Fighter fighter, int damage)
    //    {
    //        Random random = new();
    //        int increasedDamage;
    //        int multiplier = 3;

    //        if (MoonSword(random))
    //        {
    //            increasedDamage = damage * multiplier;
    //            Health += 20;
    //            base.Attack(fighter, increasedDamage);
    //            Console.WriteLine($"{Name} восстановил 20 ед. здоровья и нанес критический урон");
    //        }
    //        else
    //        {
    //            base.Attack(fighter, damage);
    //        }
    //    }

    //    private bool MoonSword(Random random)
    //    {
    //        int number = random.Next(1, 6);

    //        if (number == 5)
    //        {
    //            return true;
    //        }
    //        else
    //        {
    //            return false;
    //        }
    //    }
    //}
}





