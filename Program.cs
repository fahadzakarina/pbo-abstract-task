using System;

public class Program
{
    public static void Main()
    {
        BasicRobot bot1 = new BasicRobot("Bot1", 100, 10, 10);
        BossRobot boss = new BossRobot("Boss", 200, 20, 30, 25);

        IAbility repair = new Repair();
        bot1.UseAbility(repair);

        IAbility electricShock = new ElectricShock();
        boss.UseAbility(electricShock);

        IAbility plasmaBlast = new PlasmaBlast();
        bot1.UseAbility(plasmaBlast);

        boss.ReceiveAttack(bot1);
        boss.DisplayInfo();

        bot1.RecoverEnergy();
        boss.RecoverEnergy();
    }
}

public abstract class Robot
{
    public string Name { get; set; }
    public int Energy { get; set; }
    public int Armor { get; set; }
    public int AttackPower { get; set; }
    private const int EnergyRecoveryRate = 10;

    public Robot(string name, int energy, int armor, int attackPower)
    {
        Name = name;
        Energy = energy;
        Armor = armor;
        AttackPower = attackPower;
    }

    public void Attack(Robot target)
    {
        int damage = AttackPower - target.Armor;
        if (damage < 0)
        {
            damage = 0;
        }
        target.Energy -= damage;
        Console.WriteLine($"{Name} menyerang {target.Name} dengan {damage} damage.");
    }

    public abstract void UseAbility(IAbility ability);

    public void DisplayInfo()
    {
        Console.WriteLine($"Nama: {Name}, Energi: {Energy}, Armor: {Armor}, Serangan: {AttackPower}");
    }

    public void RecoverEnergy()
    {
        Energy += EnergyRecoveryRate;
        Console.WriteLine($"{Name} memulihkan {EnergyRecoveryRate} energi.");
    }
}

public class BasicRobot : Robot
{
    public BasicRobot(string name, int energy, int armor, int attackPower)
        : base(name, energy, armor, attackPower)
    {
    }

    public override void UseAbility(IAbility ability)
    {
        ability.Activate(this);
    }
}

public class BossRobot : Robot
{
    public int Defense { get; set; }

    public BossRobot(string name, int energy, int armor, int attackPower, int defense)
        : base(name, energy, armor, attackPower)
    {
        Defense = defense;
        Armor += Defense;
    }

    public void ReceiveAttack(Robot attacker)
    {
        int damage = attacker.AttackPower - Armor;
        if (damage < 0)
        {
            damage = 0;
        }
        Energy -= damage;
        Console.WriteLine($"{Name} diserang oleh {attacker.Name} dengan {damage} damage.");
        if (Energy <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Console.WriteLine($"{Name} telah mati.");
    }

    public override void UseAbility(IAbility ability)
    {
        ability.Activate(this);
    }
}

public interface IAbility
{
    void Activate(Robot target);
    bool IsOnCooldown { get; }
}

public class Repair : IAbility
{
    public bool IsOnCooldown { get; private set; }

    public void Activate(Robot target)
    {
        if (!IsOnCooldown)
        {
            target.Energy += 20;
            IsOnCooldown = true;
            Console.WriteLine($"{target.Name} menggunakan Perbaikan, energi bertambah 20.");
        }
        else
        {
            Console.WriteLine("Perbaikan sedang cooldown.");
        }
    }
}

public class ElectricShock : IAbility
{
    public bool IsOnCooldown { get; private set; }

    public void Activate(Robot target)
    {
        if (!IsOnCooldown)
        {
            target.Energy -= 30;
            target.Armor = 0;
            IsOnCooldown = true;
            Console.WriteLine($"{target.Name} terkena Serangan Listrik, energi berkurang 30.");
        }
        else
        {
            Console.WriteLine("Serangan Listrik sedang cooldown.");
        }
    }
}

public class PlasmaBlast : IAbility
{
    public bool IsOnCooldown { get; private set; }

    public void Activate(Robot target)
    {
        if (!IsOnCooldown)
        {
            target.Energy -= 25;
            IsOnCooldown = true;
            Console.WriteLine($"{target.Name} terkena Serangan Plasma, energi berkurang 25.");
        }
        else
        {
            Console.WriteLine("Serangan Plasma sedang cooldown.");
        }
    }
}
