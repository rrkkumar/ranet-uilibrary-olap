/*   
    Copyright (C) 2009 Galaktika Corporation ZAO

    This file is a part of Ranet.UILibrary.Olap
 
    Ranet.UILibrary.Olap is a free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.
      
    You should have received a copy of the GNU General Public License
    along with Ranet.UILibrary.Olap.  If not, see <http://www.gnu.org/licenses/>.
  
    If GPL v.3 is not suitable for your products or company,
    Galaktika Corp provides Ranet.UILibrary.Olap under a flexible commercial license
    designed to meet your specific usage and distribution requirements.
    If you have already obtained a commercial license from Galaktika Corp,
    you can use this file under those license terms.
*/


// The variable 'variable' is assigned but its value is never used.
#pragma warning disable 168, 219
// Unreachable code detected.
#pragma warning disable 162

using System;

namespace Ranet.Olap.Mdx.Compiler.Parser
{
	using Antlr.Runtime;
	using System.Collections.Generic;

	public partial class mdxParser : Parser
	{
		public static readonly string[] tokenNames = new string[] 
	{
        "<invalid>", 
		"<EOR>", 
		"<DOWN>", 
		"<UP>", 
		"UPDATE", 
		"CUBE", 
		"SET", 
		"BY", 
		"WITH", 
		"CELL", 
		"PROPERTIES", 
		"SELECT", 
		"FROM", 
		"WHERE", 
		"CALCULATED", 
		"MEMBER", 
		"AS", 
		"CALCULATION", 
		"FOR", 
		"NON", 
		"EMPTY", 
		"DIMENSION", 
		"ON", 
		"INTEGER", 
		"VISUAL", 
		"RANET_EXPRESSION", 
		"CELL_ORDINAL", 
		"VALUE", 
		"FORMATTED_VALUE", 
		"XOR", 
		"OR", 
		"AND", 
		"NOT", 
		"IS", 
		"FLOAT", 
		"CASE", 
		"ELSE", 
		"END", 
		"WHEN", 
		"THEN", 
		"ID", 
		"QUOTED_ID", 
		"STRING", 
		"CREATE", 
		"GLOBAL", 
		"SESSION", 
		"USE_EQUAL_ALLOCATION", 
		"USE_EQUAL_INCREMENT", 
		"USE_WEIGHTED_ALLOCATION", 
		"USE_WEIGHTED_INCREMENT", 
		"WS", 
		"LINE_COMMENT", 
		"COMMENT", 
		"'='", 
		"'USE_EQUAL_ALLOCATION'", 
		"'USE_EQUAL_INCREMENT'", 
		"'USE_WEIGHTED_ALLOCATION'", 
		"'USE_WEIGHTED_INCREMENT'", 
		"','", 
		"'*'", 
		"'\\''", 
		"'.'", 
		"'('", 
		"')'", 
		"'<>'", 
		"'<'", 
		"'>'", 
		"'<='", 
		"'>='", 
		"'+'", 
		"'-'", 
		"'/'", 
		"'^'", 
		"':'", 
		"'&'", 
		"'{'", 
		"'}'"
    };

		public const int T__68 = 68;
		public const int T__69 = 69;
		public const int T__66 = 66;
		public const int WHERE = 13;
		public const int T__67 = 67;
		public const int T__64 = 64;
		public const int MEMBER = 15;
		public const int T__65 = 65;
		public const int T__62 = 62;
		public const int PROPERTIES = 10;
		public const int T__63 = 63;
		public const int NON = 19;
		public const int CASE = 35;
		public const int UPDATE = 4;
		public const int FOR = 18;
		public const int CELL = 9;
		public const int FLOAT = 34;
		public const int NOT = 32;
		public const int USE_WEIGHTED_INCREMENT = 49;
		public const int USE_WEIGHTED_ALLOCATION = 48;
		public const int ID = 40;
		public const int AND = 31;
		public const int T__61 = 61;
		public const int T__60 = 60;
		public const int EOF = -1;
		public const int QUOTED_ID = 41;
		public const int VISUAL = 24;
		public const int T__55 = 55;
		public const int T__56 = 56;
		public const int AS = 16;
		public const int T__57 = 57;
		public const int CREATE = 43;
		public const int T__58 = 58;
		public const int THEN = 39;
		public const int T__53 = 53;
		public const int T__54 = 54;
		public const int IS = 33;
		public const int T__59 = 59;
		public const int FORMATTED_VALUE = 28;
		public const int DIMENSION = 21;
		public const int COMMENT = 52;
		public const int SELECT = 11;
		public const int RANET_EXPRESSION = 25;
		public const int WITH = 8;
		public const int INTEGER = 23;
		public const int BY = 7;
		public const int XOR = 29;
		public const int LINE_COMMENT = 51;
		public const int CELL_ORDINAL = 26;
		public const int ELSE = 36;
		public const int USE_EQUAL_ALLOCATION = 46;
		public const int ON = 22;
		public const int SET = 6;
		public const int SESSION = 45;
		public const int VALUE = 27;
		public const int EMPTY = 20;
		public const int WS = 50;
		public const int T__71 = 71;
		public const int T__72 = 72;
		public const int CALCULATED = 14;
		public const int T__70 = 70;
		public const int CUBE = 5;
		public const int WHEN = 38;
		public const int USE_EQUAL_INCREMENT = 47;
		public const int OR = 30;
		public const int CALCULATION = 17;
		public const int GLOBAL = 44;
		public const int END = 37;
		public const int T__76 = 76;
		public const int FROM = 12;
		public const int T__75 = 75;
		public const int T__74 = 74;
		public const int T__73 = 73;
		public const int STRING = 42;

		// delegates
		// delegators



		public mdxParser(ITokenStream input)
			: this(input, new RecognizerSharedState())
		{
		}

		public mdxParser(ITokenStream input, RecognizerSharedState state)
			: base(input, state)
		{
			InitializeCyclicDFAs();


		}


		override public string[] TokenNames
		{
			get { return mdxParser.tokenNames; }
		}

		override public string GrammarFileName
		{
			get { return "D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g"; }
		}





		// $ANTLR start "mdx_statement"
		// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:23:1: mdx_statement returns [MdxObject value] : mdx_statement_single EOF ;
		public MdxObject mdx_statement() // throws RecognitionException [1]
		{

			MdxObject value = default(MdxObject);

			MdxStatement mdx_statement_single1 = default(MdxStatement);


			try
			{
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:24:2: ( mdx_statement_single EOF )
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:24:4: mdx_statement_single EOF
				{
					PushFollow(FOLLOW_mdx_statement_single_in_mdx_statement67);
					mdx_statement_single1 = mdx_statement_single();
					state.followingStackPointer--;

					Match(input, EOF, FOLLOW_EOF_in_mdx_statement69);
					value = mdx_statement_single1;

				}

			}
			catch (RecognitionException re)
			{
				ReportError(re);
				Recover(input, re);
			}
			finally
			{
			}
			return value;
		}
		// $ANTLR end "mdx_statement"


		// $ANTLR start "mdx_statement_single"
		// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:28:1: mdx_statement_single returns [MdxStatement value] : ( select_statement | update_statement );
		public MdxStatement mdx_statement_single() // throws RecognitionException [1]
		{

			MdxStatement value = default(MdxStatement);

			MdxSelectStatement select_statement2 = default(MdxSelectStatement);

			MdxUpdateStatement update_statement3 = default(MdxUpdateStatement);


			try
			{
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:29:2: ( select_statement | update_statement )
				int alt1 = 2;
				int LA1_0 = input.LA(1);

				if ((LA1_0 == WITH || LA1_0 == SELECT))
				{
					alt1 = 1;
				}
				else if ((LA1_0 == UPDATE))
				{
					alt1 = 2;
				}
				else
				{
					NoViableAltException nvae_d1s0 =
							new NoViableAltException("", 1, 0, input);

					throw nvae_d1s0;
				}
				switch (alt1)
				{
					case 1:
						// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:29:4: select_statement
						{
							PushFollow(FOLLOW_select_statement_in_mdx_statement_single87);
							select_statement2 = select_statement();
							state.followingStackPointer--;

							value = select_statement2;

						}
						break;
					case 2:
						// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:31:4: update_statement
						{
							PushFollow(FOLLOW_update_statement_in_mdx_statement_single95);
							update_statement3 = update_statement();
							state.followingStackPointer--;

							value = update_statement3;

						}
						break;

				}
			}
			catch (RecognitionException re)
			{
				ReportError(re);
				Recover(input, re);
			}
			finally
			{
			}
			return value;
		}
		// $ANTLR end "mdx_statement_single"


		// $ANTLR start "update_statement"
		// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:37:1: update_statement returns [MdxUpdateStatement value] : UPDATE ( CUBE )? c= cube_specification SET dest= expression_property '=' source= expression_or_xor ( 'USE_EQUAL_ALLOCATION' | 'USE_EQUAL_INCREMENT' | 'USE_WEIGHTED_ALLOCATION' ( BY w1= weight_value_expression )? 'USE_WEIGHTED_INCREMENT' ( BY w2= weight_value_expression )? )? ;
		public MdxUpdateStatement update_statement() // throws RecognitionException [1]
		{

			MdxUpdateStatement value = default(MdxUpdateStatement);

			MdxObject c = default(MdxObject);

			MdxExpression dest = default(MdxExpression);

			MdxExpression source = default(MdxExpression);

			MdxExpression w1 = default(MdxExpression);

			MdxExpression w2 = default(MdxExpression);


			try
			{
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:38:2: ( UPDATE ( CUBE )? c= cube_specification SET dest= expression_property '=' source= expression_or_xor ( 'USE_EQUAL_ALLOCATION' | 'USE_EQUAL_INCREMENT' | 'USE_WEIGHTED_ALLOCATION' ( BY w1= weight_value_expression )? 'USE_WEIGHTED_INCREMENT' ( BY w2= weight_value_expression )? )? )
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:38:4: UPDATE ( CUBE )? c= cube_specification SET dest= expression_property '=' source= expression_or_xor ( 'USE_EQUAL_ALLOCATION' | 'USE_EQUAL_INCREMENT' | 'USE_WEIGHTED_ALLOCATION' ( BY w1= weight_value_expression )? 'USE_WEIGHTED_INCREMENT' ( BY w2= weight_value_expression )? )?
				{
					Match(input, UPDATE, FOLLOW_UPDATE_in_update_statement116);
					// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:38:11: ( CUBE )?
					int alt2 = 2;
					int LA2_0 = input.LA(1);

					if ((LA2_0 == CUBE))
					{
						alt2 = 1;
					}
					switch (alt2)
					{
						case 1:
							// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:38:11: CUBE
							{
								Match(input, CUBE, FOLLOW_CUBE_in_update_statement118);

							}
							break;

					}

					value = new MdxUpdateStatement();
					PushFollow(FOLLOW_cube_specification_in_update_statement133);
					c = cube_specification();
					state.followingStackPointer--;

					value.Cube = c;
					Match(input, SET, FOLLOW_SET_in_update_statement145);
					PushFollow(FOLLOW_expression_property_in_update_statement149);
					dest = expression_property();
					state.followingStackPointer--;

					value.Destination = dest;
					Match(input, 53, FOLLOW_53_in_update_statement162);
					PushFollow(FOLLOW_expression_or_xor_in_update_statement166);
					source = expression_or_xor();
					state.followingStackPointer--;

					value.Source = source;
					// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:46:7: ( 'USE_EQUAL_ALLOCATION' | 'USE_EQUAL_INCREMENT' | 'USE_WEIGHTED_ALLOCATION' ( BY w1= weight_value_expression )? 'USE_WEIGHTED_INCREMENT' ( BY w2= weight_value_expression )? )?
					int alt5 = 4;
					switch (input.LA(1))
					{
						case 54:
							{
								alt5 = 1;
							}
							break;
						case 55:
							{
								alt5 = 2;
							}
							break;
						case 56:
							{
								alt5 = 3;
							}
							break;
					}

					switch (alt5)
					{
						case 1:
							// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:46:9: 'USE_EQUAL_ALLOCATION'
							{
								Match(input, 54, FOLLOW_54_in_update_statement181);
								value.UpdateBehavior = UpdateBehavior.USE_EQUAL_ALLOCATION;

							}
							break;
						case 2:
							// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:47:9: 'USE_EQUAL_INCREMENT'
							{
								Match(input, 55, FOLLOW_55_in_update_statement193);
								value.UpdateBehavior = UpdateBehavior.USE_EQUAL_INCREMENT;

							}
							break;
						case 3:
							// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:48:9: 'USE_WEIGHTED_ALLOCATION' ( BY w1= weight_value_expression )? 'USE_WEIGHTED_INCREMENT' ( BY w2= weight_value_expression )?
							{
								Match(input, 56, FOLLOW_56_in_update_statement205);
								value.UpdateBehavior = UpdateBehavior.USE_WEIGHTED_ALLOCATION;
								// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:49:9: ( BY w1= weight_value_expression )?
								int alt3 = 2;
								int LA3_0 = input.LA(1);

								if ((LA3_0 == BY))
								{
									alt3 = 1;
								}
								switch (alt3)
								{
									case 1:
										// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:49:10: BY w1= weight_value_expression
										{
											Match(input, BY, FOLLOW_BY_in_update_statement219);
											PushFollow(FOLLOW_weight_value_expression_in_update_statement223);
											w1 = weight_value_expression();
											state.followingStackPointer--;

											value.ByWeightedAllocation = w1;

										}
										break;

								}

								Match(input, 57, FOLLOW_57_in_update_statement239);
								value.UpdateBehavior = UpdateBehavior.USE_WEIGHTED_INCREMENT;
								// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:51:9: ( BY w2= weight_value_expression )?
								int alt4 = 2;
								int LA4_0 = input.LA(1);

								if ((LA4_0 == BY))
								{
									alt4 = 1;
								}
								switch (alt4)
								{
									case 1:
										// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:51:10: BY w2= weight_value_expression
										{
											Match(input, BY, FOLLOW_BY_in_update_statement253);
											PushFollow(FOLLOW_weight_value_expression_in_update_statement257);
											w2 = weight_value_expression();
											state.followingStackPointer--;

											value.ByWeightedIncrement = w2;

										}
										break;

								}


							}
							break;

					}


				}

			}
			catch (RecognitionException re)
			{
				ReportError(re);
				Recover(input, re);
			}
			finally
			{
			}
			return value;
		}
		// $ANTLR end "update_statement"


		// $ANTLR start "weight_value_expression"
		// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:55:1: weight_value_expression returns [MdxExpression value] : expression_property ;
		public MdxExpression weight_value_expression() // throws RecognitionException [1]
		{

			MdxExpression value = default(MdxExpression);

			MdxExpression expression_property4 = default(MdxExpression);


			try
			{
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:56:2: ( expression_property )
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:56:4: expression_property
				{
					PushFollow(FOLLOW_expression_property_in_weight_value_expression294);
					expression_property4 = expression_property();
					state.followingStackPointer--;

					value = expression_property4;

				}

			}
			catch (RecognitionException re)
			{
				ReportError(re);
				Recover(input, re);
			}
			finally
			{
			}
			return value;
		}
		// $ANTLR end "weight_value_expression"


		// $ANTLR start "condition"
		// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:60:1: condition returns [MdxExpression value] : expression_property ;
		public MdxExpression condition() // throws RecognitionException [1]
		{

			MdxExpression value = default(MdxExpression);

			MdxExpression expression_property5 = default(MdxExpression);


			try
			{
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:61:2: ( expression_property )
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:61:4: expression_property
				{
					PushFollow(FOLLOW_expression_property_in_condition312);
					expression_property5 = expression_property();
					state.followingStackPointer--;

					value = expression_property5;

				}

			}
			catch (RecognitionException re)
			{
				ReportError(re);
				Recover(input, re);
			}
			finally
			{
			}
			return value;
		}
		// $ANTLR end "condition"


