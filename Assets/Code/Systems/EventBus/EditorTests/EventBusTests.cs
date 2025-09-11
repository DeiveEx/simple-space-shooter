using NUnit.Framework;
using Systems.EventBus;

public class EventBusTests
{
    private class TestEvent : IEvent
    {
        public int TestValue;
    }
    
    private SimpleEventBus _eventBus;

    [SetUp]
    public void Setup()
    {
        _eventBus = new SimpleEventBus();
    }

    [Test]
    public void TestRegisterHandler()
    {
        _eventBus.RegisterHandler<TestEvent>(DummyHandler);
        Assert.That(_eventBus._handlers.ContainsKey(typeof(TestEvent)), Is.True);
        Assert.That(_eventBus._handlers.Count, Is.EqualTo(1));
        Assert.That(_eventBus._handlers[typeof(TestEvent)].Count, Is.EqualTo(1));
    }
    
    [Test]
    public void TestUnregisterHandler()
    {
        _eventBus.RegisterHandler<TestEvent>(DummyHandler);
        _eventBus.UnregisterHandler<TestEvent>(DummyHandler);
        Assert.That(_eventBus._handlers.ContainsKey(typeof(TestEvent)), Is.False);
        Assert.That(_eventBus._handlers.Count, Is.EqualTo(0));
    }
    
    [Test]
    [TestCase(0)]
    [TestCase(10)]
    [TestCase(-1)]
    [TestCase(int.MaxValue)]
    public void TestPublish(int value)
    {
        _eventBus.RegisterHandler<TestEvent>(Handler);
        _eventBus.Publish(new TestEvent() { TestValue = value });

        void Handler(TestEvent e)
        {
            Assert.That(e.TestValue, Is.EqualTo(value));
        }
    }

    void DummyHandler(TestEvent e) {}
}
