using MyCollection;
using Newtonsoft.Json.Linq;

namespace MyCollectionTesting
{
    public abstract class Tests
    {
        public MyLinkedList<int> _list;
        [SetUp]
        public void Setup()
        {
            _list = new MyLinkedList<int>();
        }
    }

    [TestFixture]
    public class ConstructorTests
    {
        [Test]
        public void ConstructorWithCollection_NormalCollection_ResultListIsEqualToInitial()
        {
            List<int> oldList = new List<int>() { 2, 3, 4 };
            var newList = new MyLinkedList<int>(oldList);
            Assert.Multiple(() =>
            {
                Assert.That(newList.Count, Is.EqualTo(3));
                Assert.That(newList.First!.item, Is.EqualTo(2));
                Assert.That(newList.Last!.item, Is.EqualTo(4));
                Assert.That(newList.Last.prev!.item, Is.EqualTo(3));
            });
        }
    }

    [TestFixture]
    public class AddLastTests : Tests
    {
        [Test]
        public void Add_NewValue_EmptyList_ResultListWithOneNode()
        {
            var value = 5;
            _list.Add(value);
            Assert.Multiple(() =>
            {
                Assert.That(_list.Last!.item, Is.EqualTo(value));
                Assert.That(_list.Count, Is.EqualTo(1));
            }
            );
        }

        [Test]
        public void Add_NewValue_ListWithOneNode_ResultLastValueIsNewValue()
        {
            var value = 5;
            _list.Add(value);
            var value1 = 7;
            _list.Add(value1);
            Assert.Multiple(() =>
            {
                Assert.That(_list.Last!.item, Is.EqualTo(value1));
                Assert.That(_list.Count, Is.EqualTo(2));
            });
        }
    }

    [TestFixture]
    public class AddFirstTests : Tests
    {
        [Test]
        public void AddFirst_NewValue_EmptyList_ResultListWithOneNode()
        {
            var value = 5;
            _list.AddFirst(value);
            Assert.Multiple(() =>
            {
                Assert.That(_list.First!.item, Is.EqualTo(value));
                Assert.That(_list.Count, Is.EqualTo(1));
            });
        }

        [Test]
        public void AddFirst_NewValue_ListWithOneNode_ResultFirstIsNewValue()
        {
            var value = 5;
            _list.AddFirst(value);
            var value1 = 7;
            _list.AddFirst(value1);
            Assert.Multiple(() =>
            {
                Assert.That(_list.First!.item, Is.EqualTo(value1));
                Assert.That(_list.Count, Is.EqualTo(2));
            });
        }
    }

    [TestFixture]
    public class AddAfterTests : Tests
    {
        [Test]
        public void AddAfter_NewValue_BetweenTwoValues_ResultNewValueInItsPlace()
        {
            var value = 5;
            _list.Add(value);
            var value1 = 7;
            _list.Add(value1);
            var value2 = 6;
            _list.AddAfter(_list.First!, value2);
            var node = _list.Find(value2);
            Assert.Multiple(() =>
            {
                Assert.That(_list.First!.next, Is.EqualTo(node));
                Assert.That(node!.prev, Is.EqualTo(_list.First));
                Assert.That(_list.Last!.prev, Is.EqualTo(node));
                Assert.That(node!.next, Is.EqualTo(_list.Last));
                Assert.That(_list.Count, Is.EqualTo(3));
            });
        }

        [Test]
        public void AddAfter_NewValue_AfterLast_ResultTheLastIsNewValue()
        {
            var value = 5;
            _list.Add(value);
            var value1 = 7;
            _list.AddAfter(_list.Last!, value1);
            var node = _list.Find(7);
            Assert.Multiple(() =>
            {
                Assert.That(_list.First!.next, Is.EqualTo(node));
                Assert.That(node!.prev, Is.EqualTo(_list.First));
                Assert.That(node, Is.EqualTo(_list.Last));
                Assert.That(node!.next, Is.EqualTo(null));
                Assert.That(_list.Count, Is.EqualTo(2));
            });
        }

