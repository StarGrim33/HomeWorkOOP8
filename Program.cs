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
        private Fighter? _fighter1;
        private Fighter? _fighter2;

        private List<Fighter> _fighters = new();

        public Arena()
        {
            _fighters.Add(new Monk("Монах", 100, 45, 5));
            _fighters.Add(new Warrior("Воин", 110, 50, 5));
            _fighters.Add(new Hunter("Охотник", 100, 45, 5));
            _fighters.Add(new Paladin("Паладин", 120, 40, 10));
            _fighters.Add(new Wizard("Маг", 90, 50, 5));
        }

        public void StartFight()
        {
            ChooseFighters();
            Battle();
            DetermineTheWinner();
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

        private void DetermineTheWinner()
        {
            if (_fighter1.Health > 0)
            {
                Console.WriteLine($"Победил: {_fighter1.Name}");
            }
            else if (_fighter2.Health > 0)
            {
                Console.WriteLine($"Победил: {_fighter2.Name}");
            }
            else
            {
                Console.WriteLine("Ничья");
            }
        }

        private void Battle()
        {
            Console.WriteLine("Для начала боя нажмите любую клавишу: ");
            Console.ReadKey();
            Console.Clear();

            _fighter1.ShowCurrentHealth();
            _fighter2.ShowCurrentHealth();

            Console.WriteLine($"{new string('-', 25)}");

            while (_fighter1.Health > 0 && _fighter2.Health > 0)
            {
                _fighter1.Attack(_fighter2);

                _fighter2.Attack(_fighter1);

                _fighter1.ShowCurrentHealth();
                _fighter2.ShowCurrentHealth();
            }
        }

        private void ChooseFighters()
        {
            Fighter? fighter = null;

            while (_fighter1 == null && _fighter2 == null)
            {
                ShowFighters();

                if (ChooseAFighter(out fighter))
                {
                    _fighter1 = fighter;
                }

                if (ChooseAFighter(out fighter))
                {
                    _fighter2 = fighter;
                }

                if (_fighter1 == _fighter2)
                {
                    Console.WriteLine("Нельзя выбрать одинаковых бойцов");
                    Console.ReadKey();
                    _fighter1 = null;
                    _fighter2 = null;
                    Console.Clear();
                }
            }
        }

        private bool ChooseAFighter(out Fighter fighter)
        {
            fighter = null;
            bool isProgramOn = true;

            while (isProgramOn)
            {
                Console.WriteLine($"\nВыберите бойца: ");
                bool isNumber = int.TryParse(Console.ReadLine(), out int userChoice);

                if (isNumber == false)
                {
                    Console.WriteLine("Нужно ввести число");
                }

                if (userChoice <= _fighters.Count && userChoice > 0)
                {
                    fighter = _fighters[userChoice - 1];

                    Console.WriteLine($"Выбран боец в левом углу {fighter.Name}");
                    Console.WriteLine($"{new string('-', 25)}");
                    return true;
                }
            }

            return false;
        }
    }

    abstract class Fighter
    {
        public Fighter(string name, int health, int damage, int armor)
        {
            Name = name;
            Health = health;
            Damage = damage;
            Armor = armor;
            Spell = "";
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
        private readonly int _criticalDamage = 40;

        public Monk(string name, int health, int damage, int armor) : base(name, health, damage, armor)
        {
            Spell = "Каждый третий удар наносит противнику " + _criticalDamage + "ед. урона";
        }

        public override void Attack(Fighter fighter)
        {
            int criticalAttackNumber = 3;

            _attackCount++;

            if (_attackCount == criticalAttackNumber)
            {
                fighter.TakeDamage(_criticalDamage);
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
        private readonly int _thresholdHealthValue = 35;
        private int _increasedDamage = 40;

        public Warrior(string name, int health, int damage, int armor) : base(name, health, damage, armor)
        {
            Spell = "При падении здоровья ниже " + _thresholdHealthValue + " наносит увеличенный урон в размере " + _increasedDamage + " ед.";
        }

        public override void Attack(Fighter fighter)
        {
            if (Health < _thresholdHealthValue)
            {
                fighter.TakeDamage(_increasedDamage);
            }
            else
            {
                fighter.TakeDamage(Damage);
            }
        }
    }

    class Hunter : Fighter
    {
        private int _chance = 10;
        private int _selfHealing = 15;

        public Hunter(string name, int health, int damage, int armor) : base(name, health, damage, armor)
        {
            Spell = "Имеет шанс " + _chance + "% восстановить " + _selfHealing + " ед. здоровья";
        }

        public override void Attack(Fighter fighter)
        {
            Random random = new();

            if (CanUseSkillRecover(random))
            {
                Health += _selfHealing;
                Console.WriteLine($"{Name} восстановил {_selfHealing} ед. здоровья");
            }

            fighter.TakeDamage(Damage);
        }

        private bool CanUseSkillRecover(Random random)
        {
            int minNumber = 1;
            int maxNumber = 100;
            int number = random.Next(minNumber, maxNumber);

            return number < _chance;
        }
    }

    class Paladin : Fighter
    {
        private int _chance = 15;
        private int _thresholdHealthValue = 40;
        private int _increasedDamage = 45;

        public Paladin(string name, int health, int damage, int armor) : base(name, health, damage, armor)
        {
            Spell = "С вероятностью " + _chance + "% блокирует входящий урон, \nпри снижении здоровья меньше" + _thresholdHealthValue +
                "увеличивается наносимый урон до" + _increasedDamage + " ед.";
        }

        public override void Attack(Fighter fighter)
        {
            if (Health < _thresholdHealthValue)
            {
                _increasedDamage = 30;
                fighter.TakeDamage(_increasedDamage);
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

            if (IsBlockOn(random))
            {
                Health -= damage * zeroDamageMultiplier;
            }
            else
            {
                base.TakeDamage(Damage);
            }
        }

        private bool IsBlockOn(Random random)
        {
            int minNumber = 1;
            int maxNumber = 100;
            int number = random.Next(minNumber, maxNumber);

            return number < _chance;
        }
    }

    class Wizard : Fighter
    {
        private int _chance = 20;
        private int _selfHealing = 5;

        public Wizard(string name, int health, int damage, int armor) : base(name, health, damage, armor)
        {
            Spell = "Имеет шанс " + _chance + " нанести тройной урон и восстановить" + _selfHealing + " ед. здоровья";
        }

        public override void Attack(Fighter fighter)
        {
            Random random = new();
            int increasedDamage;
            int multiplierDamage = 3;

            if (CanUseMoonSword(random))
            {
                increasedDamage = Damage * multiplierDamage;
                Health += _selfHealing;
                Console.WriteLine($"{Name} восстановил {_selfHealing} ед. здоровья и нанес критический урон");
                fighter.TakeDamage(increasedDamage);
            }
            else
            {
                fighter.TakeDamage(Damage);
            }
        }

        private bool CanUseMoonSword(Random random)
        {
            int minNumber = 1;
            int maxNumber = 100;
            int number = random.Next(minNumber, maxNumber);

            return number < _chance;
        }
    }
}










