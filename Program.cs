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
        private Fighter? fighter1;
        private Fighter? fighter2;

        private List<Fighter> _fighters = new();

        public Arena()
        {
            _fighters.Add(new Monk("Монах", 100, 20, 5, "Каждый третий удар наносит противнику 40 ед. урона"));
            _fighters.Add(new Warrior("Воин", 110, 25, 5, "При падении здоровья ниже 50% наносит увеличенный урон"));
            _fighters.Add(new Hunter("Охотник", 100, 30, 5, "Имеет шанс 30% восстановить 20 ед. здоровья"));
            _fighters.Add(new Paladin("Паладин", 120, 20, 10, "С вероятностью 15% блокирует входящий урон, \nпри снижении здоровья меньше 30 увеличивается наносимый урон"));
            _fighters.Add(new Wizard("Маг", 90, 30, 5, "Имеет шанс 20% нанести тройной урон и восстановить 20 ед. здоровья"));
        }

        public void StartFight()
        {
            ShowFighters();
            fighter1 = ChooseFighter();
            fighter2 = ChooseFighter();

            Console.WriteLine("Для начала боя нажмите любую клавишу: ");
            Console.ReadKey();
            Console.Clear();

            fighter1.ShowCurrentHealth();
            fighter2.ShowCurrentHealth();

            Console.WriteLine($"{new string('-', 25)}");

            while (fighter1.Health > 0 && fighter2.Health > 0)
            {
                fighter1.Attack(fighter2);

                fighter2.Attack(fighter1);

                fighter1.ShowCurrentHealth();
                fighter2.ShowCurrentHealth();
            }

            if (fighter1.Health > 0)
            {
                Console.WriteLine($"Победил: {fighter1.Name}");
            }
            else if (fighter2.Health > 0)
            {
                Console.WriteLine($"Победил: {fighter2.Name}");
            }
            else
            {
                Console.WriteLine("Ничья");
            }
        }

        private void ShowFighters()
        {
            Console.WriteLine("Список бойцов: ");

            for (int i = 0; i < _fighters.Count; i++)
            {
                Console.Write((i + 1) + " ");
                _fighters[i].ShowStats();
            }
        }

        private Fighter ChooseFighter()
        {
            Console.WriteLine($"\nВыберите бойца: ");
            bool isNumber = int.TryParse(Console.ReadLine(), out int userChoice);
            userChoice -= 1;

            Random random = new();
            Fighter fightеr;

            if (isNumber == false)
            {
                Console.WriteLine("Нужно ввести число");
            }

            if (userChoice < _fighters.Count && userChoice > 0)
            {
                Fighter fighter = _fighters[userChoice];

                if (fighter1 == fighter)
                {
                    Console.WriteLine("Такой боец уже выбран, выберите другого");
                }

                Console.WriteLine($"Выбран боец в левом углу {fighter.Name}");
                Console.WriteLine($"{new string('-', 25)}");
                return fighter;
            }

            fightеr = _fighters[random.Next(_fighters.Count)];

            Console.WriteLine($"Выбран рандомный боец: {fightеr.Name}");
            Console.WriteLine("Нажмите любую кнопку");
            Console.ReadKey();

            return fightеr;
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

        public abstract void Attack(Fighter fighter);
        public virtual void TakeDamage(int damage)
        {
            int percentDivider = 100;
            Health -= (damage * Armor) / percentDivider;
        }
    }

    class Monk : Fighter
    {
        private int _attackCount;

        public Monk(string name, int health, int damage, int armor, string spell) : base(name, health, damage, armor, spell)
        {

        }

        public override void Attack(Fighter fighter)
        {
            int criticalDamage = 40;
            int criticalAttackNumber = 3;

            _attackCount++;

            if (_attackCount == criticalAttackNumber)
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

        public override void Attack(Fighter fighter)
        {
            int increasedDamage;
            int thresholdHealthValue = 35;

            if (Health < thresholdHealthValue)
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
        private int _health;

        public Hunter(string name, int health, int damage, int armor, string spell) : base(name, health, damage, armor, spell)
        {
            health = _health;
        }

        public override void Attack(Fighter fighter)
        {
            Random random = new();
            int selfHealing = 20;

            if (CanUseSkillRecover(random))
            {
                _health += selfHealing;
                Console.WriteLine($"{Name} восстановил {selfHealing} ед. здоровья");
            }

            fighter.TakeDamage(Damage);
        }

        private bool CanUseSkillRecover(Random random)
        {
            int chance = 30;
            int number = random.Next(1, 100);

            return number < chance;
        }
    }

    class Paladin : Fighter
    {
        public Paladin(string name, int health, int damage, int armor, string spell) : base(name, health, damage, armor, spell)
        {

        }

        public override void Attack(Fighter fighter)
        {
            int increasedDamage;
            int thresholdHealthValue = 40;

            if (Health < thresholdHealthValue)
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
            int zeroDamageMultiplier = 0;

            if (LightProtection(random))
            {
                Health -= damage * zeroDamageMultiplier;
            }
            else
            {
                base.TakeDamage(Damage);
            }
        }

        private bool LightProtection(Random random)
        {
            int chance = 15;
            int number = random.Next(1, 100);

            return number < chance;
        }
    }

    class Wizard : Fighter
    {

        public Wizard(string name, int health, int damage, int armor, string spell) : base(name, health, damage, armor, spell)
        {
        }

        public override void Attack(Fighter fighter)
        {
            Random random = new();
            int increasedDamage;
            int multiplierDamage = 3;
            int selfHealing = 20;

            if (CanUseMoonSword(random))
            {
                increasedDamage = Damage * multiplierDamage;
                Health += selfHealing;
                Console.WriteLine($"{Name} восстановил {selfHealing} ед. здоровья и нанес критический урон");
                fighter.TakeDamage(increasedDamage);
            }
            else
            {
                fighter.TakeDamage(Damage);
            }
        }

        private bool CanUseMoonSword(Random random)
        {
            int chance = 20;
            int number = random.Next(1, 100);

            return number < chance;
        }
    }
}