		// $ANTLR start "select_statement"
		// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:65:1: select_statement returns [MdxSelectStatement value] : ( WITH ( with_clause_single )+ )? select_statement_subcube ( ( CELL )? PROPERTIES c1= cell_property ( ',' c2= cell_property )* )? ;
		public MdxSelectStatement select_statement() // throws RecognitionException [1]
		{

			MdxSelectStatement value = default(MdxSelectStatement);

			string c1 = default(string);

			string c2 = default(string);

			MdxWithClauseItem with_clause_single6 = default(MdxWithClauseItem);

			MdxSelectStatement select_statement_subcube7 = default(MdxSelectStatement);


			try
			{
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:66:2: ( ( WITH ( with_clause_single )+ )? select_statement_subcube ( ( CELL )? PROPERTIES c1= cell_property ( ',' c2= cell_property )* )? )
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:66:4: ( WITH ( with_clause_single )+ )? select_statement_subcube ( ( CELL )? PROPERTIES c1= cell_property ( ',' c2= cell_property )* )?
				{
					var withList = new List<MdxWithClauseItem>();
					// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:67:4: ( WITH ( with_clause_single )+ )?
					int alt7 = 2;
					int LA7_0 = input.LA(1);

					if ((LA7_0 == WITH))
					{
						alt7 = 1;
					}
					switch (alt7)
					{
						case 1:
							// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:67:6: WITH ( with_clause_single )+
							{
								Match(input, WITH, FOLLOW_WITH_in_select_statement337);
								// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:68:6: ( with_clause_single )+
								int cnt6 = 0;
								do
								{
									int alt6 = 2;
									int LA6_0 = input.LA(1);

									if ((LA6_0 == SET || LA6_0 == CELL || (LA6_0 >= CALCULATED && LA6_0 <= MEMBER)))
									{
										alt6 = 1;
									}


									switch (alt6)
									{
										case 1:
											// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:68:8: with_clause_single
											{
												PushFollow(FOLLOW_with_clause_single_in_select_statement347);
												with_clause_single6 = with_clause_single();
												state.followingStackPointer--;

												withList.Add(with_clause_single6);

											}
											break;

										default:
											if (cnt6 >= 1) goto loop6;
											EarlyExitException eee6 =
													new EarlyExitException(6, input);
											throw eee6;
									}
									cnt6++;
								} while (true);

							loop6:
								;	// Stops C# compiler whinging that label 'loop6' has no statements


							}
							break;

					}

					PushFollow(FOLLOW_select_statement_subcube_in_select_statement376);
					select_statement_subcube7 = select_statement_subcube();
					state.followingStackPointer--;


					value = select_statement_subcube7;
					if (value != null) value.With.AddRange(withList);

					// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:77:4: ( ( CELL )? PROPERTIES c1= cell_property ( ',' c2= cell_property )* )?
					int alt10 = 2;
					int LA10_0 = input.LA(1);

					if (((LA10_0 >= CELL && LA10_0 <= PROPERTIES)))
					{
						alt10 = 1;
					}
					switch (alt10)
					{
						case 1:
							// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:77:6: ( CELL )? PROPERTIES c1= cell_property ( ',' c2= cell_property )*
							{
								// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:77:6: ( CELL )?
								int alt8 = 2;
								int LA8_0 = input.LA(1);

								if ((LA8_0 == CELL))
								{
									alt8 = 1;
								}
								switch (alt8)
								{
									case 1:
										// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:77:6: CELL
										{
											Match(input, CELL, FOLLOW_CELL_in_select_statement389);

										}
										break;

								}

								Match(input, PROPERTIES, FOLLOW_PROPERTIES_in_select_statement392);
								PushFollow(FOLLOW_cell_property_in_select_statement396);
								c1 = cell_property();
								state.followingStackPointer--;

								if (value != null) value.CellProperties.Add(new MdxObjectReferenceExpression(c1));
								// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:79:4: ( ',' c2= cell_property )*
								do
								{
									int alt9 = 2;
									int LA9_0 = input.LA(1);

									if ((LA9_0 == 58))
									{
										alt9 = 1;
									}


									switch (alt9)
									{
										case 1:
											// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:79:5: ',' c2= cell_property
											{
												Match(input, 58, FOLLOW_58_in_select_statement407);
												PushFollow(FOLLOW_cell_property_in_select_statement411);
												c2 = cell_property();
												state.followingStackPointer--;

												if (value != null) value.CellProperties.Add(new MdxObjectReferenceExpression(c2));

											}
											break;

										default:
											goto loop9;
									}
								} while (true);

							loop9:
								;	// Stops C# compiler whining that label 'loop9' has no statements


							}
							break;

					}


				}

			}
			catch (RecognitionException re)
			{
				ReportError(re);
				Recover(input, re);
			}
			finally
			{
			}
			return value;
		}
		// $ANTLR end "select_statement"


		// $ANTLR start "select_statement_subcube"
		// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:85:1: select_statement_subcube returns [MdxSelectStatement value] : SELECT ( '*' | a1= axis_specification ( ',' a2= axis_specification )* )? FROM c= cube_specification ( WHERE t= condition )? ;
		public MdxSelectStatement select_statement_subcube() // throws RecognitionException [1]
		{

			MdxSelectStatement value = default(MdxSelectStatement);

			MdxAxis a1 = default(MdxAxis);

			MdxAxis a2 = default(MdxAxis);

			MdxObject c = default(MdxObject);

			MdxExpression t = default(MdxExpression);


			try
			{
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:86:2: ( SELECT ( '*' | a1= axis_specification ( ',' a2= axis_specification )* )? FROM c= cube_specification ( WHERE t= condition )? )
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:86:4: SELECT ( '*' | a1= axis_specification ( ',' a2= axis_specification )* )? FROM c= cube_specification ( WHERE t= condition )?
				{
					Match(input, SELECT, FOLLOW_SELECT_in_select_statement_subcube445);
					value = new MdxSelectStatement();
					// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:87:4: ( '*' | a1= axis_specification ( ',' a2= axis_specification )* )?
					int alt12 = 3;
					int LA12_0 = input.LA(1);

					if ((LA12_0 == 59))
					{
						alt12 = 1;
					}
					else if ((LA12_0 == PROPERTIES || LA12_0 == NON || LA12_0 == DIMENSION || LA12_0 == INTEGER || LA12_0 == RANET_EXPRESSION || LA12_0 == NOT || (LA12_0 >= FLOAT && LA12_0 <= CASE) || (LA12_0 >= ID && LA12_0 <= STRING) || LA12_0 == 62 || (LA12_0 >= 69 && LA12_0 <= 70) || LA12_0 == 75))
					{
						alt12 = 2;
					}
					switch (alt12)
					{
						case 1:
							// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:87:6: '*'
							{
								Match(input, 59, FOLLOW_59_in_select_statement_subcube454);

							}
							break;
						case 2:
							// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:88:6: a1= axis_specification ( ',' a2= axis_specification )*
							{
								PushFollow(FOLLOW_axis_specification_in_select_statement_subcube463);
								a1 = axis_specification();
								state.followingStackPointer--;

								value.Axes.Add(a1);
								// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:89:6: ( ',' a2= axis_specification )*
								do
								{
									int alt11 = 2;
									int LA11_0 = input.LA(1);

									if ((LA11_0 == 58))
									{
										alt11 = 1;
									}


									switch (alt11)
									{
										case 1:
											// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:89:8: ',' a2= axis_specification
											{
												Match(input, 58, FOLLOW_58_in_select_statement_subcube474);
												PushFollow(FOLLOW_axis_specification_in_select_statement_subcube478);
												a2 = axis_specification();
												state.followingStackPointer--;

												value.Axes.Add(a2);

											}
											break;

										default:
											goto loop11;
									}
								} while (true);

							loop11:
								;	// Stops C# compiler whining that label 'loop11' has no statements


							}
							break;

					}

					Match(input, FROM, FOLLOW_FROM_in_select_statement_subcube494);
					PushFollow(FOLLOW_cube_specification_in_select_statement_subcube498);
					c = cube_specification();
					state.followingStackPointer--;

					value.CubeSpecification = c;
					// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:92:4: ( WHERE t= condition )?
					int alt13 = 2;
					int LA13_0 = input.LA(1);

					if ((LA13_0 == WHERE))
					{
						alt13 = 1;
					}
					switch (alt13)
					{
						case 1:
							// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:92:6: WHERE t= condition
							{
								Match(input, WHERE, FOLLOW_WHERE_in_select_statement_subcube507);
								PushFollow(FOLLOW_condition_in_select_statement_subcube511);
								t = condition();
								state.followingStackPointer--;

								value.Where = new MdxWhereClause(t);

							}
							break;

					}


				}

			}
			catch (RecognitionException re)
			{
				ReportError(re);
				Recover(input, re);
			}
			finally
			{
			}
			return value;
		}
		// $ANTLR end "select_statement_subcube"


		// $ANTLR start "with_clause_single"
		// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:96:1: with_clause_single returns [MdxWithClauseItem value] : ( ( ( CALCULATED )? MEMBER member_name AS | CELL CALCULATION FOR e3= expression AS ) ( '\\'' e1= expression '\\'' | e2= expression ) ( ',' p1= property_definition )* | SET set_name AS ( '\\'' e1= expression '\\'' | e2= expression ) );
		public MdxWithClauseItem with_clause_single() // throws RecognitionException [1]
		{

			MdxWithClauseItem value = default(MdxWithClauseItem);

			MdxExpression e3 = default(MdxExpression);

			MdxExpression e1 = default(MdxExpression);

			MdxExpression e2 = default(MdxExpression);

			MdxCalcProperty p1 = default(MdxCalcProperty);

			string member_name8 = default(string);

			string set_name9 = default(string);


			try
			{
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:97:2: ( ( ( CALCULATED )? MEMBER member_name AS | CELL CALCULATION FOR e3= expression AS ) ( '\\'' e1= expression '\\'' | e2= expression ) ( ',' p1= property_definition )* | SET set_name AS ( '\\'' e1= expression '\\'' | e2= expression ) )
				int alt19 = 2;
				int LA19_0 = input.LA(1);

				if ((LA19_0 == CELL || (LA19_0 >= CALCULATED && LA19_0 <= MEMBER)))
				{
					alt19 = 1;
				}
				else if ((LA19_0 == SET))
				{
					alt19 = 2;
				}
				else
				{
					NoViableAltException nvae_d19s0 =
							new NoViableAltException("", 19, 0, input);

					throw nvae_d19s0;
				}
				switch (alt19)
				{
					case 1:
						// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:97:3: ( ( CALCULATED )? MEMBER member_name AS | CELL CALCULATION FOR e3= expression AS ) ( '\\'' e1= expression '\\'' | e2= expression ) ( ',' p1= property_definition )*
						{
							// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:97:3: ( ( CALCULATED )? MEMBER member_name AS | CELL CALCULATION FOR e3= expression AS )
							int alt15 = 2;
							int LA15_0 = input.LA(1);

							if (((LA15_0 >= CALCULATED && LA15_0 <= MEMBER)))
							{
								alt15 = 1;
							}
							else if ((LA15_0 == CELL))
							{
								alt15 = 2;
							}
							else
							{
								NoViableAltException nvae_d15s0 =
										new NoViableAltException("", 15, 0, input);

								throw nvae_d15s0;
							}
							switch (alt15)
							{
								case 1:
									// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:97:5: ( CALCULATED )? MEMBER member_name AS
									{
										// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:97:5: ( CALCULATED )?
										int alt14 = 2;
										int LA14_0 = input.LA(1);

										if ((LA14_0 == CALCULATED))
										{
											alt14 = 1;
										}
										switch (alt14)
										{
											case 1:
												// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:97:5: CALCULATED
												{
													Match(input, CALCULATED, FOLLOW_CALCULATED_in_with_clause_single536);

												}
												break;

										}

										Match(input, MEMBER, FOLLOW_MEMBER_in_with_clause_single539);
										PushFollow(FOLLOW_member_name_in_with_clause_single541);
										member_name8 = member_name();
										state.followingStackPointer--;

										Match(input, AS, FOLLOW_AS_in_with_clause_single543);
										value = new MdxWithMemberItem(new MdxObjectReferenceExpression(member_name8), null, null);

									}
									break;
								case 2:
									// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:99:5: CELL CALCULATION FOR e3= expression AS
									{
										Match(input, CELL, FOLLOW_CELL_in_with_clause_single554);
										Match(input, CALCULATION, FOLLOW_CALCULATION_in_with_clause_single556);
										Match(input, FOR, FOLLOW_FOR_in_with_clause_single558);
										PushFollow(FOLLOW_expression_in_with_clause_single562);
										e3 = expression();
										state.followingStackPointer--;

										Match(input, AS, FOLLOW_AS_in_with_clause_single564);
										value = new MdxWithCalculatedCellItem(e3, null);

									}
									break;

							}

							// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:102:3: ( '\\'' e1= expression '\\'' | e2= expression )
							int alt16 = 2;
							int LA16_0 = input.LA(1);

							if ((LA16_0 == 60))
							{
								alt16 = 1;
							}
							else if ((LA16_0 == PROPERTIES || LA16_0 == DIMENSION || LA16_0 == INTEGER || LA16_0 == RANET_EXPRESSION || LA16_0 == NOT || (LA16_0 >= FLOAT && LA16_0 <= CASE) || (LA16_0 >= ID && LA16_0 <= STRING) || LA16_0 == 62 || (LA16_0 >= 69 && LA16_0 <= 70) || LA16_0 == 75))
							{
								alt16 = 2;
							}
							else
							{
								NoViableAltException nvae_d16s0 =
										new NoViableAltException("", 16, 0, input);

								throw nvae_d16s0;
							}
							switch (alt16)
							{
								case 1:
									// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:102:5: '\\'' e1= expression '\\''
									{
										Match(input, 60, FOLLOW_60_in_with_clause_single580);
										PushFollow(FOLLOW_expression_in_with_clause_single584);
										e1 = expression();
										state.followingStackPointer--;

										Match(input, 60, FOLLOW_60_in_with_clause_single586);
										if (value != null) value.Expression = e1;

									}
									break;
								case 2:
									// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:104:5: e2= expression
									{
										PushFollow(FOLLOW_expression_in_with_clause_single601);
										e2 = expression();
										state.followingStackPointer--;

										if (value != null) value.Expression = e2;

									}
									break;

							}

							// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:107:3: ( ',' p1= property_definition )*
							do
							{
								int alt17 = 2;
								int LA17_0 = input.LA(1);

								if ((LA17_0 == 58))
								{
									alt17 = 1;
								}


								switch (alt17)
								{
									case 1:
										// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:107:4: ',' p1= property_definition
										{
											Match(input, 58, FOLLOW_58_in_with_clause_single616);
											PushFollow(FOLLOW_property_definition_in_with_clause_single620);
											p1 = property_definition();
											state.followingStackPointer--;

											if (value != null) value.CalcProperties.Add(p1);

										}
										break;

									default:
										goto loop17;
								}
							} while (true);

						loop17:
							;	// Stops C# compiler whining that label 'loop17' has no statements


						}
						break;
					case 2:
						// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:108:4: SET set_name AS ( '\\'' e1= expression '\\'' | e2= expression )
						{
							Match(input, SET, FOLLOW_SET_in_with_clause_single632);
							PushFollow(FOLLOW_set_name_in_with_clause_single634);
							set_name9 = set_name();
							state.followingStackPointer--;

							Match(input, AS, FOLLOW_AS_in_with_clause_single636);
							// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:109:3: ( '\\'' e1= expression '\\'' | e2= expression )
							int alt18 = 2;
							int LA18_0 = input.LA(1);

							if ((LA18_0 == 60))
							{
								alt18 = 1;
							}
							else if ((LA18_0 == PROPERTIES || LA18_0 == DIMENSION || LA18_0 == INTEGER || LA18_0 == RANET_EXPRESSION || LA18_0 == NOT || (LA18_0 >= FLOAT && LA18_0 <= CASE) || (LA18_0 >= ID && LA18_0 <= STRING) || LA18_0 == 62 || (LA18_0 >= 69 && LA18_0 <= 70) || LA18_0 == 75))
							{
								alt18 = 2;
							}
							else
							{
								NoViableAltException nvae_d18s0 =
										new NoViableAltException("", 18, 0, input);

								throw nvae_d18s0;
							}
							switch (alt18)
							{
								case 1:
									// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:109:5: '\\'' e1= expression '\\''
									{
										Match(input, 60, FOLLOW_60_in_with_clause_single642);
										PushFollow(FOLLOW_expression_in_with_clause_single646);
										e1 = expression();
										state.followingStackPointer--;

										Match(input, 60, FOLLOW_60_in_with_clause_single648);
										value = new MdxWithSetItem(new MdxObjectReferenceExpression(set_name9), e1, null);

									}
									break;
								case 2:
									// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:112:5: e2= expression
									{
										PushFollow(FOLLOW_expression_in_with_clause_single664);
										e2 = expression();
										state.followingStackPointer--;

										value = new MdxWithSetItem(new MdxObjectReferenceExpression(set_name9), e2, null);

									}
									break;

							}


						}
						break;

				}
			}
			catch (RecognitionException re)
			{
				ReportError(re);
				Recover(input, re);
			}
			finally
			{
			}
			return value;
		}
		// $ANTLR end "with_clause_single"


