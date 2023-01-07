﻿using Bang.Components;
using Murder.Attributes;
using Murder.Core.Dialogs;
using Newtonsoft.Json;
using System.Collections.Immutable;

namespace Murder.Components
{
    public enum AfterInteractRule
    {
        InteractOnlyOnce,
        
        /// <summary>
        /// Instead of removing this component once triggered, this will only disable it.
        /// </summary>
        InteractOnReload,

        /// <summary>
        /// Instead of removing this component once triggered, this will remove the entity.
        /// </summary>
        RemoveEntity
    }
    
    public readonly struct InteractOnRuleMatchComponent : IComponent
    {
        [Tooltip("Expected behavior once the rule is met.")]
        public readonly AfterInteractRule AfterInteraction = AfterInteractRule.InteractOnlyOnce;

        /// <summary>
        /// This will only be triggered once the component has been interacted with.
        /// Used if <see cref="AfterInteractRule.InteractOnReload"/> is set.
        /// </summary>
        [JsonIgnore]
        public readonly bool Triggered = false;

        /// <summary>
        /// List of requirements which will trigger the interactive component within the same entity.
        /// </summary>
        public readonly ImmutableArray<CriterionNode> Requirements = ImmutableArray<CriterionNode>.Empty;

        public InteractOnRuleMatchComponent() { }

        public InteractOnRuleMatchComponent(AfterInteractRule after, bool triggered, ImmutableArray<CriterionNode> requirements) => 
            (AfterInteraction, Triggered, Requirements) = (after, triggered, requirements);

        public InteractOnRuleMatchComponent(params CriterionNode[] criteria) => Requirements = criteria.ToImmutableArray();

        public InteractOnRuleMatchComponent Disable() => new(AfterInteraction, triggered: true, Requirements);
    }
}
