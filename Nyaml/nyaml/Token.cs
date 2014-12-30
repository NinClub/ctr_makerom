using System;
namespace nyaml
{
	public enum Token
	{
		token_eof,
		token_BlockBegin,
		token_BlockEnd,
		token_BlockScalarHeader,
		token_Bool,
		token_DocumentBegin,
		token_DocumentEnd,
		token_EOL,
		token_Literal,
		token_LongNumber,
		token_Mapper,
		token_Number,
		token_SequenceHeader,
		token_Text
	}
}