		// $ANTLR start "member_name"
		// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:143:1: member_name returns [string value] : compound_id ;
		public string member_name() // throws RecognitionException [1]
		{

			string value = default(string);

			string compound_id10 = default(string);


			try
			{
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:144:2: ( compound_id )
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:144:4: compound_id
				{
					PushFollow(FOLLOW_compound_id_in_member_name691);
					compound_id10 = compound_id();
					state.followingStackPointer--;

					value = compound_id10;

				}

			}
			catch (RecognitionException re)
			{
				ReportError(re);
				Recover(input, re);
			}
			finally
			{
			}
			return value;
		}
		// $ANTLR end "member_name"


		// $ANTLR start "property_definition"
		// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:148:1: property_definition returns [MdxCalcProperty value] : identifier '=' expression_or_xor ;
		public MdxCalcProperty property_definition() // throws RecognitionException [1]
		{

			MdxCalcProperty value = default(MdxCalcProperty);

			string identifier11 = default(string);

			MdxExpression expression_or_xor12 = default(MdxExpression);


			try
			{
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:149:2: ( identifier '=' expression_or_xor )
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:149:4: identifier '=' expression_or_xor
				{
					PushFollow(FOLLOW_identifier_in_property_definition710);
					identifier11 = identifier();
					state.followingStackPointer--;

					Match(input, 53, FOLLOW_53_in_property_definition712);
					PushFollow(FOLLOW_expression_or_xor_in_property_definition714);
					expression_or_xor12 = expression_or_xor();
					state.followingStackPointer--;

					value = new MdxCalcProperty(identifier11, expression_or_xor12);

				}

			}
			catch (RecognitionException re)
			{
				ReportError(re);
				Recover(input, re);
			}
			finally
			{
			}
			return value;
		}
		// $ANTLR end "property_definition"


		// $ANTLR start "set_name"
		// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:153:1: set_name returns [string value] : compound_id ;
		public string set_name() // throws RecognitionException [1]
		{

			string value = default(string);

			string compound_id13 = default(string);


			try
			{
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:154:2: ( compound_id )
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:154:4: compound_id
				{
					PushFollow(FOLLOW_compound_id_in_set_name742);
					compound_id13 = compound_id();
					state.followingStackPointer--;

					value = compound_id13;

				}

			}
			catch (RecognitionException re)
			{
				ReportError(re);
				Recover(input, re);
			}
			finally
			{
			}
			return value;
		}
		// $ANTLR end "set_name"


		// $ANTLR start "compound_id"
		// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:156:1: compound_id returns [string value] : id= identifier ( '.' id2= identifier )* ;
		public string compound_id() // throws RecognitionException [1]
		{

			string value = default(string);

			string id = default(string);

			string id2 = default(string);


			try
			{
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:157:2: (id= identifier ( '.' id2= identifier )* )
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:157:4: id= identifier ( '.' id2= identifier )*
				{
					PushFollow(FOLLOW_identifier_in_compound_id764);
					id = identifier();
					state.followingStackPointer--;

					value = id;
					// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:158:3: ( '.' id2= identifier )*
					do
					{
						int alt20 = 2;
						int LA20_0 = input.LA(1);

						if ((LA20_0 == 61))
						{
							alt20 = 1;
						}


						switch (alt20)
						{
							case 1:
								// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:158:5: '.' id2= identifier
								{
									Match(input, 61, FOLLOW_61_in_compound_id772);
									PushFollow(FOLLOW_identifier_in_compound_id776);
									id2 = identifier();
									state.followingStackPointer--;

									value += "." + id2;

								}
								break;

							default:
								goto loop20;
						}
					} while (true);

				loop20:
					;	// Stops C# compiler whining that label 'loop20' has no statements


				}

			}
			catch (RecognitionException re)
			{
				ReportError(re);
				Recover(input, re);
			}
			finally
			{
			}
			return value;
		}
		// $ANTLR end "compound_id"


		// $ANTLR start "axis_specification"
		// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:161:1: axis_specification returns [MdxAxis value] : ( NON EMPTY )? expression ( ( DIMENSION )? PROPERTIES p1= property ( ',' p2= property )* )? ON an= axis_name ;
		public MdxAxis axis_specification() // throws RecognitionException [1]
		{

			MdxAxis value = default(MdxAxis);

			MdxExpression p1 = default(MdxExpression);

			MdxExpression p2 = default(MdxExpression);

			string an = default(string);

			MdxExpression expression14 = default(MdxExpression);


			try
			{
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:162:2: ( ( NON EMPTY )? expression ( ( DIMENSION )? PROPERTIES p1= property ( ',' p2= property )* )? ON an= axis_name )
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:162:4: ( NON EMPTY )? expression ( ( DIMENSION )? PROPERTIES p1= property ( ',' p2= property )* )? ON an= axis_name
				{
					bool NonEmpty = false;
					// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:163:2: ( NON EMPTY )?
					int alt21 = 2;
					int LA21_0 = input.LA(1);

					if ((LA21_0 == NON))
					{
						alt21 = 1;
					}
					switch (alt21)
					{
						case 1:
							// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:163:3: NON EMPTY
							{
								Match(input, NON, FOLLOW_NON_in_axis_specification806);
								Match(input, EMPTY, FOLLOW_EMPTY_in_axis_specification808);
								NonEmpty = true;

							}
							break;

					}

					PushFollow(FOLLOW_expression_in_axis_specification817);
					expression14 = expression();
					state.followingStackPointer--;

					value = new MdxAxis(null, expression14, null, null);


					// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:168:3: ( ( DIMENSION )? PROPERTIES p1= property ( ',' p2= property )* )?
					int alt24 = 2;
					int LA24_0 = input.LA(1);

					if ((LA24_0 == PROPERTIES || LA24_0 == DIMENSION))
					{
						alt24 = 1;
					}
					switch (alt24)
					{
						case 1:
							// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:168:4: ( DIMENSION )? PROPERTIES p1= property ( ',' p2= property )*
							{
								// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:168:4: ( DIMENSION )?
								int alt22 = 2;
								int LA22_0 = input.LA(1);

								if ((LA22_0 == DIMENSION))
								{
									alt22 = 1;
								}
								switch (alt22)
								{
									case 1:
										// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:168:4: DIMENSION
										{
											Match(input, DIMENSION, FOLLOW_DIMENSION_in_axis_specification825);

										}
										break;

								}

								Match(input, PROPERTIES, FOLLOW_PROPERTIES_in_axis_specification828);
								PushFollow(FOLLOW_property_in_axis_specification832);
								p1 = property();
								state.followingStackPointer--;

								if (value != null) value.DimensionProperties.Add(p1);
								// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:169:5: ( ',' p2= property )*
								do
								{
									int alt23 = 2;
									int LA23_0 = input.LA(1);

									if ((LA23_0 == 58))
									{
										alt23 = 1;
									}


									switch (alt23)
									{
										case 1:
											// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:169:7: ',' p2= property
											{
												Match(input, 58, FOLLOW_58_in_axis_specification842);
												PushFollow(FOLLOW_property_in_axis_specification846);
												p2 = property();
												state.followingStackPointer--;

												if (value != null) value.DimensionProperties.Add(p2);

											}
											break;

										default:
											goto loop23;
									}
								} while (true);

							loop23:
								;	// Stops C# compiler whining that label 'loop23' has no statements


							}
							break;

					}

					Match(input, ON, FOLLOW_ON_in_axis_specification860);
					PushFollow(FOLLOW_axis_name_in_axis_specification864);
					an = axis_name();
					state.followingStackPointer--;


					if (value != null) value.Name = an;


				}

			}
			catch (RecognitionException re)
			{
				ReportError(re);
				Recover(input, re);
			}
			finally
			{
			}
			return value;
		}
		// $ANTLR end "axis_specification"


		// $ANTLR start "axis_name"
		// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:177:1: axis_name returns [string value] : ( identifier | INTEGER );
		public string axis_name() // throws RecognitionException [1]
		{

			string value = default(string);

			IToken INTEGER16 = null;
			string identifier15 = default(string);


			try
			{
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:178:2: ( identifier | INTEGER )
				int alt25 = 2;
				int LA25_0 = input.LA(1);

				if ((LA25_0 == PROPERTIES || LA25_0 == DIMENSION || (LA25_0 >= ID && LA25_0 <= QUOTED_ID)))
				{
					alt25 = 1;
				}
				else if ((LA25_0 == INTEGER))
				{
					alt25 = 2;
				}
				else
				{
					NoViableAltException nvae_d25s0 =
							new NoViableAltException("", 25, 0, input);

					throw nvae_d25s0;
				}
				switch (alt25)
				{
					case 1:
						// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:178:4: identifier
						{
							PushFollow(FOLLOW_identifier_in_axis_name888);
							identifier15 = identifier();
							state.followingStackPointer--;

							value = identifier15;

						}
						break;
					case 2:
						// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:179:4: INTEGER
						{
							INTEGER16 = (IToken)Match(input, INTEGER, FOLLOW_INTEGER_in_axis_name895);
							value = ((INTEGER16 != null) ? INTEGER16.Text : null);

						}
						break;

				}
			}
			catch (RecognitionException re)
			{
				ReportError(re);
				Recover(input, re);
			}
			finally
			{
			}
			return value;
		}
		// $ANTLR end "axis_name"


		// $ANTLR start "property"
		// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:182:1: property returns [MdxExpression value] : compound_id ;
		public MdxExpression property() // throws RecognitionException [1]
		{

			MdxExpression value = default(MdxExpression);

			string compound_id17 = default(string);


			try
			{
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:183:2: ( compound_id )
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:183:4: compound_id
				{
					PushFollow(FOLLOW_compound_id_in_property912);
					compound_id17 = compound_id();
					state.followingStackPointer--;

					value = new MdxObjectReferenceExpression(compound_id17);

				}

			}
			catch (RecognitionException re)
			{
				ReportError(re);
				Recover(input, re);
			}
			finally
			{
			}
			return value;
		}
		// $ANTLR end "property"


		// $ANTLR start "cube_specification"
		// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:186:1: cube_specification returns [MdxObject value] : ( cube_name | ( NON VISUAL )? '(' select_statement_subcube ')' );
		public MdxObject cube_specification() // throws RecognitionException [1]
		{

			MdxObject value = default(MdxObject);

			string cube_name18 = default(string);

			MdxSelectStatement select_statement_subcube19 = default(MdxSelectStatement);


			try
			{
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:187:2: ( cube_name | ( NON VISUAL )? '(' select_statement_subcube ')' )
				int alt27 = 2;
				int LA27_0 = input.LA(1);

				if ((LA27_0 == PROPERTIES || LA27_0 == DIMENSION || LA27_0 == RANET_EXPRESSION || (LA27_0 >= ID && LA27_0 <= QUOTED_ID)))
				{
					alt27 = 1;
				}
				else if ((LA27_0 == NON || LA27_0 == 62))
				{
					alt27 = 2;
				}
				else
				{
					NoViableAltException nvae_d27s0 =
							new NoViableAltException("", 27, 0, input);

					throw nvae_d27s0;
				}
				switch (alt27)
				{
					case 1:
						// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:187:4: cube_name
						{
							PushFollow(FOLLOW_cube_name_in_cube_specification934);
							cube_name18 = cube_name();
							state.followingStackPointer--;

							value = new MdxObjectReferenceExpression(cube_name18, cube_name18, MdxObjectReferenceKind.Cube);

						}
						break;
					case 2:
						// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:189:4: ( NON VISUAL )? '(' select_statement_subcube ')'
						{
							// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:189:4: ( NON VISUAL )?
							int alt26 = 2;
							int LA26_0 = input.LA(1);

							if ((LA26_0 == NON))
							{
								alt26 = 1;
							}
							switch (alt26)
							{
								case 1:
									// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:189:6: NON VISUAL
									{
										Match(input, NON, FOLLOW_NON_in_cube_specification945);
										Match(input, VISUAL, FOLLOW_VISUAL_in_cube_specification947);

									}
									break;

							}

							Match(input, 62, FOLLOW_62_in_cube_specification952);
							PushFollow(FOLLOW_select_statement_subcube_in_cube_specification954);
							select_statement_subcube19 = select_statement_subcube();
							state.followingStackPointer--;

							Match(input, 63, FOLLOW_63_in_cube_specification956);
							value = select_statement_subcube19;

						}
						break;

				}
			}
			catch (RecognitionException re)
			{
				ReportError(re);
				Recover(input, re);
			}
			finally
			{
			}
			return value;
		}
		// $ANTLR end "cube_specification"


		// $ANTLR start "cube_name"
		// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:193:1: cube_name returns [string value] : ( compound_id | RANET_EXPRESSION );
		public string cube_name() // throws RecognitionException [1]
		{

			string value = default(string);

			IToken RANET_EXPRESSION21 = null;
			string compound_id20 = default(string);


			try
			{
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:194:3: ( compound_id | RANET_EXPRESSION )
				int alt28 = 2;
				int LA28_0 = input.LA(1);

				if ((LA28_0 == PROPERTIES || LA28_0 == DIMENSION || (LA28_0 >= ID && LA28_0 <= QUOTED_ID)))
				{
					alt28 = 1;
				}
				else if ((LA28_0 == RANET_EXPRESSION))
				{
					alt28 = 2;
				}
				else
				{
					NoViableAltException nvae_d28s0 =
							new NoViableAltException("", 28, 0, input);

					throw nvae_d28s0;
				}
				switch (alt28)
				{
					case 1:
						// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:194:5: compound_id
						{
							PushFollow(FOLLOW_compound_id_in_cube_name976);
							compound_id20 = compound_id();
							state.followingStackPointer--;

							value = compound_id20;

						}
						break;
					case 2:
						// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:195:5: RANET_EXPRESSION
						{
							RANET_EXPRESSION21 = (IToken)Match(input, RANET_EXPRESSION, FOLLOW_RANET_EXPRESSION_in_cube_name984);
							value = ((RANET_EXPRESSION21 != null) ? RANET_EXPRESSION21.Text : null);

						}
						break;

				}
			}
			catch (RecognitionException re)
			{
				ReportError(re);
				Recover(input, re);
			}
			finally
			{
			}
			return value;
		}
		// $ANTLR end "cube_name"


