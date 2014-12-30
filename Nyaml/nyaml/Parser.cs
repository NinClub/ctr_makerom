using System;
using System.Collections.Generic;
namespace nyaml
{
	internal class Parser
	{
		private class stack_frame
		{
			public Parser.state_type state;
			public Parser.gotof_type gotof;
			public object value;
			public stack_frame(Parser.state_type s, Parser.gotof_type g, object v)
			{
				this.state = s;
				this.gotof = g;
				this.value = v;
			}
		}
		private class Stack
		{
			private List<Parser.stack_frame> stack = new List<Parser.stack_frame>();
			private List<Parser.stack_frame> tmp = new List<Parser.stack_frame>();
			private int gap;
			public Stack()
			{
				this.gap = 0;
			}
			public void reset_tmp()
			{
				this.gap = this.stack.Count;
				this.tmp.Clear();
			}
			public void commit_tmp()
			{
				int num = this.gap + this.tmp.Count;
				if (num > this.stack.Capacity)
				{
					this.stack.Capacity = num;
				}
				this.stack.RemoveRange(this.gap, this.stack.Count - this.gap);
				this.stack.AddRange(this.tmp);
			}
			public bool push(Parser.stack_frame f)
			{
				this.tmp.Add(f);
				return true;
			}
			public void pop(int n)
			{
				if (this.tmp.Count < n)
				{
					n -= this.tmp.Count;
					this.tmp.Clear();
					this.gap -= n;
					return;
				}
				this.tmp.RemoveRange(this.tmp.Count - n, n);
			}
			public Parser.stack_frame top()
			{
				if (this.tmp.Count != 0)
				{
					return this.tmp[this.tmp.Count - 1];
				}
				return this.stack[this.gap - 1];
			}
			public Parser.stack_frame get_arg(int b, int i)
			{
				int count = this.tmp.Count;
				if (b - i <= count)
				{
					return this.tmp[count - (b - i)];
				}
				return this.stack[this.gap - (b - count) + i];
			}
			public void clear()
			{
				this.stack.Clear();
			}
		}
		private delegate bool state_type(Token token, object value);
		private delegate bool gotof_type(int i, object value);
		private ISemanticAction sa;
		private Parser.Stack stack;
		private bool accepted;
		private bool error;
		private object accepted_value;
		public Parser(ISemanticAction sa)
		{
			this.stack = new Parser.Stack();
			this.sa = sa;
			this.reset();
		}
		public void reset()
		{
			this.error = false;
			this.accepted = false;
			this.clear_stack();
			this.reset_tmp_stack();
			if (this.push_stack(new Parser.state_type(this.state_0), new Parser.gotof_type(this.gotof_0), new object()))
			{
				this.commit_tmp_stack();
				return;
			}
			this.sa.stack_overflow();
			this.error = true;
		}
		public bool post(Token token, object value)
		{
			this.reset_tmp_stack();
			while (this.stack_top().state(token, value))
			{
			}
			if (!this.error)
			{
				this.commit_tmp_stack();
			}
			return this.accepted;
		}
		public bool accept(out object v)
		{
			if (this.error)
			{
				v = new object();
				return false;
			}
			v = this.accepted_value;
			return true;
		}
		public bool Error()
		{
			return this.error;
		}
		private bool push_stack(Parser.state_type s, Parser.gotof_type g, object v)
		{
			bool flag = this.stack.push(new Parser.stack_frame(s, g, v));
			if (!flag)
			{
				this.error = true;
				this.sa.stack_overflow();
			}
			return flag;
		}
		private void pop_stack(int n)
		{
			this.stack.pop(n);
		}
		private Parser.stack_frame stack_top()
		{
			return this.stack.top();
		}
		private object get_arg(int b, int i)
		{
			return this.stack.get_arg(b, i).value;
		}
		private void clear_stack()
		{
			this.stack.clear();
		}
		private void reset_tmp_stack()
		{
			this.stack.reset_tmp();
		}
		private void commit_tmp_stack()
		{
			this.stack.commit_tmp();
		}
		private bool gotof_0(int nonterminal_index, object v)
		{
			return nonterminal_index == 8 && this.push_stack(new Parser.state_type(this.state_1), new Parser.gotof_type(this.gotof_1), v);
		}
		private bool state_0(Token token, object value)
		{
			if (token == Token.token_DocumentBegin)
			{
				this.push_stack(new Parser.state_type(this.state_2), new Parser.gotof_type(this.gotof_2), value);
				return false;
			}
			this.sa.syntax_error();
			this.error = true;
			return false;
		}
		private bool gotof_1(int nonterminal_index, object v)
		{
			return true;
		}
		private bool state_1(Token token, object value)
		{
			if (token == Token.token_eof)
			{
				this.accepted = true;
				this.accepted_value = this.get_arg(1, 0);
				return false;
			}
			this.sa.syntax_error();
			this.error = true;
			return false;
		}
		private bool gotof_2(int nonterminal_index, object v)
		{
			if (nonterminal_index != 0)
			{
				switch (nonterminal_index)
				{
				case 4:
					return this.push_stack(new Parser.state_type(this.state_3), new Parser.gotof_type(this.gotof_3), v);
				case 5:
					break;
				case 6:
					return this.push_stack(new Parser.state_type(this.state_14), new Parser.gotof_type(this.gotof_14), v);
				default:
					if (nonterminal_index == 10)
					{
						return this.push_stack(new Parser.state_type(this.state_13), new Parser.gotof_type(this.gotof_13), v);
					}
					break;
				}
				return false;
			}
			return this.push_stack(new Parser.state_type(this.state_15), new Parser.gotof_type(this.gotof_15), v);
		}
		private bool state_2(Token token, object value)
		{
			if (token == Token.token_BlockBegin)
			{
				this.push_stack(new Parser.state_type(this.state_33), new Parser.gotof_type(this.gotof_33), value);
				return false;
			}
			this.sa.syntax_error();
			this.error = true;
			return false;
		}
		private bool gotof_3(int nonterminal_index, object v)
		{
			return true;
		}
		private bool state_3(Token token, object value)
		{
			if (token == Token.token_DocumentEnd)
			{
				this.push_stack(new Parser.state_type(this.state_4), new Parser.gotof_type(this.gotof_4), value);
				return false;
			}
			this.sa.syntax_error();
			this.error = true;
			return false;
		}
		private bool gotof_4(int nonterminal_index, object v)
		{
			return true;
		}
		private bool state_4(Token token, object value)
		{
			if (token == Token.token_eof)
			{
				Collection arg = (Collection)this.get_arg(3, 1);
				Nyaml nyaml;
				this.sa.GetNyaml(out nyaml, arg);
				object value2 = nyaml;
				this.pop_stack(3);
				return this.stack_top().gotof(8, value2);
			}
			this.sa.syntax_error();
			this.error = true;
			return false;
		}
		private bool gotof_5(int nonterminal_index, object v)
		{
			switch (nonterminal_index)
			{
			case 0:
				return this.push_stack(new Parser.state_type(this.state_15), new Parser.gotof_type(this.gotof_15), v);
			case 2:
				return this.push_stack(new Parser.state_type(this.state_48), new Parser.gotof_type(this.gotof_48), v);
			case 4:
				return this.push_stack(new Parser.state_type(this.state_31), new Parser.gotof_type(this.gotof_31), v);
			case 5:
				return this.push_stack(new Parser.state_type(this.state_19), new Parser.gotof_type(this.gotof_19), v);
			case 6:
				return this.push_stack(new Parser.state_type(this.state_14), new Parser.gotof_type(this.gotof_14), v);
			case 9:
				return this.push_stack(new Parser.state_type(this.state_30), new Parser.gotof_type(this.gotof_30), v);
			case 10:
				return this.push_stack(new Parser.state_type(this.state_13), new Parser.gotof_type(this.gotof_13), v);
			}
			return false;
		}
		private bool state_5(Token token, object value)
		{
			switch (token)
			{
			case Token.token_BlockBegin:
				this.push_stack(new Parser.state_type(this.state_33), new Parser.gotof_type(this.gotof_33), value);
				return false;
			case Token.token_BlockScalarHeader:
				this.push_stack(new Parser.state_type(this.state_49), new Parser.gotof_type(this.gotof_49), value);
				return false;
			case Token.token_Bool:
				this.push_stack(new Parser.state_type(this.state_46), new Parser.gotof_type(this.gotof_46), value);
				return false;
			case Token.token_EOL:
				this.push_stack(new Parser.state_type(this.state_7), new Parser.gotof_type(this.gotof_7), value);
				return false;
			case Token.token_Literal:
				this.push_stack(new Parser.state_type(this.state_44), new Parser.gotof_type(this.gotof_44), value);
				return false;
			case Token.token_LongNumber:
				this.push_stack(new Parser.state_type(this.state_40), new Parser.gotof_type(this.gotof_40), value);
				return false;
			case Token.token_Number:
				this.push_stack(new Parser.state_type(this.state_38), new Parser.gotof_type(this.gotof_38), value);
				return false;
			case Token.token_Text:
				this.push_stack(new Parser.state_type(this.state_42), new Parser.gotof_type(this.gotof_42), value);
				return false;
			}
			this.sa.syntax_error();
			this.error = true;
			return false;
		}
		private bool gotof_6(int nonterminal_index, object v)
		{
			switch (nonterminal_index)
			{
			case 0:
				return this.push_stack(new Parser.state_type(this.state_15), new Parser.gotof_type(this.gotof_15), v);
			case 2:
				return this.push_stack(new Parser.state_type(this.state_48), new Parser.gotof_type(this.gotof_48), v);
			case 4:
				return this.push_stack(new Parser.state_type(this.state_31), new Parser.gotof_type(this.gotof_31), v);
			case 5:
				return this.push_stack(new Parser.state_type(this.state_21), new Parser.gotof_type(this.gotof_21), v);
			case 6:
				return this.push_stack(new Parser.state_type(this.state_14), new Parser.gotof_type(this.gotof_14), v);
			case 9:
				return this.push_stack(new Parser.state_type(this.state_30), new Parser.gotof_type(this.gotof_30), v);
			case 10:
				return this.push_stack(new Parser.state_type(this.state_13), new Parser.gotof_type(this.gotof_13), v);
			}
			return false;
		}
		private bool state_6(Token token, object value)
		{
			switch (token)
			{
			case Token.token_BlockBegin:
				this.push_stack(new Parser.state_type(this.state_33), new Parser.gotof_type(this.gotof_33), value);
				return false;
			case Token.token_BlockScalarHeader:
				this.push_stack(new Parser.state_type(this.state_49), new Parser.gotof_type(this.gotof_49), value);
				return false;
			case Token.token_Bool:
				this.push_stack(new Parser.state_type(this.state_46), new Parser.gotof_type(this.gotof_46), value);
				return false;
			case Token.token_EOL:
				this.push_stack(new Parser.state_type(this.state_8), new Parser.gotof_type(this.gotof_8), value);
				return false;
			case Token.token_Literal:
				this.push_stack(new Parser.state_type(this.state_44), new Parser.gotof_type(this.gotof_44), value);
				return false;
			case Token.token_LongNumber:
				this.push_stack(new Parser.state_type(this.state_40), new Parser.gotof_type(this.gotof_40), value);
				return false;
			case Token.token_Number:
				this.push_stack(new Parser.state_type(this.state_38), new Parser.gotof_type(this.gotof_38), value);
				return false;
			case Token.token_Text:
				this.push_stack(new Parser.state_type(this.state_42), new Parser.gotof_type(this.gotof_42), value);
				return false;
			}
			this.sa.syntax_error();
			this.error = true;
			return false;
		}
		private bool gotof_7(int nonterminal_index, object v)
		{
			if (nonterminal_index != 0)
			{
				switch (nonterminal_index)
				{
				case 4:
					return this.push_stack(new Parser.state_type(this.state_22), new Parser.gotof_type(this.gotof_22), v);
				case 5:
					break;
				case 6:
					return this.push_stack(new Parser.state_type(this.state_14), new Parser.gotof_type(this.gotof_14), v);
				default:
					if (nonterminal_index == 10)
					{
						return this.push_stack(new Parser.state_type(this.state_13), new Parser.gotof_type(this.gotof_13), v);
					}
					break;
				}
				return false;
			}
			return this.push_stack(new Parser.state_type(this.state_15), new Parser.gotof_type(this.gotof_15), v);
		}
		private bool state_7(Token token, object value)
		{
			switch (token)
			{
			case Token.token_BlockBegin:
				this.push_stack(new Parser.state_type(this.state_33), new Parser.gotof_type(this.gotof_33), value);
				return false;
			case Token.token_BlockEnd:
			{
				Scalar scalar;
				this.sa.GetScalar(out scalar);
				object value2 = scalar;
				this.pop_stack(1);
				return this.stack_top().gotof(9, value2);
			}
			default:
			{
				if (token != Token.token_Text)
				{
					this.sa.syntax_error();
					this.error = true;
					return false;
				}
				Scalar scalar2;
				this.sa.GetScalar(out scalar2);
				object value3 = scalar2;
				this.pop_stack(1);
				return this.stack_top().gotof(9, value3);
			}
			}
		}
		private bool gotof_8(int nonterminal_index, object v)
		{
			if (nonterminal_index != 0)
			{
				switch (nonterminal_index)
				{
				case 4:
					return this.push_stack(new Parser.state_type(this.state_23), new Parser.gotof_type(this.gotof_23), v);
				case 5:
					break;
				case 6:
					return this.push_stack(new Parser.state_type(this.state_14), new Parser.gotof_type(this.gotof_14), v);
				default:
					if (nonterminal_index == 10)
					{
						return this.push_stack(new Parser.state_type(this.state_13), new Parser.gotof_type(this.gotof_13), v);
					}
					break;
				}
				return false;
			}
			return this.push_stack(new Parser.state_type(this.state_15), new Parser.gotof_type(this.gotof_15), v);
		}
		private bool state_8(Token token, object value)
		{
			switch (token)
			{
			case Token.token_BlockBegin:
				this.push_stack(new Parser.state_type(this.state_33), new Parser.gotof_type(this.gotof_33), value);
				return false;
			case Token.token_BlockEnd:
			{
				Scalar scalar;
				this.sa.GetScalar(out scalar);
				object value2 = scalar;
				this.pop_stack(1);
				return this.stack_top().gotof(9, value2);
			}
			default:
			{
				if (token != Token.token_Text)
				{
					this.sa.syntax_error();
					this.error = true;
					return false;
				}
				Scalar scalar2;
				this.sa.GetScalar(out scalar2);
				object value3 = scalar2;
				this.pop_stack(1);
				return this.stack_top().gotof(9, value3);
			}
			}
		}
		private bool gotof_9(int nonterminal_index, object v)
		{
			switch (nonterminal_index)
			{
			case 0:
				return this.push_stack(new Parser.state_type(this.state_15), new Parser.gotof_type(this.gotof_15), v);
			case 2:
				return this.push_stack(new Parser.state_type(this.state_48), new Parser.gotof_type(this.gotof_48), v);
			case 4:
				return this.push_stack(new Parser.state_type(this.state_31), new Parser.gotof_type(this.gotof_31), v);
			case 5:
				return this.push_stack(new Parser.state_type(this.state_26), new Parser.gotof_type(this.gotof_26), v);
			case 6:
				return this.push_stack(new Parser.state_type(this.state_14), new Parser.gotof_type(this.gotof_14), v);
			case 9:
				return this.push_stack(new Parser.state_type(this.state_30), new Parser.gotof_type(this.gotof_30), v);
			case 10:
				return this.push_stack(new Parser.state_type(this.state_13), new Parser.gotof_type(this.gotof_13), v);
			}
			return false;
		}
		private bool state_9(Token token, object value)
		{
			switch (token)
			{
			case Token.token_BlockBegin:
				this.push_stack(new Parser.state_type(this.state_33), new Parser.gotof_type(this.gotof_33), value);
				return false;
			case Token.token_BlockScalarHeader:
				this.push_stack(new Parser.state_type(this.state_49), new Parser.gotof_type(this.gotof_49), value);
				return false;
			case Token.token_Bool:
				this.push_stack(new Parser.state_type(this.state_46), new Parser.gotof_type(this.gotof_46), value);
				return false;
			case Token.token_EOL:
				this.push_stack(new Parser.state_type(this.state_11), new Parser.gotof_type(this.gotof_11), value);
				return false;
			case Token.token_Literal:
				this.push_stack(new Parser.state_type(this.state_44), new Parser.gotof_type(this.gotof_44), value);
				return false;
			case Token.token_LongNumber:
				this.push_stack(new Parser.state_type(this.state_40), new Parser.gotof_type(this.gotof_40), value);
				return false;
			case Token.token_Number:
				this.push_stack(new Parser.state_type(this.state_38), new Parser.gotof_type(this.gotof_38), value);
				return false;
			case Token.token_Text:
				this.push_stack(new Parser.state_type(this.state_42), new Parser.gotof_type(this.gotof_42), value);
				return false;
			}
			this.sa.syntax_error();
			this.error = true;
			return false;
		}
		private bool gotof_10(int nonterminal_index, object v)
		{
			switch (nonterminal_index)
			{
			case 0:
				return this.push_stack(new Parser.state_type(this.state_15), new Parser.gotof_type(this.gotof_15), v);
			case 2:
				return this.push_stack(new Parser.state_type(this.state_48), new Parser.gotof_type(this.gotof_48), v);
			case 4:
				return this.push_stack(new Parser.state_type(this.state_31), new Parser.gotof_type(this.gotof_31), v);
			case 5:
				return this.push_stack(new Parser.state_type(this.state_27), new Parser.gotof_type(this.gotof_27), v);
			case 6:
				return this.push_stack(new Parser.state_type(this.state_14), new Parser.gotof_type(this.gotof_14), v);
			case 9:
				return this.push_stack(new Parser.state_type(this.state_30), new Parser.gotof_type(this.gotof_30), v);
			case 10:
				return this.push_stack(new Parser.state_type(this.state_13), new Parser.gotof_type(this.gotof_13), v);
			}
			return false;
		}
		private bool state_10(Token token, object value)
		{
			switch (token)
			{
			case Token.token_BlockBegin:
				this.push_stack(new Parser.state_type(this.state_33), new Parser.gotof_type(this.gotof_33), value);
				return false;
			case Token.token_BlockScalarHeader:
				this.push_stack(new Parser.state_type(this.state_49), new Parser.gotof_type(this.gotof_49), value);
				return false;
			case Token.token_Bool:
				this.push_stack(new Parser.state_type(this.state_46), new Parser.gotof_type(this.gotof_46), value);
				return false;
			case Token.token_EOL:
				this.push_stack(new Parser.state_type(this.state_12), new Parser.gotof_type(this.gotof_12), value);
				return false;
			case Token.token_Literal:
				this.push_stack(new Parser.state_type(this.state_44), new Parser.gotof_type(this.gotof_44), value);
				return false;
			case Token.token_LongNumber:
				this.push_stack(new Parser.state_type(this.state_40), new Parser.gotof_type(this.gotof_40), value);
				return false;
			case Token.token_Number:
				this.push_stack(new Parser.state_type(this.state_38), new Parser.gotof_type(this.gotof_38), value);
				return false;
			case Token.token_Text:
				this.push_stack(new Parser.state_type(this.state_42), new Parser.gotof_type(this.gotof_42), value);
				return false;
			}
			this.sa.syntax_error();
			this.error = true;
			return false;
		}
		private bool gotof_11(int nonterminal_index, object v)
		{
			if (nonterminal_index != 0)
			{
				switch (nonterminal_index)
				{
				case 4:
					return this.push_stack(new Parser.state_type(this.state_28), new Parser.gotof_type(this.gotof_28), v);
				case 5:
					break;
				case 6:
					return this.push_stack(new Parser.state_type(this.state_14), new Parser.gotof_type(this.gotof_14), v);
				default:
					if (nonterminal_index == 10)
					{
						return this.push_stack(new Parser.state_type(this.state_13), new Parser.gotof_type(this.gotof_13), v);
					}
					break;
				}
				return false;
			}
			return this.push_stack(new Parser.state_type(this.state_15), new Parser.gotof_type(this.gotof_15), v);
		}
		private bool state_11(Token token, object value)
		{
			switch (token)
			{
			case Token.token_BlockBegin:
				this.push_stack(new Parser.state_type(this.state_33), new Parser.gotof_type(this.gotof_33), value);
				return false;
			case Token.token_BlockEnd:
			{
				Scalar scalar;
				this.sa.GetScalar(out scalar);
				object value2 = scalar;
				this.pop_stack(1);
				return this.stack_top().gotof(9, value2);
			}
			default:
			{
				if (token != Token.token_SequenceHeader)
				{
					this.sa.syntax_error();
					this.error = true;
					return false;
				}
				Scalar scalar2;
				this.sa.GetScalar(out scalar2);
				object value3 = scalar2;
				this.pop_stack(1);
				return this.stack_top().gotof(9, value3);
			}
			}
		}
		private bool gotof_12(int nonterminal_index, object v)
		{
			if (nonterminal_index != 0)
			{
				switch (nonterminal_index)
				{
				case 4:
					return this.push_stack(new Parser.state_type(this.state_29), new Parser.gotof_type(this.gotof_29), v);
				case 5:
					break;
				case 6:
					return this.push_stack(new Parser.state_type(this.state_14), new Parser.gotof_type(this.gotof_14), v);
				default:
					if (nonterminal_index == 10)
					{
						return this.push_stack(new Parser.state_type(this.state_13), new Parser.gotof_type(this.gotof_13), v);
					}
					break;
				}
				return false;
			}
			return this.push_stack(new Parser.state_type(this.state_15), new Parser.gotof_type(this.gotof_15), v);
		}
		private bool state_12(Token token, object value)
		{
			switch (token)
			{
			case Token.token_BlockBegin:
				this.push_stack(new Parser.state_type(this.state_33), new Parser.gotof_type(this.gotof_33), value);
				return false;
			case Token.token_BlockEnd:
			{
				Scalar scalar;
				this.sa.GetScalar(out scalar);
				object value2 = scalar;
				this.pop_stack(1);
				return this.stack_top().gotof(9, value2);
			}
			default:
			{
				if (token != Token.token_SequenceHeader)
				{
					this.sa.syntax_error();
					this.error = true;
					return false;
				}
				Scalar scalar2;
				this.sa.GetScalar(out scalar2);
				object value3 = scalar2;
				this.pop_stack(1);
				return this.stack_top().gotof(9, value3);
			}
			}
		}
		private bool gotof_13(int nonterminal_index, object v)
		{
			return true;
		}
		private bool state_13(Token token, object value)
		{
			if (token == Token.token_BlockEnd)
			{
				Sequence arg = (Sequence)this.get_arg(1, 0);
				Collection collection;
				this.sa.GetCollection(out collection, arg);
				object value2 = collection;
				this.pop_stack(1);
				return this.stack_top().gotof(4, value2);
			}
			if (token == Token.token_DocumentEnd)
			{
				Sequence arg2 = (Sequence)this.get_arg(1, 0);
				Collection collection2;
				this.sa.GetCollection(out collection2, arg2);
				object value3 = collection2;
				this.pop_stack(1);
				return this.stack_top().gotof(4, value3);
			}
			switch (token)
			{
			case Token.token_SequenceHeader:
			{
				Sequence arg3 = (Sequence)this.get_arg(1, 0);
				Collection collection3;
				this.sa.GetCollection(out collection3, arg3);
				object value4 = collection3;
				this.pop_stack(1);
				return this.stack_top().gotof(4, value4);
			}
			case Token.token_Text:
			{
				Sequence arg4 = (Sequence)this.get_arg(1, 0);
				Collection collection4;
				this.sa.GetCollection(out collection4, arg4);
				object value5 = collection4;
				this.pop_stack(1);
				return this.stack_top().gotof(4, value5);
			}
			default:
				this.sa.syntax_error();
				this.error = true;
				return false;
			}
		}
		private bool gotof_14(int nonterminal_index, object v)
		{
			return true;
		}
		private bool state_14(Token token, object value)
		{
			if (token == Token.token_BlockEnd)
			{
				Mapping arg = (Mapping)this.get_arg(1, 0);
				Collection collection;
				this.sa.GetCollection(out collection, arg);
				object value2 = collection;
				this.pop_stack(1);
				return this.stack_top().gotof(4, value2);
			}
			if (token == Token.token_DocumentEnd)
			{
				Mapping arg2 = (Mapping)this.get_arg(1, 0);
				Collection collection2;
				this.sa.GetCollection(out collection2, arg2);
				object value3 = collection2;
				this.pop_stack(1);
				return this.stack_top().gotof(4, value3);
			}
			switch (token)
			{
			case Token.token_SequenceHeader:
			{
				Mapping arg3 = (Mapping)this.get_arg(1, 0);
				Collection collection3;
				this.sa.GetCollection(out collection3, arg3);
				object value4 = collection3;
				this.pop_stack(1);
				return this.stack_top().gotof(4, value4);
			}
			case Token.token_Text:
			{
				Mapping arg4 = (Mapping)this.get_arg(1, 0);
				Collection collection4;
				this.sa.GetCollection(out collection4, arg4);
				object value5 = collection4;
				this.pop_stack(1);
				return this.stack_top().gotof(4, value5);
			}
			default:
				this.sa.syntax_error();
				this.error = true;
				return false;
			}
		}
		private bool gotof_15(int nonterminal_index, object v)
		{
			if (nonterminal_index != 7)
			{
				return nonterminal_index == 11 && this.push_stack(new Parser.state_type(this.state_24), new Parser.gotof_type(this.gotof_24), v);
			}
			return this.push_stack(new Parser.state_type(this.state_16), new Parser.gotof_type(this.gotof_16), v);
		}
		private bool state_15(Token token, object value)
		{
			switch (token)
			{
			case Token.token_SequenceHeader:
				this.push_stack(new Parser.state_type(this.state_9), new Parser.gotof_type(this.gotof_9), value);
				return false;
			case Token.token_Text:
				this.push_stack(new Parser.state_type(this.state_18), new Parser.gotof_type(this.gotof_18), value);
				return false;
			default:
				this.sa.syntax_error();
				this.error = true;
				return false;
			}
		}
		private bool gotof_16(int nonterminal_index, object v)
		{
			return nonterminal_index == 1 && this.push_stack(new Parser.state_type(this.state_17), new Parser.gotof_type(this.gotof_17), v);
		}
		private bool state_16(Token token, object value)
		{
			if (token == Token.token_BlockEnd)
			{
				this.push_stack(new Parser.state_type(this.state_36), new Parser.gotof_type(this.gotof_36), value);
				return false;
			}
			if (token != Token.token_Text)
			{
				this.sa.syntax_error();
				this.error = true;
				return false;
			}
			this.push_stack(new Parser.state_type(this.state_20), new Parser.gotof_type(this.gotof_20), value);
			return false;
		}
		private bool gotof_17(int nonterminal_index, object v)
		{
			return true;
		}
		private bool state_17(Token token, object value)
		{
			if (token == Token.token_BlockEnd)
			{
				Mapping arg = (Mapping)this.get_arg(3, 1);
				Mapping mapping;
				this.sa.GetMapping(out mapping, arg);
				object value2 = mapping;
				this.pop_stack(3);
				return this.stack_top().gotof(6, value2);
			}
			if (token == Token.token_DocumentEnd)
			{
				Mapping arg2 = (Mapping)this.get_arg(3, 1);
				Mapping mapping2;
				this.sa.GetMapping(out mapping2, arg2);
				object value3 = mapping2;
				this.pop_stack(3);
				return this.stack_top().gotof(6, value3);
			}
			switch (token)
			{
			case Token.token_SequenceHeader:
			{
				Mapping arg3 = (Mapping)this.get_arg(3, 1);
				Mapping mapping3;
				this.sa.GetMapping(out mapping3, arg3);
				object value4 = mapping3;
				this.pop_stack(3);
				return this.stack_top().gotof(6, value4);
			}
			case Token.token_Text:
			{
				Mapping arg4 = (Mapping)this.get_arg(3, 1);
				Mapping mapping4;
				this.sa.GetMapping(out mapping4, arg4);
				object value5 = mapping4;
				this.pop_stack(3);
				return this.stack_top().gotof(6, value5);
			}
			default:
				this.sa.syntax_error();
				this.error = true;
				return false;
			}
		}
		private bool gotof_18(int nonterminal_index, object v)
		{
			return true;
		}
		private bool state_18(Token token, object value)
		{
			if (token == Token.token_Mapper)
			{
				this.push_stack(new Parser.state_type(this.state_5), new Parser.gotof_type(this.gotof_5), value);
				return false;
			}
			this.sa.syntax_error();
			this.error = true;
			return false;
		}
		private bool gotof_19(int nonterminal_index, object v)
		{
			return true;
		}
		private bool state_19(Token token, object value)
		{
			if (token == Token.token_BlockEnd)
			{
				string arg = (string)this.get_arg(3, 0);
				CollectionElement arg2 = (CollectionElement)this.get_arg(3, 2);
				Mapping mapping;
				this.sa.GetMappingElement(out mapping, arg, arg2);
				object value2 = mapping;
				this.pop_stack(3);
				return this.stack_top().gotof(7, value2);
			}
			if (token != Token.token_Text)
			{
				this.sa.syntax_error();
				this.error = true;
				return false;
			}
			string arg3 = (string)this.get_arg(3, 0);
			CollectionElement arg4 = (CollectionElement)this.get_arg(3, 2);
			Mapping mapping2;
			this.sa.GetMappingElement(out mapping2, arg3, arg4);
			object value3 = mapping2;
			this.pop_stack(3);
			return this.stack_top().gotof(7, value3);
		}
		private bool gotof_20(int nonterminal_index, object v)
		{
			return true;
		}
		private bool state_20(Token token, object value)
		{
			if (token == Token.token_Mapper)
			{
				this.push_stack(new Parser.state_type(this.state_6), new Parser.gotof_type(this.gotof_6), value);
				return false;
			}
			this.sa.syntax_error();
			this.error = true;
			return false;
		}
		private bool gotof_21(int nonterminal_index, object v)
		{
			return true;
		}
		private bool state_21(Token token, object value)
		{
			if (token == Token.token_BlockEnd)
			{
				Mapping arg = (Mapping)this.get_arg(4, 0);
				string arg2 = (string)this.get_arg(4, 1);
				CollectionElement arg3 = (CollectionElement)this.get_arg(4, 3);
				Mapping mapping;
				this.sa.GetMappingElement(out mapping, arg, arg2, arg3);
				object value2 = mapping;
				this.pop_stack(4);
				return this.stack_top().gotof(7, value2);
			}
			if (token != Token.token_Text)
			{
				this.sa.syntax_error();
				this.error = true;
				return false;
			}
			Mapping arg4 = (Mapping)this.get_arg(4, 0);
			string arg5 = (string)this.get_arg(4, 1);
			CollectionElement arg6 = (CollectionElement)this.get_arg(4, 3);
			Mapping mapping2;
			this.sa.GetMappingElement(out mapping2, arg4, arg5, arg6);
			object value3 = mapping2;
			this.pop_stack(4);
			return this.stack_top().gotof(7, value3);
		}
		private bool gotof_22(int nonterminal_index, object v)
		{
			return true;
		}
		private bool state_22(Token token, object value)
		{
			if (token == Token.token_BlockEnd)
			{
				string arg = (string)this.get_arg(4, 0);
				Collection arg2 = (Collection)this.get_arg(4, 3);
				Mapping mapping;
				this.sa.GetMappingElement(out mapping, arg, arg2);
				object value2 = mapping;
				this.pop_stack(4);
				return this.stack_top().gotof(7, value2);
			}
			if (token != Token.token_Text)
			{
				this.sa.syntax_error();
				this.error = true;
				return false;
			}
			string arg3 = (string)this.get_arg(4, 0);
			Collection arg4 = (Collection)this.get_arg(4, 3);
			Mapping mapping2;
			this.sa.GetMappingElement(out mapping2, arg3, arg4);
			object value3 = mapping2;
			this.pop_stack(4);
			return this.stack_top().gotof(7, value3);
		}
		private bool gotof_23(int nonterminal_index, object v)
		{
			return true;
		}
		private bool state_23(Token token, object value)
		{
			if (token == Token.token_BlockEnd)
			{
				Mapping arg = (Mapping)this.get_arg(5, 0);
				string arg2 = (string)this.get_arg(5, 1);
				Collection arg3 = (Collection)this.get_arg(5, 4);
				Mapping mapping;
				this.sa.GetMappingElement(out mapping, arg, arg2, arg3);
				object value2 = mapping;
				this.pop_stack(5);
				return this.stack_top().gotof(7, value2);
			}
			if (token != Token.token_Text)
			{
				this.sa.syntax_error();
				this.error = true;
				return false;
			}
			Mapping arg4 = (Mapping)this.get_arg(5, 0);
			string arg5 = (string)this.get_arg(5, 1);
			Collection arg6 = (Collection)this.get_arg(5, 4);
			Mapping mapping2;
			this.sa.GetMappingElement(out mapping2, arg4, arg5, arg6);
			object value3 = mapping2;
			this.pop_stack(5);
			return this.stack_top().gotof(7, value3);
		}
		private bool gotof_24(int nonterminal_index, object v)
		{
			return nonterminal_index == 1 && this.push_stack(new Parser.state_type(this.state_25), new Parser.gotof_type(this.gotof_25), v);
		}
		private bool state_24(Token token, object value)
		{
			if (token == Token.token_BlockEnd)
			{
				this.push_stack(new Parser.state_type(this.state_36), new Parser.gotof_type(this.gotof_36), value);
				return false;
			}
			if (token != Token.token_SequenceHeader)
			{
				this.sa.syntax_error();
				this.error = true;
				return false;
			}
			this.push_stack(new Parser.state_type(this.state_10), new Parser.gotof_type(this.gotof_10), value);
			return false;
		}
		private bool gotof_25(int nonterminal_index, object v)
		{
			return true;
		}
		private bool state_25(Token token, object value)
		{
			if (token == Token.token_BlockEnd)
			{
				Sequence arg = (Sequence)this.get_arg(3, 1);
				Sequence sequence;
				this.sa.GetSequence(out sequence, arg);
				object value2 = sequence;
				this.pop_stack(3);
				return this.stack_top().gotof(10, value2);
			}
			if (token == Token.token_DocumentEnd)
			{
				Sequence arg2 = (Sequence)this.get_arg(3, 1);
				Sequence sequence2;
				this.sa.GetSequence(out sequence2, arg2);
				object value3 = sequence2;
				this.pop_stack(3);
				return this.stack_top().gotof(10, value3);
			}
			switch (token)
			{
			case Token.token_SequenceHeader:
			{
				Sequence arg3 = (Sequence)this.get_arg(3, 1);
				Sequence sequence3;
				this.sa.GetSequence(out sequence3, arg3);
				object value4 = sequence3;
				this.pop_stack(3);
				return this.stack_top().gotof(10, value4);
			}
			case Token.token_Text:
			{
				Sequence arg4 = (Sequence)this.get_arg(3, 1);
				Sequence sequence4;
				this.sa.GetSequence(out sequence4, arg4);
				object value5 = sequence4;
				this.pop_stack(3);
				return this.stack_top().gotof(10, value5);
			}
			default:
				this.sa.syntax_error();
				this.error = true;
				return false;
			}
		}
		private bool gotof_26(int nonterminal_index, object v)
		{
			return true;
		}
		private bool state_26(Token token, object value)
		{
			if (token == Token.token_BlockEnd)
			{
				CollectionElement arg = (CollectionElement)this.get_arg(2, 1);
				Sequence sequence;
				this.sa.GetSequenceElement(out sequence, arg);
				object value2 = sequence;
				this.pop_stack(2);
				return this.stack_top().gotof(11, value2);
			}
			if (token != Token.token_SequenceHeader)
			{
				this.sa.syntax_error();
				this.error = true;
				return false;
			}
			CollectionElement arg2 = (CollectionElement)this.get_arg(2, 1);
			Sequence sequence2;
			this.sa.GetSequenceElement(out sequence2, arg2);
			object value3 = sequence2;
			this.pop_stack(2);
			return this.stack_top().gotof(11, value3);
		}
		private bool gotof_27(int nonterminal_index, object v)
		{
			return true;
		}
		private bool state_27(Token token, object value)
		{
			if (token == Token.token_BlockEnd)
			{
				Sequence arg = (Sequence)this.get_arg(3, 0);
				CollectionElement arg2 = (CollectionElement)this.get_arg(3, 2);
				Sequence sequence;
				this.sa.GetSequenceElement(out sequence, arg, arg2);
				object value2 = sequence;
				this.pop_stack(3);
				return this.stack_top().gotof(11, value2);
			}
			if (token != Token.token_SequenceHeader)
			{
				this.sa.syntax_error();
				this.error = true;
				return false;
			}
			Sequence arg3 = (Sequence)this.get_arg(3, 0);
			CollectionElement arg4 = (CollectionElement)this.get_arg(3, 2);
			Sequence sequence2;
			this.sa.GetSequenceElement(out sequence2, arg3, arg4);
			object value3 = sequence2;
			this.pop_stack(3);
			return this.stack_top().gotof(11, value3);
		}
		private bool gotof_28(int nonterminal_index, object v)
		{
			return true;
		}
		private bool state_28(Token token, object value)
		{
			if (token == Token.token_BlockEnd)
			{
				Collection arg = (Collection)this.get_arg(3, 2);
				Sequence sequence;
				this.sa.GetSequenceElement(out sequence, arg);
				object value2 = sequence;
				this.pop_stack(3);
				return this.stack_top().gotof(11, value2);
			}
			if (token != Token.token_SequenceHeader)
			{
				this.sa.syntax_error();
				this.error = true;
				return false;
			}
			Collection arg2 = (Collection)this.get_arg(3, 2);
			Sequence sequence2;
			this.sa.GetSequenceElement(out sequence2, arg2);
			object value3 = sequence2;
			this.pop_stack(3);
			return this.stack_top().gotof(11, value3);
		}
		private bool gotof_29(int nonterminal_index, object v)
		{
			return true;
		}
		private bool state_29(Token token, object value)
		{
			if (token == Token.token_BlockEnd)
			{
				Sequence arg = (Sequence)this.get_arg(4, 0);
				Collection arg2 = (Collection)this.get_arg(4, 3);
				Sequence sequence;
				this.sa.GetSequenceElement(out sequence, arg, arg2);
				object value2 = sequence;
				this.pop_stack(4);
				return this.stack_top().gotof(11, value2);
			}
			if (token != Token.token_SequenceHeader)
			{
				this.sa.syntax_error();
				this.error = true;
				return false;
			}
			Sequence arg3 = (Sequence)this.get_arg(4, 0);
			Collection arg4 = (Collection)this.get_arg(4, 3);
			Sequence sequence2;
			this.sa.GetSequenceElement(out sequence2, arg3, arg4);
			object value3 = sequence2;
			this.pop_stack(4);
			return this.stack_top().gotof(11, value3);
		}
		private bool gotof_30(int nonterminal_index, object v)
		{
			return true;
		}
		private bool state_30(Token token, object value)
		{
			if (token == Token.token_BlockEnd)
			{
				Scalar arg = (Scalar)this.get_arg(1, 0);
				CollectionElement collectionElement;
				this.sa.GetCollectionElement(out collectionElement, arg);
				object value2 = collectionElement;
				this.pop_stack(1);
				return this.stack_top().gotof(5, value2);
			}
			switch (token)
			{
			case Token.token_SequenceHeader:
			{
				Scalar arg2 = (Scalar)this.get_arg(1, 0);
				CollectionElement collectionElement2;
				this.sa.GetCollectionElement(out collectionElement2, arg2);
				object value3 = collectionElement2;
				this.pop_stack(1);
				return this.stack_top().gotof(5, value3);
			}
			case Token.token_Text:
			{
				Scalar arg3 = (Scalar)this.get_arg(1, 0);
				CollectionElement collectionElement3;
				this.sa.GetCollectionElement(out collectionElement3, arg3);
				object value4 = collectionElement3;
				this.pop_stack(1);
				return this.stack_top().gotof(5, value4);
			}
			default:
				this.sa.syntax_error();
				this.error = true;
				return false;
			}
		}
		private bool gotof_31(int nonterminal_index, object v)
		{
			return true;
		}
		private bool state_31(Token token, object value)
		{
			if (token == Token.token_BlockEnd)
			{
				Collection arg = (Collection)this.get_arg(1, 0);
				CollectionElement collectionElement;
				this.sa.GetCollectionElement(out collectionElement, arg);
				object value2 = collectionElement;
				this.pop_stack(1);
				return this.stack_top().gotof(5, value2);
			}
			switch (token)
			{
			case Token.token_SequenceHeader:
			{
				Collection arg2 = (Collection)this.get_arg(1, 0);
				CollectionElement collectionElement2;
				this.sa.GetCollectionElement(out collectionElement2, arg2);
				object value3 = collectionElement2;
				this.pop_stack(1);
				return this.stack_top().gotof(5, value3);
			}
			case Token.token_Text:
			{
				Collection arg3 = (Collection)this.get_arg(1, 0);
				CollectionElement collectionElement3;
				this.sa.GetCollectionElement(out collectionElement3, arg3);
				object value4 = collectionElement3;
				this.pop_stack(1);
				return this.stack_top().gotof(5, value4);
			}
			default:
				this.sa.syntax_error();
				this.error = true;
				return false;
			}
		}
		private bool gotof_32(int nonterminal_index, object v)
		{
			return nonterminal_index == 0 && this.push_stack(new Parser.state_type(this.state_50), new Parser.gotof_type(this.gotof_50), v);
		}
		private bool state_32(Token token, object value)
		{
			if (token == Token.token_BlockBegin)
			{
				this.push_stack(new Parser.state_type(this.state_33), new Parser.gotof_type(this.gotof_33), value);
				return false;
			}
			this.sa.syntax_error();
			this.error = true;
			return false;
		}
		private bool gotof_33(int nonterminal_index, object v)
		{
			return true;
		}
		private bool state_33(Token token, object value)
		{
			if (token == Token.token_EOL)
			{
				this.push_stack(new Parser.state_type(this.state_34), new Parser.gotof_type(this.gotof_34), value);
				return false;
			}
			this.sa.syntax_error();
			this.error = true;
			return false;
		}
		private bool gotof_34(int nonterminal_index, object v)
		{
			return true;
		}
		private bool state_34(Token token, object value)
		{
			switch (token)
			{
			case Token.token_LongNumber:
				this.pop_stack(2);
				return this.stack_top().gotof(0, new object());
			case Token.token_Number:
				this.pop_stack(2);
				return this.stack_top().gotof(0, new object());
			case Token.token_SequenceHeader:
				this.pop_stack(2);
				return this.stack_top().gotof(0, new object());
			case Token.token_Text:
				this.pop_stack(2);
				return this.stack_top().gotof(0, new object());
			}
			this.sa.syntax_error();
			this.error = true;
			return false;
		}
		private bool gotof_35(int nonterminal_index, object v)
		{
			return nonterminal_index == 1 && this.push_stack(new Parser.state_type(this.state_51), new Parser.gotof_type(this.gotof_51), v);
		}
		private bool state_35(Token token, object value)
		{
			if (token != Token.token_BlockEnd)
			{
				switch (token)
				{
				case Token.token_LongNumber:
					this.push_stack(new Parser.state_type(this.state_58), new Parser.gotof_type(this.gotof_58), value);
					return false;
				case Token.token_Number:
					this.push_stack(new Parser.state_type(this.state_54), new Parser.gotof_type(this.gotof_54), value);
					return false;
				case Token.token_Text:
					this.push_stack(new Parser.state_type(this.state_62), new Parser.gotof_type(this.gotof_62), value);
					return false;
				}
				this.sa.syntax_error();
				this.error = true;
				return false;
			}
			this.push_stack(new Parser.state_type(this.state_36), new Parser.gotof_type(this.gotof_36), value);
			return false;
		}
		private bool gotof_36(int nonterminal_index, object v)
		{
			return true;
		}
		private bool state_36(Token token, object value)
		{
			if (token == Token.token_EOL)
			{
				this.push_stack(new Parser.state_type(this.state_37), new Parser.gotof_type(this.gotof_37), value);
				return false;
			}
			this.sa.syntax_error();
			this.error = true;
			return false;
		}
		private bool gotof_37(int nonterminal_index, object v)
		{
			return true;
		}
		private bool state_37(Token token, object value)
		{
			if (token == Token.token_BlockEnd)
			{
				this.pop_stack(2);
				return this.stack_top().gotof(1, new object());
			}
			if (token == Token.token_DocumentEnd)
			{
				this.pop_stack(2);
				return this.stack_top().gotof(1, new object());
			}
			switch (token)
			{
			case Token.token_SequenceHeader:
				this.pop_stack(2);
				return this.stack_top().gotof(1, new object());
			case Token.token_Text:
				this.pop_stack(2);
				return this.stack_top().gotof(1, new object());
			default:
				this.sa.syntax_error();
				this.error = true;
				return false;
			}
		}
		private bool gotof_38(int nonterminal_index, object v)
		{
			return true;
		}
		private bool state_38(Token token, object value)
		{
			if (token == Token.token_EOL)
			{
				this.push_stack(new Parser.state_type(this.state_39), new Parser.gotof_type(this.gotof_39), value);
				return false;
			}
			this.sa.syntax_error();
			this.error = true;
			return false;
		}
		private bool gotof_39(int nonterminal_index, object v)
		{
			return true;
		}
		private bool state_39(Token token, object value)
		{
			if (token == Token.token_BlockEnd)
			{
				int arg = (int)this.get_arg(2, 0);
				Scalar scalar;
				this.sa.GetScalar(out scalar, arg);
				object value2 = scalar;
				this.pop_stack(2);
				return this.stack_top().gotof(9, value2);
			}
			switch (token)
			{
			case Token.token_SequenceHeader:
			{
				int arg2 = (int)this.get_arg(2, 0);
				Scalar scalar2;
				this.sa.GetScalar(out scalar2, arg2);
				object value3 = scalar2;
				this.pop_stack(2);
				return this.stack_top().gotof(9, value3);
			}
			case Token.token_Text:
			{
				int arg3 = (int)this.get_arg(2, 0);
				Scalar scalar3;
				this.sa.GetScalar(out scalar3, arg3);
				object value4 = scalar3;
				this.pop_stack(2);
				return this.stack_top().gotof(9, value4);
			}
			default:
				this.sa.syntax_error();
				this.error = true;
				return false;
			}
		}
		private bool gotof_40(int nonterminal_index, object v)
		{
			return true;
		}
		private bool state_40(Token token, object value)
		{
			if (token == Token.token_EOL)
			{
				this.push_stack(new Parser.state_type(this.state_41), new Parser.gotof_type(this.gotof_41), value);
				return false;
			}
			this.sa.syntax_error();
			this.error = true;
			return false;
		}
		private bool gotof_41(int nonterminal_index, object v)
		{
			return true;
		}
		private bool state_41(Token token, object value)
		{
			if (token == Token.token_BlockEnd)
			{
				long arg = (long)this.get_arg(2, 0);
				Scalar scalar;
				this.sa.GetScalar(out scalar, arg);
				object value2 = scalar;
				this.pop_stack(2);
				return this.stack_top().gotof(9, value2);
			}
			switch (token)
			{
			case Token.token_SequenceHeader:
			{
				long arg2 = (long)this.get_arg(2, 0);
				Scalar scalar2;
				this.sa.GetScalar(out scalar2, arg2);
				object value3 = scalar2;
				this.pop_stack(2);
				return this.stack_top().gotof(9, value3);
			}
			case Token.token_Text:
			{
				long arg3 = (long)this.get_arg(2, 0);
				Scalar scalar3;
				this.sa.GetScalar(out scalar3, arg3);
				object value4 = scalar3;
				this.pop_stack(2);
				return this.stack_top().gotof(9, value4);
			}
			default:
				this.sa.syntax_error();
				this.error = true;
				return false;
			}
		}
		private bool gotof_42(int nonterminal_index, object v)
		{
			return true;
		}
		private bool state_42(Token token, object value)
		{
			if (token == Token.token_EOL)
			{
				this.push_stack(new Parser.state_type(this.state_43), new Parser.gotof_type(this.gotof_43), value);
				return false;
			}
			this.sa.syntax_error();
			this.error = true;
			return false;
		}
		private bool gotof_43(int nonterminal_index, object v)
		{
			return true;
		}
		private bool state_43(Token token, object value)
		{
			if (token == Token.token_BlockEnd)
			{
				string arg = (string)this.get_arg(2, 0);
				Scalar scalar;
				this.sa.GetScalar(out scalar, arg);
				object value2 = scalar;
				this.pop_stack(2);
				return this.stack_top().gotof(9, value2);
			}
			switch (token)
			{
			case Token.token_SequenceHeader:
			{
				string arg2 = (string)this.get_arg(2, 0);
				Scalar scalar2;
				this.sa.GetScalar(out scalar2, arg2);
				object value3 = scalar2;
				this.pop_stack(2);
				return this.stack_top().gotof(9, value3);
			}
			case Token.token_Text:
			{
				string arg3 = (string)this.get_arg(2, 0);
				Scalar scalar3;
				this.sa.GetScalar(out scalar3, arg3);
				object value4 = scalar3;
				this.pop_stack(2);
				return this.stack_top().gotof(9, value4);
			}
			default:
				this.sa.syntax_error();
				this.error = true;
				return false;
			}
		}
		private bool gotof_44(int nonterminal_index, object v)
		{
			return true;
		}
		private bool state_44(Token token, object value)
		{
			if (token == Token.token_EOL)
			{
				this.push_stack(new Parser.state_type(this.state_45), new Parser.gotof_type(this.gotof_45), value);
				return false;
			}
			this.sa.syntax_error();
			this.error = true;
			return false;
		}
		private bool gotof_45(int nonterminal_index, object v)
		{
			return true;
		}
		private bool state_45(Token token, object value)
		{
			if (token == Token.token_BlockEnd)
			{
				string arg = (string)this.get_arg(2, 0);
				Scalar scalar;
				this.sa.GetScalar(out scalar, arg);
				object value2 = scalar;
				this.pop_stack(2);
				return this.stack_top().gotof(9, value2);
			}
			switch (token)
			{
			case Token.token_SequenceHeader:
			{
				string arg2 = (string)this.get_arg(2, 0);
				Scalar scalar2;
				this.sa.GetScalar(out scalar2, arg2);
				object value3 = scalar2;
				this.pop_stack(2);
				return this.stack_top().gotof(9, value3);
			}
			case Token.token_Text:
			{
				string arg3 = (string)this.get_arg(2, 0);
				Scalar scalar3;
				this.sa.GetScalar(out scalar3, arg3);
				object value4 = scalar3;
				this.pop_stack(2);
				return this.stack_top().gotof(9, value4);
			}
			default:
				this.sa.syntax_error();
				this.error = true;
				return false;
			}
		}
		private bool gotof_46(int nonterminal_index, object v)
		{
			return true;
		}
		private bool state_46(Token token, object value)
		{
			if (token == Token.token_EOL)
			{
				this.push_stack(new Parser.state_type(this.state_47), new Parser.gotof_type(this.gotof_47), value);
				return false;
			}
			this.sa.syntax_error();
			this.error = true;
			return false;
		}
		private bool gotof_47(int nonterminal_index, object v)
		{
			return true;
		}
		private bool state_47(Token token, object value)
		{
			if (token == Token.token_BlockEnd)
			{
				bool arg = (bool)this.get_arg(2, 0);
				Scalar scalar;
				this.sa.GetScalar(out scalar, arg);
				object value2 = scalar;
				this.pop_stack(2);
				return this.stack_top().gotof(9, value2);
			}
			switch (token)
			{
			case Token.token_SequenceHeader:
			{
				bool arg2 = (bool)this.get_arg(2, 0);
				Scalar scalar2;
				this.sa.GetScalar(out scalar2, arg2);
				object value3 = scalar2;
				this.pop_stack(2);
				return this.stack_top().gotof(9, value3);
			}
			case Token.token_Text:
			{
				bool arg3 = (bool)this.get_arg(2, 0);
				Scalar scalar3;
				this.sa.GetScalar(out scalar3, arg3);
				object value4 = scalar3;
				this.pop_stack(2);
				return this.stack_top().gotof(9, value4);
			}
			default:
				this.sa.syntax_error();
				this.error = true;
				return false;
			}
		}
		private bool gotof_48(int nonterminal_index, object v)
		{
			return true;
		}
		private bool state_48(Token token, object value)
		{
			if (token == Token.token_BlockEnd)
			{
				BlockScalar arg = (BlockScalar)this.get_arg(1, 0);
				Scalar scalar;
				this.sa.GetScalar(out scalar, arg);
				object value2 = scalar;
				this.pop_stack(1);
				return this.stack_top().gotof(9, value2);
			}
			switch (token)
			{
			case Token.token_SequenceHeader:
			{
				BlockScalar arg2 = (BlockScalar)this.get_arg(1, 0);
				Scalar scalar2;
				this.sa.GetScalar(out scalar2, arg2);
				object value3 = scalar2;
				this.pop_stack(1);
				return this.stack_top().gotof(9, value3);
			}
			case Token.token_Text:
			{
				BlockScalar arg3 = (BlockScalar)this.get_arg(1, 0);
				Scalar scalar3;
				this.sa.GetScalar(out scalar3, arg3);
				object value4 = scalar3;
				this.pop_stack(1);
				return this.stack_top().gotof(9, value4);
			}
			default:
				this.sa.syntax_error();
				this.error = true;
				return false;
			}
		}
		private bool gotof_49(int nonterminal_index, object v)
		{
			return true;
		}
		private bool state_49(Token token, object value)
		{
			if (token == Token.token_EOL)
			{
				this.push_stack(new Parser.state_type(this.state_32), new Parser.gotof_type(this.gotof_32), value);
				return false;
			}
			this.sa.syntax_error();
			this.error = true;
			return false;
		}
		private bool gotof_50(int nonterminal_index, object v)
		{
			return nonterminal_index == 3 && this.push_stack(new Parser.state_type(this.state_35), new Parser.gotof_type(this.gotof_35), v);
		}
		private bool state_50(Token token, object value)
		{
			switch (token)
			{
			case Token.token_LongNumber:
				this.push_stack(new Parser.state_type(this.state_56), new Parser.gotof_type(this.gotof_56), value);
				return false;
			case Token.token_Number:
				this.push_stack(new Parser.state_type(this.state_52), new Parser.gotof_type(this.gotof_52), value);
				return false;
			case Token.token_Text:
				this.push_stack(new Parser.state_type(this.state_60), new Parser.gotof_type(this.gotof_60), value);
				return false;
			}
			this.sa.syntax_error();
			this.error = true;
			return false;
		}
		private bool gotof_51(int nonterminal_index, object v)
		{
			return true;
		}
		private bool state_51(Token token, object value)
		{
			if (token == Token.token_BlockEnd)
			{
				string arg = (string)this.get_arg(5, 0);
				BlockScalar arg2 = (BlockScalar)this.get_arg(5, 3);
				BlockScalar blockScalar;
				this.sa.GetBlockScalar(out blockScalar, arg, arg2);
				object value2 = blockScalar;
				this.pop_stack(5);
				return this.stack_top().gotof(2, value2);
			}
			switch (token)
			{
			case Token.token_SequenceHeader:
			{
				string arg3 = (string)this.get_arg(5, 0);
				BlockScalar arg4 = (BlockScalar)this.get_arg(5, 3);
				BlockScalar blockScalar2;
				this.sa.GetBlockScalar(out blockScalar2, arg3, arg4);
				object value3 = blockScalar2;
				this.pop_stack(5);
				return this.stack_top().gotof(2, value3);
			}
			case Token.token_Text:
			{
				string arg5 = (string)this.get_arg(5, 0);
				BlockScalar arg6 = (BlockScalar)this.get_arg(5, 3);
				BlockScalar blockScalar3;
				this.sa.GetBlockScalar(out blockScalar3, arg5, arg6);
				object value4 = blockScalar3;
				this.pop_stack(5);
				return this.stack_top().gotof(2, value4);
			}
			default:
				this.sa.syntax_error();
				this.error = true;
				return false;
			}
		}
		private bool gotof_52(int nonterminal_index, object v)
		{
			return true;
		}
		private bool state_52(Token token, object value)
		{
			if (token == Token.token_EOL)
			{
				this.push_stack(new Parser.state_type(this.state_53), new Parser.gotof_type(this.gotof_53), value);
				return false;
			}
			this.sa.syntax_error();
			this.error = true;
			return false;
		}
		private bool gotof_53(int nonterminal_index, object v)
		{
			return true;
		}
		private bool state_53(Token token, object value)
		{
			if (token != Token.token_BlockEnd)
			{
				switch (token)
				{
				case Token.token_LongNumber:
				{
					int arg = (int)this.get_arg(2, 0);
					BlockScalar blockScalar;
					this.sa.GetBlockScalarData(out blockScalar, arg);
					object value2 = blockScalar;
					this.pop_stack(2);
					return this.stack_top().gotof(3, value2);
				}
				case Token.token_Number:
				{
					int arg2 = (int)this.get_arg(2, 0);
					BlockScalar blockScalar2;
					this.sa.GetBlockScalarData(out blockScalar2, arg2);
					object value3 = blockScalar2;
					this.pop_stack(2);
					return this.stack_top().gotof(3, value3);
				}
				case Token.token_Text:
				{
					int arg3 = (int)this.get_arg(2, 0);
					BlockScalar blockScalar3;
					this.sa.GetBlockScalarData(out blockScalar3, arg3);
					object value4 = blockScalar3;
					this.pop_stack(2);
					return this.stack_top().gotof(3, value4);
				}
				}
				this.sa.syntax_error();
				this.error = true;
				return false;
			}
			int arg4 = (int)this.get_arg(2, 0);
			BlockScalar blockScalar4;
			this.sa.GetBlockScalarData(out blockScalar4, arg4);
			object value5 = blockScalar4;
			this.pop_stack(2);
			return this.stack_top().gotof(3, value5);
		}
		private bool gotof_54(int nonterminal_index, object v)
		{
			return true;
		}
		private bool state_54(Token token, object value)
		{
			if (token == Token.token_EOL)
			{
				this.push_stack(new Parser.state_type(this.state_55), new Parser.gotof_type(this.gotof_55), value);
				return false;
			}
			this.sa.syntax_error();
			this.error = true;
			return false;
		}
		private bool gotof_55(int nonterminal_index, object v)
		{
			return true;
		}
		private bool state_55(Token token, object value)
		{
			if (token != Token.token_BlockEnd)
			{
				switch (token)
				{
				case Token.token_LongNumber:
				{
					BlockScalar arg = (BlockScalar)this.get_arg(3, 0);
					int arg2 = (int)this.get_arg(3, 1);
					BlockScalar blockScalar;
					this.sa.GetBlockScalarData(out blockScalar, arg, arg2);
					object value2 = blockScalar;
					this.pop_stack(3);
					return this.stack_top().gotof(3, value2);
				}
				case Token.token_Number:
				{
					BlockScalar arg3 = (BlockScalar)this.get_arg(3, 0);
					int arg4 = (int)this.get_arg(3, 1);
					BlockScalar blockScalar2;
					this.sa.GetBlockScalarData(out blockScalar2, arg3, arg4);
					object value3 = blockScalar2;
					this.pop_stack(3);
					return this.stack_top().gotof(3, value3);
				}
				case Token.token_Text:
				{
					BlockScalar arg5 = (BlockScalar)this.get_arg(3, 0);
					int arg6 = (int)this.get_arg(3, 1);
					BlockScalar blockScalar3;
					this.sa.GetBlockScalarData(out blockScalar3, arg5, arg6);
					object value4 = blockScalar3;
					this.pop_stack(3);
					return this.stack_top().gotof(3, value4);
				}
				}
				this.sa.syntax_error();
				this.error = true;
				return false;
			}
			BlockScalar arg7 = (BlockScalar)this.get_arg(3, 0);
			int arg8 = (int)this.get_arg(3, 1);
			BlockScalar blockScalar4;
			this.sa.GetBlockScalarData(out blockScalar4, arg7, arg8);
			object value5 = blockScalar4;
			this.pop_stack(3);
			return this.stack_top().gotof(3, value5);
		}
		private bool gotof_56(int nonterminal_index, object v)
		{
			return true;
		}
		private bool state_56(Token token, object value)
		{
			if (token == Token.token_EOL)
			{
				this.push_stack(new Parser.state_type(this.state_57), new Parser.gotof_type(this.gotof_57), value);
				return false;
			}
			this.sa.syntax_error();
			this.error = true;
			return false;
		}
		private bool gotof_57(int nonterminal_index, object v)
		{
			return true;
		}
		private bool state_57(Token token, object value)
		{
			if (token != Token.token_BlockEnd)
			{
				switch (token)
				{
				case Token.token_LongNumber:
				{
					long arg = (long)this.get_arg(2, 0);
					BlockScalar blockScalar;
					this.sa.GetBlockScalarData(out blockScalar, arg);
					object value2 = blockScalar;
					this.pop_stack(2);
					return this.stack_top().gotof(3, value2);
				}
				case Token.token_Number:
				{
					long arg2 = (long)this.get_arg(2, 0);
					BlockScalar blockScalar2;
					this.sa.GetBlockScalarData(out blockScalar2, arg2);
					object value3 = blockScalar2;
					this.pop_stack(2);
					return this.stack_top().gotof(3, value3);
				}
				case Token.token_Text:
				{
					long arg3 = (long)this.get_arg(2, 0);
					BlockScalar blockScalar3;
					this.sa.GetBlockScalarData(out blockScalar3, arg3);
					object value4 = blockScalar3;
					this.pop_stack(2);
					return this.stack_top().gotof(3, value4);
				}
				}
				this.sa.syntax_error();
				this.error = true;
				return false;
			}
			long arg4 = (long)this.get_arg(2, 0);
			BlockScalar blockScalar4;
			this.sa.GetBlockScalarData(out blockScalar4, arg4);
			object value5 = blockScalar4;
			this.pop_stack(2);
			return this.stack_top().gotof(3, value5);
		}
		private bool gotof_58(int nonterminal_index, object v)
		{
			return true;
		}
		private bool state_58(Token token, object value)
		{
			if (token == Token.token_EOL)
			{
				this.push_stack(new Parser.state_type(this.state_59), new Parser.gotof_type(this.gotof_59), value);
				return false;
			}
			this.sa.syntax_error();
			this.error = true;
			return false;
		}
		private bool gotof_59(int nonterminal_index, object v)
		{
			return true;
		}
		private bool state_59(Token token, object value)
		{
			if (token != Token.token_BlockEnd)
			{
				switch (token)
				{
				case Token.token_LongNumber:
				{
					BlockScalar arg = (BlockScalar)this.get_arg(3, 0);
					long arg2 = (long)this.get_arg(3, 1);
					BlockScalar blockScalar;
					this.sa.GetBlockScalarData(out blockScalar, arg, arg2);
					object value2 = blockScalar;
					this.pop_stack(3);
					return this.stack_top().gotof(3, value2);
				}
				case Token.token_Number:
				{
					BlockScalar arg3 = (BlockScalar)this.get_arg(3, 0);
					long arg4 = (long)this.get_arg(3, 1);
					BlockScalar blockScalar2;
					this.sa.GetBlockScalarData(out blockScalar2, arg3, arg4);
					object value3 = blockScalar2;
					this.pop_stack(3);
					return this.stack_top().gotof(3, value3);
				}
				case Token.token_Text:
				{
					BlockScalar arg5 = (BlockScalar)this.get_arg(3, 0);
					long arg6 = (long)this.get_arg(3, 1);
					BlockScalar blockScalar3;
					this.sa.GetBlockScalarData(out blockScalar3, arg5, arg6);
					object value4 = blockScalar3;
					this.pop_stack(3);
					return this.stack_top().gotof(3, value4);
				}
				}
				this.sa.syntax_error();
				this.error = true;
				return false;
			}
			BlockScalar arg7 = (BlockScalar)this.get_arg(3, 0);
			long arg8 = (long)this.get_arg(3, 1);
			BlockScalar blockScalar4;
			this.sa.GetBlockScalarData(out blockScalar4, arg7, arg8);
			object value5 = blockScalar4;
			this.pop_stack(3);
			return this.stack_top().gotof(3, value5);
		}
		private bool gotof_60(int nonterminal_index, object v)
		{
			return true;
		}
		private bool state_60(Token token, object value)
		{
			if (token == Token.token_EOL)
			{
				this.push_stack(new Parser.state_type(this.state_61), new Parser.gotof_type(this.gotof_61), value);
				return false;
			}
			this.sa.syntax_error();
			this.error = true;
			return false;
		}
		private bool gotof_61(int nonterminal_index, object v)
		{
			return true;
		}
		private bool state_61(Token token, object value)
		{
			if (token != Token.token_BlockEnd)
			{
				switch (token)
				{
				case Token.token_LongNumber:
				{
					string arg = (string)this.get_arg(2, 0);
					BlockScalar blockScalar;
					this.sa.GetBlockScalarData(out blockScalar, arg);
					object value2 = blockScalar;
					this.pop_stack(2);
					return this.stack_top().gotof(3, value2);
				}
				case Token.token_Number:
				{
					string arg2 = (string)this.get_arg(2, 0);
					BlockScalar blockScalar2;
					this.sa.GetBlockScalarData(out blockScalar2, arg2);
					object value3 = blockScalar2;
					this.pop_stack(2);
					return this.stack_top().gotof(3, value3);
				}
				case Token.token_Text:
				{
					string arg3 = (string)this.get_arg(2, 0);
					BlockScalar blockScalar3;
					this.sa.GetBlockScalarData(out blockScalar3, arg3);
					object value4 = blockScalar3;
					this.pop_stack(2);
					return this.stack_top().gotof(3, value4);
				}
				}
				this.sa.syntax_error();
				this.error = true;
				return false;
			}
			string arg4 = (string)this.get_arg(2, 0);
			BlockScalar blockScalar4;
			this.sa.GetBlockScalarData(out blockScalar4, arg4);
			object value5 = blockScalar4;
			this.pop_stack(2);
			return this.stack_top().gotof(3, value5);
		}
		private bool gotof_62(int nonterminal_index, object v)
		{
			return true;
		}
		private bool state_62(Token token, object value)
		{
			if (token == Token.token_EOL)
			{
				this.push_stack(new Parser.state_type(this.state_63), new Parser.gotof_type(this.gotof_63), value);
				return false;
			}
			this.sa.syntax_error();
			this.error = true;
			return false;
		}
		private bool gotof_63(int nonterminal_index, object v)
		{
			return true;
		}
		private bool state_63(Token token, object value)
		{
			if (token != Token.token_BlockEnd)
			{
				switch (token)
				{
				case Token.token_LongNumber:
				{
					BlockScalar arg = (BlockScalar)this.get_arg(3, 0);
					string arg2 = (string)this.get_arg(3, 1);
					BlockScalar blockScalar;
					this.sa.GetBlockScalarData(out blockScalar, arg, arg2);
					object value2 = blockScalar;
					this.pop_stack(3);
					return this.stack_top().gotof(3, value2);
				}
				case Token.token_Number:
				{
					BlockScalar arg3 = (BlockScalar)this.get_arg(3, 0);
					string arg4 = (string)this.get_arg(3, 1);
					BlockScalar blockScalar2;
					this.sa.GetBlockScalarData(out blockScalar2, arg3, arg4);
					object value3 = blockScalar2;
					this.pop_stack(3);
					return this.stack_top().gotof(3, value3);
				}
				case Token.token_Text:
				{
					BlockScalar arg5 = (BlockScalar)this.get_arg(3, 0);
					string arg6 = (string)this.get_arg(3, 1);
					BlockScalar blockScalar3;
					this.sa.GetBlockScalarData(out blockScalar3, arg5, arg6);
					object value4 = blockScalar3;
					this.pop_stack(3);
					return this.stack_top().gotof(3, value4);
				}
				}
				this.sa.syntax_error();
				this.error = true;
				return false;
			}
			BlockScalar arg7 = (BlockScalar)this.get_arg(3, 0);
			string arg8 = (string)this.get_arg(3, 1);
			BlockScalar blockScalar4;
			this.sa.GetBlockScalarData(out blockScalar4, arg7, arg8);
			object value5 = blockScalar4;
			this.pop_stack(3);
			return this.stack_top().gotof(3, value5);
		}
	}
}
