using HeroesGame.Constant;
using HeroesGame.Contract;
using HeroesGame.Implementation.Hero;
using Moq;
using Moq.Protected;

namespace HeroesGame.Tests
{
    public class Tests
    {
        private Mock<Mage> _hero;

        [SetUp]
        public void Setup()
        {
            this._hero = new Mock<Mage>();
            this._hero.Protected().Setup("LevelUp").CallBase();
        }

        [Test]
        public void HaveCorrectInitialValues()
        {
            // Arrange
            // Act
            var hero = new Mage();

            // Assert
            Assert.That(hero.Level, Is.EqualTo(HeroConstants.InitialLevel));
            Assert.That(hero.Experience, Is.EqualTo(HeroConstants.InitialExperience));
            Assert.That(hero.MaxHealth, Is.EqualTo(HeroConstants.InitialMaxHealth));
            Assert.That(hero.Health, Is.EqualTo(HeroConstants.InitialMaxHealth));
            Assert.That(hero.Armor, Is.EqualTo(HeroConstants.InitialArmor));
            Assert.That(hero.Weapon, Is.Not.Null);
        }

        [Test]
        public void TakeHitCorrectly()
        {
            // Arrange 
            var hero = new Warrior();

            // Act 
            var damage = 5;
            hero.TakeHit(damage);

            // Assert 
            Assert.That(hero.Health, Is.EqualTo(HeroConstants.InitialMaxHealth - damage + HeroConstants.InitialArmor));
        }

        [Test]
        [TestCase(10)]
        [TestCase(20)]
        [TestCase(30)]
        public void TakeHitCorrectly_TestCases(int damage)
        {
            // Arrange 
            var hero = new Warrior();

            // Act 
            hero.TakeHit(damage);

            // Assert 
            Assert.That(hero.Health, Is.EqualTo(HeroConstants.InitialMaxHealth - damage + HeroConstants.InitialArmor));
        }

        [Test]
        public void TakeHitCorrectly_Combinatorial([Values(40,50,60)] int damage)
        {
            // Arrange 
            var hero = new Warrior();

            // Act 
            hero.TakeHit(damage);

            // Assert 
            Assert.That(hero.Health, Is.EqualTo(HeroConstants.InitialMaxHealth - damage + HeroConstants.InitialArmor));
        }

        [Test]
        public void TakeHitCorrectly([Range(70,100,10)] int damage)
        {
            // Arrange 
            var hero = new Warrior();

            // Act 
            hero.TakeHit(damage);

            // Assert 
            Assert.That(hero.Health, Is.EqualTo(HeroConstants.InitialMaxHealth - damage + HeroConstants.InitialArmor));
        }

        [Test]
        public void ThrowsExceptionForNegativeTakeHitValue()
        {
            // Act
            var damage = -50;

            // Assert
            Assert.Throws<ArgumentException>(() => _hero.Object.TakeHit(damage), "Damage value cannot be negative.");
        }

        [Test]
        public void GainExperienceCorrectly([Range(25,500,25)] double xp)
        {
            // Act 
            this._hero.Object.GainExperience(xp);

            // Assert
            if(xp >= HeroConstants.MaximumExperience)
            {
                var expectedXP = (HeroConstants.InitialExperience + xp) % HeroConstants.MaximumExperience;
                Assert.That(this._hero.Object.Experience, Is.EqualTo(expectedXP));
                Assert.That(this._hero.Object.Level, Is.EqualTo(HeroConstants.InitialLevel + 1));
            }
            else
            {
                Assert.That(this._hero.Object.Experience, Is.EqualTo(HeroConstants.InitialExperience + xp));
            }
        }

        [Test]
        public void HealCorrectly([Range(5,25,1)] int level, [Range(25,500,25)] int damage)
        {
            // Act
            this.LevelUp(level);
            double totalDamage = HeroConstants.InitialMaxHealth + damage;
            totalDamage = this._hero.Object.TakeHit(totalDamage);
            this._hero.Object.Heal();

            // Assert
            var healValue = this._hero.Object.Level * HeroConstants.HealPerLevel;
            var expectedHealth = (this._hero.Object.MaxHealth - totalDamage) + healValue;

            if(expectedHealth > this._hero.Object.MaxHealth)
            {
                expectedHealth = this._hero.Object.MaxHealth;
            }

            Assert.That(this._hero.Object.Health, Is.EqualTo(expectedHealth));
        }

        private void LevelUp(int levels)
        {
            for (int i = 0; i < levels; i++)
            {
                this._hero.Object.GainExperience(HeroConstants.MaximumExperience);
            }
        }

        [Test]
        public void NotBeBornDead()
        {
            // Act
            var isDead = this._hero.Object.IsDead();

            // Assert
            Assert.That(this._hero.Object.IsDead, Is.False);
        }

        [Test]
        public void BeDeadWhenCriticallyHit([Range(50,150,25)] double damage)
        {
            // Act 
            damage = this._hero.Object.TakeHit(damage);

            // Assert
            if (damage > this._hero.Object.MaxHealth)
            {
                Assert.That(this._hero.Object.IsDead);
            }
            else Assert.That(this._hero.Object.IsDead, Is.False);
        }
    }
}