		// $ANTLR start "cell_property"
		// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:198:1: cell_property returns [string value] : ( CELL_ORDINAL | VALUE | FORMATTED_VALUE | provider_specific_cell_property );
		public string cell_property() // throws RecognitionException [1]
		{

			string value = default(string);

			IToken CELL_ORDINAL22 = null;
			IToken VALUE23 = null;
			IToken FORMATTED_VALUE24 = null;
			string provider_specific_cell_property25 = default(string);


			try
			{
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:199:2: ( CELL_ORDINAL | VALUE | FORMATTED_VALUE | provider_specific_cell_property )
				int alt29 = 4;
				switch (input.LA(1))
				{
					case CELL_ORDINAL:
						{
							alt29 = 1;
						}
						break;
					case VALUE:
						{
							alt29 = 2;
						}
						break;
					case FORMATTED_VALUE:
						{
							alt29 = 3;
						}
						break;
					case PROPERTIES:
					case DIMENSION:
					case ID:
					case QUOTED_ID:
						{
							alt29 = 4;
						}
						break;
					default:
						NoViableAltException nvae_d29s0 =
								new NoViableAltException("", 29, 0, input);

						throw nvae_d29s0;
				}

				switch (alt29)
				{
					case 1:
						// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:199:4: CELL_ORDINAL
						{
							CELL_ORDINAL22 = (IToken)Match(input, CELL_ORDINAL, FOLLOW_CELL_ORDINAL_in_cell_property1004);
							value = ((CELL_ORDINAL22 != null) ? CELL_ORDINAL22.Text : null);

						}
						break;
					case 2:
						// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:200:4: VALUE
						{
							VALUE23 = (IToken)Match(input, VALUE, FOLLOW_VALUE_in_cell_property1011);
							value = ((VALUE23 != null) ? VALUE23.Text : null);

						}
						break;
					case 3:
						// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:201:4: FORMATTED_VALUE
						{
							FORMATTED_VALUE24 = (IToken)Match(input, FORMATTED_VALUE, FOLLOW_FORMATTED_VALUE_in_cell_property1018);
							value = ((FORMATTED_VALUE24 != null) ? FORMATTED_VALUE24.Text : null);

						}
						break;
					case 4:
						// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:202:4: provider_specific_cell_property
						{
							PushFollow(FOLLOW_provider_specific_cell_property_in_cell_property1025);
							provider_specific_cell_property25 = provider_specific_cell_property();
							state.followingStackPointer--;

							value = provider_specific_cell_property25;

						}
						break;

				}
			}
			catch (RecognitionException re)
			{
				ReportError(re);
				Recover(input, re);
			}
			finally
			{
			}
			return value;
		}
		// $ANTLR end "cell_property"


		// $ANTLR start "provider_specific_cell_property"
		// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:204:1: provider_specific_cell_property returns [string value] : identifier ;
		public string provider_specific_cell_property() // throws RecognitionException [1]
		{

			string value = default(string);

			string identifier26 = default(string);


			try
			{
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:205:2: ( identifier )
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:205:4: identifier
				{
					PushFollow(FOLLOW_identifier_in_provider_specific_cell_property1042);
					identifier26 = identifier();
					state.followingStackPointer--;

					value = identifier26;

				}

			}
			catch (RecognitionException re)
			{
				ReportError(re);
				Recover(input, re);
			}
			finally
			{
			}
			return value;
		}
		// $ANTLR end "provider_specific_cell_property"


		// $ANTLR start "expression"
		// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:208:1: expression returns [MdxExpression value] : v1= expression_or_xor ;
		public MdxExpression expression() // throws RecognitionException [1]
		{

			MdxExpression value = default(MdxExpression);

			MdxExpression v1 = default(MdxExpression);


			try
			{
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:209:2: (v1= expression_or_xor )
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:209:4: v1= expression_or_xor
				{
					PushFollow(FOLLOW_expression_or_xor_in_expression1073);
					v1 = expression_or_xor();
					state.followingStackPointer--;

					value = v1;

				}

			}
			catch (RecognitionException re)
			{
				ReportError(re);
				Recover(input, re);
			}
			finally
			{
			}
			return value;
		}
		// $ANTLR end "expression"


		// $ANTLR start "expression_or_xor"
		// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:212:1: expression_or_xor returns [MdxExpression value] : t1= expression_and ( ( XOR | OR ) t2= expression_and )* ;
		public MdxExpression expression_or_xor() // throws RecognitionException [1]
		{

			MdxExpression value = default(MdxExpression);

			MdxExpression t1 = default(MdxExpression);

			MdxExpression t2 = default(MdxExpression);


			try
			{
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:213:2: (t1= expression_and ( ( XOR | OR ) t2= expression_and )* )
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:213:4: t1= expression_and ( ( XOR | OR ) t2= expression_and )*
				{
					PushFollow(FOLLOW_expression_and_in_expression_or_xor1093);
					t1 = expression_and();
					state.followingStackPointer--;

					value = t1; string op = "";
					// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:214:3: ( ( XOR | OR ) t2= expression_and )*
					do
					{
						int alt31 = 2;
						int LA31_0 = input.LA(1);

						if (((LA31_0 >= XOR && LA31_0 <= OR)))
						{
							alt31 = 1;
						}


						switch (alt31)
						{
							case 1:
								// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:215:5: ( XOR | OR ) t2= expression_and
								{
									// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:215:5: ( XOR | OR )
									int alt30 = 2;
									int LA30_0 = input.LA(1);

									if ((LA30_0 == XOR))
									{
										alt30 = 1;
									}
									else if ((LA30_0 == OR))
									{
										alt30 = 2;
									}
									else
									{
										NoViableAltException nvae_d30s0 =
												new NoViableAltException("", 30, 0, input);

										throw nvae_d30s0;
									}
									switch (alt30)
									{
										case 1:
											// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:215:7: XOR
											{
												Match(input, XOR, FOLLOW_XOR_in_expression_or_xor1107);
												op = "XOR";

											}
											break;
										case 2:
											// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:216:7: OR
											{
												Match(input, OR, FOLLOW_OR_in_expression_or_xor1117);
												op = "OR";

											}
											break;

									}

									PushFollow(FOLLOW_expression_and_in_expression_or_xor1129);
									t2 = expression_and();
									state.followingStackPointer--;

									value = new MdxBinaryExpression(value, t2, op);

								}
								break;

							default:
								goto loop31;
						}
					} while (true);

				loop31:
					;	// Stops C# compiler whining that label 'loop31' has no statements


				}

			}
			catch (RecognitionException re)
			{
				ReportError(re);
				Recover(input, re);
			}
			finally
			{
			}
			return value;
		}
		// $ANTLR end "expression_or_xor"


		// $ANTLR start "expression_and"
		// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:221:1: expression_and returns [MdxExpression value] : t1= expression_compare ( AND t2= expression_compare )* ;
		public MdxExpression expression_and() // throws RecognitionException [1]
		{

			MdxExpression value = default(MdxExpression);

			MdxExpression t1 = default(MdxExpression);

			MdxExpression t2 = default(MdxExpression);


			try
			{
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:222:2: (t1= expression_compare ( AND t2= expression_compare )* )
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:222:4: t1= expression_compare ( AND t2= expression_compare )*
				{
					PushFollow(FOLLOW_expression_compare_in_expression_and1159);
					t1 = expression_compare();
					state.followingStackPointer--;

					value = t1;
					// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:223:3: ( AND t2= expression_compare )*
					do
					{
						int alt32 = 2;
						int LA32_0 = input.LA(1);

						if ((LA32_0 == AND))
						{
							alt32 = 1;
						}


						switch (alt32)
						{
							case 1:
								// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:223:5: AND t2= expression_compare
								{
									Match(input, AND, FOLLOW_AND_in_expression_and1168);
									PushFollow(FOLLOW_expression_compare_in_expression_and1172);
									t2 = expression_compare();
									state.followingStackPointer--;

									value = new MdxBinaryExpression(value, t2, "AND");

								}
								break;

							default:
								goto loop32;
						}
					} while (true);

				loop32:
					;	// Stops C# compiler whining that label 'loop32' has no statements


				}

			}
			catch (RecognitionException re)
			{
				ReportError(re);
				Recover(input, re);
			}
			finally
			{
			}
			return value;
		}
		// $ANTLR end "expression_and"


		// $ANTLR start "expression_compare"
		// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:226:1: expression_compare returns [MdxExpression value] : t1= expression_add ( ( '=' | '<>' | '<' | '>' | '<=' | '>=' ) t2= expression_add )* ;
		public MdxExpression expression_compare() // throws RecognitionException [1]
		{

			MdxExpression value = default(MdxExpression);

			MdxExpression t1 = default(MdxExpression);

			MdxExpression t2 = default(MdxExpression);


			try
			{
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:227:2: (t1= expression_add ( ( '=' | '<>' | '<' | '>' | '<=' | '>=' ) t2= expression_add )* )
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:227:4: t1= expression_add ( ( '=' | '<>' | '<' | '>' | '<=' | '>=' ) t2= expression_add )*
				{
					PushFollow(FOLLOW_expression_add_in_expression_compare1195);
					t1 = expression_add();
					state.followingStackPointer--;

					value = t1; string op = "";
					// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:228:3: ( ( '=' | '<>' | '<' | '>' | '<=' | '>=' ) t2= expression_add )*
					do
					{
						int alt34 = 2;
						int LA34_0 = input.LA(1);

						if ((LA34_0 == 53 || (LA34_0 >= 64 && LA34_0 <= 68)))
						{
							alt34 = 1;
						}


						switch (alt34)
						{
							case 1:
								// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:229:4: ( '=' | '<>' | '<' | '>' | '<=' | '>=' ) t2= expression_add
								{
									// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:229:4: ( '=' | '<>' | '<' | '>' | '<=' | '>=' )
									int alt33 = 6;
									switch (input.LA(1))
									{
										case 53:
											{
												alt33 = 1;
											}
											break;
										case 64:
											{
												alt33 = 2;
											}
											break;
										case 65:
											{
												alt33 = 3;
											}
											break;
										case 66:
											{
												alt33 = 4;
											}
											break;
										case 67:
											{
												alt33 = 5;
											}
											break;
										case 68:
											{
												alt33 = 6;
											}
											break;
										default:
											NoViableAltException nvae_d33s0 =
													new NoViableAltException("", 33, 0, input);

											throw nvae_d33s0;
									}

									switch (alt33)
									{
										case 1:
											// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:229:6: '='
											{
												Match(input, 53, FOLLOW_53_in_expression_compare1208);
												op = "=";

											}
											break;
										case 2:
											// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:230:6: '<>'
											{
												Match(input, 64, FOLLOW_64_in_expression_compare1217);
												op = "<>";

											}
											break;
										case 3:
											// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:231:6: '<'
											{
												Match(input, 65, FOLLOW_65_in_expression_compare1226);
												op = "<";

											}
											break;
										case 4:
											// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:232:6: '>'
											{
												Match(input, 66, FOLLOW_66_in_expression_compare1235);
												op = ">";

											}
											break;
										case 5:
											// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:233:6: '<='
											{
												Match(input, 67, FOLLOW_67_in_expression_compare1244);
												op = "<=";

											}
											break;
										case 6:
											// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:234:6: '>='
											{
												Match(input, 68, FOLLOW_68_in_expression_compare1253);
												op = ">=";

											}
											break;

									}

									PushFollow(FOLLOW_expression_add_in_expression_compare1264);
									t2 = expression_add();
									state.followingStackPointer--;

									value = new MdxBinaryExpression(value, t2, op);

								}
								break;

							default:
								goto loop34;
						}
					} while (true);

				loop34:
					;	// Stops C# compiler whining that label 'loop34' has no statements


				}

			}
			catch (RecognitionException re)
			{
				ReportError(re);
				Recover(input, re);
			}
			finally
			{
			}
			return value;
		}
		// $ANTLR end "expression_compare"


		// $ANTLR start "expression_add"
		// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:239:1: expression_add returns [MdxExpression value] : t1= expression_mult ( ( '+' | '-' ) t2= expression_mult )* ;
		public MdxExpression expression_add() // throws RecognitionException [1]
		{

			MdxExpression value = default(MdxExpression);

			MdxExpression t1 = default(MdxExpression);

			MdxExpression t2 = default(MdxExpression);


			try
			{
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:240:2: (t1= expression_mult ( ( '+' | '-' ) t2= expression_mult )* )
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:240:4: t1= expression_mult ( ( '+' | '-' ) t2= expression_mult )*
				{
					PushFollow(FOLLOW_expression_mult_in_expression_add1294);
					t1 = expression_mult();
					state.followingStackPointer--;

					value = t1; string op = "";
					// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:241:3: ( ( '+' | '-' ) t2= expression_mult )*
					do
					{
						int alt36 = 2;
						int LA36_0 = input.LA(1);

						if (((LA36_0 >= 69 && LA36_0 <= 70)))
						{
							alt36 = 1;
						}


						switch (alt36)
						{
							case 1:
								// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:242:4: ( '+' | '-' ) t2= expression_mult
								{
									// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:242:4: ( '+' | '-' )
									int alt35 = 2;
									int LA35_0 = input.LA(1);

									if ((LA35_0 == 69))
									{
										alt35 = 1;
									}
									else if ((LA35_0 == 70))
									{
										alt35 = 2;
									}
									else
									{
										NoViableAltException nvae_d35s0 =
												new NoViableAltException("", 35, 0, input);

										throw nvae_d35s0;
									}
									switch (alt35)
									{
										case 1:
											// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:242:6: '+'
											{
												Match(input, 69, FOLLOW_69_in_expression_add1307);
												op = "+";

											}
											break;
										case 2:
											// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:243:6: '-'
											{
												Match(input, 70, FOLLOW_70_in_expression_add1316);
												op = "-";

											}
											break;

									}

									PushFollow(FOLLOW_expression_mult_in_expression_add1327);
									t2 = expression_mult();
									state.followingStackPointer--;

									value = new MdxBinaryExpression(value, t2, op);

								}
								break;

							default:
								goto loop36;
						}
					} while (true);

				loop36:
					;	// Stops C# compiler whining that label 'loop36' has no statements


				}

			}
			catch (RecognitionException re)
			{
				ReportError(re);
				Recover(input, re);
			}
			finally
			{
			}
			return value;
		}
		// $ANTLR end "expression_add"