        [Test]
        public void AddAfter_NewValue_AfterNull_ResultExeption()
        {
            var node = _list.Last;
            var value = 5;
            Assert.Throws<ArgumentNullException>(() => _list.AddAfter(node, value));
        }

        [Test]
        public void AddAfter_NewValue_FakeNode_ResultExeption()
        {
            var node = new Node<int>(5);
            var value = 7;
            Assert.Throws<InvalidOperationException>(() => _list.AddAfter(node, value));
        }
    }

    [TestFixture]
    public class AddBeforeTests : Tests
    {
        [Test]
        public void AddBefore_NewValue_BetweenTwoValues_ResultNewValueInItsPlace()
        {
            var value = 5;
            _list.Add(value);
            var value1 = 7;
            _list.Add(value1);
            var value2 = 6;
            _list.AddBefore(_list.Last, value2);
            var node = _list.Find(value2);
            Assert.Multiple(() =>
            {
                Assert.That(_list.First!.next, Is.EqualTo(node));
                Assert.That(node!.prev, Is.EqualTo(_list.First));
                Assert.That(_list.Last!.prev, Is.EqualTo(node));
                Assert.That(node!.next, Is.EqualTo(_list.Last));
                Assert.That(_list.Count, Is.EqualTo(3));
            });
        }

        [Test]
        public void AddBefore_NewValue_BeforeFirst_ResultTheFirstIsNewValue()
        {
            var value = 5;
            _list.Add(value);
            var value1 = 7;
            _list.AddBefore(_list.First, value1);
            var node = _list.Find(7);
            Assert.Multiple(() =>
            {
                Assert.That(_list.Last!.prev, Is.EqualTo(node));
                Assert.That(node!.next, Is.EqualTo(_list.Last));
                Assert.That(node, Is.EqualTo(_list.First));
                Assert.That(node!.prev, Is.EqualTo(null));
                Assert.That(_list.Count, Is.EqualTo(2));
            });
        }

        [Test]
        public void AddBefore_NewValue_AfterNull_ResultExeption()
        {
            var node = _list.Last;
            var value = 5;
            Assert.Throws<ArgumentNullException>(() => _list.AddBefore(node, value));
        }

        [Test]
        public void AddBefore_NewValue_FakeNode_ResultExeption()
        {
            var node = new Node<int>(5);
            var value = 7;
            Assert.Throws<InvalidOperationException>(() => _list.AddBefore(node, value));
        }
    }

    [TestFixture]
    public class RemoveTests : Tests
    {
        [Test]
        public void Remove_EmptyList_ResultExeption()
        {
            Assert.That(_list.Remove(0), Is.EqualTo(false));
        }

        [Test]
        public void Remove_BetweenTwoNodes_ResultValueRemoved()
        {
            var value = 5;
            var value1 = 6;
            var value2 = 7;
            _list.Add(value);
            _list.Add(value1);
            _list.Add(value2);
            _list.Remove(6);
            Assert.Multiple(() =>
            {
                Assert.That(_list.First!.next, Is.EqualTo(_list.Last));
                Assert.That(_list.Last!.prev, Is.EqualTo(_list.First));
                Assert.That(_list.Count, Is.EqualTo(2));
            });
        }

        [Test]
        public void Remove_FakeNode_ResultExeption()
        {
            var node = new Node<int>(5);
            Assert.Throws<InvalidOperationException>(() => _list.Remove(node));
        }

        [Test]
        public void Remove_OnlyNode_ResultEmptyList()
        {
            var node = new Node<int>(5);
            _list.AddLast(node);
            _list.Remove(node);
            Assert.That(_list.Count, Is.EqualTo(0));
        }
    }

    [TestFixture]
    public class RemoveFirstTests : Tests
    {
        [Test]
        public void RemoveFirst_EmptyList_ResultExeption()
        {
            Assert.Throws<InvalidOperationException>(() => _list.RemoveFirst());
        }

