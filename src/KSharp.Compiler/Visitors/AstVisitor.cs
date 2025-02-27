using KSharp.Compiler.Ast;

namespace KSharp.Compiler.Visitors
{
    /// <summary>
    /// Visitor base para atravessar a AST
    /// </summary>
    public abstract class AstVisitor
    {
        public virtual void VisitProgram(SourceFile sourceFile) { }

        public virtual void VisitFunctionDeclaration(FunctionDeclaration function)
        {
            foreach (var parameter in function.Parameters)
            {
                parameter.Accept(this);
            }

            function.Body?.Accept(this);
        }

        public virtual void VisitParameter(Parameter parameter)
        {
            // Nó folha - não tem filhos para visitar
        }

        public virtual void VisitBlockStatement(BlockStatement block)
        {
            foreach (var statement in block.Statements)
            {
                statement.Accept(this);
            }
        }

        public virtual void VisitVariableDeclaration(VariableDeclaration varDecl)
        {
            varDecl.Initializer?.Accept(this);
        }

        public virtual void VisitReturnStatement(ReturnStatement returnStmt)
        {
            returnStmt.Expression?.Accept(this);
        }

        public virtual void VisitIfStatement(IfStatement ifStmt)
        {
            ifStmt.Condition?.Accept(this);
        }

        public virtual void VisitFunctionCallExpression(FunctionCallExpression funcCall)
        {
            foreach (var argument in funcCall.Arguments)
            {
                argument.Accept(this);
            }
        }

        public virtual void VisitBinaryExpression(BinaryExpression binExpr)
        {
            binExpr.Left?.Accept(this);
            binExpr.Right?.Accept(this);
        }

        public virtual void VisitLiteralExpression(LiteralExpression literal)
        {
            // Nó folha - não tem filhos para visitar
        }

        public virtual void VisitIdentifierExpression(IdentifierExpression identifier)
        {
            // Nó folha - não tem filhos para visitar
        }

        public virtual void VisitStringInterpolationExpression(
            StringInterpolationExpression interpolation
        )
        {
            foreach (var part in interpolation.Parts)
            {
                part.Accept(this);
            }
        }
    }
}
