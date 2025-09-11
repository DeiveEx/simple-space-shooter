using NUnit.Framework;
using Systems.Health;
using UnityEngine;
using UnityEngine.TestTools;

public class HealthTests
{
    private HealthComponent _health;
    
    [SetUp]
    public void Setup()
    {
        var go = new GameObject();
        _health = go.AddComponent<HealthComponent>();
        _health.Setup(3);
    }
    
    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(_health.gameObject);
    }
    
    [TestCase(5,ExpectedResult = true)]
    [TestCase(100, ExpectedResult = true)]
    [TestCase(0, ExpectedResult = false)]
    [TestCase(-1, ExpectedResult = false)]
    public bool TestSetup(int value)
    {
        LogAssert.ignoreFailingMessages = true;
        
        _health.Setup(value);
        return _health.CurrentHealth == value;
    }

    [TestCase(0, ExpectedResult = 3)]
    [TestCase(1, ExpectedResult = 2)]
    [TestCase(10, ExpectedResult = 0)]
    [TestCase(-1, ExpectedResult = 3)]
    [TestCase(-100, ExpectedResult = 3)]
    public int TestTakeDamage(int damageAmount)
    {
        _health.Damage(damageAmount);
        return _health.CurrentHealth;
    }
    
    [TestCase(0, ExpectedResult = false)]
    [TestCase(1, ExpectedResult = false)]
    [TestCase(3, ExpectedResult = true)]
    [TestCase(10, ExpectedResult = true)]
    [TestCase(-1, ExpectedResult = false)]
    [TestCase(-100, ExpectedResult = false)]
    public bool TestDeath(int damageAmount)
    {
        _health.Damage(damageAmount);
        return _health.IsDead;
    }

    [Test(ExpectedResult = true)]
    public bool TestHealthChangedEvent()
    {
        bool eventCalled = false;
        _health.HealthChanged += OnHealthChanged;
        _health.Damage(1);
        return eventCalled;

        void OnHealthChanged()
        {
            Assert.That(_health.CurrentHealth, Is.EqualTo(2));
            eventCalled = true;
        }
    }
    
    [Test(ExpectedResult = true)]
    public bool TestDeathEvent()
    {
        bool eventCalled = false;
        _health.Died += OnDeath;
        _health.Damage(999);
        return eventCalled;

        void OnDeath()
        {
            Assert.That(_health.CurrentHealth, Is.EqualTo(0));
            eventCalled = true;
        }
    }
}
