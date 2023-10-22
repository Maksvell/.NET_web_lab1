using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lab1;
using MyCollection;
using NUnit.Framework;

namespace MyCollectionTesting
{
    [TestFixture]
    public class EventListenerTests
    {
        private EventListener<int> _eventListener;
        private MyLinkedList<int> _list;
        [SetUp]
        public void SetUp()
        {
            _eventListener = new EventListener<int>();
            _list = new MyLinkedList<int>();
        }

        [Test]
        public void OnAddedHandler_AddHandler_TureReturn()
        {
            var wasCalled = false;

            _list.ItemAdded += _eventListener.OnAdded!;
            _list.ItemRemoved += _eventListener.OnRemoved!;

            _list.ItemAdded += (o, e) => wasCalled = true;

            Node<int> node = new(2);
            Node<int> node1 = new(3);
            _list.AddLast(node);
            _list.AddLast(node1);

            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void OnRemovedHandler_AddHandler_TureReturn()
        {
            var wasCalled = false;

            _list.ItemAdded += _eventListener.OnAdded!;
            _list.ItemRemoved += _eventListener.OnRemoved!;

            _list.ItemAdded += (o, e) => wasCalled = true;

            Node<int> node = new(2);
            Node<int> node1 = new(3);
            _list.AddLast(node);
            _list.AddLast(node1);
            _list.RemoveLast();

            Assert.That(wasCalled, Is.True);
        }
    }
}