		// $ANTLR start "expression_mult"
		// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:250:1: expression_mult returns [MdxExpression value] : f1= expression_power ( (op1= '/' | op2= '*' ) f2= expression_power )* ;
		public MdxExpression expression_mult() // throws RecognitionException [1]
		{

			MdxExpression value = default(MdxExpression);

			IToken op1 = null;
			IToken op2 = null;
			MdxExpression f1 = default(MdxExpression);

			MdxExpression f2 = default(MdxExpression);


			try
			{
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:251:3: (f1= expression_power ( (op1= '/' | op2= '*' ) f2= expression_power )* )
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:251:5: f1= expression_power ( (op1= '/' | op2= '*' ) f2= expression_power )*
				{
					PushFollow(FOLLOW_expression_power_in_expression_mult1359);
					f1 = expression_power();
					state.followingStackPointer--;

					value = f1; string op = "";
					// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:252:4: ( (op1= '/' | op2= '*' ) f2= expression_power )*
					do
					{
						int alt38 = 2;
						int LA38_0 = input.LA(1);

						if ((LA38_0 == 59 || LA38_0 == 71))
						{
							alt38 = 1;
						}


						switch (alt38)
						{
							case 1:
								// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:253:6: (op1= '/' | op2= '*' ) f2= expression_power
								{
									// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:253:6: (op1= '/' | op2= '*' )
									int alt37 = 2;
									int LA37_0 = input.LA(1);

									if ((LA37_0 == 71))
									{
										alt37 = 1;
									}
									else if ((LA37_0 == 59))
									{
										alt37 = 2;
									}
									else
									{
										NoViableAltException nvae_d37s0 =
												new NoViableAltException("", 37, 0, input);

										throw nvae_d37s0;
									}
									switch (alt37)
									{
										case 1:
											// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:253:8: op1= '/'
											{
												op1 = (IToken)Match(input, 71, FOLLOW_71_in_expression_mult1378);
												op = ((op1 != null) ? op1.Text : null);

											}
											break;
										case 2:
											// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:254:7: op2= '*'
											{
												op2 = (IToken)Match(input, 59, FOLLOW_59_in_expression_mult1390);
												op = ((op2 != null) ? op2.Text : null);

											}
											break;

									}

									PushFollow(FOLLOW_expression_power_in_expression_mult1407);
									f2 = expression_power();
									state.followingStackPointer--;

									value = new MdxBinaryExpression(value, f2, op);

								}
								break;

							default:
								goto loop38;
						}
					} while (true);

				loop38:
					;	// Stops C# compiler whining that label 'loop38' has no statements


				}

			}
			catch (RecognitionException re)
			{
				ReportError(re);
				Recover(input, re);
			}
			finally
			{
			}
			return value;
		}
		// $ANTLR end "expression_mult"


		// $ANTLR start "expression_power"
		// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:260:1: expression_power returns [MdxExpression value] : f1= expression_unary ( '^' f2= expression_unary )* ;
		public MdxExpression expression_power() // throws RecognitionException [1]
		{

			MdxExpression value = default(MdxExpression);

			MdxExpression f1 = default(MdxExpression);

			MdxExpression f2 = default(MdxExpression);


			try
			{
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:261:3: (f1= expression_unary ( '^' f2= expression_unary )* )
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:261:5: f1= expression_unary ( '^' f2= expression_unary )*
				{
					PushFollow(FOLLOW_expression_unary_in_expression_power1437);
					f1 = expression_unary();
					state.followingStackPointer--;

					value = f1;
					// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:262:4: ( '^' f2= expression_unary )*
					do
					{
						int alt39 = 2;
						int LA39_0 = input.LA(1);

						if ((LA39_0 == 72))
						{
							alt39 = 1;
						}


						switch (alt39)
						{
							case 1:
								// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:262:6: '^' f2= expression_unary
								{
									Match(input, 72, FOLLOW_72_in_expression_power1446);
									PushFollow(FOLLOW_expression_unary_in_expression_power1450);
									f2 = expression_unary();
									state.followingStackPointer--;

									value = new MdxBinaryExpression(value, f2, "^");

								}
								break;

							default:
								goto loop39;
						}
					} while (true);

				loop39:
					;	// Stops C# compiler whining that label 'loop39' has no statements


				}

			}
			catch (RecognitionException re)
			{
				ReportError(re);
				Recover(input, re);
			}
			finally
			{
			}
			return value;
		}
		// $ANTLR end "expression_power"


		// $ANTLR start "expression_unary"
		// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:266:1: expression_unary returns [MdxExpression value] : ( '-' e1= expression_range_is | '+' e2= expression_range_is | NOT t1= expression_range_is | e3= expression_range_is );
		public MdxExpression expression_unary() // throws RecognitionException [1]
		{

			MdxExpression value = default(MdxExpression);

			MdxExpression e1 = default(MdxExpression);

			MdxExpression e2 = default(MdxExpression);

			MdxExpression t1 = default(MdxExpression);

			MdxExpression e3 = default(MdxExpression);


			try
			{
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:267:2: ( '-' e1= expression_range_is | '+' e2= expression_range_is | NOT t1= expression_range_is | e3= expression_range_is )
				int alt40 = 4;
				switch (input.LA(1))
				{
					case 70:
						{
							alt40 = 1;
						}
						break;
					case 69:
						{
							alt40 = 2;
						}
						break;
					case NOT:
						{
							alt40 = 3;
						}
						break;
					case PROPERTIES:
					case DIMENSION:
					case INTEGER:
					case RANET_EXPRESSION:
					case FLOAT:
					case CASE:
					case ID:
					case QUOTED_ID:
					case STRING:
					case 62:
					case 75:
						{
							alt40 = 4;
						}
						break;
					default:
						NoViableAltException nvae_d40s0 =
								new NoViableAltException("", 40, 0, input);

						throw nvae_d40s0;
				}

				switch (alt40)
				{
					case 1:
						// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:267:4: '-' e1= expression_range_is
						{
							Match(input, 70, FOLLOW_70_in_expression_unary1479);
							PushFollow(FOLLOW_expression_range_is_in_expression_unary1483);
							e1 = expression_range_is();
							state.followingStackPointer--;

							value = new MdxUnaryExpression("-", e1);

						}
						break;
					case 2:
						// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:268:4: '+' e2= expression_range_is
						{
							Match(input, 69, FOLLOW_69_in_expression_unary1490);
							PushFollow(FOLLOW_expression_range_is_in_expression_unary1494);
							e2 = expression_range_is();
							state.followingStackPointer--;

							value = new MdxUnaryExpression("+", e2);

						}
						break;
					case 3:
						// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:269:4: NOT t1= expression_range_is
						{
							Match(input, NOT, FOLLOW_NOT_in_expression_unary1502);
							PushFollow(FOLLOW_expression_range_is_in_expression_unary1506);
							t1 = expression_range_is();
							state.followingStackPointer--;

							value = new MdxUnaryExpression("NOT", t1);

						}
						break;
					case 4:
						// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:270:4: e3= expression_range_is
						{
							PushFollow(FOLLOW_expression_range_is_in_expression_unary1516);
							e3 = expression_range_is();
							state.followingStackPointer--;

							value = e3;

						}
						break;

				}
			}
			catch (RecognitionException re)
			{
				ReportError(re);
				Recover(input, re);
			}
			finally
			{
			}
			return value;
		}
		// $ANTLR end "expression_unary"


		// $ANTLR start "expression_range_is"
		// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:273:1: expression_range_is returns [MdxExpression value] : e1= expression_property ( ':' e2= expression_property | IS e3= expression_property )? ;
		public MdxExpression expression_range_is() // throws RecognitionException [1]
		{

			MdxExpression value = default(MdxExpression);

			MdxExpression e1 = default(MdxExpression);

			MdxExpression e2 = default(MdxExpression);

			MdxExpression e3 = default(MdxExpression);


			try
			{
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:274:2: (e1= expression_property ( ':' e2= expression_property | IS e3= expression_property )? )
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:274:4: e1= expression_property ( ':' e2= expression_property | IS e3= expression_property )?
				{
					PushFollow(FOLLOW_expression_property_in_expression_range_is1535);
					e1 = expression_property();
					state.followingStackPointer--;

					value = e1;
					// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:275:4: ( ':' e2= expression_property | IS e3= expression_property )?
					int alt41 = 3;
					int LA41_0 = input.LA(1);

					if ((LA41_0 == 73))
					{
						alt41 = 1;
					}
					else if ((LA41_0 == IS))
					{
						alt41 = 2;
					}
					switch (alt41)
					{
						case 1:
							// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:275:6: ':' e2= expression_property
							{
								Match(input, 73, FOLLOW_73_in_expression_range_is1544);
								PushFollow(FOLLOW_expression_property_in_expression_range_is1548);
								e2 = expression_property();
								state.followingStackPointer--;

								value = new MdxBinaryExpression(value, e2, ":");

							}
							break;
						case 2:
							// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:276:6: IS e3= expression_property
							{
								Match(input, IS, FOLLOW_IS_in_expression_range_is1558);
								PushFollow(FOLLOW_expression_property_in_expression_range_is1562);
								e3 = expression_property();
								state.followingStackPointer--;

								value = new MdxBinaryExpression(value, e3, "IS");

							}
							break;

					}


				}

			}
			catch (RecognitionException re)
			{
				ReportError(re);
				Recover(input, re);
			}
			finally
			{
			}
			return value;
		}
		// $ANTLR end "expression_range_is"


		// $ANTLR start "expression_property"
		// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:280:1: expression_property returns [MdxExpression value] : e1= expression_simple ( '.' (id1= unquoted_identifier | '&' id2= quoted_identifier | id3= quoted_identifier | f1= expression_function ) )* ;
		public MdxExpression expression_property() // throws RecognitionException [1]
		{

			MdxExpression value = default(MdxExpression);

			MdxExpression e1 = default(MdxExpression);

			string id1 = default(string);

			mdxParser.quoted_identifier_return id2 = default(mdxParser.quoted_identifier_return);

			mdxParser.quoted_identifier_return id3 = default(mdxParser.quoted_identifier_return);

			MdxFunctionExpression f1 = default(MdxFunctionExpression);


			try
			{
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:281:2: (e1= expression_simple ( '.' (id1= unquoted_identifier | '&' id2= quoted_identifier | id3= quoted_identifier | f1= expression_function ) )* )
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:281:4: e1= expression_simple ( '.' (id1= unquoted_identifier | '&' id2= quoted_identifier | id3= quoted_identifier | f1= expression_function ) )*
				{
					PushFollow(FOLLOW_expression_simple_in_expression_property1588);
					e1 = expression_simple();
					state.followingStackPointer--;

					value = e1;
					// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:282:3: ( '.' (id1= unquoted_identifier | '&' id2= quoted_identifier | id3= quoted_identifier | f1= expression_function ) )*
					do
					{
						int alt43 = 2;
						int LA43_0 = input.LA(1);

						if ((LA43_0 == 61))
						{
							alt43 = 1;
						}


						switch (alt43)
						{
							case 1:
								// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:282:5: '.' (id1= unquoted_identifier | '&' id2= quoted_identifier | id3= quoted_identifier | f1= expression_function )
								{
									Match(input, 61, FOLLOW_61_in_expression_property1596);
									// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:283:4: (id1= unquoted_identifier | '&' id2= quoted_identifier | id3= quoted_identifier | f1= expression_function )
									int alt42 = 4;
									switch (input.LA(1))
									{
										case ID:
											{
												int LA42_1 = input.LA(2);

												if ((LA42_1 == 62))
												{
													alt42 = 4;
												}
												else if ((LA42_1 == EOF || LA42_1 == SET || (LA42_1 >= CELL && LA42_1 <= SELECT) || (LA42_1 >= CALCULATED && LA42_1 <= AS) || (LA42_1 >= DIMENSION && LA42_1 <= ON) || (LA42_1 >= XOR && LA42_1 <= AND) || LA42_1 == IS || (LA42_1 >= ELSE && LA42_1 <= THEN) || (LA42_1 >= 53 && LA42_1 <= 61) || (LA42_1 >= 63 && LA42_1 <= 73) || LA42_1 == 76))
												{
													alt42 = 1;
												}
												else
												{
													NoViableAltException nvae_d42s1 =
															new NoViableAltException("", 42, 1, input);

													throw nvae_d42s1;
												}
											}
											break;
										case DIMENSION:
											{
												int LA42_2 = input.LA(2);

												if ((LA42_2 == 62))
												{
													alt42 = 4;
												}
												else if ((LA42_2 == EOF || LA42_2 == SET || (LA42_2 >= CELL && LA42_2 <= SELECT) || (LA42_2 >= CALCULATED && LA42_2 <= AS) || (LA42_2 >= DIMENSION && LA42_2 <= ON) || (LA42_2 >= XOR && LA42_2 <= AND) || LA42_2 == IS || (LA42_2 >= ELSE && LA42_2 <= THEN) || (LA42_2 >= 53 && LA42_2 <= 61) || (LA42_2 >= 63 && LA42_2 <= 73) || LA42_2 == 76))
												{
													alt42 = 1;
												}
												else
												{
													NoViableAltException nvae_d42s2 =
															new NoViableAltException("", 42, 2, input);

													throw nvae_d42s2;
												}
											}
											break;
										case PROPERTIES:
											{
												int LA42_3 = input.LA(2);

												if ((LA42_3 == EOF || LA42_3 == SET || (LA42_3 >= CELL && LA42_3 <= SELECT) || (LA42_3 >= CALCULATED && LA42_3 <= AS) || (LA42_3 >= DIMENSION && LA42_3 <= ON) || (LA42_3 >= XOR && LA42_3 <= AND) || LA42_3 == IS || (LA42_3 >= ELSE && LA42_3 <= THEN) || (LA42_3 >= 53 && LA42_3 <= 61) || (LA42_3 >= 63 && LA42_3 <= 73) || LA42_3 == 76))
												{
													alt42 = 1;
												}
												else if ((LA42_3 == 62))
												{
													alt42 = 4;
												}
												else
												{
													NoViableAltException nvae_d42s3 =
															new NoViableAltException("", 42, 3, input);

													throw nvae_d42s3;
												}
											}
											break;
										case 74:
											{
												alt42 = 2;
											}
											break;
										case QUOTED_ID:
											{
												int LA42_5 = input.LA(2);

												if ((LA42_5 == 62))
												{
													alt42 = 4;
												}
												else if ((LA42_5 == EOF || LA42_5 == SET || (LA42_5 >= CELL && LA42_5 <= SELECT) || (LA42_5 >= CALCULATED && LA42_5 <= AS) || (LA42_5 >= DIMENSION && LA42_5 <= ON) || (LA42_5 >= XOR && LA42_5 <= AND) || LA42_5 == IS || (LA42_5 >= ELSE && LA42_5 <= THEN) || (LA42_5 >= 53 && LA42_5 <= 61) || (LA42_5 >= 63 && LA42_5 <= 73) || LA42_5 == 76))
												{
													alt42 = 3;
												}
												else
												{
													NoViableAltException nvae_d42s5 =
															new NoViableAltException("", 42, 5, input);

													throw nvae_d42s5;
												}
											}
											break;
										default:
											NoViableAltException nvae_d42s0 =
													new NoViableAltException("", 42, 0, input);

											throw nvae_d42s0;
									}

									switch (alt42)
									{
										case 1:
											// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:283:6: id1= unquoted_identifier
											{
												PushFollow(FOLLOW_unquoted_identifier_in_expression_property1606);
												id1 = unquoted_identifier();
												state.followingStackPointer--;

												value = new MdxPropertyExpression(value, id1);

											}
											break;
										case 2:
											// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:285:6: '&' id2= quoted_identifier
											{
												Match(input, 74, FOLLOW_74_in_expression_property1619);
												PushFollow(FOLLOW_quoted_identifier_in_expression_property1623);
												id2 = quoted_identifier();
												state.followingStackPointer--;

												value = new MdxPropertyExpression(value, "&" + ((id2 != null) ? input.ToString((IToken)(id2.Start), (IToken)(id2.Stop)) : null));

											}
											break;
										case 3:
											// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:287:6: id3= quoted_identifier
											{
												PushFollow(FOLLOW_quoted_identifier_in_expression_property1638);
												id3 = quoted_identifier();
												state.followingStackPointer--;

												value = new MdxPropertyExpression(value, ((id3 != null) ? input.ToString((IToken)(id3.Start), (IToken)(id3.Stop)) : null));

											}
											break;
										case 4:
											// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:289:6: f1= expression_function
											{
												PushFollow(FOLLOW_expression_function_in_expression_property1653);
												f1 = expression_function();
												state.followingStackPointer--;

												value = new MdxPropertyExpression(value, f1);

											}
											break;

									}


								}
								break;

							default:
								goto loop43;
						}
					} while (true);

				loop43:
					;	// Stops C# compiler whining that label 'loop43' has no statements


				}

			}
			catch (RecognitionException re)
			{
				ReportError(re);
				Recover(input, re);
			}
			finally
			{
			}
			return value;
		}
		// $ANTLR end "expression_property"


