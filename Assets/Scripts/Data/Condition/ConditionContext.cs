using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using TSoft.InGame;
using TSoft.InGame.CardSystem;
using TSoft.InGame.GamePlaySystem;
using UnityEngine;

namespace TSoft.Data.Condition
{
    [Serializable]
    public class ConditionContext
    {
        public List<ConditionToken> tokens;
        
        public bool Evaluate(ConditionApplier applier)
        {
            var prefixTokens = ConvertToPrefix(tokens);
            var root = BuildExpressionTree(prefixTokens);
            
            return root.Interpret(applier);
        }
        
        private List<ConditionToken> ConvertToPrefix(List<ConditionToken> infixTokens)
        {
            Stack<ConditionToken> operators = new Stack<ConditionToken>();
            Stack<ConditionToken> output = new Stack<ConditionToken>();

            for (int i = infixTokens.Count - 1; i >= 0; i--)
            {
                ConditionToken token = infixTokens[i];

                switch (token.tokenType)
                {
                    case ConditionTokenType.Condition:
                        output.Push(token);
                        break;

                    case ConditionTokenType.RightBracket:
                        operators.Push(token);
                        break;

                    case ConditionTokenType.LeftBracket:
                        while (operators.Count > 0 && operators.Peek().tokenType != ConditionTokenType.RightBracket)
                        {
                            output.Push(operators.Pop());
                        }
                        operators.Pop();
                        break;

                    default: // 연산자
                        while (operators.Count > 0 && operators.Peek().tokenType != ConditionTokenType.RightBracket)
                        {
                            output.Push(operators.Pop());
                        }
                        operators.Push(token);
                        break;
                }
            }

            while (operators.Count > 0)
            {
                output.Push(operators.Pop());
            }

            return new List<ConditionToken>(output);
        }
        
        private IConditionExpression BuildExpressionTree(List<ConditionToken> prefixTokens)
        {
            Stack<IConditionExpression> stack = new Stack<IConditionExpression>();

            for (int i = prefixTokens.Count - 1; i >= 0; i--) // Prefix 연산
            {
                ConditionToken token = prefixTokens[i];

                if (token.tokenType == ConditionTokenType.Condition)
                {
                    stack.Push(new ConditionExpression(token.condition));
                }
                else
                {
                    IConditionExpression right = stack.Pop();
                    IConditionExpression left = null;

                    if (token.tokenType != ConditionTokenType.Not)
                        left = stack.Pop();

                    switch (token.tokenType)
                    {
                        case ConditionTokenType.And:
                            stack.Push(new AndExpression(left, right));
                            break;
                        case ConditionTokenType.Or:
                            stack.Push(new OrExpression(left, right));
                            break;
                        case ConditionTokenType.Xor:
                            stack.Push(new XorExpression(left, right));
                            break;
                        case ConditionTokenType.Not:
                            stack.Push(new NotExpression(right));
                            break;
                    }
                }
            }

            return stack.Pop();
        }
    }
    public interface IConditionExpression
    {
        bool Interpret(ConditionApplier applier);
    }
    
    public class AndExpression : IConditionExpression
    {
        private readonly IConditionExpression expr1;
        private readonly IConditionExpression expr2;

        public AndExpression(IConditionExpression expr1, IConditionExpression expr2)
        {
            this.expr1 = expr1;
            this.expr2 = expr2;
        }

        public bool Interpret(ConditionApplier applier)
        {
            return expr1.Interpret(applier) && expr2.Interpret(applier);
        }
    }
    
    public class OrExpression : IConditionExpression
    {
        private readonly IConditionExpression expr1;
        private readonly IConditionExpression expr2;

        public OrExpression(IConditionExpression expr1, IConditionExpression expr2)
        {
            this.expr1 = expr1;
            this.expr2 = expr2;
        }

        public bool Interpret(ConditionApplier applier)
        {
            return expr1.Interpret(applier) || expr2.Interpret(applier);
        }
    }
    
    public class XorExpression : IConditionExpression
    {
        private readonly IConditionExpression expr1;
        private readonly IConditionExpression expr2;

        public XorExpression(IConditionExpression expr1, IConditionExpression expr2)
        {
            this.expr1 = expr1;
            this.expr2 = expr2;
        }

        public bool Interpret(ConditionApplier applier)
        {
            return expr1.Interpret(applier) ^ expr2.Interpret(applier);
        }
    }
    
    public class NotExpression : IConditionExpression
    {
        private readonly IConditionExpression expr;

        public NotExpression(IConditionExpression expr)
        {
            this.expr = expr;
        }

        public bool Interpret(ConditionApplier applier)
        {
            return !expr.Interpret(applier);
        }
    }
    
    public class ConditionExpression : IConditionExpression
    {
        private readonly ConditionSO condition;

        public ConditionExpression(ConditionSO condition)
        {
            this.condition = condition;
        }

        public bool Interpret(ConditionApplier applier)
        {
            return condition.Interpret(applier);
        }
    }

    public enum ConditionTokenType
    {
        LeftBracket,
        RightBracket,
        Condition,
        And,
        Or,
        Xor,
        Not
    }

    [Serializable]
    public struct ConditionToken
    {
        public ConditionTokenType tokenType;
        
        [ShowIf("tokenType", ConditionTokenType.Condition)]
        public ConditionSO condition;
    }
    
}
