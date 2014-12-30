using System;
namespace nyaml
{
	internal class SemanticAction : ISemanticAction
	{
		public void syntax_error()
		{
			throw new SyntaxErrorException();
		}
		public void stack_overflow()
		{
			throw new StackOverflowException();
		}
		public void GetCollection(out Collection arg0, Mapping arg1)
		{
			arg0 = arg1;
		}
		public void GetCollection(out Collection arg0, Sequence arg1)
		{
			arg0 = arg1;
		}
		public void GetCollectionElement(out CollectionElement arg0, Collection arg1)
		{
			arg0 = arg1;
		}
		public void GetCollectionElement(out CollectionElement arg0, Scalar arg1)
		{
			arg0 = arg1;
		}
		public void GetMapping(out Mapping arg0, Mapping arg1)
		{
			arg0 = arg1;
		}
		public void GetMappingElement(out Mapping arg0, Mapping arg1, string arg2, CollectionElement arg3)
		{
			arg0 = arg1;
			arg0.Elements.Add(arg2, arg3);
		}
		public void GetMappingElement(out Mapping arg0, string arg1, CollectionElement arg2)
		{
			arg0 = new Mapping(arg1, arg2);
		}
		public void GetMappingElement(out Mapping arg0, string arg1, Collection arg2)
		{
			arg0 = new Mapping(arg1, arg2);
		}
		public void GetMappingElement(out Mapping arg0, Mapping arg1, string arg2, Collection arg3)
		{
			arg0 = arg1;
			arg0.Elements.Add(arg2, arg3);
		}
		public void GetNyaml(out Nyaml arg0, Collection arg1)
		{
			arg0 = new Nyaml(arg1);
		}
		public void GetScalar(out Scalar arg0)
		{
			arg0 = new ScalarNull();
		}
		public void GetScalar(out Scalar arg0, int arg1)
		{
			arg0 = new ScalarInteger(arg1);
		}
		public void GetScalar(out Scalar arg0, long arg1)
		{
			arg0 = new ScalarLong(arg1);
		}
		public void GetScalar(out Scalar arg0, string arg1)
		{
			arg0 = new ScalarString(arg1);
		}
		public void GetScalar(out Scalar arg0, bool arg1)
		{
			arg0 = new ScalarBool(arg1);
		}
		public void GetScalar(out Scalar arg0, BlockScalar arg1)
		{
			arg0 = arg1;
		}
		public void GetSequence(out Sequence arg0, Sequence arg1)
		{
			arg0 = arg1;
		}
		public void GetSequenceElement(out Sequence arg0, CollectionElement arg1)
		{
			arg0 = new Sequence(arg1);
		}
		public void GetSequenceElement(out Sequence arg0, Sequence arg1, CollectionElement arg2)
		{
			arg0 = arg1;
			arg0.Elements.Add(arg2);
		}
		public void GetSequenceElement(out Sequence arg0, Collection arg1)
		{
			arg0 = new Sequence(arg1);
		}
		public void GetSequenceElement(out Sequence arg0, Sequence arg1, Collection arg2)
		{
			arg0 = arg1;
			arg0.Elements.Add(arg2);
		}
		public void GetBlockScalarData(out BlockScalar arg0, string arg1)
		{
			arg0 = new BlockScalar(arg1);
		}
		public void GetBlockScalarData(out BlockScalar arg0, int arg1)
		{
			arg0 = new BlockScalar(arg1);
		}
		public void GetBlockScalarData(out BlockScalar arg0, long arg1)
		{
			arg0 = new BlockScalar(arg1);
		}
		public void GetBlockScalarData(out BlockScalar arg0, BlockScalar arg1, string arg2)
		{
			arg0 = arg1;
			arg0.Element.Add(arg2);
		}
		public void GetBlockScalarData(out BlockScalar arg0, BlockScalar arg1, int arg2)
		{
			arg0 = arg1;
			arg0.Element.Add(arg2.ToString());
		}
		public void GetBlockScalarData(out BlockScalar arg0, BlockScalar arg1, long arg2)
		{
			arg0 = arg1;
			arg0.Element.Add(arg2.ToString());
		}
		public void GetBlockScalar(out BlockScalar arg0, string arg1, BlockScalar arg2)
		{
			arg0 = arg2;
		}
	}
}