		// $ANTLR start "expression_simple"
		// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:295:1: expression_simple returns [MdxExpression value] : ( expression_function | '(' expressions_list ')' | '{' ( expressions_list )? '}' | expression_case | string_value | INTEGER | FLOAT | identifier | RANET_EXPRESSION );
		public MdxExpression expression_simple() // throws RecognitionException [1]
		{

			MdxExpression value = default(MdxExpression);

			IToken INTEGER32 = null;
			IToken FLOAT33 = null;
			IToken RANET_EXPRESSION35 = null;
			MdxFunctionExpression expression_function27 = default(MdxFunctionExpression);

			List<MdxExpression> expressions_list28 = default(List<MdxExpression>);

			List<MdxExpression> expressions_list29 = default(List<MdxExpression>);

			MdxCaseExpression expression_case30 = default(MdxCaseExpression);

			string string_value31 = default(string);

			string identifier34 = default(string);


			try
			{
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:296:2: ( expression_function | '(' expressions_list ')' | '{' ( expressions_list )? '}' | expression_case | string_value | INTEGER | FLOAT | identifier | RANET_EXPRESSION )
				int alt45 = 9;
				alt45 = dfa45.Predict(input);
				switch (alt45)
				{
					case 1:
						// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:296:4: expression_function
						{
							PushFollow(FOLLOW_expression_function_in_expression_simple1683);
							expression_function27 = expression_function();
							state.followingStackPointer--;

							value = expression_function27;

						}
						break;
					case 2:
						// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:297:4: '(' expressions_list ')'
						{
							Match(input, 62, FOLLOW_62_in_expression_simple1690);
							PushFollow(FOLLOW_expressions_list_in_expression_simple1692);
							expressions_list28 = expressions_list();
							state.followingStackPointer--;

							Match(input, 63, FOLLOW_63_in_expression_simple1694);

							value = null;
							if (expressions_list28 != null)
								if (expressions_list28.Count == 1)
									value = expressions_list28[0] as MdxTupleExpression;

							if (value == null)
								value = new MdxTupleExpression(expressions_list28);



						}
						break;
					case 3:
						// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:308:4: '{' ( expressions_list )? '}'
						{
							Match(input, 75, FOLLOW_75_in_expression_simple1703);
							// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:308:8: ( expressions_list )?
							int alt44 = 2;
							int LA44_0 = input.LA(1);

							if ((LA44_0 == PROPERTIES || LA44_0 == DIMENSION || LA44_0 == INTEGER || LA44_0 == RANET_EXPRESSION || LA44_0 == NOT || (LA44_0 >= FLOAT && LA44_0 <= CASE) || (LA44_0 >= ID && LA44_0 <= STRING) || LA44_0 == 62 || (LA44_0 >= 69 && LA44_0 <= 70) || LA44_0 == 75))
							{
								alt44 = 1;
							}
							switch (alt44)
							{
								case 1:
									// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:308:9: expressions_list
									{
										PushFollow(FOLLOW_expressions_list_in_expression_simple1706);
										expressions_list29 = expressions_list();
										state.followingStackPointer--;


									}
									break;

							}

							Match(input, 76, FOLLOW_76_in_expression_simple1710);
							value = new MdxSetExpression(expressions_list29);

						}
						break;
					case 4:
						// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:310:4: expression_case
						{
							PushFollow(FOLLOW_expression_case_in_expression_simple1719);
							expression_case30 = expression_case();
							state.followingStackPointer--;

							value = expression_case30;

						}
						break;
					case 5:
						// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:311:4: string_value
						{
							PushFollow(FOLLOW_string_value_in_expression_simple1726);
							string_value31 = string_value();
							state.followingStackPointer--;

							value = new MdxConstantExpression(string_value31, MdxConstantKind.String);

						}
						break;
					case 6:
						// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:312:4: INTEGER
						{
							INTEGER32 = (IToken)Match(input, INTEGER, FOLLOW_INTEGER_in_expression_simple1733);
							value = new MdxConstantExpression(((INTEGER32 != null) ? INTEGER32.Text : null), MdxConstantKind.Integer);

						}
						break;
					case 7:
						// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:313:4: FLOAT
						{
							FLOAT33 = (IToken)Match(input, FLOAT, FOLLOW_FLOAT_in_expression_simple1740);
							value = new MdxConstantExpression(((FLOAT33 != null) ? FLOAT33.Text : null), MdxConstantKind.Float);

						}
						break;
					case 8:
						// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:314:4: identifier
						{
							PushFollow(FOLLOW_identifier_in_expression_simple1747);
							identifier34 = identifier();
							state.followingStackPointer--;

							value = new MdxObjectReferenceExpression(identifier34);

						}
						break;
					case 9:
						// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:315:4: RANET_EXPRESSION
						{
							RANET_EXPRESSION35 = (IToken)Match(input, RANET_EXPRESSION, FOLLOW_RANET_EXPRESSION_in_expression_simple1754);
							value = new MdxObjectReferenceExpression(((RANET_EXPRESSION35 != null) ? RANET_EXPRESSION35.Text : null));

						}
						break;

				}
			}
			catch (RecognitionException re)
			{
				ReportError(re);
				Recover(input, re);
			}
			finally
			{
			}
			return value;
		}
		// $ANTLR end "expression_simple"


		// $ANTLR start "expressions_list"
		// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:318:1: expressions_list returns [List<MdxExpression> value] : e1= expression ( ',' e2= expression )* ;
		public List<MdxExpression> expressions_list() // throws RecognitionException [1]
		{

			List<MdxExpression> value = default(List<MdxExpression>);

			MdxExpression e1 = default(MdxExpression);

			MdxExpression e2 = default(MdxExpression);


			try
			{
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:319:2: (e1= expression ( ',' e2= expression )* )
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:319:4: e1= expression ( ',' e2= expression )*
				{
					PushFollow(FOLLOW_expression_in_expressions_list1782);
					e1 = expression();
					state.followingStackPointer--;

					value = new List<MdxExpression>(); value.Add(e1);
					// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:321:3: ( ',' e2= expression )*
					do
					{
						int alt46 = 2;
						int LA46_0 = input.LA(1);

						if ((LA46_0 == 58))
						{
							alt46 = 1;
						}


						switch (alt46)
						{
							case 1:
								// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:321:5: ',' e2= expression
								{
									Match(input, 58, FOLLOW_58_in_expressions_list1792);
									PushFollow(FOLLOW_expression_in_expressions_list1796);
									e2 = expression();
									state.followingStackPointer--;

									value.Add(e2);

								}
								break;

							default:
								goto loop46;
						}
					} while (true);

				loop46:
					;	// Stops C# compiler whining that label 'loop46' has no statements


				}

			}
			catch (RecognitionException re)
			{
				ReportError(re);
				Recover(input, re);
			}
			finally
			{
			}
			return value;
		}
		// $ANTLR end "expressions_list"


		// $ANTLR start "expression_function"
		// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:323:1: expression_function returns [MdxFunctionExpression value] : identifier '(' ( expressions_list )? ')' ;
		public MdxFunctionExpression expression_function() // throws RecognitionException [1]
		{

			MdxFunctionExpression value = default(MdxFunctionExpression);

			string identifier36 = default(string);

			List<MdxExpression> expressions_list37 = default(List<MdxExpression>);


			try
			{
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:324:2: ( identifier '(' ( expressions_list )? ')' )
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:324:4: identifier '(' ( expressions_list )? ')'
				{
					PushFollow(FOLLOW_identifier_in_expression_function1815);
					identifier36 = identifier();
					state.followingStackPointer--;

					Match(input, 62, FOLLOW_62_in_expression_function1817);
					// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:324:19: ( expressions_list )?
					int alt47 = 2;
					int LA47_0 = input.LA(1);

					if ((LA47_0 == PROPERTIES || LA47_0 == DIMENSION || LA47_0 == INTEGER || LA47_0 == RANET_EXPRESSION || LA47_0 == NOT || (LA47_0 >= FLOAT && LA47_0 <= CASE) || (LA47_0 >= ID && LA47_0 <= STRING) || LA47_0 == 62 || (LA47_0 >= 69 && LA47_0 <= 70) || LA47_0 == 75))
					{
						alt47 = 1;
					}
					switch (alt47)
					{
						case 1:
							// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:324:21: expressions_list
							{
								PushFollow(FOLLOW_expressions_list_in_expression_function1821);
								expressions_list37 = expressions_list();
								state.followingStackPointer--;


							}
							break;

					}

					Match(input, 63, FOLLOW_63_in_expression_function1826);
					value = new MdxFunctionExpression(identifier36, expressions_list37);

				}

			}
			catch (RecognitionException re)
			{
				ReportError(re);
				Recover(input, re);
			}
			finally
			{
			}
			return value;
		}
		// $ANTLR end "expression_function"


		// $ANTLR start "expression_case"
		// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:327:1: expression_case returns [MdxCaseExpression value] : CASE (ve1= expression )? (w1= when_clause (w2= when_clause )* )? ( ELSE ve2= expression )? END ;
		public MdxCaseExpression expression_case() // throws RecognitionException [1]
		{

			MdxCaseExpression value = default(MdxCaseExpression);

			MdxExpression ve1 = default(MdxExpression);

			MdxWhenClause w1 = default(MdxWhenClause);

			MdxWhenClause w2 = default(MdxWhenClause);

			MdxExpression ve2 = default(MdxExpression);


			try
			{
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:328:2: ( CASE (ve1= expression )? (w1= when_clause (w2= when_clause )* )? ( ELSE ve2= expression )? END )
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:328:4: CASE (ve1= expression )? (w1= when_clause (w2= when_clause )* )? ( ELSE ve2= expression )? END
				{
					Match(input, CASE, FOLLOW_CASE_in_expression_case1843);
					// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:328:9: (ve1= expression )?
					int alt48 = 2;
					int LA48_0 = input.LA(1);

					if ((LA48_0 == PROPERTIES || LA48_0 == DIMENSION || LA48_0 == INTEGER || LA48_0 == RANET_EXPRESSION || LA48_0 == NOT || (LA48_0 >= FLOAT && LA48_0 <= CASE) || (LA48_0 >= ID && LA48_0 <= STRING) || LA48_0 == 62 || (LA48_0 >= 69 && LA48_0 <= 70) || LA48_0 == 75))
					{
						alt48 = 1;
					}
					switch (alt48)
					{
						case 1:
							// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:328:10: ve1= expression
							{
								PushFollow(FOLLOW_expression_in_expression_case1848);
								ve1 = expression();
								state.followingStackPointer--;


							}
							break;

					}

					var wl = new List<MdxWhenClause>();
					// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:330:5: (w1= when_clause (w2= when_clause )* )?
					int alt50 = 2;
					int LA50_0 = input.LA(1);

					if ((LA50_0 == WHEN))
					{
						alt50 = 1;
					}
					switch (alt50)
					{
						case 1:
							// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:330:6: w1= when_clause (w2= when_clause )*
							{
								PushFollow(FOLLOW_when_clause_in_expression_case1865);
								w1 = when_clause();
								state.followingStackPointer--;

								wl.Add(w1);
								// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:331:6: (w2= when_clause )*
								do
								{
									int alt49 = 2;
									int LA49_0 = input.LA(1);

									if ((LA49_0 == WHEN))
									{
										alt49 = 1;
									}


									switch (alt49)
									{
										case 1:
											// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:331:8: w2= when_clause
											{
												PushFollow(FOLLOW_when_clause_in_expression_case1878);
												w2 = when_clause();
												state.followingStackPointer--;

												wl.Add(w2);

											}
											break;

										default:
											goto loop49;
									}
								} while (true);

							loop49:
								;	// Stops C# compiler whining that label 'loop49' has no statements


							}
							break;

					}

					// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:333:5: ( ELSE ve2= expression )?
					int alt51 = 2;
					int LA51_0 = input.LA(1);

					if ((LA51_0 == ELSE))
					{
						alt51 = 1;
					}
					switch (alt51)
					{
						case 1:
							// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:333:7: ELSE ve2= expression
							{
								Match(input, ELSE, FOLLOW_ELSE_in_expression_case1899);
								PushFollow(FOLLOW_expression_in_expression_case1903);
								ve2 = expression();
								state.followingStackPointer--;


							}
							break;

					}

					Match(input, END, FOLLOW_END_in_expression_case1911);
					value = new MdxCaseExpression(ve1, wl, ve2);

				}

			}
			catch (RecognitionException re)
			{
				ReportError(re);
				Recover(input, re);
			}
			finally
			{
			}
			return value;
		}
		// $ANTLR end "expression_case"


		// $ANTLR start "when_clause"
		// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:338:1: when_clause returns [MdxWhenClause value] : WHEN e1= expression THEN e2= expression ;
		public MdxWhenClause when_clause() // throws RecognitionException [1]
		{

			MdxWhenClause value = default(MdxWhenClause);

			MdxExpression e1 = default(MdxExpression);

			MdxExpression e2 = default(MdxExpression);


			try
			{
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:339:2: ( WHEN e1= expression THEN e2= expression )
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:339:4: WHEN e1= expression THEN e2= expression
				{
					Match(input, WHEN, FOLLOW_WHEN_in_when_clause1936);
					PushFollow(FOLLOW_expression_in_when_clause1940);
					e1 = expression();
					state.followingStackPointer--;

					Match(input, THEN, FOLLOW_THEN_in_when_clause1942);
					PushFollow(FOLLOW_expression_in_when_clause1946);
					e2 = expression();
					state.followingStackPointer--;

					value = new MdxWhenClause(e1, e2);

				}

			}
			catch (RecognitionException re)
			{
				ReportError(re);
				Recover(input, re);
			}
			finally
			{
			}
			return value;
		}
		// $ANTLR end "when_clause"


		// $ANTLR start "identifier"
		// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:344:1: identifier returns [string value] : ( unquoted_identifier | quoted_identifier );
		public string identifier() // throws RecognitionException [1]
		{

			string value = default(string);

			string unquoted_identifier38 = default(string);

			mdxParser.quoted_identifier_return quoted_identifier39 = default(mdxParser.quoted_identifier_return);


			try
			{
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:345:2: ( unquoted_identifier | quoted_identifier )
				int alt52 = 2;
				int LA52_0 = input.LA(1);

				if ((LA52_0 == PROPERTIES || LA52_0 == DIMENSION || LA52_0 == ID))
				{
					alt52 = 1;
				}
				else if ((LA52_0 == QUOTED_ID))
				{
					alt52 = 2;
				}
				else
				{
					NoViableAltException nvae_d52s0 =
							new NoViableAltException("", 52, 0, input);

					throw nvae_d52s0;
				}
				switch (alt52)
				{
					case 1:
						// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:345:4: unquoted_identifier
						{
							PushFollow(FOLLOW_unquoted_identifier_in_identifier1974);
							unquoted_identifier38 = unquoted_identifier();
							state.followingStackPointer--;

							value = unquoted_identifier38;

						}
						break;
					case 2:
						// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:346:8: quoted_identifier
						{
							PushFollow(FOLLOW_quoted_identifier_in_identifier1985);
							quoted_identifier39 = quoted_identifier();
							state.followingStackPointer--;

							value = ((quoted_identifier39 != null) ? quoted_identifier39.value : default(string));

						}
						break;

				}
			}
			catch (RecognitionException re)
			{
				ReportError(re);
				Recover(input, re);
			}
			finally
			{
			}
			return value;
		}
		// $ANTLR end "identifier"


