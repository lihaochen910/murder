﻿using ImGuiNET;
using Murder.Assets.Graphics;
using Murder.Components;
using Murder.Editor.Attributes;
using System.Collections.Immutable;

namespace Murder.Editor.CustomComponents
{
    [CustomComponentOf(typeof(SpriteComponent))]
    internal class SpriteComponentEditor : CustomComponent
    {
        protected override bool DrawAllMembersWithTable(ref object target)
        {
            bool fileChanged = false;

            if (ImGui.BeginTable($"field_{target.GetType().Name}", 2,
                ImGuiTableFlags.SizingFixedSame | ImGuiTableFlags.BordersOuter | ImGuiTableFlags.BordersInnerH))
            {
                ImGui.TableSetupColumn("a", ImGuiTableColumnFlags.WidthFixed, -1, 0);
                ImGui.TableSetupColumn("b", ImGuiTableColumnFlags.WidthStretch, -1, 1);
                fileChanged |= DrawAllMembers(target);

                var component = (SpriteComponent)target;
                if (Game.Data.TryGetAsset<SpriteAsset>(component.AnimationGuid) is SpriteAsset ase)
                {
                    if (ImGui.BeginCombo($"##AnimationID", component.CurrentAnimation))
                    {
                        foreach (var value in ase.Animations.Keys)
                        {
                            if (string.IsNullOrWhiteSpace(value))
                            {
                                continue;
                            }

                            if (ImGui.MenuItem(value))
                            {
                                ImmutableArray<string> nextAnimations;
                                if (component.NextAnimations.Length == 0)
                                {
                                    nextAnimations = new string[] { value }.ToImmutableArray();
                                }
                                else
                                {
                                    nextAnimations = component.NextAnimations.SetItem(0, value);
                                }

                                target.GetType().GetField("NextAnimations")!.SetValue(target, nextAnimations);
                                fileChanged = true;
                            }
                        }
                        ImGui.EndCombo();
                    }
                }

                ImGui.EndTable();
            }

            return fileChanged;
        }
    }
}