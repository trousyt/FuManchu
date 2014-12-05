﻿namespace FuManchu.Tests
{
	using System.Linq;
	using FuManchu.Parser.SyntaxTree;
	using FuManchu.Text;
	using FuManchu.Tokenizer;

	public class Factory
	{
		private readonly SourceLocationTracker _tracker = new SourceLocationTracker();

		public Block Block(BlockType type, string name = null, params SyntaxTreeNode[] children)
		{
			var builder = new BlockBuilder();
			builder.Type = type;
			builder.Name = name;

			foreach (var child in children)
			{
				builder.Children.Add(child);
			}

			return builder.Build();
		}

		public Block Document(params SyntaxTreeNode[] children)
		{
			return Block(BlockType.Document, null, children);
		}

		public Block Tag(string name, params SyntaxTreeNode[] children)
		{
			return Block(BlockType.Tag, name, children);
		}

		public Block TagElement(string name, params SyntaxTreeNode[] children)
		{
			return Block(BlockType.TagElement, name, children);
		}

		public Block Expression(params SyntaxTreeNode[] children)
		{
			return Block(BlockType.Expression, null, children);
		}

		public Span Span(SpanKind kind, params ISymbol[] symbols)
		{
			var builder = new SpanBuilder();
			builder.Kind = kind;

			foreach (var symbol in symbols)
			{
				builder.Accept(symbol);
			}

			return builder.Build();
		}

		public Span Text(string content)
		{
			return Span(SpanKind.Text, Symbol(content, HandlebarsSymbolType.Text));
		}

		public Span WhiteSpace(int length)
		{
			return Span(SpanKind.WhiteSpace, Symbol(string.Join("", Enumerable.Repeat(" ", length)), HandlebarsSymbolType.WhiteSpace));
		}

		public Span Parameter(string content, HandlebarsSymbolType type = HandlebarsSymbolType.Identifier)
		{
			return Span(SpanKind.Parameter, Symbol(content, HandlebarsSymbolType.Identifier));
		}

		public Span MetaCode(string content, HandlebarsSymbolType type)
		{
			return Span(SpanKind.MetaCode, Symbol(content, type));
		}

		public Span Expression(params ISymbol[] symbols)
		{
			return Span(SpanKind.Expression, symbols);
		}

		public ISymbol Symbol(string content, HandlebarsSymbolType type)
		{
			var location = _tracker.CurrentLocation;
			_tracker.UpdateLocation(content);

			return new HandlebarsSymbol(location, content, type);
		}
	}
}