		// $ANTLR start "unquoted_identifier"
		// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:349:1: unquoted_identifier returns [string value] : ( ID | keyword );
		public string unquoted_identifier() // throws RecognitionException [1]
		{

			string value = default(string);

			IToken ID40 = null;
			string keyword41 = default(string);


			try
			{
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:350:2: ( ID | keyword )
				int alt53 = 2;
				int LA53_0 = input.LA(1);

				if ((LA53_0 == ID))
				{
					alt53 = 1;
				}
				else if ((LA53_0 == PROPERTIES || LA53_0 == DIMENSION))
				{
					alt53 = 2;
				}
				else
				{
					NoViableAltException nvae_d53s0 =
							new NoViableAltException("", 53, 0, input);

					throw nvae_d53s0;
				}
				switch (alt53)
				{
					case 1:
						// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:350:4: ID
						{
							ID40 = (IToken)Match(input, ID, FOLLOW_ID_in_unquoted_identifier2006);
							value = ((ID40 != null) ? ID40.Text : null);

						}
						break;
					case 2:
						// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:351:4: keyword
						{
							PushFollow(FOLLOW_keyword_in_unquoted_identifier2013);
							keyword41 = keyword();
							state.followingStackPointer--;

							value = keyword41;

						}
						break;

				}
			}
			catch (RecognitionException re)
			{
				ReportError(re);
				Recover(input, re);
			}
			finally
			{
			}
			return value;
		}
		// $ANTLR end "unquoted_identifier"

		public class quoted_identifier_return : ParserRuleReturnScope
		{
			public string value;
		};

		// $ANTLR start "quoted_identifier"
		// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:354:1: quoted_identifier returns [string value] : QUOTED_ID ;
		public mdxParser.quoted_identifier_return quoted_identifier() // throws RecognitionException [1]
		{
			mdxParser.quoted_identifier_return retval = new mdxParser.quoted_identifier_return();
			retval.Start = input.LT(1);

			IToken QUOTED_ID42 = null;

			try
			{
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:355:2: ( QUOTED_ID )
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:355:4: QUOTED_ID
				{
					QUOTED_ID42 = (IToken)Match(input, QUOTED_ID, FOLLOW_QUOTED_ID_in_quoted_identifier2030);
					retval.value = ((QUOTED_ID42 != null) ? QUOTED_ID42.Text : null);

				}

				retval.Stop = input.LT(-1);

			}
			catch (RecognitionException re)
			{
				ReportError(re);
				Recover(input, re);
			}
			finally
			{
			}
			return retval;
		}
		// $ANTLR end "quoted_identifier"


		// $ANTLR start "keyword"
		// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:358:1: keyword returns [string value] : ( DIMENSION | PROPERTIES );
		public string keyword() // throws RecognitionException [1]
		{

			string value = default(string);

			IToken DIMENSION43 = null;
			IToken PROPERTIES44 = null;

			try
			{
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:359:2: ( DIMENSION | PROPERTIES )
				int alt54 = 2;
				int LA54_0 = input.LA(1);

				if ((LA54_0 == DIMENSION))
				{
					alt54 = 1;
				}
				else if ((LA54_0 == PROPERTIES))
				{
					alt54 = 2;
				}
				else
				{
					NoViableAltException nvae_d54s0 =
							new NoViableAltException("", 54, 0, input);

					throw nvae_d54s0;
				}
				switch (alt54)
				{
					case 1:
						// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:359:4: DIMENSION
						{
							DIMENSION43 = (IToken)Match(input, DIMENSION, FOLLOW_DIMENSION_in_keyword2047);
							value = ((DIMENSION43 != null) ? DIMENSION43.Text : null);

						}
						break;
					case 2:
						// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:360:4: PROPERTIES
						{
							PROPERTIES44 = (IToken)Match(input, PROPERTIES, FOLLOW_PROPERTIES_in_keyword2054);
							value = ((PROPERTIES44 != null) ? PROPERTIES44.Text : null);

						}
						break;

				}
			}
			catch (RecognitionException re)
			{
				ReportError(re);
				Recover(input, re);
			}
			finally
			{
			}
			return value;
		}
		// $ANTLR end "keyword"


		// $ANTLR start "string_value"
		// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:363:1: string_value returns [string value] : STRING ;
		public string string_value() // throws RecognitionException [1]
		{

			string value = default(string);

			IToken STRING45 = null;

			try
			{
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:364:2: ( STRING )
				// D:\\tfs\\Ranet.UILibrary.OLAP4\\trunk\\UILibrary.Olap\\Ranet.Olap.Mdx\\Compiler\\Parser\\mdx.g:364:4: STRING
				{
					STRING45 = (IToken)Match(input, STRING, FOLLOW_STRING_in_string_value2071);
					value = ((STRING45 != null) ? STRING45.Text : null).Substring(1, ((STRING45 != null) ? STRING45.Text : null).Length - 2);

				}

			}
			catch (RecognitionException re)
			{
				ReportError(re);
				Recover(input, re);
			}
			finally
			{
			}
			return value;
		}
		// $ANTLR end "string_value"

		// Delegated rules


		protected DFA45 dfa45;
		private void InitializeCyclicDFAs()
		{
			this.dfa45 = new DFA45(this);
		}

		const string DFA45_eotS =
				"\x0e\uffff";
		const string DFA45_eofS =
				"\x01\uffff\x04\x0c\x09\uffff";
		const string DFA45_minS =
				"\x01\x0a\x04\x06\x09\uffff";
		const string DFA45_maxS =
				"\x01\x4b\x04\x4c\x09\uffff";
		const string DFA45_acceptS =
				"\x05\uffff\x01\x02\x01\x03\x01\x04\x01\x05\x01\x06\x01\x07\x01" +
				"\x09\x01\x08\x01\x01";
		const string DFA45_specialS =
				"\x0e\uffff}>";
		static readonly string[] DFA45_transitionS = {
            "\x01\x03\x0a\uffff\x01\x02\x01\uffff\x01\x09\x01\uffff\x01"+
            "\x0b\x08\uffff\x01\x0a\x01\x07\x04\uffff\x01\x01\x01\x04\x01"+
            "\x08\x13\uffff\x01\x05\x0c\uffff\x01\x06",
            "\x01\x0c\x02\uffff\x03\x0c\x02\uffff\x03\x0c\x04\uffff\x02"+
            "\x0c\x06\uffff\x03\x0c\x01\uffff\x01\x0c\x02\uffff\x04\x0c\x0d"+
            "\uffff\x09\x0c\x01\x0d\x0b\x0c\x02\uffff\x01\x0c",
            "\x01\x0c\x02\uffff\x03\x0c\x02\uffff\x03\x0c\x04\uffff\x02"+
            "\x0c\x06\uffff\x03\x0c\x01\uffff\x01\x0c\x02\uffff\x04\x0c\x0d"+
            "\uffff\x09\x0c\x01\x0d\x0b\x0c\x02\uffff\x01\x0c",
            "\x01\x0c\x02\uffff\x03\x0c\x02\uffff\x03\x0c\x04\uffff\x02"+
            "\x0c\x06\uffff\x03\x0c\x01\uffff\x01\x0c\x02\uffff\x04\x0c\x0d"+
            "\uffff\x09\x0c\x01\x0d\x0b\x0c\x02\uffff\x01\x0c",
            "\x01\x0c\x02\uffff\x03\x0c\x02\uffff\x03\x0c\x04\uffff\x02"+
            "\x0c\x06\uffff\x03\x0c\x01\uffff\x01\x0c\x02\uffff\x04\x0c\x0d"+
            "\uffff\x09\x0c\x01\x0d\x0b\x0c\x02\uffff\x01\x0c",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
    };

		static readonly short[] DFA45_eot = DFA.UnpackEncodedString(DFA45_eotS);
		static readonly short[] DFA45_eof = DFA.UnpackEncodedString(DFA45_eofS);
		static readonly char[] DFA45_min = DFA.UnpackEncodedStringToUnsignedChars(DFA45_minS);
		static readonly char[] DFA45_max = DFA.UnpackEncodedStringToUnsignedChars(DFA45_maxS);
		static readonly short[] DFA45_accept = DFA.UnpackEncodedString(DFA45_acceptS);
		static readonly short[] DFA45_special = DFA.UnpackEncodedString(DFA45_specialS);
		static readonly short[][] DFA45_transition = DFA.UnpackEncodedStringArray(DFA45_transitionS);

		protected class DFA45 : DFA
		{
			public DFA45(BaseRecognizer recognizer)
			{
				this.recognizer = recognizer;
				this.decisionNumber = 45;
				this.eot = DFA45_eot;
				this.eof = DFA45_eof;
				this.min = DFA45_min;
				this.max = DFA45_max;
				this.accept = DFA45_accept;
				this.special = DFA45_special;
				this.transition = DFA45_transition;

			}

			override public string Description
			{
				get { return "295:1: expression_simple returns [MdxExpression value] : ( expression_function | '(' expressions_list ')' | '{' ( expressions_list )? '}' | expression_case | string_value | INTEGER | FLOAT | identifier | RANET_EXPRESSION );"; }
			}

		}



