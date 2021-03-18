﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using Microsoft.Azure.Templates.Analyzer.Types;

namespace Microsoft.Azure.Templates.Analyzer.RuleEngines.JsonEngine.Expressions
{
    /// <summary>
    /// Represents an allOf expression in a JSON rule.
    /// </summary>
    internal class AllOfExpression : Expression
    {
        /// <summary>
        /// Gets the expressions to be evaluated.
        /// </summary>
        public Expression[] AllOf { get; private set; }

        /// <summary>
        /// Creates an <see cref="AllOfExpression"/>.
        /// </summary>
        /// <param name="expressions">List of expressions to perform a logical AND against.</param>
        /// <param name="resourceType">The resource type this expression evaluates.</param>
        /// <param name="path">The JSON path being evaluated.</param>
        public AllOfExpression(Expression[] expressions, string resourceType, string path)
            : base(resourceType, path)
        {
            this.AllOf = expressions ?? throw new ArgumentNullException(nameof(expressions));
        }

        /// <summary>
        /// Evaluates all expressions provided and aggregates them in a final <see cref="JsonRuleEvaluation"/>.
        /// </summary>
        /// <param name="jsonScope">The json to evaluate.</param>
        /// <returns>A <see cref="JsonRuleEvaluation"/> with the results of the evaluation.</returns>
        protected override (JsonRuleEvaluation evaluation, JsonRuleResult result) EvaluateInternal(IJsonPathResolver jsonScope)
        {
            List<JsonRuleEvaluation> jsonRuleEvaluations = new List<JsonRuleEvaluation>();
            bool evaluationPassed = true;

            foreach (var expression in AllOf)
            {
                var evaluation = expression.Evaluate(jsonScope);
                        
                evaluationPassed &= evaluation.Passed;

                jsonRuleEvaluations.Add(evaluation);
            }

            return (new JsonRuleEvaluation(this, evaluationPassed, jsonRuleEvaluations), null);
        }
    }
}
