using System;
using System.Collections.Generic;
using System.Linq;

namespace Clones
{
	public class CloneVersionSystem : ICloneVersionSystem
	{
		private List<LinkedStack<int>> cloneLearnedPrograms
			= new List<LinkedStack<int>> { { new LinkedStack<int>() } };
		private List<LinkedStack<int>> recentRollbacks
			= new List<LinkedStack<int>> { { new LinkedStack<int>() } };

		public string Execute(string query)
		{
			var command = query.Split(' ');
            switch(command[0])
            {
				case "learn":
					Learn(int.Parse(command[1]), int.Parse(command[2]));
					break;
				case "rollback":
					Rollback(int.Parse(command[1]));
					break;
				case "relearn":
					Relearn(int.Parse(command[1]));
					break;
				case "clone":
					Clone(int.Parse(command[1]));
                    break;
				case "check":
					return Check(int.Parse(command[1]));
            }
			return null;
		}

		private void Learn(int clone, int program)
        {			
			cloneLearnedPrograms[clone - 1].Push(program);
			recentRollbacks[clone - 1] = new LinkedStack<int>();
        }

		private void Rollback(int clone)
        {
            recentRollbacks[clone - 1].Push(cloneLearnedPrograms[clone - 1].Pop());
        }

		private void Relearn(int clone)
        {
			cloneLearnedPrograms[clone - 1].Push(recentRollbacks[clone - 1].Pop());
        }

		private void Clone(int clone)
		{
			cloneLearnedPrograms.Add((LinkedStack<int>)cloneLearnedPrograms[clone - 1].Clone());
			recentRollbacks.Add((LinkedStack<int>)recentRollbacks[clone - 1].Clone());
		}

		private string Check(int clone)
        {
			if (!cloneLearnedPrograms[clone - 1].IsBasic())
				return cloneLearnedPrograms[clone - 1].Peek().ToString();
			else
				return "basic";
        }
	}

	public class StackItem<T>
	{
		public T Value { get; set; }
		public StackItem<T> Next { get; set; }
	}

	public class LinkedStack<T> : ICloneable
	{
		StackItem<T> head;

		public LinkedStack(StackItem<T> head)
        {
			this.head = head;
        }

		public LinkedStack()
        {
        }

		public void Push(T value)
		{
			if (head == null)
				head = new StackItem<T> { Value = value, Next = null };
			else
			{
				var item = new StackItem<T> { Value = value, Next = head };
				head = item;
			}
		}

		public T Pop()
		{
			if (head == null) throw new InvalidOperationException();
			var result = head.Value;
			head = head.Next;
			return result;
		}

		public T Peek()
        {
			return head.Value;
        }

		public bool IsBasic()
        {
			return head == null;
        }

        public object Clone()
        {
            return new LinkedStack<T>(head);
        }
    }
}