		public static readonly BitSet FOLLOW_mdx_statement_single_in_mdx_statement67 = new BitSet(new ulong[] { 0x0000000000000000UL });
		public static readonly BitSet FOLLOW_EOF_in_mdx_statement69 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_select_statement_in_mdx_statement_single87 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_update_statement_in_mdx_statement_single95 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_UPDATE_in_update_statement116 = new BitSet(new ulong[] { 0x4000030002280420UL });
		public static readonly BitSet FOLLOW_CUBE_in_update_statement118 = new BitSet(new ulong[] { 0x4000030002280420UL });
		public static readonly BitSet FOLLOW_cube_specification_in_update_statement133 = new BitSet(new ulong[] { 0x0000000000000040UL });
		public static readonly BitSet FOLLOW_SET_in_update_statement145 = new BitSet(new ulong[] { 0x4000070C02A00400UL, 0x0000000000000800UL });
		public static readonly BitSet FOLLOW_expression_property_in_update_statement149 = new BitSet(new ulong[] { 0x0020000000000000UL });
		public static readonly BitSet FOLLOW_53_in_update_statement162 = new BitSet(new ulong[] { 0x4000070D02A00400UL, 0x0000000000000860UL });
		public static readonly BitSet FOLLOW_expression_or_xor_in_update_statement166 = new BitSet(new ulong[] { 0x01C0000000000002UL });
		public static readonly BitSet FOLLOW_54_in_update_statement181 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_55_in_update_statement193 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_56_in_update_statement205 = new BitSet(new ulong[] { 0x0200000000000080UL });
		public static readonly BitSet FOLLOW_BY_in_update_statement219 = new BitSet(new ulong[] { 0x4000070C02A00400UL, 0x0000000000000800UL });
		public static readonly BitSet FOLLOW_weight_value_expression_in_update_statement223 = new BitSet(new ulong[] { 0x0200000000000000UL });
		public static readonly BitSet FOLLOW_57_in_update_statement239 = new BitSet(new ulong[] { 0x0000000000000082UL });
		public static readonly BitSet FOLLOW_BY_in_update_statement253 = new BitSet(new ulong[] { 0x4000070C02A00400UL, 0x0000000000000800UL });
		public static readonly BitSet FOLLOW_weight_value_expression_in_update_statement257 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_expression_property_in_weight_value_expression294 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_expression_property_in_condition312 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_WITH_in_select_statement337 = new BitSet(new ulong[] { 0x000000000000C240UL });
		public static readonly BitSet FOLLOW_with_clause_single_in_select_statement347 = new BitSet(new ulong[] { 0x000000000000CA40UL });
		public static readonly BitSet FOLLOW_select_statement_subcube_in_select_statement376 = new BitSet(new ulong[] { 0x0000000000000602UL });
		public static readonly BitSet FOLLOW_CELL_in_select_statement389 = new BitSet(new ulong[] { 0x0000000000000400UL });
		public static readonly BitSet FOLLOW_PROPERTIES_in_select_statement392 = new BitSet(new ulong[] { 0x000003001C200400UL });
		public static readonly BitSet FOLLOW_cell_property_in_select_statement396 = new BitSet(new ulong[] { 0x0400000000000002UL });
		public static readonly BitSet FOLLOW_58_in_select_statement407 = new BitSet(new ulong[] { 0x000003001C200400UL });
		public static readonly BitSet FOLLOW_cell_property_in_select_statement411 = new BitSet(new ulong[] { 0x0400000000000002UL });
		public static readonly BitSet FOLLOW_SELECT_in_select_statement_subcube445 = new BitSet(new ulong[] { 0x4800070D02A81400UL, 0x0000000000000860UL });
		public static readonly BitSet FOLLOW_59_in_select_statement_subcube454 = new BitSet(new ulong[] { 0x0000000000001000UL });
		public static readonly BitSet FOLLOW_axis_specification_in_select_statement_subcube463 = new BitSet(new ulong[] { 0x0400000000001000UL });
		public static readonly BitSet FOLLOW_58_in_select_statement_subcube474 = new BitSet(new ulong[] { 0x4000070D02A80400UL, 0x0000000000000860UL });
		public static readonly BitSet FOLLOW_axis_specification_in_select_statement_subcube478 = new BitSet(new ulong[] { 0x0400000000001000UL });
		public static readonly BitSet FOLLOW_FROM_in_select_statement_subcube494 = new BitSet(new ulong[] { 0x4000030002280420UL });
		public static readonly BitSet FOLLOW_cube_specification_in_select_statement_subcube498 = new BitSet(new ulong[] { 0x0000000000002002UL });
		public static readonly BitSet FOLLOW_WHERE_in_select_statement_subcube507 = new BitSet(new ulong[] { 0x4000070C02A00400UL, 0x0000000000000800UL });
		public static readonly BitSet FOLLOW_condition_in_select_statement_subcube511 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_CALCULATED_in_with_clause_single536 = new BitSet(new ulong[] { 0x0000000000008000UL });
		public static readonly BitSet FOLLOW_MEMBER_in_with_clause_single539 = new BitSet(new ulong[] { 0x0000030000200400UL });
		public static readonly BitSet FOLLOW_member_name_in_with_clause_single541 = new BitSet(new ulong[] { 0x0000000000010000UL });
		public static readonly BitSet FOLLOW_AS_in_with_clause_single543 = new BitSet(new ulong[] { 0x5000070D02A80400UL, 0x0000000000000860UL });
		public static readonly BitSet FOLLOW_CELL_in_with_clause_single554 = new BitSet(new ulong[] { 0x0000000000020000UL });
		public static readonly BitSet FOLLOW_CALCULATION_in_with_clause_single556 = new BitSet(new ulong[] { 0x0000000000040000UL });
		public static readonly BitSet FOLLOW_FOR_in_with_clause_single558 = new BitSet(new ulong[] { 0x4000070D02A80400UL, 0x0000000000000860UL });
		public static readonly BitSet FOLLOW_expression_in_with_clause_single562 = new BitSet(new ulong[] { 0x0000000000010000UL });
		public static readonly BitSet FOLLOW_AS_in_with_clause_single564 = new BitSet(new ulong[] { 0x5000070D02A80400UL, 0x0000000000000860UL });
		public static readonly BitSet FOLLOW_60_in_with_clause_single580 = new BitSet(new ulong[] { 0x4000070D02A80400UL, 0x0000000000000860UL });
		public static readonly BitSet FOLLOW_expression_in_with_clause_single584 = new BitSet(new ulong[] { 0x1000000000000000UL });
		public static readonly BitSet FOLLOW_60_in_with_clause_single586 = new BitSet(new ulong[] { 0x0400000000000002UL });
		public static readonly BitSet FOLLOW_expression_in_with_clause_single601 = new BitSet(new ulong[] { 0x0400000000000002UL });
		public static readonly BitSet FOLLOW_58_in_with_clause_single616 = new BitSet(new ulong[] { 0x0000030000200400UL });
		public static readonly BitSet FOLLOW_property_definition_in_with_clause_single620 = new BitSet(new ulong[] { 0x0400000000000002UL });
		public static readonly BitSet FOLLOW_SET_in_with_clause_single632 = new BitSet(new ulong[] { 0x0000030000200400UL });
		public static readonly BitSet FOLLOW_set_name_in_with_clause_single634 = new BitSet(new ulong[] { 0x0000000000010000UL });
		public static readonly BitSet FOLLOW_AS_in_with_clause_single636 = new BitSet(new ulong[] { 0x5000070D02A80400UL, 0x0000000000000860UL });
		public static readonly BitSet FOLLOW_60_in_with_clause_single642 = new BitSet(new ulong[] { 0x4000070D02A80400UL, 0x0000000000000860UL });
		public static readonly BitSet FOLLOW_expression_in_with_clause_single646 = new BitSet(new ulong[] { 0x1000000000000000UL });
		public static readonly BitSet FOLLOW_60_in_with_clause_single648 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_expression_in_with_clause_single664 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_compound_id_in_member_name691 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_identifier_in_property_definition710 = new BitSet(new ulong[] { 0x0020000000000000UL });
		public static readonly BitSet FOLLOW_53_in_property_definition712 = new BitSet(new ulong[] { 0x4000070D02A00400UL, 0x0000000000000860UL });
		public static readonly BitSet FOLLOW_expression_or_xor_in_property_definition714 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_compound_id_in_set_name742 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_identifier_in_compound_id764 = new BitSet(new ulong[] { 0x2000000000000002UL });
		public static readonly BitSet FOLLOW_61_in_compound_id772 = new BitSet(new ulong[] { 0x0000030000200400UL });
		public static readonly BitSet FOLLOW_identifier_in_compound_id776 = new BitSet(new ulong[] { 0x2000000000000002UL });
		public static readonly BitSet FOLLOW_NON_in_axis_specification806 = new BitSet(new ulong[] { 0x0000000000100000UL });
		public static readonly BitSet FOLLOW_EMPTY_in_axis_specification808 = new BitSet(new ulong[] { 0x4000070D02A80400UL, 0x0000000000000860UL });
		public static readonly BitSet FOLLOW_expression_in_axis_specification817 = new BitSet(new ulong[] { 0x0000000000600400UL });
		public static readonly BitSet FOLLOW_DIMENSION_in_axis_specification825 = new BitSet(new ulong[] { 0x0000000000000400UL });
		public static readonly BitSet FOLLOW_PROPERTIES_in_axis_specification828 = new BitSet(new ulong[] { 0x0000030000200400UL });
		public static readonly BitSet FOLLOW_property_in_axis_specification832 = new BitSet(new ulong[] { 0x0400000000400000UL });
		public static readonly BitSet FOLLOW_58_in_axis_specification842 = new BitSet(new ulong[] { 0x0000030000200400UL });
		public static readonly BitSet FOLLOW_property_in_axis_specification846 = new BitSet(new ulong[] { 0x0400000000400000UL });
		public static readonly BitSet FOLLOW_ON_in_axis_specification860 = new BitSet(new ulong[] { 0x0000030000A00400UL });
		public static readonly BitSet FOLLOW_axis_name_in_axis_specification864 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_identifier_in_axis_name888 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_INTEGER_in_axis_name895 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_compound_id_in_property912 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_cube_name_in_cube_specification934 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_NON_in_cube_specification945 = new BitSet(new ulong[] { 0x0000000001000000UL });
		public static readonly BitSet FOLLOW_VISUAL_in_cube_specification947 = new BitSet(new ulong[] { 0x4000000000000000UL });
		public static readonly BitSet FOLLOW_62_in_cube_specification952 = new BitSet(new ulong[] { 0x0000000000000800UL });
		public static readonly BitSet FOLLOW_select_statement_subcube_in_cube_specification954 = new BitSet(new ulong[] { 0x8000000000000000UL });
		public static readonly BitSet FOLLOW_63_in_cube_specification956 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_compound_id_in_cube_name976 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_RANET_EXPRESSION_in_cube_name984 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_CELL_ORDINAL_in_cell_property1004 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_VALUE_in_cell_property1011 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_FORMATTED_VALUE_in_cell_property1018 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_provider_specific_cell_property_in_cell_property1025 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_identifier_in_provider_specific_cell_property1042 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_expression_or_xor_in_expression1073 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_expression_and_in_expression_or_xor1093 = new BitSet(new ulong[] { 0x0000000060000002UL });
		public static readonly BitSet FOLLOW_XOR_in_expression_or_xor1107 = new BitSet(new ulong[] { 0x4000070D02A00400UL, 0x0000000000000860UL });
		public static readonly BitSet FOLLOW_OR_in_expression_or_xor1117 = new BitSet(new ulong[] { 0x4000070D02A00400UL, 0x0000000000000860UL });
		public static readonly BitSet FOLLOW_expression_and_in_expression_or_xor1129 = new BitSet(new ulong[] { 0x0000000060000002UL });
		public static readonly BitSet FOLLOW_expression_compare_in_expression_and1159 = new BitSet(new ulong[] { 0x0000000080000002UL });
		public static readonly BitSet FOLLOW_AND_in_expression_and1168 = new BitSet(new ulong[] { 0x4000070D02A00400UL, 0x0000000000000860UL });
		public static readonly BitSet FOLLOW_expression_compare_in_expression_and1172 = new BitSet(new ulong[] { 0x0000000080000002UL });
		public static readonly BitSet FOLLOW_expression_add_in_expression_compare1195 = new BitSet(new ulong[] { 0x0020000000000002UL, 0x000000000000001FUL });
		public static readonly BitSet FOLLOW_53_in_expression_compare1208 = new BitSet(new ulong[] { 0x4000070D02A00400UL, 0x0000000000000860UL });
		public static readonly BitSet FOLLOW_64_in_expression_compare1217 = new BitSet(new ulong[] { 0x4000070D02A00400UL, 0x0000000000000860UL });
		public static readonly BitSet FOLLOW_65_in_expression_compare1226 = new BitSet(new ulong[] { 0x4000070D02A00400UL, 0x0000000000000860UL });
		public static readonly BitSet FOLLOW_66_in_expression_compare1235 = new BitSet(new ulong[] { 0x4000070D02A00400UL, 0x0000000000000860UL });
		public static readonly BitSet FOLLOW_67_in_expression_compare1244 = new BitSet(new ulong[] { 0x4000070D02A00400UL, 0x0000000000000860UL });
		public static readonly BitSet FOLLOW_68_in_expression_compare1253 = new BitSet(new ulong[] { 0x4000070D02A00400UL, 0x0000000000000860UL });
		public static readonly BitSet FOLLOW_expression_add_in_expression_compare1264 = new BitSet(new ulong[] { 0x0020000000000002UL, 0x000000000000001FUL });
		public static readonly BitSet FOLLOW_expression_mult_in_expression_add1294 = new BitSet(new ulong[] { 0x0000000000000002UL, 0x0000000000000060UL });
		public static readonly BitSet FOLLOW_69_in_expression_add1307 = new BitSet(new ulong[] { 0x4000070D02A00400UL, 0x0000000000000860UL });
		public static readonly BitSet FOLLOW_70_in_expression_add1316 = new BitSet(new ulong[] { 0x4000070D02A00400UL, 0x0000000000000860UL });
		public static readonly BitSet FOLLOW_expression_mult_in_expression_add1327 = new BitSet(new ulong[] { 0x0000000000000002UL, 0x0000000000000060UL });
		public static readonly BitSet FOLLOW_expression_power_in_expression_mult1359 = new BitSet(new ulong[] { 0x0800000000000002UL, 0x0000000000000080UL });
		public static readonly BitSet FOLLOW_71_in_expression_mult1378 = new BitSet(new ulong[] { 0x4000070D02A00400UL, 0x0000000000000860UL });
		public static readonly BitSet FOLLOW_59_in_expression_mult1390 = new BitSet(new ulong[] { 0x4000070D02A00400UL, 0x0000000000000860UL });
		public static readonly BitSet FOLLOW_expression_power_in_expression_mult1407 = new BitSet(new ulong[] { 0x0800000000000002UL, 0x0000000000000080UL });
		public static readonly BitSet FOLLOW_expression_unary_in_expression_power1437 = new BitSet(new ulong[] { 0x0000000000000002UL, 0x0000000000000100UL });
		public static readonly BitSet FOLLOW_72_in_expression_power1446 = new BitSet(new ulong[] { 0x4000070D02A00400UL, 0x0000000000000860UL });
		public static readonly BitSet FOLLOW_expression_unary_in_expression_power1450 = new BitSet(new ulong[] { 0x0000000000000002UL, 0x0000000000000100UL });
		public static readonly BitSet FOLLOW_70_in_expression_unary1479 = new BitSet(new ulong[] { 0x4000070D02A00400UL, 0x0000000000000860UL });
		public static readonly BitSet FOLLOW_expression_range_is_in_expression_unary1483 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_69_in_expression_unary1490 = new BitSet(new ulong[] { 0x4000070D02A00400UL, 0x0000000000000860UL });
		public static readonly BitSet FOLLOW_expression_range_is_in_expression_unary1494 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_NOT_in_expression_unary1502 = new BitSet(new ulong[] { 0x4000070D02A00400UL, 0x0000000000000860UL });
		public static readonly BitSet FOLLOW_expression_range_is_in_expression_unary1506 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_expression_range_is_in_expression_unary1516 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_expression_property_in_expression_range_is1535 = new BitSet(new ulong[] { 0x0000000200000002UL, 0x0000000000000200UL });
		public static readonly BitSet FOLLOW_73_in_expression_range_is1544 = new BitSet(new ulong[] { 0x4000070C02A00400UL, 0x0000000000000800UL });
		public static readonly BitSet FOLLOW_expression_property_in_expression_range_is1548 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_IS_in_expression_range_is1558 = new BitSet(new ulong[] { 0x4000070C02A00400UL, 0x0000000000000800UL });
		public static readonly BitSet FOLLOW_expression_property_in_expression_range_is1562 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_expression_simple_in_expression_property1588 = new BitSet(new ulong[] { 0x2000000000000002UL });
		public static readonly BitSet FOLLOW_61_in_expression_property1596 = new BitSet(new ulong[] { 0x0000030000200400UL, 0x0000000000000400UL });
		public static readonly BitSet FOLLOW_unquoted_identifier_in_expression_property1606 = new BitSet(new ulong[] { 0x2000000000000002UL });
		public static readonly BitSet FOLLOW_74_in_expression_property1619 = new BitSet(new ulong[] { 0x0000030000200400UL });
		public static readonly BitSet FOLLOW_quoted_identifier_in_expression_property1623 = new BitSet(new ulong[] { 0x2000000000000002UL });
		public static readonly BitSet FOLLOW_quoted_identifier_in_expression_property1638 = new BitSet(new ulong[] { 0x2000000000000002UL });
		public static readonly BitSet FOLLOW_expression_function_in_expression_property1653 = new BitSet(new ulong[] { 0x2000000000000002UL });
		public static readonly BitSet FOLLOW_expression_function_in_expression_simple1683 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_62_in_expression_simple1690 = new BitSet(new ulong[] { 0x4000070D02A80400UL, 0x0000000000000860UL });
		public static readonly BitSet FOLLOW_expressions_list_in_expression_simple1692 = new BitSet(new ulong[] { 0x8000000000000000UL });
		public static readonly BitSet FOLLOW_63_in_expression_simple1694 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_75_in_expression_simple1703 = new BitSet(new ulong[] { 0x4000070D02A80400UL, 0x0000000000001860UL });
		public static readonly BitSet FOLLOW_expressions_list_in_expression_simple1706 = new BitSet(new ulong[] { 0x0000000000000000UL, 0x0000000000001000UL });
		public static readonly BitSet FOLLOW_76_in_expression_simple1710 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_expression_case_in_expression_simple1719 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_string_value_in_expression_simple1726 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_INTEGER_in_expression_simple1733 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_FLOAT_in_expression_simple1740 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_identifier_in_expression_simple1747 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_RANET_EXPRESSION_in_expression_simple1754 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_expression_in_expressions_list1782 = new BitSet(new ulong[] { 0x0400000000000002UL });
		public static readonly BitSet FOLLOW_58_in_expressions_list1792 = new BitSet(new ulong[] { 0x4000070D02A80400UL, 0x0000000000000860UL });
		public static readonly BitSet FOLLOW_expression_in_expressions_list1796 = new BitSet(new ulong[] { 0x0400000000000002UL });
		public static readonly BitSet FOLLOW_identifier_in_expression_function1815 = new BitSet(new ulong[] { 0x4000000000000000UL });
		public static readonly BitSet FOLLOW_62_in_expression_function1817 = new BitSet(new ulong[] { 0xC000070D02A80400UL, 0x0000000000000860UL });
		public static readonly BitSet FOLLOW_expressions_list_in_expression_function1821 = new BitSet(new ulong[] { 0x8000000000000000UL });
		public static readonly BitSet FOLLOW_63_in_expression_function1826 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_CASE_in_expression_case1843 = new BitSet(new ulong[] { 0x4000077D02A80400UL, 0x0000000000000860UL });
		public static readonly BitSet FOLLOW_expression_in_expression_case1848 = new BitSet(new ulong[] { 0x0000007000000000UL });
		public static readonly BitSet FOLLOW_when_clause_in_expression_case1865 = new BitSet(new ulong[] { 0x0000007000000000UL });
		public static readonly BitSet FOLLOW_when_clause_in_expression_case1878 = new BitSet(new ulong[] { 0x0000007000000000UL });
		public static readonly BitSet FOLLOW_ELSE_in_expression_case1899 = new BitSet(new ulong[] { 0x4000070D02A80400UL, 0x0000000000000860UL });
		public static readonly BitSet FOLLOW_expression_in_expression_case1903 = new BitSet(new ulong[] { 0x0000002000000000UL });
		public static readonly BitSet FOLLOW_END_in_expression_case1911 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_WHEN_in_when_clause1936 = new BitSet(new ulong[] { 0x4000070D02A80400UL, 0x0000000000000860UL });
		public static readonly BitSet FOLLOW_expression_in_when_clause1940 = new BitSet(new ulong[] { 0x0000008000000000UL });
		public static readonly BitSet FOLLOW_THEN_in_when_clause1942 = new BitSet(new ulong[] { 0x4000070D02A80400UL, 0x0000000000000860UL });
		public static readonly BitSet FOLLOW_expression_in_when_clause1946 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_unquoted_identifier_in_identifier1974 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_quoted_identifier_in_identifier1985 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_ID_in_unquoted_identifier2006 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_keyword_in_unquoted_identifier2013 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_QUOTED_ID_in_quoted_identifier2030 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_DIMENSION_in_keyword2047 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_PROPERTIES_in_keyword2054 = new BitSet(new ulong[] { 0x0000000000000002UL });
		public static readonly BitSet FOLLOW_STRING_in_string_value2071 = new BitSet(new ulong[] { 0x0000000000000002UL });

	}
}