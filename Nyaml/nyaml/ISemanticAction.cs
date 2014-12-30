using System;
namespace nyaml
{
	internal interface ISemanticAction
	{
		void syntax_error();
		void stack_overflow();
		void GetBlockScalar(out BlockScalar arg0, string arg1, BlockScalar arg2);
		void GetBlockScalarData(out BlockScalar arg0, BlockScalar arg1, int arg2);
		void GetBlockScalarData(out BlockScalar arg0, BlockScalar arg1, long arg2);
		void GetBlockScalarData(out BlockScalar arg0, BlockScalar arg1, string arg2);
		void GetBlockScalarData(out BlockScalar arg0, int arg1);
		void GetBlockScalarData(out BlockScalar arg0, long arg1);
		void GetBlockScalarData(out BlockScalar arg0, string arg1);
		void GetCollection(out Collection arg0, Mapping arg1);
		void GetCollection(out Collection arg0, Sequence arg1);
		void GetCollectionElement(out CollectionElement arg0, Collection arg1);
		void GetCollectionElement(out CollectionElement arg0, Scalar arg1);
		void GetMapping(out Mapping arg0, Mapping arg1);
		void GetMappingElement(out Mapping arg0, Mapping arg1, string arg2, Collection arg3);
		void GetMappingElement(out Mapping arg0, Mapping arg1, string arg2, CollectionElement arg3);
		void GetMappingElement(out Mapping arg0, string arg1, Collection arg2);
		void GetMappingElement(out Mapping arg0, string arg1, CollectionElement arg2);
		void GetNyaml(out Nyaml arg0, Collection arg1);
		void GetScalar(out Scalar arg0);
		void GetScalar(out Scalar arg0, BlockScalar arg1);
		void GetScalar(out Scalar arg0, bool arg1);
		void GetScalar(out Scalar arg0, int arg1);
		void GetScalar(out Scalar arg0, long arg1);
		void GetScalar(out Scalar arg0, string arg1);
		void GetSequence(out Sequence arg0, Sequence arg1);
		void GetSequenceElement(out Sequence arg0, Collection arg1);
		void GetSequenceElement(out Sequence arg0, CollectionElement arg1);
		void GetSequenceElement(out Sequence arg0, Sequence arg1, Collection arg2);
		void GetSequenceElement(out Sequence arg0, Sequence arg1, CollectionElement arg2);
	}
}
