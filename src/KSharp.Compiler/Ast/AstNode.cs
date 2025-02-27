using KSharp.Compiler.Visitors;

namespace KSharp.Compiler.Ast
{
    /// <summary>
    /// Classe base para todos os nós da AST
    /// </summary>
    public abstract record AstNode
    {
        public virtual void Accept(AstVisitor visitor) { }
    }
}
