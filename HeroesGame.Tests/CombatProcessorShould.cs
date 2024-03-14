using HeroesGame.Contract;
using HeroesGame.Implementation.Hero;
using HeroesGame.Implementation.Monster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesGame.Tests
{
    public class CombatProcessorShould
    {
        private CombatProcessor cp;

        [SetUp]
        public void Setup()
        {
            this.cp = new CombatProcessor(new Hunter());
        }

        [Test]
        public void InitializeCorrectly()
        {
            // Assert
            Assert.That(this.cp.Hero, Is.Not.Null);
            Assert.That(this.cp.Logger, Is.Not.Null);
            Assert.That(this.cp.Logger, Is.Empty);
        }

        [Test]
        public void FightCorrectly_WeakerEnemy()
        {
            // Arrange
            IMonster monster = new MedusaTheGorgon(1);
            //this.LevelUpHero(50);

            // Act
            this.cp.Fight(monster);
            var logger = this.cp.Logger;

            // Assert
            Assert.That(logger.Count, Is.EqualTo(2));
            Assert.That(logger, Does.Contain("The Hunter hits MedusaTheGorgon dealing 510 damage to it.").And.Contains("The monster dies. (4 XP gained.)"));
        }

        [Test]
        public void FightCorrectlyAndRepeatedly_StrongerEnemy()
        {
            // Arrange
            IMonster monster = new MedusaTheGorgon(50);
            this.cp.Fight(monster);
            var logger = this.cp.Logger;

            // Assert
            Assert.That(logger, Has.Count.EqualTo(12));
            Assert.That(logger, Does.Contain("The hero dies on level 1 after 4 fights."));
        }
    }
}
