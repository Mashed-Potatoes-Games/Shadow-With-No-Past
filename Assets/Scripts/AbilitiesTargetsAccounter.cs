using ShadowWithNoPast.Entities.Abilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class AbilitiesTargetsAccounter
{
    private Dictionary<Vector2Int, List<AbilityInstance>> possibleTargetsDict = new Dictionary<Vector2Int, List<AbilityInstance>>();
    [SerializeField]
    private List<AbilityWIthTargets> abilitiesTargets = new List<AbilityWIthTargets>();

    public AbilitiesTargetsAccounter(WorldManagement world) {
        world.EventManager.ObjectMoved += (_, start, end) =>
        {
            if(start.World == world)
            {
                RecalclulateAffectedAbilities(start.Vector);
            }

            if(end.World == world)
            {
                RecalclulateAffectedAbilities(end.Vector);
            }
        };
    }

    public List<AbilityInstance> AbilitiesAffectedByMove(Vector2Int pos)
    {
        possibleTargetsDict.TryGetValue(pos, out var attacks);
        return attacks;
    }

    public List<Vector2Int> GetAbilityPossibleTargets(AbilityInstance instance)
    {
        return abilitiesTargets.Find(item => item.Instance == instance).Targets;
    }

    public void RecalclulateAffectedAbilities(Vector2Int vector)
    {
        possibleTargetsDict.TryGetValue(vector, out var abilities);
        if(abilities != null)
        {
            foreach(var instance in abilities.ToList())
            {
                instance.Caller.TelegraphController.RepaintAbility(instance);
            }
        }
    }

    public void AddPossibleTargets(AbilityInstance instance, List<Vector2Int> possibleTargets)
    {
        abilitiesTargets.Add(new AbilityWIthTargets(instance, possibleTargets));
        AddToDict(possibleTargets, instance);
    }

    public void RemovePossibleTargets(AbilityInstance instance)
    {
        if(abilitiesTargets.Any(abilitiesTargets => abilitiesTargets.Instance == instance))
        {
            abilitiesTargets.RemoveAll(item => item.Instance == instance);
            RemoveInstanceOccurenceInDict(instance);
        }
    }

    private void RemoveInstanceOccurenceInDict(AbilityInstance instance)
    {
        foreach (List<AbilityInstance> instances in possibleTargetsDict.Values)
        {
            instances.Remove(instance);
        }
    }

    public void RefreshPossibleTargets(AbilityInstance instance, List<WorldPos> possibleTargets) =>
        RefreshPossibleTargets(instance, possibleTargets.Select(pos => pos.Vector).ToList());
    public void RefreshPossibleTargets(AbilityInstance instance, List<Vector2Int> possibleTargets, bool throwException = false)
    {
        var abilityWithTargets = abilitiesTargets.Find(item => item.Instance == instance);
        if(abilityWithTargets != null)
        {
            foreach(var vector in possibleTargets.FindAll(target => abilityWithTargets.Targets.Contains(target)))
            {
                possibleTargetsDict[vector].Remove(instance);
            }

            abilityWithTargets.Targets = possibleTargets;

            AddToDict(possibleTargets, instance);
            return;
        }

        if(throwException)
        {
            throw new Exception("Ability wasn't in accounter, but refresh method was called anyways!");
        }

        AddPossibleTargets(instance, possibleTargets);
    }
    private void AddToDict(List<Vector2Int> targets, AbilityInstance instance)
    {
        foreach(var vector in targets)
        {
            if(possibleTargetsDict.ContainsKey(vector))
            {
                possibleTargetsDict[vector].Add(instance);
                continue;
            }
            possibleTargetsDict.Add(vector, new List<AbilityInstance> { instance });
        }
    }

    public void Clear()
    {
        possibleTargetsDict.Clear();
        abilitiesTargets.Clear();
    }
}

[Serializable]
public class AbilityWIthTargets
{
    public AbilityInstance Instance;
    public List<Vector2Int> Targets;

    public AbilityWIthTargets(AbilityInstance item1, List<Vector2Int> item2)
    {
        Instance = item1;
        Targets = item2;
    }
}