        [Test]
        public void RemoveFirst_TwoElements_ResultSecondBecomeFirst()
        {
            var value = 5;
            var value1 = 7;
            _list.AddLast(value);
            _list.AddLast(value1);
            _list.RemoveFirst();
            Assert.Multiple(() =>
            {
                Assert.That(_list.First!.item, Is.EqualTo(value1));
                Assert.That(_list.Count, Is.EqualTo(1));
            });
        }
    }

    [TestFixture]
    public class RemoveLastTests : Tests
    {
        [Test]
        public void RemoveLast_EmptyList_ResultExeption()
        {
            Assert.Throws<InvalidOperationException>(() => _list.RemoveLast());
        }

        [Test]
        public void RemoveLast_TwoElements_ResultFirstBecomeLast()
        {
            var value = 5;
            var value1 = 7;
            _list.AddLast(value);
            _list.AddLast(value1);
            _list.RemoveLast();
            Assert.Multiple(() =>
            {
                Assert.That(_list.Last!.item, Is.EqualTo(value));
                Assert.That(_list.Count, Is.EqualTo(1));
            });
        }
    }

    [TestFixture]
    public class ClearTests : Tests
    {
        [Test]
        public void Clear_ListWith4Elements_ResultListIsEmpty()
        {
            for(int i = 0; i < 4; i++)
            {
                _list.AddLast(i);
            }
            _list.Clear();
            Assert.Multiple(() =>
            {
                Assert.That(_list.Count, Is.EqualTo(0));
                Assert.That(_list.Last, Is.Null);
                Assert.That(_list.First, Is.Null);
            });
        }
    }

    [TestFixture]
    public class FindTests : Tests
    {
        [Test]
        public void Find_EmptyList_ResultIsNull()
        {
            var node = _list.Find(2);
            Assert.That(node, Is.Null);
        }
    }

    [TestFixture]
    public class ContainsTests : Tests
    {
        [Test]
        public void Contains_ResultIsTrue()
        {
            var value = 5;
            _list.AddLast(value);
            Assert.IsTrue(_list.Contains(value));
        }
    }

    [TestFixture]
    public class CopyToTests : Tests
    {
        [Test]
        public void CopyTo_ListWith3ElementsAndValidIndexParameter_ResultArrayWith3Elements()
        {
            int[] arr = new int[3];

            Node<int> node1 = new(2);
            Node<int> node2 = new(3);
            Node<int> node3 = new(4);

            _list.AddLast(node1);
            _list.AddLast(node2);
            _list.AddLast(node3);

            _list.CopyTo(arr, 0);

            Assert.Multiple(() =>
            {
                Assert.That(arr, Has.Length.EqualTo(3));
                Assert.That(arr, Is.All.Not.Null);
            });
        }

        [Test]
        public void CopyTo_ListWith3ElementsAndWrongIndexInParams_ThrowArgumentOutOfrangeExcep()
        {
            int[] arr = new int[3];

            Node<int> node1 = new(2);
            Node<int> node2 = new(3);
            Node<int> node3 = new(4);

            _list.AddLast(node1);
            _list.AddLast(node2);
            _list.AddLast(node3);

            TestDelegate res = () => _list.CopyTo(arr, -1);

            Assert.Throws<ArgumentOutOfRangeException>(res);
        }
    }

    [TestFixture]
    public class GetEnumeratorTests : Tests
    {
        [Test]
        public void GetEnumerator_ListWith3Elements_AllAreCorrect()
        {
            MyLinkedList<int>? list = new MyLinkedList<int>();

            Node<int> node1 = new(2);
            Node<int> node2 = new(3);
            Node<int> node3 = new(4);

            _list.AddLast(node1);
            _list.AddLast(node2);
            _list.AddLast(node3);

            foreach (var item in _list)
            {
                list.Add(item);
            }

            Assert.That(list.First!.item, Is.EqualTo(_list.First!.item));
            Assert.That(list.Last!.item, Is.EqualTo(_list.Last!.item));
        }
    }

    [TestFixture]
    public class ShowTests : Tests
    {
        [Test]
        public void Show_ResultOK()
        {
            _list.Show();
            _list.AddLast(1);
            _list.Show();
            Assert.Pass();
        }
    }
}